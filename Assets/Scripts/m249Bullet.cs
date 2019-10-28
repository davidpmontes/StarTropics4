using UnityEngine;

public class m249Bullet : MonoBehaviour
{
    private int direction;
    private readonly float SPEED = 10f;

    void Update()
    {
        Move();
        CheckEnemy();
    }

    private void Move()
    {
        transform.Translate(Vector3.right * direction * Time.deltaTime * SPEED);
    }

    private void CheckEnemy()
    {
        Collider2D c2D = Physics2D.OverlapBox(transform.position, new Vector2(0.5f, 0.5f), 0, LayerMask.GetMask("enemy"));

        if (c2D)
        {
            if (c2D.gameObject.GetComponent<IEnemyHit>().Hit())
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
