using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject playerTank;

    public GameObject[] enemyTanks;

    public List<Transform> enemySpawnPoints;
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
}
