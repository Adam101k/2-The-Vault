using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName = "ScriptableObjects/RoomData", order = 1)]
public class RoomData : ScriptableObject
{
    public GameObject room;  // no doors
    public GameObject roomT, roomB, roomL, roomR;  // single doors
    public GameObject roomTB, roomTL, roomTR, roomBL, roomBR, roomLR;  // double doors
    public GameObject roomTBL, roomTBR, roomTLR, roomBLR;  // triple doors
    public GameObject roomTBLR;  // quadruple doors

    public GameObject GetRoomPrefab(string doorIdentifier)
    {
        switch (doorIdentifier)
        {
            case "": return room;
            case "T": return roomT;
            case "B": return roomB;
            case "L": return roomL;
            case "R": return roomR;
            case "TB": return roomTB;
            case "TL": return roomTL;
            case "TR": return roomTR;
            case "BL": return roomBL;
            case "BR": return roomBR;
            case "LR": return roomLR;
            case "TBL": return roomTBL;
            case "TBR": return roomTBR;
            case "TLR": return roomTLR;
            case "BLR": return roomBLR;
            case "TBLR": return roomTBLR;
            default:
                Debug.LogError($"No prefab found for doorIdentifier: {doorIdentifier}");
                return null;  // handle error
        }
    }
}
