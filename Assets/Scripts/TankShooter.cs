using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class TankShooter : MonoBehaviour
{
    public GameObject cannonball;

    public GameObject firePoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Shoot()
    {
        GameObject newestCannonball;
        newestCannonball = Instantiate(cannonball, firePoint.transform);
        //newestCannonball.SendMessage("ApplyForce", Force);
    }
}
