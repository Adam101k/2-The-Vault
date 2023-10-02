using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth;
    public int damageAmount = 1;

    public Sprite[] headDirectionSprites; // Assign your 8-direction head sprites in the inspector.
    private SpriteRenderer headSpriteRenderer;
    private SimpleAI unit;

    public void Start() {
        headSpriteRenderer = transform.Find("Head").GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        unit = GetComponent<SimpleAI>();
        // Set default sprites
        if (headDirectionSprites.Length > 0)
        {
            headSpriteRenderer.sprite = headDirectionSprites[2]; 
        }
    }

    void Update() {
        HandleHeadDirection();
    }

    public void TakeDamage(int amount) {
        currentHealth -= amount;
        if (currentHealth <= 0) {
            Destroy(gameObject);
        }
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
