using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAI : MonoBehaviour {

	public float moveSpeed;

	public float attackRange;

	public int startHealth;
	Animator anim;
	GameObject player;

	public GameObject mySword;
	BoxCollider swordBoxColl;

	bool attacking;
	float attackingTimer;
	// Use this for initialization
	void Start () 
	{
		anim = GetComponent<Animator> ();
		player = GameObject.FindGameObjectWithTag ("Player");

		swordBoxColl = mySword.GetComponent<BoxCollider> ();
		swordBoxColl.enabled = false;
		//Validation
		if(moveSpeed == 0){moveSpeed = 1.5f;}
		if (attackRange == 0) {attackRange = 1.25f;}
		if (startHealth == 0) {startHealth = 20;}

		this.gameObject.AddComponent<Health> ();
		this.gameObject.GetComponent<Health> ().GainHealth (startHealth);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!attacking) 
		{
			transform.position = Vector3.MoveTowards (transform.position, player.transform.position, moveSpeed * Time.deltaTime);
			transform.LookAt (player.transform.position);

			float dist = Vector3.Distance (transform.position, player.transform.position);
			if (dist <= attackRange) 
			{
				attacking = true;
				anim.Play ("Attack");
				attackingTimer = 1.5f;
				swordBoxColl.enabled = true;
			}
		} else 
		{
			attackingTimer -= Time.deltaTime;
			if (attackingTimer <= 0) {attacking = false;}
			swordBoxColl.enabled = false;
		}



	}
}
