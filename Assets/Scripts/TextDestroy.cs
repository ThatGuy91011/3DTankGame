using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextDestroy : MonoBehaviour
{
    //Simple code to destroy text telling player who is who
    void Start()
    {
        StartCoroutine(Despawn());
    }

    IEnumerator Despawn()
    {
        yield return new WaitForSecondsRealtime(3);
        Destroy(this.gameObject);
    }
}
