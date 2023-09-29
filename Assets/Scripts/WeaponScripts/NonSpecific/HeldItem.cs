using UnityEngine;

public class HeldItem : MonoBehaviour
{
    public float orbitRadius = 1.0f;  // Adjust this value to set the orbit distance from the player
    public float rotationOffset = 0f;  // Adjust this value to set the rotation offset of the weapon

    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 difference = mousePosition - transform.parent.position;
        difference.Normalize();  

        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        float weaponPosX = transform.parent.position.x + orbitRadius * Mathf.Cos(rotationZ * Mathf.Deg2Rad);
        float weaponPosY = transform.parent.position.y + orbitRadius * Mathf.Sin(rotationZ * Mathf.Deg2Rad);

        transform.position = new Vector3(weaponPosX, weaponPosY, 0f);
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ + rotationOffset);
    }
}
