using System.Collections.Generic;
using UnityEngine;

public class AmmoIndicator : MonoBehaviour
{
    public static AmmoIndicator Instance { get; private set; }

    [SerializeField] private List<GameObject> m249ammo;
    private int currAmmo = 0;

    void Awake()
    {
        Instance = this;
    }

    public void RemoveAmmo()
    {
        if (currAmmo >= m249ammo.Capacity)
            return;

        m249ammo[currAmmo].SetActive(false);
        currAmmo++;
    }

    public void Reload()
    {
        foreach(GameObject ammo in m249ammo)
        {
            ammo.SetActive(true);
        }
        currAmmo = 0;
    }
}
