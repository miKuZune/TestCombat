using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatController : MonoBehaviour {

    Animator anim;

    InputController IC;

    int attackCounter;

    float timeSinceAttackCalled;

	// Use this for initialization
	void Start ()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        anim = GetComponent<Animator>();
        IC = new InputController();

        Weapon wep = GetComponentInChildren<Weapon>();
        wep.SetDamage(10);
        wep.SetHitTag("Enemy");
	}

    //Potential classes to be used for animations

    //Trigger based animations
    void AnimationPlayOnTrigger()
    {
        anim.SetTrigger("Attack");
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
            AnimationPlayOnTrigger();
        }

        timeSinceAttackCalled += Time.deltaTime;
        anim.SetFloat("timeSinceAnimCalled", timeSinceAttackCalled);
        
	}
}
