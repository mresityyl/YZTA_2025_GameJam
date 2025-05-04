using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarlarý")]
    public float moveSpeed = 5f;
    public float acceleration = 20f;
    public float decceleration = 10f;
    public float velPower = 2f;
    public float idleStopForce = 10f;

    [Header("Atlama Ayarlarý")]
    public float jumpForce = 10f;
    [Range(0, 1)]
    public float jumpCutMultiplier = 0.5f;

    [Header("Coyote Time")]
    [Tooltip("Zeminden ayrýldýktan sonra ne kadar süre zýplanabilir")]
    public float coyoteTimeDuration = 0.15f;

    [Header("Zemin Kontrolü")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    public bool isGrounded;
    private float moveInput;

    [Header("Animasyon")]
    private static int jumpHash = Animator.StringToHash("Jump");
    private static int speedHash = Animator.StringToHash("Speed");
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isJumpingInputHold;

    private float lastGroundedTime;

    private const float MIN_MOVE_THRESHOLD = 0.01f;
    private const float MIN_VELOCITY_THRESHOLD = 0.01f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        lastGroundedTime = -Mathf.Infinity;
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        animator.SetFloat(speedHash, Mathf.Abs(moveInput));

        if (moveInput > MIN_MOVE_THRESHOLD)
        {
            spriteRenderer.flipX = false;
        }
        else if (moveInput < -MIN_MOVE_THRESHOLD)
        {
            spriteRenderer.flipX = true;
        }

        if (Input.GetButtonDown("Jump") && (isGrounded || Time.time - lastGroundedTime < coyoteTimeDuration))
        {
            lastGroundedTime = -Mathf.Infinity;

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumpingInputHold = true;
        }

        if (Input.GetButtonUp("Jump"))
        {
            isJumpingInputHold = false;

            if (rb.linearVelocity.y > MIN_VELOCITY_THRESHOLD)
            {
                rb.AddForce((1 - jumpCutMultiplier) * rb.linearVelocity.y * Vector2.down, ForceMode2D.Impulse);
            }
        }
    }

    void FixedUpdate()
    {
        float targetSpeed = moveInput * moveSpeed;
        float speedDif = targetSpeed - rb.linearVelocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > MIN_MOVE_THRESHOLD) ? acceleration : decceleration;
        float movementForce = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);

        rb.AddForce(movementForce * Vector2.right);

        if (Mathf.Abs(moveInput) < MIN_MOVE_THRESHOLD)
        {
            float amount = Mathf.Min(Mathf.Abs(rb.linearVelocity.x), Mathf.Abs(idleStopForce));
            amount *= Mathf.Sign(rb.linearVelocity.x);
            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }

        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (wasGrounded && !isGrounded)
        {
            lastGroundedTime = Time.time;
        }

        if (rb.linearVelocity.y < -MIN_VELOCITY_THRESHOLD)
        {
            rb.gravityScale = 2f;
        }
        else if (rb.linearVelocity.y > MIN_VELOCITY_THRESHOLD && !isJumpingInputHold)
        {
            rb.gravityScale = 1f;
        }
        else
        {
            rb.gravityScale = 1f;
        }

        animator.SetBool(jumpHash, !isGrounded);

        if (!wasGrounded && isGrounded)
        {
            isJumpingInputHold = false;
        }
    }
}