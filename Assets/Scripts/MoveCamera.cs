using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform playertf;

    public Transform tf;
    // Start is called before the first frame update
    void Start()
    {
        playertf = GameObject.Find("Player").GetComponent<Transform>();
        tf = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playertf.position.z > tf.position.z + 25)
        {
            tf.position += new Vector3(0,0,50);
        }
        else if (playertf.position.z < tf.position.z - 25)
        {
            tf.position += new Vector3(0, 0, -50);
        }

        if (playertf.position.x > tf.position.x + 25)
        {
            tf.position += new Vector3(50, 0,0 );
        }
        else if (playertf.position.x < tf.position.x - 25)
        {
            tf.position += new Vector3(-50, 0, 0);
        }
    }
}
