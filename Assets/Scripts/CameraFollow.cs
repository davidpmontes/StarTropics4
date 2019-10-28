using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    void LateUpdate()
    {
        Move();
    }

    private void Move()
    {
        var targetPosition = target.position;
        targetPosition.z = -10;
        targetPosition.y = 0;
        transform.position = targetPosition;
    }
}
