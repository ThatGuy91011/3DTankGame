using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
using UnityEngine;

[RequireComponent(typeof(TankData))]
public class SimpleAIController4 : MonoBehaviour
{
    public enum AIState { Chase, ChaseAndFire, CheckForFlee, Flee, Rest };
    public AIState aiState = AIState.Chase;
    public enum Personalities { Inky, Pinky, Blinky, Clyde };

    public Personalities personality = Personalities.Inky;
    public float stateEnterTime;
    public float aiSenseRadius;
    public float restingHealRate = 25f; // in hp/second 
    public GameObject player;
    public TankData data;
    // Start is called before the first frame update
    void Start()
    {
        data = GetComponent<TankData>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
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
                Shoot();
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
                if (playerIsInRange())
                {
                    ChangeState(AIState.Flee);
                }
                else
                {
                    ChangeState(AIState.Rest);
                }
                break;
            case AIState.Flee:
                Flee(player);
                //Wait 30 seconds for flee
                if (Time.time >= (stateEnterTime * 30f))
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

    private void Flee(GameObject target)
    {
        throw new NotImplementedException();
    }

    private void Shoot()
    {
        throw new NotImplementedException();
    }

    private bool playerIsInRange()
    {
        return true;
    }

    private void Pinky()
    {
        throw new NotImplementedException();
    }

    private void Blinky()
    {
        throw new NotImplementedException();
    }

    private void Clyde()
    {
        throw new NotImplementedException();
    }

    public void Chase(GameObject target)
    {

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
