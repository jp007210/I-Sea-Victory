using UnityEngine;
using System.Collections.Generic;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Área de Spawn")]
    public Vector2 areaSize = new Vector2(100f, 100f);

    [Header("Configurações")]
    public GameObject[] obstaclePrefabs;
    public int numberOfObstacles = 20;
    public float spawnHeight = 0f;
    public float minDistanceBetweenObstacles = 3f;
    public float minDistanceFromSpawner = 10f; // ✅ distância mínima do centro do spawner
    public LayerMask obstacleLayer;

    [Header("Spawn Dinâmico (Opcional)")]
    public bool spawnOverTime = false;
    public float spawnInterval = 2f;

    private List<Vector3> spawnedPositions = new List<Vector3>();
    private float timer;

    void Start()
    {
        if (!spawnOverTime)
        {
            SpawnAllObstacles();
        }
    }

    void Update()
    {
        if (spawnOverTime)
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval && spawnedPositions.Count < numberOfObstacles)
            {
                SpawnOneObstacle();
                timer = 0f;
            }
        }
    }

    void SpawnAllObstacles()
    {
        for (int i = 0; i < numberOfObstacles; i++)
        {
            SpawnOneObstacle();
        }
    }

    void SpawnOneObstacle()
    {
        int safety = 0;
        while (safety < 30)
        {
            Vector3 randomPos = GetRandomPosition();

            // ✅ Garante distância mínima do centro do spawner
            if (Vector3.Distance(randomPos, transform.position) < minDistanceFromSpawner)
            {
                safety++;
                continue;
            }

            if (IsFarEnoughFromOthers(randomPos))
            {
                GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
                Instantiate(prefab, randomPos, Quaternion.identity);
                spawnedPositions.Add(randomPos);
                return;
            }
            safety++;
        }
    }

    Vector3 GetRandomPosition()
    {
        float x = Random.Range(-areaSize.x / 2f, areaSize.x / 2f);
        float z = Random.Range(-areaSize.y / 2f, areaSize.y / 2f);
        return new Vector3(x, spawnHeight, z) + transform.position;
    }

    bool IsFarEnoughFromOthers(Vector3 pos)
    {
        foreach (var p in spawnedPositions)
        {
            if (Vector3.Distance(pos, p) < minDistanceBetweenObstacles)
                return false;
        }

        Collider[] colliders = Physics.OverlapSphere(pos, minDistanceBetweenObstacles, obstacleLayer);
        return colliders.Length == 0;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(areaSize.x, 0.1f, areaSize.y));

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minDistanceFromSpawner); // ✅ visualização da distância mínima
    }
}