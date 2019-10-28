using UnityEngine;

public enum COLLIDE_DIRECTION
{
    TOP, BOTTOM, LEFT, RIGHT
}

public class CollisionReporter : MonoBehaviour
{
    [SerializeField] private COLLIDE_DIRECTION collideDirection;
    [SerializeField] private WorldController wc;
    private BoxCollider2D boxCollider2D;

    private LayerMask colliderLayer;

    private void Awake()
    {
        colliderLayer = LayerMask.NameToLayer("collide");
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (boxCollider2D.IsTouchingLayers(colliderLayer))
        {
            wc.CollisionEnter(collideDirection);
        }
        else
        {
            //wc.CollisionExit(collideDirection);
        }
    }

    
}
