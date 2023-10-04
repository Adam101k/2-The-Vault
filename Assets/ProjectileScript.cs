using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public float speed = 5f;                  // Speed of the projectile
    public float lifetime = 5f;               // Time before the projectile self-destructs

    public int damageAmount = 1;
    private Vector2 targetPosition;           // The position the projectile is aiming for
    private Transform player;                 // Reference to the player

    private void Awake()
    {
        // Assuming the player has a tag "Player"
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Set the target position to the player's position at the time of instantiation
        targetPosition = player.position;
    }

    void Start()
    {
        // Set the projectile's direction based on the target position
        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        GetComponent<Rigidbody2D>().velocity = direction * speed;

        // Destroy the projectile after a set lifetime to account for misses
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // In case you want to add more behavior or checks
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Call the player's damage method here. For example:
            // other.GetComponent<PlayerScript>().TakeDamage(someAmount);
            
            // Destroy the projectile
            PlayerHealth.PlayerH.SubtractPlayerHealth(damageAmount);
            Destroy(gameObject);
        }
    }
}
