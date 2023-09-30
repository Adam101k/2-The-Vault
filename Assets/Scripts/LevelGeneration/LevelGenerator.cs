using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int level = 1;
    public RoomData roomData;
    public Transform levelContainer;  // Parent object to keep the hierarchy organized

    private const int roomWidth = 19;
    private const int roomHeight = 11;

    public const int GridWidth = 9;
    public const int GridHeight = 8;
    public int[,] grid;
    private bool[,] levelGrid = new bool[9, 8];  // Assuming a 9x8 grid as per your description
    public List<Vector2Int> queue = new List<Vector2Int>();
    public List<Vector2Int> endRooms = new List<Vector2Int>();

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        // Reset grid and lists
        grid = new int[GridWidth, GridHeight];
        queue.Clear();
        endRooms.Clear();

        // Determine number of rooms
        int roomCount = Random.Range(0, 2) + 5 + level * 2;

        // Place starting room at the center
        Vector2Int startCell = new Vector2Int(4, 3);  // Updated line to specify the center cell
        grid[startCell.x, startCell.y] = 1;
        queue.Add(startCell);

        int roomsPlaced = 1;

        while (queue.Count > 0 && roomsPlaced < roomCount)
        {
            Vector2Int currentCell = queue[0];
            queue.RemoveAt(0);

            Vector2Int[] directions = {
                new Vector2Int(0, 1), // Up
                new Vector2Int(0, -1), // Down
                new Vector2Int(1, 0), // Right
                new Vector2Int(-1, 0) // Left
            };

            foreach (var direction in directions)
            {
                Vector2Int neighborCell = currentCell + direction;

                // Check boundaries and if neighbor cell is already occupied
                if (!IsValidCell(neighborCell) || grid[neighborCell.x, neighborCell.y] != 0)
                    continue;

                // Check the number of filled neighbors
                int filledNeighbors = GetFilledNeighborCount(neighborCell);
                if (filledNeighbors > 1)
                    continue;

                // Random 50% chance
                if (Random.value < 0.5f)
                    continue;

                // Mark neighbor cell as having a room
                grid[neighborCell.x, neighborCell.y] = 1;
                levelGrid[neighborCell.x, neighborCell.y] = true;  // Updated line

                roomsPlaced++;
                queue.Add(neighborCell);

                // If we have placed all rooms, break
                if (roomsPlaced >= roomCount)
                    break;
            }

            if (roomsPlaced >= roomCount)
                break;

            // Add current cell to endRooms if no neighbor was valid
            endRooms.Add(currentCell);
        }

        VisualizeLevel();

        // Further steps for placing special rooms and normal rooms...
    }

    void VisualizeLevel()
    {
        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                if (grid[x, y] == 1)  // If there is a room at this grid cell
                {
                    Vector3 position = new Vector3(x * roomWidth, y * roomHeight, 0);
                    GameObject prefabToInstantiate;
                    if (x == 4 && y == 3)  // Check if the current cell is the center cell
                    {
                        prefabToInstantiate = roomData.roomTBLR;  // Use the TBLR room for the center cell
                    }
                    else
                    {
                        prefabToInstantiate = SelectRoomPrefab(x, y);
                    }
                    Instantiate(prefabToInstantiate, position, Quaternion.identity, levelContainer);
                }
            }
        }
    }

    string GetDoorIdentifier(Vector2Int position)
    {
        string doorIdentifier = "";

        Vector2Int up = position + Vector2Int.up;
        Vector2Int down = position + Vector2Int.down;
        Vector2Int left = position + Vector2Int.left;
        Vector2Int right = position + Vector2Int.right;

        if (IsRoomAt(up)) doorIdentifier += "T";
        if (IsRoomAt(down)) doorIdentifier += "B";
        if (IsRoomAt(left)) doorIdentifier += "L";
        if (IsRoomAt(right)) doorIdentifier += "R";

        return doorIdentifier;
    }

    bool IsCellOccupied(Vector2Int position)
    {
        // Check if the position is within bounds
        if (position.x >= 0 && position.x < 9 && position.y >= 0 && position.y < 8)
        {
            return levelGrid[position.x, position.y];
        }
        return false;
    }

    bool IsRoomAt(Vector2Int position)
    {
        // Ensure position is within bounds
        if (position.x < 0 || position.x >= GridWidth || position.y < 0 || position.y >= GridHeight)
            return false;

        return grid[position.x, position.y] == 1;
    }

    GameObject SelectRoomPrefab(int x, int y)
    {
        string doorIdentifier = GetDoorIdentifier(new Vector2Int(x, y));
        Debug.Log($"Door Identifier: {doorIdentifier}");  // Add debug output here
        return roomData.GetRoomPrefab(doorIdentifier);
    }

    bool IsValidCell(Vector2Int cell)
    {
        return cell.x >= 0 && cell.x < GridWidth && cell.y >= 0 && cell.y < GridHeight;
    }

    int GetFilledNeighborCount(Vector2Int cell)
    {
        int count = 0;
        Vector2Int[] directions = {
            new Vector2Int(0, 1), // Up
            new Vector2Int(0, -1), // Down
            new Vector2Int(1, 0), // Right
            new Vector2Int(-1, 0) // Left
        };

        foreach (var direction in directions)
        {
            Vector2Int neighborCell = cell + direction;
            if (IsValidCell(neighborCell) && grid[neighborCell.x, neighborCell.y] != 0)
                count++;
        }

        return count;
    }
}
