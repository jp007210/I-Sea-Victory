using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance;

    public GameObject[] enemyPrefabs; // Organize os prefabs em pares (0-1, 2-3, 4-5...)
    public Transform player;
    public float spawnRadius = 30f;
    public float minDistance = 10f;
    public float spawnInterval = 5f;
    public int maxEnemies = 10;

    private float timer;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("EnemySpawner: Player com tag 'Player' não encontrado na cena.");
            }
        }
    }

    void Update()
    {
        // Ignora se não for a fase de inimigos
        if (EnemyManager.Instance != null && !EnemyManager.Instance.IsEnemyStage())
            return;

        // Tenta encontrar o player se ainda não tiver
        if (player == null)
        {
            GameObject found = GameObject.FindWithTag("Player");
            if (found != null)
            {
                player = found.transform;
                Debug.Log("EnemySpawner: Player encontrado automaticamente.");
            }
            return; // enquanto não tiver player, não spawna
        }

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

        int index1 = Mathf.Clamp(baseIndex, 0, enemyPrefabs.Length - 1);
        int index2 = Mathf.Clamp(baseIndex + 1, 0, enemyPrefabs.Length - 1);

        GameObject selectedEnemy = (Random.value < 0.5f) ? enemyPrefabs[index1] : enemyPrefabs[index2];

        Vector3 spawnPos;
        UnityEngine.AI.NavMeshHit hit;

        for (int attempt = 0; attempt < 10; attempt++)
        {
            Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(minDistance, spawnRadius);
            Vector3 tentativePos = new Vector3(randomCircle.x, 0, randomCircle.y) + player.position;

            // Garante ponto válido na NavMesh
            if (UnityEngine.AI.NavMesh.SamplePosition(tentativePos, out hit, 2f, UnityEngine.AI.NavMesh.AllAreas))
            {
                spawnPos = hit.position;

                // ⬆️ Ajusta altura manualmente após achar ponto válido na NavMesh
                spawnPos.y = 1.5f;

                GameObject enemy = Instantiate(selectedEnemy, spawnPos, Quaternion.identity);

                // Ajusta a posição real do inimigo sem quebrar o navMeshAgent
                if (enemy.TryGetComponent(out UnityEngine.AI.NavMeshAgent agent))
                {
                    agent.Warp(spawnPos); // força o posicionamento na nova altura
                }

                return;
            }
        }

        Debug.LogWarning("Não foi possível encontrar uma posição válida na NavMesh para spawnar inimigo.");
    }

    int CountEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}