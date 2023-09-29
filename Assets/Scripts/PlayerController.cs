using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int movespeed = 1;
    private Rigidbody2D rb;
    public Sprite[] headDirectionSprites; // Assign your 8-direction head sprites in the inspector.
    private SpriteRenderer headSpriteRenderer;
    private Animator bodyAnimator;  // reference to the body's animator
    private Vector2 Movement;
    private Vector2 lastMoveDirection;
    public ParticleSystem dust;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        headSpriteRenderer = transform.Find("Head").GetComponent<SpriteRenderer>();
        bodyAnimator = transform.Find("Body").GetComponent<Animator>();

        // Set default sprites
        if (headDirectionSprites.Length > 0)
        {
            headSpriteRenderer.sprite = headDirectionSprites[2]; 
        }
    }

    void Update()
    {
        HandleHeadDirection();
        HandleAnimation();
    }

    void FixedUpdate() {
        HandleMovement();
    }

    void HandleAnimation() {
        float angle = 0f;
        if (Movement.sqrMagnitude > 0.01f) {
            angle = Mathf.Atan2(Movement.y, Movement.x) * Mathf.Rad2Deg;
        }
        bodyAnimator.SetFloat("angle", angle);
        bodyAnimator.SetFloat("horizontal", Movement.x);
        bodyAnimator.SetBool("UpOrDown", Mathf.Abs(Movement.y) > 0f);
        bodyAnimator.SetBool("isWalking", Movement.sqrMagnitude > 0.01f);
    }

    void HandleHeadDirection()
    {
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        SetSpriteBasedOnAngle(headSpriteRenderer, headDirectionSprites, angle);
    }

    void HandleMovement()
    {
        Movement.x = Input.GetAxisRaw("Horizontal");
        Movement.y = Input.GetAxisRaw("Vertical");
        
        // Normalize the movement vector only if the input is outside the unit circle to maintain the intended speed.
        if (Movement.sqrMagnitude > 1)
        {
            Movement.Normalize();
        }

        if (Movement.sqrMagnitude > 0.01f)
        {
            lastMoveDirection = Movement;
        }


        rb.MovePosition(rb.position + Movement * movespeed * Time.deltaTime);
    }



    void SetSpriteBasedOnAngle(SpriteRenderer spriteRenderer, Sprite[] directionSprites, float angle)
    {
        int directionIndex = Mathf.RoundToInt(angle / 45f) % 8;
        if (directionIndex < 0) directionIndex += 8;  // Ensure index is positive
        spriteRenderer.sprite = directionSprites[directionIndex];
    }

    void CreateDust() {
        dust.Play();
    }
}
