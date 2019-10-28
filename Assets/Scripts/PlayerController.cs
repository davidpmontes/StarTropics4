using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour, IPlayerHit
{
    [SerializeField] private float JUMP_FORCE = 700f;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private GameObject lifebar;
    [SerializeField] private Transform gunPositions;


    private readonly float HORIZONTAL_SPEED = 5f;

    private Rigidbody2D rb2d;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D bc2d;
    private CircleCollider2D cc2d;

    private bool isGrounded;
    private bool isfacingRight;
    private bool isInvincible;

    private float horizontalInput;
    private bool jumpInput;
    private bool fireInput;
    private bool proneInput;
    private int currentWeapon = 1;

    private float life = 100f;

    [SerializeField] GameObject[] guns;

    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;

    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bc2d = GetComponent<BoxCollider2D>();
        cc2d = GetComponent<CircleCollider2D>();

        if (OnLandEvent == null)
        {
            OnLandEvent = new UnityEvent();
        }

        if (OnCrouchEvent == null)
        {
            OnCrouchEvent = new BoolEvent();
        }
    }

    private void Start()
    {
        SwitchToStanding();
    }

    private void Update()
    {
        GetInput();
        Move();
        Jump();
        Prone();
        FireWeapon();
        Flip();
    }

    private void FixedUpdate()
    {
        GroundCheck();
    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
            animator.speed = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
            animator.speed = 1;
        }
        else
        {
            horizontalInput = 0;
            animator.speed = 0;
        }

        if (Input.GetKey(KeyCode.Alpha0))
        {
            EquipWeapon(0);
            animator.SetInteger("currWeapon", 0);
        }
        if (Input.GetKey(KeyCode.Alpha1))
        {
            EquipWeapon(1);
            animator.SetInteger("currWeapon", 1);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            EquipWeapon(2);
            animator.SetInteger("currWeapon", 2);
        }

        fireInput = Input.GetKey(KeyCode.J);
        jumpInput = Input.GetKeyDown(KeyCode.K);

        if (Input.GetKey(KeyCode.S))
        {
            proneInput = true;
        }

        if (Input.GetKey(KeyCode.W))
        {
            proneInput = false;
        }
    }

    private void GroundCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(groundCheck.position, new Vector2(0.75f, 0.2f), 0);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                if (!isGrounded)
                {
                    isGrounded = true;
                    animator.SetBool("isJumping", false);
                    OnLandEvent.Invoke();
                }
                return;
            }
        }
        isGrounded = false;
    }

    private void Move()
    {
        rb2d.velocity = new Vector2(horizontalInput * HORIZONTAL_SPEED, rb2d.velocity.y);
    }

    private void Prone()
    {
        if (isGrounded && proneInput)
        {
            animator.SetBool("isProne", true);
            SwitchToProne();
        }

        if (!proneInput)
        {
            animator.SetBool("isProne", false);
            SwitchToStanding();
        }
    }

    private void Jump()
    {
        if (isGrounded && jumpInput)
        {
            SwitchToStanding();
            proneInput = false;
            animator.SetBool("isProne", false);
            animator.SetBool("isJumping", true);
            animator.speed = 0;
            rb2d.AddForce(new Vector2(0, JUMP_FORCE));
        }
    }

    private void FireWeapon()
    {
        if (fireInput)
        {
            guns[currentWeapon].GetComponent<IWeapon>().Fire(-(int)transform.localScale.x);
        }
    }

    private void EquipWeapon(int newWeapon)
    {
        guns[currentWeapon].SetActive(false);
        guns[newWeapon].SetActive(true);
        currentWeapon = newWeapon;
    }

    private void Flip()
    {
        if ((horizontalInput > 0 && !isfacingRight) || (horizontalInput < 0 && isfacingRight))
        {
            isfacingRight = !isfacingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }

    public bool Hit(int value)
    {
        if (isInvincible)
        {
            return false;
        }
        isInvincible = true;

        life -= value;
        UpdateLifeBar();
        StartCoroutine(InvincibleFlash());
        return true;
    }

    private IEnumerator InvincibleFlash()
    {
        float startTime = Time.time;
        bool isTransparent = false;

        while (Time.time - startTime < 1)
        {
            if (isTransparent)
            {
                spriteRenderer.color = new Color(255f, 255f, 255f, 1);
            }
            else
            {
                spriteRenderer.color = new Color(255f, 255f, 255f, 0);
            }
            isTransparent = !isTransparent;
            yield return new WaitForSeconds(0.1f);
        }
        spriteRenderer.color = new Color(255f, 255f, 255f, 1);
        isInvincible = false;
        yield return null;
    }

    private void UpdateLifeBar()
    {
        lifebar.GetComponent<RectTransform>().localScale = new Vector3(life / 100f, 1, 1);
    }

    private void SwitchToProne()
    {
        bc2d.offset = new Vector2(0, -0.25f);
        bc2d.size = new Vector2(2f, 0.5f);

        cc2d.offset = new Vector2(0, -0.15f);
        cc2d.radius = 0.4f;

        groundCheck.localPosition = new Vector3(0, -0.58f, 0);
        gunPositions.localPosition = new Vector3(-0.76f, -0.4f, 0);
    }

    private void SwitchToStanding()
    {
        bc2d.offset = new Vector2(0, -0.05f);
        bc2d.size = new Vector2(.8f, 1.88f);

        cc2d.offset = new Vector2(0, -0.6f);
        cc2d.radius = 0.4f;

        groundCheck.localPosition = new Vector3(0, -1, 0);
        gunPositions.localPosition = new Vector3(0, 0, 0);
    }
}
