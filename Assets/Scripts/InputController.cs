using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController{

    //Player movement inputs
	float forwardMove;
	float rightMove;
    bool jumped;

    //Camera input
    float rightCam;
    float upCam;
    bool lockPressed;
    bool lockChangePressed;

    public bool GetJumped()
    {
        return jumped;
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
	
	
    public void UpdateMovementInput()
    {
        forwardMove = Input.GetAxis("AxisYMove");
        rightMove = Input.GetAxis("AxisXMove");
        jumped = Input.GetButtonDown("Jump");
    }

    public void UpdateCameraInput()
    {
        rightCam = Input.GetAxis("CamXAxis");
        upCam = Input.GetAxis("CamYAxis");
        lockPressed = Input.GetButtonDown("Lock");
        lockChangePressed = Input.GetButtonDown("LockTargetChange");
    }
}
