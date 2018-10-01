using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wander : Face
{

    public float offset = 0.2f;
    public float radius = 5f;
    public float rate = 900f;

    [Header("Boundaries")]
    public bool bound;
    public Vector2 minXZ;
    public Vector2 maxXZ;

    public bool hostile;
    GameObject player;

    public override void Awake()
    {
        target = new GameObject();
        target.transform.position = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        base.Awake();
    }

    public override void Update()
    {
        base.Update();

        if(Vector3.Distance(player.transform.position, transform.position) 
            <= 7f)
        {
            Pursue myPursue = gameObject.AddComponent<Pursue>() as Pursue;
            myPursue.target = player;
            myPursue.targetAux = player;
            myPursue.chasePlayer = true;
            myPursue.minDist = 2;
            myPursue.attackStrength = 7;
            myPursue.enabled = true;
            Destroy(this);
        }

    }


    public override Steering GetSteering()
    {

        Steering steering = new Steering();
        Vector3 targetPos = Vector3.zero;
        float wanderOrient;
        float targetOrient;
        Vector3 orientVec;
        

        if(targetPos == Vector3.zero)
        {
            if (bound)
            {
                    wanderOrient = Random.Range(-1.0f, 1.0f) * rate;
                    targetOrient = wanderOrient + agent.orientation;
                    orientVec = GetOrientAsVector(agent.orientation);

                    targetPos = (offset * orientVec) + transform.position;
                    targetPos = targetPos + (GetOrientAsVector(targetOrient) * radius);
                
                    if(targetPos.x >= maxXZ.x) { targetPos.x = maxXZ.x - 1f; }
                    if(targetPos.x <= minXZ.x) { targetPos.x = minXZ.x + 1f; }
                    if(targetPos.z >= maxXZ.y) { targetPos.z = maxXZ.y - 1f; }
                    if(targetPos.z <= minXZ.y) { targetPos.z = minXZ.y + 1f; }
            }
            else
            {


                wanderOrient = Random.Range(-1.0f, 1.0f) * rate;
                targetOrient = wanderOrient + agent.orientation;
                orientVec = GetOrientAsVector(agent.orientation);

                targetPos = (offset * orientVec) + transform.position;
                targetPos = targetPos + (GetOrientAsVector(targetOrient) * radius);
            }
        }
        
        
            targetAux.transform.position = targetPos;

            steering = base.GetSteering();
            steering.linear = targetAux.transform.position - transform.position;
            steering.linear.Normalize();
            steering.linear *= agent.maxAccel;
            return steering;
        

        
    }
}
