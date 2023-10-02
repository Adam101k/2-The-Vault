using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRoom : MonoBehaviour
{
    public GameObject lockInWalls;
    public List<GameObject> enemySpawnPoints;  // Changed to List for dynamic size
    public GameObject[] enemyPrefabs;  // Prefabs of different enemies
    private bool hasEntered = false;
    public int difficultyScale = 1;
    public int minSpawnAmount = 4;
    private List<GameObject> currentEnemies = new List<GameObject>();  // List to hold current enemies in the room


    private void Awake()
    {
        lockInWalls = transform.Find("PreventLeaving").gameObject;
        if (lockInWalls == null)
        {
            Debug.LogError("PreventLeaving child object not found");
        }
        
        // Get the SpawnNodeHolder
        Transform spawnNodeHolder = transform.Find("SpawnNodeHolder");
        if (spawnNodeHolder == null)
        {
            Debug.LogError("SpawnNodeHolder child object not found");
            return;
        }

        // Automatically assign each child of SpawnNodeHolder as a node
        enemySpawnPoints = new List<GameObject>();  // Initialize list
        foreach (Transform child in spawnNodeHolder)
        {
            enemySpawnPoints.Add(child.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasEntered)  // Assuming your player has the tag "Player"
        {
            hasEntered = true;
            SpawnEnemies();
            lockInWalls.SetActive(true);
            StartCoroutine(CheckEnemyCount());
        }
    }

    private IEnumerator CheckEnemyCount()
    {
        while (true)
        {
            if (!AreEnemiesAlive())
            {
                lockInWalls.SetActive(false);
                StopCoroutine(CheckEnemyCount());
            }
            yield return new WaitForSeconds(0.5f);  // Checks every 0.5 seconds, adjust as necessary
        }
    }

    private void SpawnEnemies()
{
    List<GameObject> eligibleEnemyPrefabs = new List<GameObject>();

    // Filter enemy prefabs based on difficulty tag
    foreach (GameObject enemyPrefab in enemyPrefabs)
    {
        if (int.Parse(enemyPrefab.tag) <= difficultyScale)  // Assuming tags are strings that can be parsed into ints representing difficulty
        {
            eligibleEnemyPrefabs.Add(enemyPrefab);
        }
    }

    // Shuffle the spawn points array
    System.Random rng = new System.Random();
    int n = enemySpawnPoints.Count;
    while (n > 1)
    {
        n--;
        int k = rng.Next(n + 1);
        GameObject value = enemySpawnPoints[k];
        enemySpawnPoints[k] = enemySpawnPoints[n];
        enemySpawnPoints[n] = value;
    }

    // Set a limit on how many spawn points to use
    int spawnLimit = Mathf.Min(minSpawnAmount, enemySpawnPoints.Count);  // For example, use up to 5 spawn points

    // Only spawn enemies at the first 'spawnLimit' spawn points
    for (int i = 0; i < spawnLimit; i++)
    {
        GameObject spawnPoint = enemySpawnPoints[i];
        if (eligibleEnemyPrefabs.Count > 0)  // Check to avoid errors if no eligible prefabs
        {
            int enemyIndex = Random.Range(0, eligibleEnemyPrefabs.Count);  // Randomly select an enemy type from eligible list
            GameObject enemyInstance = Instantiate(eligibleEnemyPrefabs[enemyIndex], spawnPoint.transform.position, Quaternion.identity);
            currentEnemies.Add(enemyInstance);
        }
    }
}


    private bool AreEnemiesAlive()
    {
        for (int i = currentEnemies.Count - 1; i >= 0; i--)
        {
            if (currentEnemies[i] == null)
            {
                currentEnemies.RemoveAt(i);  // Remove null references (dead enemies) from the list
            }
        }
        return currentEnemies.Count > 0;  // If list is empty, all enemies are dead
    }

    // Call this method to update the difficulty scale if needed
    public void UpdateDifficultyScale(int newScale)
    {
        difficultyScale = newScale;
    }
}
