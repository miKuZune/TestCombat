using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BasicAI : MonoBehaviour {

    public int startHealth;
    public int damage;
    public float moveSpeed;

    GameObject player;
    NavMeshAgent AIagent;
    Weapon AIweapon;

    public float distToAttack;

    int currState;

    float timer;

    
	// Use this for initialization
	void Start ()
    {
        Health h = gameObject.AddComponent<Health>();
        h.Initalize(startHealth, 1);
        AIagent = gameObject.AddComponent<NavMeshAgent>();
        AIagent.speed = moveSpeed;

        AIweapon = GetComponentInChildren<Weapon>();
        AIweapon.SetDamage(damage);
        AIweapon.SetHitTag("Player");

        player = GameObject.FindGameObjectWithTag("Player");
	}
	
    bool InAttackRange()
    {
        float distToPlayer = Vector3.Distance(transform.position, player.transform.position);
        if(distToPlayer < distToAttack)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Attack()
    {
        currState = 1;
        timer = 0;
        AIagent.isStopped = true;
        //Idk play an animation
        GetComponent<Animator>().Play("Attack");
    }

	// Update is called once per frame
	void Update ()
    {

        switch(currState)
        {
            case 0:
                AIagent.destination = player.transform.position;
                if(InAttackRange())
                {
                    Attack();
                }
                break;
            case 1:
                timer += Time.deltaTime;
                //Replace this with the time to perform the animation.
                if(timer > 1.5f)
                {
                    currState = 0;
                    AIagent.isStopped = false;
                }
                break;
        }
	}
}
