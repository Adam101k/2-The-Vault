using UnityEngine;

[System.Serializable]
public class SpecialRoom
{
    public string type;  // e.g., "Boss", "Item", "Secret"
    public GameObject prefab;
    public Vector2Int position;  // Store the position of the special room on the grid
}
