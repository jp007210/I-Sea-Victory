using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Organize os prefabs em pares (0-1, 2-3, 4-5...)
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
        if (player == null || enemyPrefabs.Length < 2) return;

        int index = EnemyManager.Instance.GetCurrentIndex();
        int baseIndex = index * 2;

        // Garante que não vá além do array
        int index1 = Mathf.Clamp(baseIndex, 0, enemyPrefabs.Length - 1);
        int index2 = Mathf.Clamp(baseIndex + 1, 0, enemyPrefabs.Length - 1);

        // Aleatoriamente escolhe entre os dois tipos dessa fase
        GameObject selectedEnemy = (Random.value < 0.5f) ? enemyPrefabs[index1] : enemyPrefabs[index2];

        Vector3 spawnPos;
        do
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(minDistance, spawnRadius);
            spawnPos = new Vector3(randomCircle.x, 0, randomCircle.y) + player.position;
        }
        while (Vector3.Distance(spawnPos, player.position) < minDistance);

        Instantiate(selectedEnemy, spawnPos, Quaternion.identity);
    }

    int CountEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}