using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackManager : MonoBehaviour {

    public Attack firstLight;
    public Attack firstHeavy;
    public Attack special;

    InputController IC;
    Animator anim;
    Weapon playerWeapon;

    public float timeToResetAnimations;
    float timeSinceAnimPlayed;

    float animationFinishTime;

    int currAnimIndex;

    public int maxAnimationsInCombo;
    Queue animationQue;
    Attack recentAttack;

    int animationsInCombo;

    int comboCount;
    public int maxCombo;
	// Use this for initialization
	void Start () {

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        IC = new InputController();
        anim = GetComponent<Animator>();
        playerWeapon = GetComponentInChildren<Weapon>();

        playerWeapon.SetHitTag("Enemy");

        animationQue = new Queue();
	}
	
    public void AddToCombo(int num)
    {
        comboCount += num;

        if(comboCount > maxCombo)
        {
            comboCount = maxCombo;
        }

        GetComponent<PlayerUIManager>().SetComboUI(comboCount, maxCombo);
    }

    Attack GetStartingAttackOfType(string attackType)
    {
        Attack attToAdd = null;
        switch (attackType)
        {
            case "Light":
                attToAdd = firstLight;
                break;

            case "Heavy":
                attToAdd = firstHeavy;
                break;

            case "Special":
                attToAdd = special;
                break;
        }
        Debug.Log(attToAdd.attackType);
        return attToAdd;
    }

    void AddToAnimationQue(string attackTypeToAdd)
    {
        if(animationsInCombo < maxAnimationsInCombo)
        {
            Attack attToAdd = null;
            //Check if there is a most recent attack.
            if(recentAttack == null)
            {
                attToAdd = GetStartingAttackOfType(attackTypeToAdd);
            }else
            {
                //Get the next attack from the object
                switch(attackTypeToAdd)
                {
                    case "Light":
                        attToAdd = recentAttack.nextLight;
                        break;

                    case "Heavy":
                        attToAdd = recentAttack.nextHeavy;
                        break;

                    case "Special":
                        attToAdd = special;
                        break;
                }
            }
            //Checks incase the objects used do not have links to further attacks.
            if(attToAdd == null)
            {
                attToAdd = GetStartingAttackOfType(attackTypeToAdd);
            }

            animationQue.Enqueue(attToAdd);
            recentAttack = attToAdd;
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
            AddToAnimationQue("Light");
        }else if(IC.GetHeavy())
        {
            AddToAnimationQue("Heavy");
        }else if(IC.GetSpecial() && comboCount > 50)
        {
            AddToAnimationQue("Special");
        }

        //Play the next animation when the previous one is finished.
        if(timeSinceAnimPlayed >= animationFinishTime && animationQue.Count > 0)
        {
            Attack currAttack = (Attack)animationQue.Dequeue();
            anim.Play(currAttack.animation.name);
            playerWeapon.SetDamage(currAttack.damage);
            animationFinishTime = currAttack.animation.length;

            if (currAttack.attackType.ToLower() == "special") { comboCount -= 50; GetComponent<PlayerUIManager>().SetComboUI(comboCount, maxCombo); }

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
            recentAttack = null;
        }
	}
}
