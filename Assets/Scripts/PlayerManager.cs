using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerManager : MonoBehaviour {

    public int StartHealth;

    public float ForwardMoveSpeed;
    public float BackwardMoveSpeed;
    public float JumpPower;

    public float DistanceToBeGrouned;
    public float SweepDistanceBuffer;

    public float turnTowardCameraSpeed;

    private const float movementAddition = 10;

    Rigidbody playerRB;
    InputController IC ;

    Animator anim;

    public float maxDodgeTime;
    public float dodgeResetTime;
    float timeDodging;
    float timeToResetDodge;

	// Use this for initialization
	void Start ()
    {
        IC = new InputController();
        playerRB = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        Health h = gameObject.AddComponent<Health>();
        h.Initalize(StartHealth, 2);
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

        //Set animations
        anim.SetFloat("forwardMove", IC.GetForwardMove());
        anim.SetFloat("sidewaysMove", IC.GetRightMove());
        anim.SetBool("dodge", false);

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
            Camera mainCam = Camera.main;
            if(!mainCam.GetComponent<CameraController>().IsLockedOn())
            {
                //Look in the direction the camera is facing
                Vector3 targetDir = mainCam.transform.forward;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, turnTowardCameraSpeed * Time.deltaTime, 0.0f);
                newDir.y = 0;
                transform.rotation = Quaternion.LookRotation(newDir);
            }
            else
            {
                Vector3 targetDir = mainCam.GetComponent<CameraController>().GetLockOnTarget().transform.position - transform.position;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, turnTowardCameraSpeed * Time.deltaTime, 0.0f);
                newDir.y = 0;
                transform.rotation = Quaternion.LookRotation(newDir);
            }
            
        }
    }

    void Dodge()
    {
        Vector3 movement = (transform.forward * IC.GetForwardMove()) + (transform.right * IC.GetRightMove());

        movement.Normalize();
        //Make the player move backwards if they arn't giving any movement commands.
        if(movement == Vector3.zero){movement = -transform.forward;}

        //Check the nearby area for and walls and stop movement if there are some found.
        float currMoveSpeedLimiter = ForwardMoveSpeed * 2;
        RaycastHit hit;
        float sweepDist = movement.magnitude * Time.deltaTime + SweepDistanceBuffer;
        if (playerRB.SweepTest(movement, out hit, sweepDist))
        {
            playerRB.velocity = new Vector3(0, playerRB.velocity.y, 0);
        }
        //Checks if the players maginitude will exceed the players movespeed.
        else if (XZMagnitude(playerRB.velocity.x, playerRB.velocity.z) < currMoveSpeedLimiter)
        {
            playerRB.AddForce(movement * ForwardMoveSpeed * 2 * movementAddition);
        }

        timeToResetDodge = dodgeResetTime;

        anim.SetBool("dodge", true);
    }

    void CheckAndResetDodge()
    {
        timeToResetDodge -= Time.deltaTime;
        if(timeToResetDodge <= 0)
        {
            timeDodging = 0;
        }
    }

	void Update ()
    {
        IC.UpdateMovementInput();

        if (IC.GetDodging() && timeDodging < maxDodgeTime) { Dodge();  timeDodging += Time.deltaTime; }
        else{ Movement();  CheckAndResetDodge();}

        if (IC.GetPaused()) { GetComponent<PauseMenu>().PauseGame(); }
    }
}
