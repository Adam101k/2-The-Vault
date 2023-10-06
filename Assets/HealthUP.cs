using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUP : MonoBehaviour
{
    public int HealthToGive = 2;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check for collision during Charging state
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth.PlayerH.AddPlayerHealth(HealthToGive);
            Destroy(gameObject);
        }
    }
}
