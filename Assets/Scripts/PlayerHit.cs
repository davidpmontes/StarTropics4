using UnityEngine;

public class PlayerHit : MonoBehaviour, IPlayerHit
{
    [SerializeField] private PlayerController playerController;

    public bool Hit(int value)
    {
        return playerController.Hit(value);
    }
}
