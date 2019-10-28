using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldController : MonoBehaviour
{
    private enum MOVESTATUS
    {
        NONE, READY, MOVING
    }

    private Animator animator;
    private AudioSource audioSource;

    private int verticalInput;
    private int horizontalInput;

    private Vector3Int moveDirection;
    private Vector3Int targetPosition;

    private bool isMoving;
    [SerializeField] private bool[] canMove = new bool[4];

    [SerializeField] private Tilemap tm;
    [SerializeField] private AudioClip terrainHit;

    [SerializeField] private GameObject top;
    [SerializeField] private GameObject bottom;
    [SerializeField] private GameObject left;
    [SerializeField] private GameObject right;

    RaycastHit2D topRay;
    RaycastHit2D bottomRay;
    RaycastHit2D leftRay;
    RaycastHit2D rightRay;

    private LayerMask colliderLayer;

    private float terrainHitCounter;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        colliderLayer = LayerMask.GetMask("collide");
        terrainHitCounter = 0;

        canMove[0] = true;
        canMove[1] = true;
        canMove[2] = true;
        canMove[3] = true;
    }

    void Update()
    {
        GetInput();
        Collisions();
        Move();
        Audio();
    }

    private void GetInput()
    {
        verticalInput = 0;
        horizontalInput = 0;
        verticalInput += Input.GetKey(KeyCode.W) ? 1 : 0;
        verticalInput += Input.GetKey(KeyCode.S) ? -1 : 0;
        horizontalInput += Input.GetKey(KeyCode.A) ? -1 : 0;
        horizontalInput += Input.GetKey(KeyCode.D) ? 1 : 0;

        moveDirection = new Vector3Int(horizontalInput, verticalInput, 0);
    }

    private void Collisions()
    {
        canMove[0] = true;
        canMove[1] = true;
        canMove[2] = true;
        canMove[3] = true;

        topRay = Physics2D.BoxCast(top.transform.position, new Vector2(0.9f, 0.9f), 0, new Vector2(0, 0), 0, colliderLayer);
        bottomRay = Physics2D.BoxCast(bottom.transform.position, new Vector2(0.9f, 0.9f), 0, new Vector2(0, 0), 0, colliderLayer);
        leftRay = Physics2D.BoxCast(left.transform.position, new Vector2(0.9f, 0.9f), 0, new Vector2(0, 0), 0, colliderLayer);
        rightRay = Physics2D.BoxCast(right.transform.position, new Vector2(0.9f, 0.9f), 0, new Vector2(0, 0), 0, colliderLayer);

        canMove[(int)COLLIDE_DIRECTION.TOP] = (topRay.collider == null);
        canMove[(int)COLLIDE_DIRECTION.BOTTOM] = (bottomRay.collider == null);
        canMove[(int)COLLIDE_DIRECTION.LEFT] = (leftRay.collider == null);
        canMove[(int)COLLIDE_DIRECTION.RIGHT] = (rightRay.collider == null);
    }

    private void Move()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 3);

            var distance = Vector3.Distance(transform.position, targetPosition);

            if (distance < (1f / 16f))
            {
                transform.position = targetPosition;
                isMoving = false;
            }
            else
            {
                return;
            }
        }

        Vector3Int nextPosition = Vector3Int.RoundToInt(transform.position);

        if (moveDirection.x == 1)
        {
            animator.Play("SoldierWorldRight");

            if (canMove[(int)COLLIDE_DIRECTION.RIGHT])
            {
                nextPosition.x += 1;
                targetPosition = nextPosition;
                isMoving = true;
            }
            else
            {
                terrainHitCounter += Time.deltaTime;
            }
        }
        else if (moveDirection.x == -1)
        {
            animator.Play("SoldierWorldLeft");

            if (canMove[(int)COLLIDE_DIRECTION.LEFT])
            {
                nextPosition.x -= 1;
                targetPosition = nextPosition;
                isMoving = true;
            }
            else
            {
                terrainHitCounter += Time.deltaTime;
            }
        }
        else if (moveDirection.y == 1)
        {
            animator.Play("SoldierWorldUp");

            if (canMove[(int)COLLIDE_DIRECTION.TOP])
            {
                nextPosition.y += 1;
                targetPosition = nextPosition;
                isMoving = true;
            }
            else
            {
                terrainHitCounter += Time.deltaTime;
            }
        }
        else if (moveDirection.y == -1)
        {
            animator.Play("SoldierWorldDown");

            if (canMove[(int)COLLIDE_DIRECTION.BOTTOM])
            {
                nextPosition.y -= 1;
                targetPosition = nextPosition;
                isMoving = true;
            }
            else
            {
                terrainHitCounter += Time.deltaTime;
            }
        }
        else
        {
            terrainHitCounter = 0;
        }
    }

    private void Audio()
    {
        if (terrainHitCounter > 0.2f)
        {
            audioSource.PlayOneShot(terrainHit);
            terrainHitCounter = 0;
        }
    }

    public void CollisionEnter(COLLIDE_DIRECTION cd)
    {
        //canMove[(int)cd] = false;
    }

    public void CollisionExit(COLLIDE_DIRECTION cd)
    {
        //canMove[(int)cd] = true;
    }
}
