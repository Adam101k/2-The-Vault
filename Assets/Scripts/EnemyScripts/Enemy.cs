using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private SimpleFlash flashEffect;
    public int currentHealth;
    public int maxHealth;
    public int damageAmount = 1;

    public Sprite[] headDirectionSprites; // Assign your 8-direction head sprites in the inspector.
    private SpriteRenderer headSpriteRenderer;
    private SimpleAI unit;
    public bool canRotateHead = true;

    public void Start() {
        flashEffect = GetComponent<SimpleFlash>();
        headSpriteRenderer = transform.Find("Head").GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        unit = GetComponent<SimpleAI>();
        // Set default sprites
        if (headDirectionSprites.Length > 0 && canRotateHead)
        {
            headSpriteRenderer.sprite = headDirectionSprites[2]; 
        }
    }

    void Update() {
        if (canRotateHead) {
            HandleHeadDirection();
        }
    }

    public void TakeDamage(int amount) {
        currentHealth -= amount;
        if (currentHealth <= 0) {
            Destroy(gameObject);
        }
        flashEffect.Flash();

    }

    public void HandleHeadDirection() {
        Vector2 direction = unit.getTarget().position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        SetSpriteBasedOnAngle(headSpriteRenderer, headDirectionSprites, angle);
    }

    void SetSpriteBasedOnAngle(SpriteRenderer spriteRenderer, Sprite[] directionSprites, float angle)
    {
        int directionIndex = Mathf.RoundToInt(angle / 45f) % 8;
        if (directionIndex < 0) directionIndex += 8;  // Ensure index is positive
        spriteRenderer.sprite = directionSprites[directionIndex];
    }

    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            PlayerHealth.PlayerH.SubtractPlayerHealth(damageAmount);
        }
    }
}
