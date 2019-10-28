using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.PlayOverlapping("explodeA");
        Invoke("DestroySelf", 2f);
    }

    public void DestroySelf()
    {
        ObjectPool.Instance.DeactivateAndAddToPool(gameObject);
    }
}
