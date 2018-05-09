using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDealDamage : MonoBehaviour {

	public int damage;
	public GameObject player;

	void Start()
	{
		if (damage == 0) {damage = 10;}
		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.tag == "Enemy") 
		{
			coll.gameObject.GetComponent<Health> ().TakeDamage(damage);
			player.GetComponent<CharController> ().AddToComboBar (5);
			player.GetComponent<PlayerUIManager> ().SetComboUI (player.GetComponent<CharController> ().GetComboBar (), player.GetComponent<CharController> ().maxCombo);
		}
	}
}
