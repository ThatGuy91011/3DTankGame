using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TankData))]
[RequireComponent(typeof(TankMotor))]
public class TestController : MonoBehaviour
{

    public TankMotor motor;

    public TankData data;
    // Start is called before the first frame update
    void Start()
    {
        motor = gameObject.GetComponent<TankMotor>();
        data = gameObject.GetComponent<TankData>();
    }

    // Update is called once per frame
    void Update()
    {
        motor.Move(data.moveSpeed);
        motor.Rotate(data.rotateSpeed);
    }
}
