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
	}

    //Potential classes to be used for animations

    //Trigger based animations
    void AnimationPlayOnTrigger()
    {
        anim.SetTrigger("Attack");
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
            CrossfadeAttackAnimations();
        }

        timeSinceAttackCalled += Time.deltaTime;
        anim.SetFloat("timeSinceAnimCalled", timeSinceAttackCalled);
        
	}
}
