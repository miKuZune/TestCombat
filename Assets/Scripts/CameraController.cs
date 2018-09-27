using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float distanceFromPlayer;
    public float nonPlayerYOffset;

    public GameObject player;
    public GameObject target;

    public GameObject lookAtGO;
    public GameObject posGo;

    public float yOffset;
    public float xOffset;

    float camOffsetXZ;
    float camOffsetY;

    public float maxYOffset;
    public float minYOffset;

    InputController IC;
    
	// Use this for initialization
	void Start ()
    {
        IC = new InputController();
        camOffsetY = 0;
	}

    public bool IsLockedOn()
    {
        if (target == null) { return false; }
        else { return true; }
    }

    public GameObject GetLockOnTarget()
    {
        return target;
    }

    void FollowEnemy()
    {
        lookAtGO.transform.position = target.transform.position;


        Vector3 dirToTarget = player.transform.position - target.transform.position;
        dirToTarget.Normalize();

        Vector3 camPos = player.transform.position + (dirToTarget * 3);
        camPos.y += yOffset;
        camPos.x += xOffset;

        posGo.transform.position = camPos;
    }

    void FollowPlayer()
    {
        lookAtGO.transform.position = player.transform.position;

        Vector3 camPos = player.transform.position - (player.transform.forward * 3);
        camPos.x += xOffset;
        camPos.y += yOffset;

        posGo.transform.position = camPos;
    }

    GameObject FindClosestObj(GameObject currObj, string tagTypeToFind)
    {
        GameObject closestObj = null;
        float closestDist = Mathf.Infinity;

        GameObject[] objsWithTag = GameObject.FindGameObjectsWithTag(tagTypeToFind);

        for (int i = 0; i < objsWithTag.Length; i++)
        {
            float dist = Vector3.Distance(currObj.transform.position, objsWithTag[i].transform.position);
            if (dist < closestDist && currObj != objsWithTag[i])
            {
                closestDist = dist;
                closestObj = objsWithTag[i];
            }
        }

        return closestObj;
    }

    void MoveWithPlayerInput()
    {
        camOffsetXZ -= IC.GetRightCamMovement();
        camOffsetY += IC.GetUpCamMovement();
        
        
        float newX = player.transform.position.x + Mathf.Cos(camOffsetXZ) * distanceFromPlayer;
        float newZ = player.transform.position.z + Mathf.Sin(camOffsetXZ) * distanceFromPlayer;
        
        if (camOffsetY > maxYOffset)
        {
            camOffsetY = maxYOffset;
        }
        else if (camOffsetY < minYOffset)
        {
            camOffsetY = minYOffset;
        }
        float newY = player.transform.position.y + camOffsetY + nonPlayerYOffset;
        
        Vector3 newPos = new Vector3(newX, newY, newZ);
        
        posGo.transform.position = newPos;
        lookAtGO.transform.position = player.transform.position;

        //This is an artists code :^)
        //when.game (is) being.played;; initiate.fun 
        //combat=active gameObject.good combat
    }

    // Update is called once per frame
    void Update ()
    {
        if(target == null)
        {
            MoveWithPlayerInput();
        }
        else
        {
            FollowEnemy();
        }


        IC.UpdateCameraInput();
        if(IC.GetLockPressed())
        {
            if(target == null)
            {
                target = FindClosestObj(gameObject, "Enemy");
            }
            else
            {
                target = null;
            }
        }


	}
}
