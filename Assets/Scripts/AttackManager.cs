using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour {

    public Attack[] lightAttacks;
    public Attack[] heavyAttacks;
    public Attack[] specialAttacks;

    InputController IC;
    Animator anim;
    Weapon playerWeapon;

    public float timeToResetAnimations;
    float timeSinceAnimPlayed;

    int currAnimIndex;
	// Use this for initialization
	void Start () {
        IC = new InputController();
        anim = GetComponent<Animator>();
        playerWeapon = GetComponentInChildren<Weapon>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        IC.UpdateCombatInputs();
        timeSinceAnimPlayed += Time.deltaTime;

        if(IC.GetLight())
        {
            if (currAnimIndex >= lightAttacks.Length) { currAnimIndex = 0; }

            playerWeapon.SetDamage(lightAttacks[currAnimIndex].damage);

            anim.Play(lightAttacks[currAnimIndex].animation.name);
            timeSinceAnimPlayed = 0;
            currAnimIndex++;
        }else if(IC.GetHeavy())
        {
            if (currAnimIndex >= heavyAttacks.Length) { currAnimIndex = 0; }

            playerWeapon.SetDamage(heavyAttacks[currAnimIndex].damage);

            anim.Play(heavyAttacks[currAnimIndex].animation.name);
            timeSinceAnimPlayed = 0;
            currAnimIndex++;
        }else if(IC.GetSpecial())
        {
            if (currAnimIndex >= specialAttacks.Length) { currAnimIndex = 0; }

            playerWeapon.SetDamage(specialAttacks[currAnimIndex].damage);

            anim.Play(specialAttacks[currAnimIndex].animation.name);
            timeSinceAnimPlayed = 0;
            currAnimIndex++;
        }

        if(timeSinceAnimPlayed > timeToResetAnimations)
        {
            currAnimIndex = 0;
        }

	}
}
