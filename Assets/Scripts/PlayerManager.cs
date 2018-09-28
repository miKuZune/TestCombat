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

    GameObject deathUI;

	// Use this for initialization
	void Start ()
    {
        IC = new InputController();
        playerRB = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        Health h = gameObject.AddComponent<Health>();
        h.Initalize(StartHealth, 2);

        deathUI = GameObject.Find("DeathUI");
        deathUI.SetActive(false);
	}

    public GameObject GetDeathUIGameObject()
    {
        return deathUI;
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

    void MoveForwardDirectionControl()
    {
        Camera mainCam = Camera.main;

        Vector3 inputDir = transform.position +((mainCam.transform.forward * IC.GetForwardMove()) + (mainCam.transform.right * IC.GetRightMove()));
        inputDir.y = transform.position.y;

        transform.LookAt(inputDir);
        transform.position = Vector3.MoveTowards(transform.position, inputDir, ForwardMoveSpeed / 20);
        UpdateAnimations();
    }

    void UpdateAnimations()
    {
        float forwardMoveVar = IC.GetForwardMove();
        float sideWaysMoveVar = IC.GetRightMove();


        if (forwardMoveVar < 0) { forwardMoveVar *= -1; }
        if (sideWaysMoveVar < 0) { sideWaysMoveVar *= -1; }

        float totalMoveVar = (forwardMoveVar + sideWaysMoveVar) * 2;
        

        anim.SetFloat("forwardMove", totalMoveVar);
        anim.SetBool("dodge", false);
    }


    void Dodge()
    {
        //Get the players directional input
        Vector3 movement = transform.position + (transform.forward * IC.GetForwardMove()) + (transform.right * IC.GetRightMove());

        //Make the player move backwards if they arn't giving any movement commands.
        if (movement == Vector3.zero) { movement = -transform.forward; }

        //Check the nearby area for and walls and stop movement if there are some found.
        bool nearWall = false;

        float currMoveSpeedLimiter = ForwardMoveSpeed * 2;
        RaycastHit hit;
        float sweepDist = movement.magnitude * Time.deltaTime + SweepDistanceBuffer;
        if (playerRB.SweepTest(movement, out hit, sweepDist))
        {
            nearWall = true;
        }

        //Move the player
        if(!nearWall)
        {
            transform.position = Vector3.MoveTowards(transform.position, movement, ForwardMoveSpeed / 15);
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
        else{ MoveForwardDirectionControl();  CheckAndResetDodge();}

        if (IC.GetPaused()) { GetComponent<PauseMenu>().PauseGame(); }
    }
}
