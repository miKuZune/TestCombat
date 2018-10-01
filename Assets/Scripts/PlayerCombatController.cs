using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour {

    Animator anim;

    InputController IC;

    int attackCounter;

    float timeSinceAttackCalled;

    Weapon wep;

	// Use this for initialization
	void Start ()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        anim = GetComponent<Animator>();
        IC = new InputController();

        wep = GetComponentInChildren<Weapon>();
        wep.SetDamage(20);
        wep.SetHitTag("Agent");
	}

    //Potential classes to be used for animations

    //Trigger based animations
    void AnimationPlayOnTrigger(int animType)
    {
        switch (animType)
        {
            case 0:
                anim.SetTrigger("Attack");
                wep.SetDamage(10);
                break;

            case 1:
                anim.SetTrigger("heavyAttack");
                wep.SetDamage(40);
                break;
        }
        timeSinceAttackCalled = 0;
    }

    //Crossfade based animations
    void CrossfadeAttackAnimations()
    {
        attackCounter++;
        anim.CrossFade("L_Att_" + attackCounter, 0.5f);
        timeSinceAttackCalled = 0;
        if (attackCounter >= 3) { attackCounter = 0; }
    }
	
	// Update is called once per frame
	void Update ()
    {
        IC.UpdateCombatInputs();

        if (IC.GetLight())
        {
            AnimationPlayOnTrigger(0);
        }

        else if(IC.GetHeavy())
        {
            AnimationPlayOnTrigger(1);
        }

        timeSinceAttackCalled += Time.deltaTime;
        anim.SetFloat("timeSinceAnimCalled", timeSinceAttackCalled);
        
	}
}
