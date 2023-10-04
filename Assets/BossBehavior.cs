using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    public BossState currentState;

    private Rigidbody2D rb;
    public float maxVelocity = 10f;

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

    private Transform player;

    private bool isChargingPreparationStarted = false;
    
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        currentState = BossState.SlowPursuit;
    }

void Update()
{
    switch(currentState)
    {
        case BossState.SlowPursuit:
            SlowPursuit();
            
            if (Vector3.Distance(transform.position, player.position) < startChargeRange) {
                currentState = BossState.ChargingPreparation;
                break; // Important to exit the switch statement after changing state
            }

            if (Vector3.Distance(transform.position, player.position) >= startShootRange) {
                currentState = BossState.ProjectileShooting;
                rb.velocity = Vector2.zero; // Stop the boss from moving.
                StartCoroutine(ProjectileShooting());
                break;
            }
            break;

        case BossState.ChargingPreparation:
            if (!isChargingPreparationStarted)
            {
                isChargingPreparationStarted = true;
                StartCoroutine(ChargingPreparation());
            }
            break;

        case BossState.ProjectileShooting:
            // Shooting is handled in the coroutine.
            break;
    }
}


    void SlowPursuit()
    {
        float distanceToTarget = Vector2.Distance(transform.position, player.position);
        Vector2 directionToTarget = (player.position - transform.position).normalized;
        rb.velocity = Vector2.ClampMagnitude(directionToTarget * slowPursuitSpeed, maxVelocity);
    }
    
    IEnumerator ChargingPreparation()
{
    // Step 1: Stop the boss from moving.
    rb.velocity = Vector2.zero;

    // Step 2: Play your custom charge animation here.
    // For instance, if you're using an Animator, you'd do something like:
    // animator.SetTrigger("ChargeUp");
    // Note: Replace with your own code if necessary!

    // Wait for the charge preparation time.
    yield return new WaitForSeconds(chargePreparationTime);

    // Step 3: Now, let the boss charge towards the player.
    float distanceToTarget = Vector2.Distance(transform.position, player.position);
    Vector2 directionToTarget = (player.position - transform.position).normalized;
    rb.velocity = directionToTarget * chargeSpeed;

    // Reset the flag for the next charge.
    isChargingPreparationStarted = false;
}



// If you're using a collider for the walls
private void OnCollisionEnter2D(Collision2D collision)
{
    if (currentState == BossState.ChargingPreparation && collision.gameObject.CompareTag("Wall"))
    {
        Debug.Log("Boss hit wall");
        rb.velocity = Vector2.zero;  // Stop the boss
        currentState = BossState.SlowPursuit; // Transition back to slow pursuit or another state if preferred
    }
}


    IEnumerator ProjectileShooting()
{
    while(currentState == BossState.ProjectileShooting)
    {
        // If the player gets too close, exit the shooting state
        if(Vector3.Distance(transform.position, player.position) < startChargeRange) {
            currentState = BossState.SlowPursuit;
            StopCoroutine(ProjectileShooting());  // Important to stop the current coroutine
            yield break;  // Exit the coroutine
        }

        Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        yield return new WaitForSeconds(shootingDelay);
    }
}


    void OnDrawGizmosSelected()
{
    // Draw a red sphere at your boss's position with a radius of startChargeRange
    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, startChargeRange);

    // Draw a blue sphere at your boss's position with a radius of startShootRange
    Gizmos.color = Color.blue;
    Gizmos.DrawWireSphere(transform.position, startShootRange);
}

}

public enum BossState
{
    SlowPursuit,
    ChargingPreparation,
    ProjectileShooting
}
