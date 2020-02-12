using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

[RequireComponent(typeof(TankData))]
public class SimpleAIController4 : MonoBehaviour
{
    private Transform tf;
    public enum AIState { Chase, ChaseAndFire, CheckForFlee, Flee, Rest };
    public AIState aiState = AIState.Chase;
    public enum Personalities { Inky, Pinky, Blinky, Clyde };

    public Personalities personality = Personalities.Inky;
    public float stateEnterTime;
    public float aiSenseRadius;
    public float restingHealRate; // in hp/second 
    public GameObject player;
    public TankData data;
    public TankMotor motor;

    public float hearingDistance = 25f;

    public float FOV = 45f;

    public float inSight = 10f;
    public float distance;

    public Transform[] waypoints;
    private int currentWaypoint = 0;
    public enum LoopType { Stop, Loop, PingPong };
    public LoopType loopType = LoopType.Stop;
    private float closeEnough = 1.0f;
    private bool isPatrolForward = true;




    public enum AttackMode { Chase, Flee };
    public AttackMode attackMode;
    public Transform target;
    public float fleeDistance = 1.0f;



    public enum AvoidanceStage { None, Rotate, Move };
    public AvoidanceStage avoidanceStage;
    public float avoidanceTime = 2.0f;
    private float exitTime;
    // Start is called before the first frame update
    void Start()
    {
        tf = GetComponent<Transform>();
        data = GetComponent<TankData>();
        motor = GetComponent<TankMotor>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(player.GetComponent<Transform>().position, tf.position);
        switch (personality)
        {
            case Personalities.Inky:
                Inky();
                break;
            case Personalities.Pinky:
                Pinky();
                break;
            case Personalities.Blinky:
                Blinky();
                break;
            case Personalities.Clyde:
                Clyde();
                break;
        }
    }

   

    private void Flee(GameObject target)
    {
        // The vector from ai to target is target position minus our position.
        Vector3 vectorToTarget = target.GetComponent<Transform>().position - tf.position;
        // We can flip this vector by -1 to get a vector AWAY from our target
        Vector3 vectorAway = vectorToTarget * -1;
        // Now, we can normalize that vector to give it a magnitude of 1
        vectorAway.Normalize();
        // A normalized vector can be multiplied by a length to make a vector of that length.
        vectorAway *= fleeDistance;
        // We can find the position in space we want to move to by adding our vector away from our AI to our AI's position.
        //This gives us a point that is "that vector away" from our current position.
        Vector3 fleePosition = vectorAway + tf.position;
        motor.RotateTowards(fleePosition, data.rotateSpeed);
        motor.Move(data.moveSpeed);
    }

    private bool playerIsInRange()
    {
        if (distance < 5f)
        {
            return true;
        }
        else
        {
            Rest();
            return false;
        }
            

    }
    private void Inky()
    {
        switch (aiState)
        {
            case AIState.Chase:
                //State behaviors
                Chase(player);
                //Check for transitions in order of priority
                if (data.health < (data.maxHealth * .5))
                {
                    ChangeState(AIState.CheckForFlee);
                }
                else if (playerIsInRange())
                {
                    ChangeState(AIState.ChaseAndFire);
                }
                break;
            case AIState.ChaseAndFire:
                Chase(player);
                if (data.health < (data.maxHealth * .5))
                {
                    ChangeState(AIState.CheckForFlee);
                }
                else if (!playerIsInRange())
                {
                    ChangeState(AIState.Chase);
                }
                break;
            case AIState.CheckForFlee:
                if (!playerIsInRange())
                {
                    ChangeState(AIState.Rest);
                }
                else
                {
                    ChangeState(AIState.Flee);
                }
                break;
            case AIState.Flee:
                Flee(player);
                //Wait 30 seconds for flee
                if (Time.time >= (stateEnterTime * 3f))
                {
                    ChangeState(AIState.CheckForFlee);
                }
                break;
            case AIState.Rest:
                Rest();
                if (playerIsInRange())
                {
                    ChangeState(AIState.Flee);
                }

                else if (Mathf.Approximately(data.health, data.maxHealth))
                {
                    ChangeState(AIState.Chase);
                }
                break;
            default:
                break;
        }
    }
    private void Pinky()
    {
        if (motor.RotateTowards(waypoints[currentWaypoint].position, data.rotateSpeed))
        {
            // Do nothing!
        }
        else
        {
            // Move forward
            motor.Move(data.moveSpeed);
        }
        // If we are close to the waypoint,
        if (Vector3.SqrMagnitude(waypoints[currentWaypoint].position - tf.position) < (closeEnough * closeEnough))
        {
            switch (loopType)
            {
                case LoopType.Stop:

                    // Advance to the next waypoint, if we are still in range
                    if (currentWaypoint < waypoints.Length - 1)
                    {
                        currentWaypoint++;
                    }

                    break;

                case LoopType.Loop:
                    if (currentWaypoint < waypoints.Length - 1)
                    {
                        currentWaypoint++;
                    }
                    else
                    {
                        currentWaypoint = 0;
                    }

                    break;
                case LoopType.PingPong:
                    if (isPatrolForward)
                    {
                        // Advance to the next waypoint, if we are still in range
                        if (currentWaypoint < waypoints.Length - 1)
                        {
                            currentWaypoint++;
                        }
                        else
                        {
                            //Otherwise reverse direction and decrement our current waypoint
                            isPatrolForward = false;
                            currentWaypoint--;
                        }
                    }
                    else
                    {
                        // Advance to the next waypoint, if we are still in range
                        if (currentWaypoint > 0)
                        {
                            currentWaypoint--;
                        }
                        else
                        {
                            //Otherwise reverse direction and decrement our current waypoint
                            isPatrolForward = true;
                            currentWaypoint++;
                        }
                    }

                    break;
                default:
                    Debug.LogError("LoopType not implemented.");
                    break;
            }
        }
    }

    private void Blinky()
    {
        switch (attackMode)
        {
            case AttackMode.Chase:
                Chase(player);
                break;
            case AttackMode.Flee:
                Flee(player);
                break;
            default:
                Debug.LogError("Attack Mode not implemented");
                break;
        }
    }

    private void Clyde()
    {
        if (attackMode == AttackMode.Chase)
        {
            if (avoidanceStage != AvoidanceStage.None)
            {
                Avoid();
            }
            else
            {
                Chase(player);
            }
        }
    }

    private void Avoid()
    {
        switch (avoidanceStage)
        {
            case AvoidanceStage.Rotate:
                motor.Rotate(data.rotateSpeed);
                if (CanMove(data.moveSpeed))
                {
                    avoidanceStage = AvoidanceStage.Move;
                    exitTime = avoidanceTime;
                }
                break;
            case AvoidanceStage.Move:
                if (CanMove(data.rotateSpeed))
                {
                    exitTime -= Time.deltaTime;
                    motor.Move(data.moveSpeed);

                    if (exitTime <= 0f)
                    {
                        avoidanceStage = AvoidanceStage.None;
                    }
                }
                else
                {
                    avoidanceStage = AvoidanceStage.Rotate;
                }
                break;
        }
    }

    public bool CanMove(float speed)
    {
        RaycastHit hit;
        if (Physics.Raycast(tf.position, tf.forward, out hit, speed))
        {
            // ... and if what we hit is not the player...
            if (!hit.collider.CompareTag("Player"))
            {
                // ... then we can't move
                return false;
            }
        }

        // otherwise, return true
        return true;
    }

    public void Chase(GameObject target)
    {
        target = GameObject.Find("Player");
        //Rotate towards player
        motor.RotateTowards(target.GetComponent<Transform>().position, data.rotateSpeed);
        //Move towards player
        motor.Move(data.moveSpeed);
    }
    public void CheckForFlee()
    {
        // TODO: Write the CheckForFlee state.
    }

    public void Rest()
    {
        // Increase our health. Remember that our increase is "per second"!
        data.health += restingHealRate * Time.deltaTime;

        // But never go over our max health
        data.health = Mathf.Min(data.health, data.maxHealth);
    }

    public void ChangeState(AIState newState)
    {

        // Change our state
        aiState = newState;

        // save the time we changed states
        stateEnterTime = Time.time;
    }
}
