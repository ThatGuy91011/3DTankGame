using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannonball : MonoBehaviour
{
    public float damage;
    public float force;
    private Rigidbody rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    public void ApplyForce(float force)
    {
        rb.AddForce(transform.forward * force);
    }

    void Update()
    {
        //After set amount of time destroy cannonball

    }

    void OnCollisionEnter3D()
    {

    }
        
}
