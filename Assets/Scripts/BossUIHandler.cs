using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossUIHandler : MonoBehaviour {

    public GameObject bossHealth;
    public GameObject bossText;
    public GameObject boss;

    void Start()
    {
        bossHealth.SetActive(false);
        bossText.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            bossHealth.SetActive(true);
            bossText.SetActive(true);
            bossText.GetComponent<Text>().text = boss.name;
        }
    }
}
