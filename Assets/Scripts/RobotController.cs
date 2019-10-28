using UnityEngine;

public class RobotController : MonoBehaviour, IEnemyHit
{
    private Rigidbody2D rb2d;
    private int direction = 1;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform fireCheck;
    [SerializeField] private Transform barrelPosition;
    private bool isfacingRight;

    public float fireRate = 0.5f;
    private float lastFired;

    public float moveSpeed = 5;

    void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
        Flip();
        FireCheck();
        PlayerCheck();
    }

    private void FixedUpdate()
    {
        WallCheck();
    }

    private void WallCheck()
    {
        Collider2D c2D = Physics2D.OverlapBox(wallCheck.position, new Vector2(0.2f, 0.2f), 0, LayerMask.GetMask("collide"));

        if (c2D)
        {
            direction *= -1;
        }
    }

    private void Move()
    {
        rb2d.velocity = new Vector2(direction * moveSpeed, rb2d.velocity.y);
    }

    private void Flip()
    {
        if ((direction > 0 && !isfacingRight) || (direction < 0 && isfacingRight))
        {
            isfacingRight = !isfacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    private void FireCheck()
    {
        Collider2D c2D = Physics2D.OverlapBox(fireCheck.position, new Vector2(6f, 1f), 0, LayerMask.GetMask("player"));

        if (c2D)
        {
            Fire();
        }
    }

    private void PlayerCheck()
    {
        Collider2D c2D = Physics2D.OverlapBox(transform.position, new Vector2(1, 2), 0, LayerMask.GetMask("player"));

        if (c2D)
        {
            if (c2D.gameObject.GetComponent<IPlayerHit>().Hit(25))
            {
                AudioManager.Instance.PlayOverlapping("explodeA");

            }
        }
    }

    private void Fire()
    {
        if (Time.time - lastFired > fireRate)
        {
            GameObject robotBullet = ObjectPool.Instance.GetFromPoolInactive(Pools.robotBullet);
            robotBullet.transform.position = barrelPosition.position;
            robotBullet.SetActive(true);
            robotBullet.GetComponent<RobotBullet>().Fire(direction);
            lastFired = Time.time;
        }
    }

    public bool Hit()
    {
        return true;
    }
}
