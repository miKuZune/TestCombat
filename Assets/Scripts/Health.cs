using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

	public int health;
	public int maxHealth;


	bool DMGedRecently = false;
	float invunrabilityTime;

	public void SetHealth(int toSetTo)
	{
		health = toSetTo;
	}



	public void TakeDamage(int damage)
	{
		health -= damage;
		Debug.Log (health + " " + maxHealth);


		if (this.GetComponent<PlayerUIManager> () != null) {
			this.GetComponent<PlayerUIManager> ().SetHealthUI (health, maxHealth);
		}

		CheckForDeath ();
		invunrabilityTime = 0.5f;
	}

	public void GainHealth(int gain)
	{
		health += gain;
	}

	void CheckForDeath()
	{
		if (health <= 0) 
		{
			Destroy (this.gameObject);
		}
	}

	void Update()
	{
		if (DMGedRecently) 
		{
			invunrabilityTime -= Time.deltaTime;
			if (invunrabilityTime <= 0) {DMGedRecently = false;}
		}
	}
}
