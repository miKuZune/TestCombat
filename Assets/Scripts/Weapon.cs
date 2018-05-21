using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {
    int damage;

    string hitTag;

    public void SetHitTag(string newHitTag)
    {
        hitTag = newHitTag;
    }

    public void SetDamage(int newDmg)
    {
        damage = newDmg;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == hitTag)
        {
            other.GetComponent<Health>().TakeDamage(damage);
        }
    }
}
