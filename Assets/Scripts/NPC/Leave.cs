using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leave : AgentBehaviour
{
    public float escapeRadius;
    public float dangerRadius;
    public float timeToTarget = 0.1f;

	public override Steering GetSteering()
    {
        Steering steering = new Steering();
        Vector3 dir = transform.position - target.transform.position;
        float dist = dir.magnitude;
        if(dist > dangerRadius)
        {
            return steering;
        }
        float reduce;
        if(dist < escapeRadius)
        {
            reduce = 0f;
        }else
        {
            reduce = dist / dangerRadius * agent.maxSpeed;
        }
        float targetSpeed = agent.maxSpeed - reduce;

        Vector3 desiredVel = dir;
        desiredVel.Normalize();
        desiredVel *= targetSpeed;
        steering.linear = desiredVel - agent.velocity;
        steering.linear /= timeToTarget;
        if (steering.linear.magnitude > agent.maxAccel)
        {
            steering.linear.Normalize();
            steering.linear *= agent.maxAccel;
        }
        return steering;
    }
}
