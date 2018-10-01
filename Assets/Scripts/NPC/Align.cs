using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align : AgentBehaviour
{

    public float targetRadius;
    public float slowRadius;
    public float timeToTarget = 0.1f;

    public override Steering GetSteering()
    {
        Steering steering = new Steering();
        float targetOrient = target.GetComponent<Agent>().orientation;
        float rot = targetOrient - agent.orientation;
        rot = MapToRange(rot);
        float rotSize = Mathf.Abs(rot);
        if(rotSize < targetRadius)
        {
            return steering;
        }
        float targetRotation;
        if(rotSize > slowRadius)
        {
            targetRotation = agent.maxRot;
        }else
        {
            targetRotation = agent.maxRot * rotSize / slowRadius;
        }
        targetRotation *= rot / rotSize;
        steering.angular = targetRotation - agent.rotation;
        steering.angular /= timeToTarget;
        float angularAccel = Mathf.Abs(steering.angular);
        if(angularAccel > agent.maxAngularAccel)
        {
            steering.angular /= angularAccel;
            steering.angular *= agent.maxAngularAccel;
        }
        return steering;
    }

}
