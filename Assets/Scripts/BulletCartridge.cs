using UnityEngine;

public class BulletCartridge : MonoBehaviour
{
    private Rigidbody2D rb2d;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void Eject(int direction)
    {
        rb2d.AddForce(new Vector2(-direction, 2) * Random.Range(150, 200));
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(80, 100));
        Invoke("DestroySelf", 2f);
    }

    public void DestroySelf()
    {
        transform.rotation = Quaternion.Euler(0, 0, 90);
        ObjectPool.Instance.DeactivateAndAddToPool(gameObject);
    }
}
