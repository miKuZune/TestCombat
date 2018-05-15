using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerManager : MonoBehaviour {

    public float ForwardMoveSpeed;
    public float BackwardMoveSpeed;
    public float JumpPower;

    public float DistanceToBeGrouned;
    public float SweepDistanceBuffer;

    public float turnTowardCameraSpeed;

    private const float movementAddition = 10;

    Rigidbody playerRB;
    InputController IC ;

	// Use this for initialization
	void Start ()
    {
        IC = new InputController();
        playerRB = GetComponent<Rigidbody>();
	}
	
    //Move the player in an upward direction.
    void Jump()
    {
        playerRB.AddForce(new Vector3(0,JumpPower,0), ForceMode.Impulse);
    }
    //Check if the player is on the ground.
    bool IsGrouned()
    {
        bool isGrouned = false;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, Vector3.down, out hit, DistanceToBeGrouned))
        {
            isGrouned = true;
        }

        return isGrouned;
    }

    //Get the magnitude from a give x and z
    float XZMagnitude(float x, float z)
    {
        float magnitude = 0;

        magnitude = (x * x) + (z * z);
        magnitude = Mathf.Sqrt(magnitude);

        return magnitude;
    }

    //Handle the players movement.
    void Movement()
    {
        Vector3 movement = (transform.forward * IC.GetForwardMove()) + (transform.right * IC.GetRightMove());

        movement.Normalize();

        if (IC.GetJumped())
        {
            if (IsGrouned())
            {
                Jump();
            }
        }

        float currMoveSpeedLimiter = 0;
        //Check if the player is moving forward or backwards, limit their speed accordingly.
        if(IC.GetForwardMove() >= 0)
        {
            currMoveSpeedLimiter = ForwardMoveSpeed;
        }else if(IC.GetForwardMove() < 0)
        {
            currMoveSpeedLimiter = BackwardMoveSpeed;
        }

        //Checks if the player is going to colide with an object.
        RaycastHit hit;
        float sweepDist = movement.magnitude * Time.deltaTime + SweepDistanceBuffer;
        if (playerRB.SweepTest(movement, out hit, sweepDist))
        {
            playerRB.velocity = new Vector3(0, playerRB.velocity.y, 0);
        }
        //Checks if the players maginitude will exceed the players movespeed.
        else if (XZMagnitude(playerRB.velocity.x, playerRB.velocity.z) < currMoveSpeedLimiter)
        {
            playerRB.AddForce(movement * ForwardMoveSpeed * movementAddition);
        }

        if(movement.magnitude != 0)
        {
            //Look in the direction the camera is facing
            Vector3 targetDir = Camera.main.transform.forward;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, turnTowardCameraSpeed * Time.deltaTime, 0.0f);
            newDir.y = 0;
            transform.rotation = Quaternion.LookRotation(newDir);
        }  
    }

	void Update ()
    {
        IC.UpdateMovementInput();
        Movement();
    }
}
