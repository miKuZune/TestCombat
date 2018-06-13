using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Attack", menuName = "Attack")]
public class Attack : ScriptableObject
{
    public string attackType;

    public AnimationClip animation;

    public int damage;

    public Attack nextLight;
    public Attack nextHeavy;

    public void GetAnimationTimeFromAnimation()
    {
        Debug.Log(animation.length);
    }
}
