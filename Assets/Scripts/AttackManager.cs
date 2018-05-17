﻿using System.Collections;
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

    float animationFinishTime;

    int currAnimIndex;

    public int maxAnimationsInCombo;
    Queue animationQue;
    int animationsInCombo;
	// Use this for initialization
	void Start () {

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        IC = new InputController();
        anim = GetComponent<Animator>();
        playerWeapon = GetComponentInChildren<Weapon>();

        animationQue = new Queue();
	}
	
    void AddToAnimationQue(Attack[] attackToAdd)
    {
        if(animationsInCombo < maxAnimationsInCombo)
        {
            if (currAnimIndex >= attackToAdd.Length) { currAnimIndex = 0; }

            animationQue.Enqueue(attackToAdd[currAnimIndex]);
            currAnimIndex++;
            animationsInCombo++;
        }
    }

	// Update is called once per frame
	void Update ()
    {
        IC.UpdateCombatInputs();
        timeSinceAnimPlayed += Time.deltaTime;

        //Takes the inputs and then looks to add a new item to the que.
        if(IC.GetLight())
        {
            AddToAnimationQue(lightAttacks);
        }else if(IC.GetHeavy())
        {
            AddToAnimationQue(heavyAttacks);
        }else if(IC.GetSpecial())
        {
            AddToAnimationQue(specialAttacks);
        }

        //Play the next animation when the previous one is finished.
        if(timeSinceAnimPlayed >= animationFinishTime && animationQue.Count > 0)
        {
            Attack currAttack = (Attack)animationQue.Dequeue();
            anim.Play(currAttack.animation.name);
            playerWeapon.SetDamage(currAttack.damage);
            animationFinishTime = currAttack.animation.length;

            timeSinceAnimPlayed = 0;
        }else if(timeSinceAnimPlayed >= animationFinishTime && animationQue.Count == 0)
        {
            animationsInCombo = 0;
            animationFinishTime = 0;
        }

        if(timeSinceAnimPlayed > timeToResetAnimations)
        {
            currAnimIndex = 0;
            timeSinceAnimPlayed = 0;
            animationFinishTime = 0;
        }
	}
}
