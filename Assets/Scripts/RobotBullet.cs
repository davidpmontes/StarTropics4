using UnityEngine;

public class RobotBullet : MonoBehaviour
{
    private int direction;
    public float bulletSpeed = 10f;

    void Update()
	{
        Move();
        CheckPlayer();
    }

    private void Move()
    {
        transform.Translate(Vector3.right * direction * Time.deltaTime * bulletSpeed);
    }

    private void CheckPlayer()
    {
        Collider2D c2D = Physics2D.OverlapBox(transform.position, new Vector2(0.5f, 0.5f), 0, LayerMask.GetMask("player"));

        if (c2D)
        {
            if (c2D.gameObject.GetComponent<IPlayerHit>().Hit(25))
            {
                var explodeA = ObjectPool.Instance.GetFromPoolInactive(Pools.explodeA);
                explodeA.transform.position = transform.position;
                explodeA.SetActive(true);

                ObjectPool.Instance.DeactivateAndAddToPool(gameObject);
            }
        }
    }

    public void Fire(int direction)
    {
        AudioManager.Instance.PlayOverlapping("robotBullet");
        this.direction = direction;
        Invoke("DestroySelf", 2f);
    }

    public void DestroySelf()
    {
        ObjectPool.Instance.DeactivateAndAddToPool(gameObject);
    }
}
