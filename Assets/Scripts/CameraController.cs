using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public float distanceFromPlayer;

    InputController IC;
    GameObject player;

    float camOffsetXZ;
    float camOffsetY;

    public float maxYOffset;
    public float minYOffset;

    GameObject currLockOn;
    public Vector3 lockOnOffset;

    bool doingOtherStuff;

	// Use this for initialization
	void Start () {
        IC = new InputController();
        player = GameObject.FindGameObjectWithTag("Player");
        doingOtherStuff = false;
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
        else if (camOffsetY < -minYOffset)
        {
            camOffsetY = -minYOffset;
        }
        float newY = player.transform.position.y + camOffsetY;

        Vector3 newPos = new Vector3(newX, newY, newZ);
        transform.position = newPos;
    }

    GameObject FindClosestObj(GameObject currObj, string tagTypeToFind)
    {
        GameObject closestObj = null;
        float closestDist = Mathf.Infinity;

        GameObject[] objsWithTag = GameObject.FindGameObjectsWithTag(tagTypeToFind);

        for(int i = 0; i < objsWithTag.Length; i++)
        {
            float dist = Vector3.Distance( currObj.transform.position , objsWithTag[i].transform.position);
            if(dist < closestDist && currObj != objsWithTag[i])
            {
                closestDist = dist;
                closestObj = objsWithTag[i];
            }
        }

        return closestObj;
    }

    void LookAtObject(GameObject objToLookAt)
    {
        transform.LookAt(objToLookAt.transform);
    }

    void FollowWithOffset(Vector3 offset)
    {
        Vector3 normalisedPlayerForward = player.transform.forward;
        normalisedPlayerForward.Normalize();
        Vector3 newPos = player.transform.position + (-transform.forward * offset.z) + (transform.right * offset.x);

        newPos.y += offset.y;

        transform.position = newPos;
    }

    void TransitionToMoveTo(GameObject objToMoveTo)
    {
        transform.position = Vector3.MoveTowards(transform.position, objToMoveTo.transform.position, 0.15f);
        
        Vector3 targetDir = objToMoveTo.transform.position - transform.position;
        Vector3 newDir = Vector3.RotateTowards(transform.position, targetDir, 1.5f, 0.0f);
        transform.rotation = Quaternion.LookRotation(newDir);
    }


    public bool IsLockedOn()
    {
        if(currLockOn == null)
        {
            return false;
        }else
        {
            return true;
        }
    }

    public GameObject GetLockOnTarget()
    {
        return currLockOn;
    }

	// Update is called once per frame
	void Update ()
    {
        IC.UpdateCameraInput();
        
        if(Input.GetKeyDown(KeyCode.Q))
        {
            doingOtherStuff = !doingOtherStuff;
        }

        if(!doingOtherStuff)
        {
            if (!IsLockedOn())
            {
                MoveWithPlayerInput();
                transform.LookAt(player.transform);
                if (IC.GetLockPressed())
                {
                    currLockOn = FindClosestObj(gameObject, "Enemy");
                }
            }
            else
            {
                FollowWithOffset(lockOnOffset);
                LookAtObject(currLockOn);

                if (IC.GetLockTargetChange())
                {
                    currLockOn = FindClosestObj(currLockOn, "Enemy");
                }

                if (IC.GetLockPressed())
                {
                    currLockOn = null;
                }

            }
        }
        else
        {
            TransitionToMoveTo(GameObject.Find("Face"));
        }
	}
}
