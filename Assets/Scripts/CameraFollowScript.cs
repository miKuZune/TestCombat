using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowScript : MonoBehaviour {

	public float distance;

	GameObject player;

	float camOffset;
	float yOffset;

	//Constant globals
	const float yCamUpperLimit = 1.5f;
	const float yCamLowerLimit = -0.5f;
	public void CamOffsetAdd(float addition)
	{
		camOffset += addition;
	}
		
	public void AddToCamYOffset(float addition)
	{
		yOffset += addition;
		ValidateYOffset ();
	}

	void ValidateYOffset()
	{
		if (yOffset >= yCamUpperLimit) {yOffset = yCamUpperLimit;} 
		else if (yOffset <= yCamLowerLimit) {yOffset = yCamLowerLimit;}
	}

	void Start () 
	{
		player = GameObject.FindGameObjectWithTag ("Player");
		if (distance == 0) {distance = 3;}
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 pos = player.transform.position;

		pos.x += Mathf.Cos (camOffset) * distance ;
		pos.z += Mathf.Sin (camOffset) * distance ;
		pos.y += yOffset;

		transform.position = pos;



		transform.LookAt (player.transform);
	}
}
