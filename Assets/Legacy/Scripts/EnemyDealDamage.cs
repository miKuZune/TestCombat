using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDealDamage : MonoBehaviour {

	void OnTriggerEnter(Collider coll)
	{
		if (coll.gameObject.tag == "Player") 
		{
			coll.gameObject.GetComponent<Health> ().TakeDamage (3);
		}
	}
}
