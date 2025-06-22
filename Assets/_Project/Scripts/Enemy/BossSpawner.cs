using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public GameObject[] bossPrefabs;

    void Start()
    {
        int index = EnemyManager.Instance.GetCurrentIndex();
        int clampedIndex = Mathf.Clamp(index, 0, bossPrefabs.Length - 1);
        GameObject boss = Instantiate(bossPrefabs[clampedIndex], Vector3.zero, Quaternion.identity);
    }
}