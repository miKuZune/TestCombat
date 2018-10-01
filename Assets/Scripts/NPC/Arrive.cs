using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : AgentBehaviour
{

    public float targetRadius;
    public float slowRadius;
    public float timeToTarget = 0.1f;

    public override Steering GetSteering()
    {
        Steering steering = new Steering();
        Vector3 dir = target.transform.position - transform.position;
        float dist = dir.magnitude;
        float targetSpeed;
        if(dist < targetRadius)
        {
            return steering;
        }
        if(dist > slowRadius)
        {
            targetSpeed = agent.maxSpeed;
        }
        else
        {
            targetSpeed = agent.maxSpeed * dist / slowRadius;
        }

        Vector3 desiredVel = dir;
        desiredVel.Normalize();
        desiredVel *= targetSpeed;
        steering.linear = desiredVel - agent.velocity;
        steering.linear /= timeToTarget;
        if(steering.linear.magnitude > agent.maxAccel)
        {
            steering.linear.Normalize();
            steering.linear *= agent.maxAccel;
        }
        return steering;
    }
}
