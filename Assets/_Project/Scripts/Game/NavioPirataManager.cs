using UnityEngine;

public class NavioPirataManager : MonoBehaviour
{
    public int requiredScore = 100;
    public Transform arenaSpawnPoint;
    public GameObject boss;
    public GameObject arenaWalls;

    private bool bossFightStarted = false;

    void Update()
    {

    }
  
    void StartBossFight()
    {
        bossFightStarted = true;
        arenaWalls.SetActive(true);

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = arenaSpawnPoint.position;

        boss.SetActive(true);
    }

    public void EndBossFight()
    {
        arenaWalls.SetActive(false);
    }
}