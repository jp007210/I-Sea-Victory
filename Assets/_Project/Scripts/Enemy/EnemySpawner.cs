using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;    // Array com vários tipos de inimigos
    public Transform player;
    public float spawnRadius = 30f;
    public float minDistance = 10f;
    public float spawnInterval = 5f;
    public int maxEnemies = 10;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            if (CountEnemies() < maxEnemies)
            {
                SpawnEnemy();
            }

            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        if (player == null || enemyPrefabs.Length == 0) return;

        Vector3 spawnPos;

        do
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(minDistance, spawnRadius);
            spawnPos = new Vector3(randomCircle.x, 0, randomCircle.y) + player.position;
        }
        while (Vector3.Distance(spawnPos, player.position) < minDistance);

        // Escolhe um prefab aleatório para spawnar
        int index = Random.Range(0, enemyPrefabs.Length);
        Instantiate(enemyPrefabs[index], spawnPos, Quaternion.identity);
    }

    int CountEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}