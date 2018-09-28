using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public int health;
    public int maxHealth;

    int deathType;

    float noDmgTime;
    const float timeToNoInvun = 0.75f;

    public void Initalize(int startHealth, int typeOfDeath)
    {
        maxHealth = startHealth;
        health = maxHealth;

        deathType = typeOfDeath;
        GetComponent<PlayerUIManager>().SetHealthUI(health, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (noDmgTime > 0) { return; }

        health -= damage;

        noDmgTime = timeToNoInvun;

        GetComponent<PlayerUIManager>().SetHealthUI(health, maxHealth);

        if(CheckIfDead())
        {
            OnDeath(deathType);
        }
    }

    bool CheckIfDead()
    {
        if(health <= 0)
        {
            return true;
        }else
        {
            return false;
        }
    }

    void DestroyOnDeath()
    {
        Destroy(gameObject);
    }

    void AnimationOnDeath()
    {
        Debug.Log("errrrr");
        Destroy(this.gameObject);
    }

    void OnPlayerDeath()
    {
        Debug.Log("Player has died");
        GameObject deathUI = GetComponent<PlayerManager>().GetDeathUIGameObject();
        deathUI.SetActive(true);

        Destroy(this.gameObject);
    }

    void OnDeath(int switchCase)
    {
        switch(switchCase)
        {
            case 0:
                DestroyOnDeath();
                break;
            case 1:
                AnimationOnDeath();
                break;
            case 2:
                OnPlayerDeath();
                break;
        } 
    }

    void Update()
    {
        noDmgTime -= Time.deltaTime;
    }
}
