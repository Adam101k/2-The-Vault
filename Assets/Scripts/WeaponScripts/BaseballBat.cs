using UnityEngine;

public class BaseballBat : MonoBehaviour
{
    public Animator animator;
    public float attackRadius = 1.5f;
    public LayerMask enemyLayer;
    public int arcDamage = 10; // Area damage
    public int directHitDamage = 20;  // Damage for direct hits
    public float attackCooldown = 1f;
    private float nextAttackTime = 0f;

    public AudioSource SwingSource;

    public AudioClip[] SwingSounds;
    public AudioClip[] ImpactSounds;

    public bool IsAttacking;

    private bool swungRightLastTime = true;  // Assuming the first swing is to the left

    public float knockbackMagnitude = 10f;
    private Vector2 directionToMouse;


    void Update()
    {
        // Calculate the mouse position in world coordinates
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;  // Set z to 0, as we're in 2D

        // Calculate the direction from the player to the mouse
        directionToMouse = (mousePosition - transform.position).normalized;

        // Calculate the angle of rotation
        float angle = Mathf.Atan2(directionToMouse.y, directionToMouse.x) * Mathf.Rad2Deg;

        // Set the rotation of the player or weapon
        transform.rotation = Quaternion.Euler(0, 0, angle);
        if (Time.time >= nextAttackTime && Input.GetMouseButtonDown(0))
        {
            if (swungRightLastTime)
            {
                // Trigger the animation to swing left
                animator.SetTrigger("SwingLeft");
                SwingSource.time = 0.2f;
                SwingSource.PlayOneShot(SwingSounds[0]);
                swungRightLastTime = false;
            } else {
                // Trigger the animation to swing right
                animator.SetTrigger("SwingRight");
                SwingSource.time = 0.2f;
                SwingSource.PlayOneShot(SwingSounds[1]);
                swungRightLastTime = true;
            }
            PerformRangeAttack(directionToMouse);
            nextAttackTime = Time.time + attackCooldown;
        }
    }
    public void StartAttack()
    {
        IsAttacking = true;
    }

    public void EndAttack()
    {
        IsAttacking = false;
    }

    void PerformRangeAttack(Vector2 attackDirection)
    {
        // Get all enemies within the maximum range of the attack.
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRadius, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            // Impact Audio
            AudioSource[] enemyAud = enemy.gameObject.GetComponents<AudioSource>();
            PlayRandomHitSound(enemyAud[0]);

            Vector2 toEnemy = (enemy.transform.position - transform.position).normalized;
            float angle = Vector2.Angle(attackDirection, toEnemy);
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            // Check if enemy is within a 90 degree arc in front of the player
            // and within the maximum range of the attack.
            if (angle < 45f && distance < attackRadius)
            {
                enemy.GetComponent<Enemy>().TakeDamage(arcDamage);
            }

            Vector2 knockbackDirection = enemy.transform.position - transform.position;
            enemy.GetComponent<SimpleAI>().ApplyKnockback(knockbackDirection, knockbackMagnitude);
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsAttacking && collision.gameObject.layer == LayerMask.NameToLayer("Enemy")) {
                Debug.Log("This was called");
                Enemy enemy = collision.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(directHitDamage);
                }
            }
    }

    void PlayRandomHitSound(AudioSource ImpactSource)
    {
        if (ImpactSounds.Length > 0 && ImpactSource != null)
        {
            int randomIndex = Random.Range(0, ImpactSounds.Length);  // Pick a random index
            ImpactSource.PlayOneShot(ImpactSounds[randomIndex]);  // Play the corresponding audio clip
        }
    }

    private void OnDrawGizmosSelected()
{
    // Fetch the current rotation angle of the player or weapon
    float angle = transform.eulerAngles.z * Mathf.Deg2Rad;  // Convert to radians for trigonometry

    // Recalculate the attack direction based on the current rotation
    Vector3 attackDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;

    float coneAngle = 45f;  // Half the full angle of the cone
    float coneDistance = attackRadius;

    // Calculate the direction vectors for the edges of the cone
    Vector3 edge1Direction = Quaternion.Euler(0, 0, coneAngle) * attackDirection;
    Vector3 edge2Direction = Quaternion.Euler(0, 0, -coneAngle) * attackDirection;

    // Calculate the endpoints of the lines representing the edges of the cone
    Vector3 edge1EndPoint = transform.position + edge1Direction * coneDistance;
    Vector3 edge2EndPoint = transform.position + edge2Direction * coneDistance;

    // Draw lines representing the edges of the cone
    Gizmos.color = Color.red;
    Gizmos.DrawLine(transform.position, edge1EndPoint);
    Gizmos.DrawLine(transform.position, edge2EndPoint);
}

}
