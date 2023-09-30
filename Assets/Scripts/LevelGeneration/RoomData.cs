using UnityEngine;

[CreateAssetMenu(fileName = "RoomData", menuName = "ScriptableObjects/RoomData", order = 1)]
public class RoomData : ScriptableObject
{
    public GameObject roomT; // top door only
    public GameObject roomL; // left door only
    public GameObject roomR; // right door only
    public GameObject roomB; // bottom door only
    public GameObject roomLB; // left bottom door only
    public GameObject roomLR; // left right door only
    public GameObject roomRB; // right bottom door only
    public GameObject roomTB; // top bottom door only
    public GameObject roomTL; // top left door only
    public GameObject roomTR; // top right door only
                             // ... (any additional rooms can be added here)

    public GameObject GetRoomPrefab(string doorIdentifier)
    {
        switch (doorIdentifier)
        {
            case "T": return roomT;
            case "L": return roomL;
            case "R": return roomR;
            case "B": return roomB;
            case "LB": return roomLB;
            case "LR": return roomLR;
            case "RB": return roomRB;
            case "TB": return roomTB;
            case "TL": return roomTL;
            case "TR": return roomTR;
            // ... (include all combinations)
            default: return null; // handle error
        }
    }
}
