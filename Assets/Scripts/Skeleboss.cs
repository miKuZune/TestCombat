using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
public class Skeleboss : MonoBehaviour {

    public Attack[] bossAttacks;

    public float distToAttack;

    GameObject player;

    bool isAttacking;

    Weapon[] hits;

    Animator anim;

    float resetTime;

    public GameObject head;

    public int health;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        hits = gameObject.GetComponentsInChildren<Weapon>();
        anim = GetComponent<Animator>();

        Health h = gameObject.AddComponent<Health>();
        h.Initalize(health, 1);

        isAttacking = false;
	}
	
    void RotateTowardObj(GameObject toRotate)
    {
        Debug.Log("rotating");
        Vector3 targetDir = player.transform.position - toRotate.transform.position;

        float step = 5 * Time.deltaTime;
        Vector3 newDir = Vector3.RotateTowards(Vector3.forward, targetDir, step, 0.0f);

        transform.rotation = Quaternion.LookRotation(newDir);
    }

    bool PlayerCloseEnough()
    {
        float currDist = Vector3.Distance(transform.position, player.transform.position);
        if(currDist <= distToAttack)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void LookAtPlayer()
    {
        transform.LookAt(player.transform);
    }

    Attack ChooseAttack()
    {
        System.Random random = new System.Random();
        int randNum = random.Next(0, bossAttacks.Length);

        return bossAttacks[randNum];
    }

	// Update is called once per frame
	void Update ()
    {
		if(!isAttacking)
        {
            RotateTowardObj(transform.gameObject);

            if(PlayerCloseEnough())
            {
                isAttacking = true;
                Attack newAtt = ChooseAttack();

                foreach(Weapon wep in hits)
                {
                    wep.SetDamage(newAtt.damage);
                    wep.SetHitTag("Player");
                }
                anim.Play(newAtt.animation.name);
                resetTime = newAtt.animation.length + 2.5f;
            }
        }
        else
        {
            RotateTowardObj(head);
            resetTime -= Time.deltaTime;
            if(resetTime <= 0)
            {
                isAttacking = false;
            }
        }
	}
}
