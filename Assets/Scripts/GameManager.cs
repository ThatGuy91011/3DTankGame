using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject instantiatedPlayerTank;
    public GameObject playerTankPrefab;
    public GameObject icamera;
    public GameObject camera;
    public GameObject[] enemyTanks;
    public List<GameObject> playerSpawnPoints;
    public List<GameObject> instantiatedEnemyTanks;
    public List<GameObject> enemySpawnPoints;
    // Runs before any Start() functions run
    void Awake()
    {
        //Makes sure there is only one GameManager
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("ERROR: There can only be one GameManager.");
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }


    public void SpawnPlayer(GameObject spawnpoint)
    {
        instantiatedPlayerTank = Instantiate(playerTankPrefab, spawnpoint.transform.position, Quaternion.identity);
        SpawnCamera(spawnpoint);
    }

    public void SpawnCamera(GameObject spawnpoint)
    {
        icamera = Instantiate(camera, new Vector3(spawnpoint.transform.position.x, 52, spawnpoint.transform.position.z),
            new Quaternion(45, 0, 0, 45));
    }
    public GameObject RandomSpawn(List<GameObject> playerSpawnPoints)
    {
        int spawnToGet = UnityEngine.Random.Range(0, playerSpawnPoints.Count - 1);
        return playerSpawnPoints[spawnToGet];
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < enemyTanks.Length; ++i)
        {
            GameObject instantiatedEnemyTank = Instantiate(enemyTanks[i], RandomSpawn(enemySpawnPoints).transform.position, Quaternion.identity);
            instantiatedEnemyTanks.Add(instantiatedEnemyTank);
        }
    }
}
