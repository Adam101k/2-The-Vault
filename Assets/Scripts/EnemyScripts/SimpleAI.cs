using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAI : MonoBehaviour
{
    public float speed;
    public float stoppingDistance;
    private Transform target;
    private Rigidbody2D rb;
    private bool isKnockedBack;
    private float knockbackEndTime;
    public float maxVelocity = 10f;
    public bool canKnockBack = true;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    public Transform getTarget()
    {
        return target;
    }

    void Update()
{
    if (!isKnockedBack)
    {
        float distanceToTarget = Vector2.Distance(transform.position, target.position);
        if (distanceToTarget > stoppingDistance)
        {
            Vector2 directionToTarget = (target.position - transform.position).normalized;
            rb.velocity = directionToTarget * speed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    else if (Time.time >= knockbackEndTime)
    {
        isKnockedBack = false;
        rb.velocity = Vector2.zero;
    }
}

void FixedUpdate()
{
    // Cap the maximum velocity
    rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);
}


    public void ApplyKnockback(Vector2 knockbackDirection, float knockbackMagnitude)
{
    if (!isKnockedBack && canKnockBack)  // Only apply knockback if the enemy is not already being knocked back
    {
        Vector2 normalizedKnockbackDirection = knockbackDirection.normalized;
        rb.velocity = Vector2.zero;  // Reset velocity
        rb.AddForce(normalizedKnockbackDirection * knockbackMagnitude, ForceMode2D.Impulse);
        isKnockedBack = true;
        knockbackEndTime = Time.time + 0.5f;  // Assume knockback effect lasts for 0.5 seconds
    }
}

}
