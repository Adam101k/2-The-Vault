using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public int level = 1;
    public RoomData roomData;
    public Transform levelContainer; // Parent object to keep the hierarchy organized

    public const int GridWidth = 9;
    public const int GridHeight = 8;
    
    public int[,] grid;
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
        
        // Place starting room
        Vector2Int startCell = new Vector2Int(3, 5);
        grid[startCell.x, startCell.y] = 1;
        queue.Add(startCell);
        
        int roomsPlaced = 1;
        
        while(queue.Count > 0 && roomsPlaced < roomCount)
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
                    Vector3 position = new Vector3(x * 10, y * 10, 0);
                    GameObject prefabToInstantiate = SelectRoomPrefab(x, y);
                    Instantiate(prefabToInstantiate, position, Quaternion.identity, levelContainer);
                }
            }
        }
    }

    GameObject SelectRoomPrefab(int x, int y)
    {
        // Example: Place boss room in the last room of the endRooms list
        if (endRooms.Contains(new Vector2Int(x, y)) && endRooms.IndexOf(new Vector2Int(x, y)) == endRooms.Count - 1)
        {
            return roomData.bossRoomPrefab;
        }

        // Example: Place secret room if there are at least 3 neighboring rooms
        int filledNeighborCount = GetFilledNeighborCount(new Vector2Int(x, y));
        if (filledNeighborCount >= 3)
        {
            return roomData.secretRoomPrefab;
        }

        // Default to normal room prefab
        return roomData.normalRoomPrefab;
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
