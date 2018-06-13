using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController{

    //Player movement inputs
	float forwardMove;
	float rightMove;
    bool jumped;
    bool dodging;

    //Camera input
    float rightCam;
    float upCam;
    bool lockPressed;
    bool lockChangePressed;

    //Attack inputs
    bool lightAttackPressed;
    bool heavyAttackPressed;
    bool specialAttackPressed;

    bool Paused;

    public bool GetPaused()
    {
        return Input.GetButtonDown("Pause");
    }

    public bool GetJumped()
    {
        return jumped;
    }

    public bool GetDodging()
    {
        return dodging;
    }
	
    public float GetForwardMove()
    {
        return forwardMove;
    }

    public float GetRightMove()
    {
        return rightMove;
    }

    public float GetRightCamMovement()
    {
        return rightCam;
    }

    public float GetUpCamMovement()
    {
        return upCam;
    }

    public bool GetLockPressed()
    {
        return lockPressed;
    }

    public bool GetLockTargetChange()
    {
        return lockChangePressed;
    }

    public bool GetLight()
    {
        return lightAttackPressed;
    }

    public bool GetHeavy()
    {
        return heavyAttackPressed;
    }

    public bool GetSpecial()
    {
        return specialAttackPressed;
    }
	
	
    public void UpdateMovementInput()
    {
        forwardMove = Input.GetAxis("AxisYMove");
        rightMove = Input.GetAxis("AxisXMove");
        jumped = Input.GetButtonDown("Jump");
        dodging = Input.GetButton("Dodge");
    }

    public void UpdateCameraInput()
    {
        rightCam = Input.GetAxis("CamXAxis");
        upCam = Input.GetAxis("CamYAxis");

        lockPressed = Input.GetButtonUp("Lock");
        lockChangePressed = Input.GetButtonDown("LockTargetChange");
    }

    public void UpdateCombatInputs()
    {
        lightAttackPressed = Input.GetButtonDown("LightAttack");
        heavyAttackPressed = Input.GetButtonDown("HeavyAttack");
        specialAttackPressed = Input.GetButtonDown("SpecialAttack");
    }
}
