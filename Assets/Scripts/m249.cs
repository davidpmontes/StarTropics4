using UnityEngine;

public class m249 : MonoBehaviour, IWeapon
{
    [SerializeField] Transform barrelPosition;
    [SerializeField] Transform ejectionPortPosition;

    private float fireRate = 0.1f;
    private int clipSize = 50;
    private float reloadRate = 1f;

    private int currentClipCount;
    private float lastReloaded;
    private float lastFired;
    private bool reloading;

    private void Awake()
    {
        currentClipCount = clipSize;
    }

    public void Fire(int direction)
    {
        if (Time.time - lastReloaded < reloadRate)
        {
            return;
        }

        if (reloading)
        {
            AmmoIndicator.Instance.Reload();
            reloading = false;
        }

        if (currentClipCount <= 0)
        {
            currentClipCount = clipSize;
            lastReloaded = Time.time;
            reloading = true;
            return;
        }

        if (Time.time - lastFired > fireRate)
        {
            AmmoIndicator.Instance.RemoveAmmo();
            currentClipCount--;

            AudioManager.Instance.PlayOverlapping("m249Bullet");

            GameObject m249Bullet = ObjectPool.Instance.GetFromPoolInactive(Pools.m249Bullet);
            m249Bullet.transform.position = barrelPosition.transform.position;
            m249Bullet.GetComponent<m249Bullet>().Fire(direction);
            m249Bullet.SetActive(true);

            GameObject bulletCartridge = ObjectPool.Instance.GetFromPoolInactive(Pools.bulletCartridge);
            bulletCartridge.transform.position = ejectionPortPosition.position;
            bulletCartridge.SetActive(true);
            bulletCartridge.GetComponent<BulletCartridge>().Eject(direction);

            lastFired = Time.time;
        }
    }
}
