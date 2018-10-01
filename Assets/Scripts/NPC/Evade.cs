using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evade : Flee
{

    public float maxPrediction;
    private GameObject targetAux;
    private Agent targetAgent;

    public override void Awake()
    {
        base.Awake();
        targetAgent = target.GetComponent<Agent>();
        targetAux = target;
        target = new GameObject();
    }

    void OnDestroy()
    {
        Destroy(targetAux);
    }

    public override Steering GetSteering()
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
        return base.GetSteering();
    }
}
