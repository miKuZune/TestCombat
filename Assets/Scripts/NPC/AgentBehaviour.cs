using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentBehaviour : MonoBehaviour
{

    public GameObject target;
    protected Agent agent;
    public float maxSpeed;
    public float maxAccel;
    public float maxRot;
    public float maxAngularAccel;

    public float weight = 1.0f;

    public virtual void Awake()
    {
        agent = gameObject.GetComponent<Agent>();
    }

    public virtual void Update()
    {
        agent.SetSteering(GetSteering(), weight);
    }

    public virtual Steering GetSteering()
    {
        return new Steering();
    }
	
    public float MapToRange (float rot)
    {
        rot %= 360.0f;
        if(Mathf.Abs(rot) > 180.0f)
        {
            if(rot < 0.0f)
            {
                rot += 360.0f;
            }else
            {
                rot -= 360.0f;
            }
        }
        return rot;
    }

    public Vector3 GetOrientAsVector (float orient)
    {
        Vector3 vector = Vector3.zero;
        vector.x = Mathf.Sin(orient * Mathf.Deg2Rad * 1.0f);
        vector.z = Mathf.Cos(orient * Mathf.Deg2Rad * 1.0f);
        return vector.normalized;
    }
}
