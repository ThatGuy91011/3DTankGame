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

    void Update()
    {
        if (instantiatedPlayerTank == null)
        {
            SpawnPlayer(RandomSpawn(playerSpawnPoints));
        }
    }

    public void SpawnPlayer(GameObject spawnpoint)
    {
        instantiatedPlayerTank = Instantiate(playerTankPrefab, spawnpoint.transform.position, Quaternion.identity);
        icamera = Instantiate(camera, new Vector3(spawnpoint.transform.position.x, 52, spawnpoint.transform.position.z),
            new Quaternion(45, 0, 0, 0));
    }

    private GameObject RandomSpawn(List<GameObject> playerSpawnPoints)
    {
        int spawnToGet = UnityEngine.Random.Range(0, playerSpawnPoints.Count - 1);
        return playerSpawnPoints[spawnToGet];
    }

    public void SpawnEnemies()
    {

    }
}
