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

    Vector3 movement = Vector3.zero;

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
        Camera mainCam = Camera.main;

        //Get the players directional input
        

        //Make the player move backwards if they arn't giving any movement commands.
        if (movement == Vector3.zero) { movement = -(mainCam.transform.forward) ; }

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
            transform.position = Vector3.MoveTowards(transform.position, transform.position + movement, ForwardMoveSpeed / 10);
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
            movement = Vector3.zero;
        }
    }

	void Update ()
    {
        IC.UpdateMovementInput();

        if (IC.GetDodging() && timeDodging < maxDodgeTime)
        {  
            if(movement == Vector3.zero)
            {
                Debug.Log("Hello there!");
                Camera mainCam = Camera.main;
                movement =(mainCam.transform.forward * IC.GetForwardMove()) + (mainCam.transform.right * IC.GetRightMove());
                movement.y = 0;
            }
           
            Dodge();
            timeDodging += Time.deltaTime;
        }
        else{ MoveForwardDirectionControl();  CheckAndResetDodge(); movement = Vector3.zero; }

        if (IC.GetPaused()) { GetComponent<PauseMenu>().PauseGame(); }
    }
}
