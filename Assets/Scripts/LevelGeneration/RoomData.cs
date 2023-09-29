using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName = "ScriptableObjects/RoomData", order = 1)]
public class RoomData : ScriptableObject
{
    public GameObject normalRoomPrefab;
    public GameObject bossRoomPrefab;
    public GameObject secretRoomPrefab;
    // ... (add other room prefabs as needed)
}
