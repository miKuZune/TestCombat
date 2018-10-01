using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : Seek
{

    public float maxPrediction;
    public GameObject targetAux;
    private Agent targetAgent;

    public bool chasePlayer;
    bool chase;

    public float minDist;
    public int attackStrength;

    public override void Awake()
    {
        base.Awake();

        if (chasePlayer)
        {
            targetAux = GameObject.FindGameObjectWithTag("Player");
        }
        else
        {
            if (targetAgent.GetComponent<Agent>() != null)
            {
                targetAgent = target.GetComponent<Agent>();
                targetAux = target;
                target = new GameObject();
            }
        }
        
    }

    void OnDestroy()
    {
        HiveMind.playerIsAttacked = false;
        //Destroy(targetAux);
    }

    public override void Update()
    {
        base.Update();

        // FSMy stuff for now, later Util
        if(Vector3.Distance(targetAux.transform.position, transform.position) <= minDist)
        {
            if (!HiveMind.playerIsAttacked)
            {
                // stop + attack
                HiveMind.playerIsAttacked = true;
                HiveMind.heWhomstAttack = gameObject;
                chase = false;
                //Debug.Log("Stopping to attack...");
                GetComponent<Animator>().SetTrigger("Attack");
                //targetAux.GetComponent<Health>().TakeDamage(attackStrength);
            }else if (HiveMind.playerIsAttacked && HiveMind.heWhomstAttack == gameObject)
            {
                chase = false;
                //Debug.Log("Stopping to attack...");
                //targetAux.GetComponent<Health>().TakeDamage(attackStrength);
                GetComponent<Animator>().SetTrigger("Attack");
            }
            else
            {
                chase = false;
            }
        }else
        {
            //Debug.Log("Resuming chase...");
            chase = true;
        }

    }

    public void DamagePlayer()
    {
        targetAux.GetComponent<Health>().TakeDamage(attackStrength);
    }

    public override Steering GetSteering()
    {
        if (!chase)
        {
            Steering steering = new Steering();
            return steering;
        }

        if (chasePlayer)
        {
            Vector3 dir = targetAux.transform.position - transform.position;
            target.transform.position = targetAux.transform.position;
            
        }
        else
        {



            Vector3 dir = targetAux.transform.position - transform.position;
            float dist = dir.magnitude;
            float speed = agent.velocity.magnitude;
            float prediction;
            if (speed <= dist / maxPrediction)
            {
                prediction = maxPrediction;
            }
            else
            {
                prediction = dist / speed;
            }
            target.transform.position = targetAux.transform.position;
            target.transform.position += targetAgent.velocity * prediction;
        }
        return base.GetSteering();
    }
}
