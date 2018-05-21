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

    bool seenPlayer;
    
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

        currState = int.MaxValue;
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

    bool CanSeePlayer()
    {
        bool canSee = false;

        Vector3 dirToPlayer = player.transform.position - transform.position;
        dirToPlayer.Normalize();

        float distToPlayer = Vector3.Distance(transform.position, player.transform.position) + 1;

        RaycastHit hit;
        
        if(Physics.Raycast(transform.position, dirToPlayer, out hit, Mathf.Infinity))
        {
            if(hit.transform.tag == "Player")
            {
                canSee = true;
            }
        }

        return canSee;
    }

    void Attack()
    {
        currState = 1;
        timer = 0;
        AIagent.isStopped = true;

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

            default:
                if (CanSeePlayer()) { currState = 1; }
                break;
        }
	}
}
