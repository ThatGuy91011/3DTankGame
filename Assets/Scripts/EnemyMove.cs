using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public Transform tf;
   
    void Start()
    {
        tf = GetComponent<Transform>();
    }

    //Make the enemy tank spin continuously
    void Update()
    {
        tf.Rotate(0f, 2f, 0f);
    }
}
