using System.Collections;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    public BossState currentState;

    private Rigidbody2D rb;
    public float maxVelocity = 5f;

    // Pursuit variables
    public float slowPursuitSpeed = 2.0f;

    // Charging variables
    public float chargePreparationTime = 2.0f;
    public float chargeSpeed = 10.0f;

    // Shooting variables
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float shootingDelay = 2.0f;

    // State Trigger variables
    public float startChargeRange = 5f;
    public float startShootRange = 8f;

    // Charging variables
    public float chargeCooldownTime = 5.0f; // NEW: Cooldown time after a charge.
    private bool isChargeOnCooldown = false; // NEW: Check if charge is on cooldown.

    private Transform player;

    private bool isChargingPreparationStarted = false;

    private Color originalColor;
    public SpriteRenderer bodySprite;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();

        originalColor = bodySprite.color; // store the original color of the boss
        currentState = BossState.SlowPursuit;
    }

    void Update()
    {
        switch (currentState)
        {
            case BossState.SlowPursuit:
                SlowPursuit();

                if (Vector3.Distance(transform.position, player.position) < startChargeRange)
                {
                    currentState = BossState.ChargingPreparation;
                }
                else if (Vector3.Distance(transform.position, player.position) >= startShootRange)
                {
                    currentState = BossState.ProjectileShooting;
                    rb.velocity = Vector2.zero;
                    StartCoroutine(ProjectileShooting());
                }
                break;

            case BossState.ChargingPreparation:
                if (!isChargingPreparationStarted)
                {
                    isChargingPreparationStarted = true;
                    StartCoroutine(ChargingPreparation());
                }
                break;

            case BossState.Charging:
                // Charging is initiated in the ChargingPreparation coroutine 
                // and continued here until a collision occurs.
                break;

            case BossState.ProjectileShooting:
                // Shooting is handled in the coroutine.
                break;
        }
    }

    void SlowPursuit()
{
    Vector2 directionToTarget = (player.position - transform.position).normalized;
    rb.velocity = Vector2.ClampMagnitude(directionToTarget * slowPursuitSpeed, maxVelocity);

    if (!isChargeOnCooldown)
    {
        if (Vector3.Distance(transform.position, player.position) < startChargeRange)
        {
            currentState = BossState.ChargingPreparation;
        }
        else if (Vector3.Distance(transform.position, player.position) >= startShootRange)
        {
            currentState = BossState.ProjectileShooting;
            rb.velocity = Vector2.zero;
            StartCoroutine(ProjectileShooting());
        }
    }
}


    IEnumerator ChargingPreparation()
    {
        rb.velocity = Vector2.zero;
        // Add your animation or sound effects here.
        yield return new WaitForSeconds(chargePreparationTime);

        // Switch to Charging state after preparation is complete.
        currentState = BossState.Charging;
        Vector2 directionToTarget = (player.position - transform.position).normalized;
        rb.velocity = directionToTarget * chargeSpeed;

        isChargingPreparationStarted = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
{
    // Check for collision during Charging state
    if (currentState == BossState.Charging && (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player")))
    {
        Debug.Log("Boss finished charging");
        rb.velocity = Vector2.zero;  // Stop the boss
        currentState = BossState.SlowPursuit;
        if (!isChargeOnCooldown) // Ensure the cooldown isn't already running
        {
            StartCoroutine(StartChargeCooldown());  // Start the cooldown after a charge
        }
    }
}


    IEnumerator StartChargeCooldown()
{
    Debug.Log("Cooldown Started!"); // log for debugging
    bodySprite.color = Color.red; // change boss color to red for visual indication

    isChargeOnCooldown = true;  // Start the cooldown
    yield return new WaitForSeconds(chargeCooldownTime);  // Wait for the cooldown duration

    Debug.Log("Cooldown Ended!"); // log for debugging
    bodySprite.color = originalColor; // revert boss color to its original color

    isChargeOnCooldown = false;  // End the cooldown
}

    IEnumerator ProjectileShooting()
    {
        while (currentState == BossState.ProjectileShooting)
        {
            if (Vector3.Distance(transform.position, player.position) < startChargeRange)
            {
                currentState = BossState.SlowPursuit;
                StopCoroutine(ProjectileShooting());
                yield break;
            }

            Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            yield return new WaitForSeconds(shootingDelay);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, startChargeRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, startShootRange);
    }
}

public enum BossState
{
    SlowPursuit,
    ChargingPreparation,
    Charging,
    ProjectileShooting
}
