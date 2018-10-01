using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : Align
{

    protected GameObject targetAux;

    public override void Awake()
    {
        base.Awake();

        targetAux = target;
        target = new GameObject();
        target.AddComponent<Agent>();
    }

    void OnDestroy()
    {
        Destroy(target);
    }

    public override Steering GetSteering()
    {
        Vector3 dir = targetAux.transform.position - transform.position;
        if(dir.magnitude > 0.0f)
        {
            float targetOrient = Mathf.Atan2(dir.x, dir.z);
            targetOrient *= Mathf.Rad2Deg;
            target.GetComponent<Agent>().orientation = targetOrient;
        }

        return base.GetSteering();
    }

}
