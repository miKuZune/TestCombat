using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hedgehog : Agent
{

    int wanderScore;
    int proximity;
    bool otherAttackers;

    float attackCooldown; // perhaps this should be moved to an attack behaviour

	// Use this for initialization
	void Start () {
		
	}

    public override void Update()
    {


        base.Update();
    }
}
