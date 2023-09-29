using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int movespeed = 1;
    public Sprite[] directionSprites; // Sprites assigned in inspector
    public SpriteRenderer spriteRenderer;
    public Camera cam;
    public float mouseOffSet = 45f;

    public Rigidbody2D rb;

    Vector2 movement;

    void start() {
        rb = GetComponent<Rigidbody2D>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        Vector2 direction = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        SetSpriteBasedOnAngle(angle);
    }

    void SetSpriteBasedOnAngle(float angle) {
        int directionIndex = Mathf.RoundToInt(angle / mouseOffSet) % 8;
        if (directionIndex < 0) {
            directionIndex += 8; // Ensure index is positive
        }
        spriteRenderer.sprite = directionSprites[directionIndex];
        Debug.Log("Angle: " + angle + ", Index: " + directionIndex);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement.normalized * movespeed * Time.deltaTime);
    }
}
