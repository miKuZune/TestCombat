using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour {
	//Public global variables
	public float moveSpeed;
	public float sensitivity;
	public float jumpPower;

	float comboBar;

	public int maxHealth;
	public int maxCombo;

	public float dodgeMoveSpeed;
	public float maxDodgeTime;
	Vector3 dodgeDir;
	bool canDodge = true;
	bool isDodging = false;
	float timeSpentDodging;

	float playerSpecifiedDodgeTime;

	string currChosenEnemy = null;

	//Private globals
	Camera cam;
	CameraFollowScript CFS;
	Rigidbody rb;

	Animator anim;
	//Constant globals
	int recentAnimationID;
	float timeSinceLastAnimation;

	const float approximateDistToGround = 1.1f;
	// Use this for initialization
	void Start () 
	{
		if (moveSpeed == 0) {moveSpeed = 3;	}
		if (sensitivity == 0) {sensitivity = 5;}
		if (jumpPower == 0) {jumpPower = 5;}

		cam = Camera.main;
		CFS = cam.GetComponent<CameraFollowScript> ();
		rb = GetComponent<Rigidbody> ();
		anim = GetComponent<Animator> ();

		Health h = this.gameObject.AddComponent<Health> ();
		//h.SetHealth (maxHealth);
		//h.maxHealth = maxHealth;

		this.GetComponent<PlayerUIManager> ().SetHealthUI (maxHealth, maxHealth);
		this.GetComponent<PlayerUIManager> ().SetComboUI (0, maxCombo);

		//Deal with mouse
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
		
	void FaceInCameraDirection()
	{
		Quaternion rot = cam.transform.rotation;
		rot.x = 0;
		rot.z = 0;
		transform.rotation = rot;
	}


	void LookAtGameObject(string objName)
	{
		GameObject enemy = GameObject.Find (objName);
		cam.transform.LookAt (enemy.transform);
		transform.LookAt (enemy.transform);
		Debug.Log (enemy);
	}

	string ChooseNewEnemy()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");

		string enemyToLookAt = null;

		float leastDistance = 0;
		for (int i = 0; i < enemies.Length; i++) {
			float dist = Vector3.Distance (enemies [i].transform.position, transform.position);
			if (dist < leastDistance || leastDistance == 0) {
				leastDistance = dist;
				enemyToLookAt = enemies [i].name;
			}
		}
		return enemyToLookAt;
	}

	void Move(Vector3 direction, float speed)
	{
		transform.Translate (direction * speed * Time.deltaTime);
	}

	void Turn(Vector3 rot)
	{
		Vector3 bodyRotation = Vector3.zero;
		Vector3 camRotation = Vector3.zero;

		bodyRotation.y = rot.y;
		camRotation.x = rot.x;

		CFS.AddToCamYOffset(rot.x * sensitivity * Time.deltaTime);
		CFS.CamOffsetAdd(-rot.y * sensitivity * Time.deltaTime);
	}

	bool isGrouned()
	{
		bool grounded = false;

		RaycastHit hit;
		if (Physics.Raycast (transform.position, -transform.up, out hit)) 
		{
			if (hit.distance >= approximateDistToGround) {grounded = false;}
			else {grounded = true;}
		}

		return grounded;
	}

	void Jump(float jumpForce)
	{
		if (isGrouned()) 
		{
			Vector3 currVel = rb.velocity;
			currVel.y = jumpForce;
			rb.velocity = currVel;
		}
	}


	void Dodge(Vector3 inputDir)
	{
		if (inputDir == Vector3.zero) {
			inputDir = -Vector3.forward;
		}


		if (canDodge) {
			Move (inputDir, dodgeMoveSpeed);
			timeSpentDodging += Time.deltaTime;
			Debug.Log (timeSpentDodging);
			if (timeSpentDodging > maxDodgeTime) {canDodge = false;}
		}
	}


	Vector3 RotationInputManager( )
	{
		Vector3 rot = Vector3.zero;

		rot.y = Input.GetAxis ("Mouse X");
		rot.x = Input.GetAxis ("Mouse Y");

		if (rot.y >= -0.1f && rot.y <= 0.1f) {rot.y = 0;}
		if (rot.x >= -0.1f && rot.x <= 0.1f) {rot.x = 0;}

		return rot;
	}

	Vector3 MovementInputManager()
	{
		Vector3 dir = Vector3.zero;

		dir.x = Input.GetAxis ("Horizontal");
		dir.z = Input.GetAxis ("Vertical");

		if (Input.GetButtonDown ("Jump")) {Jump (jumpPower);}
		if (Input.GetButton ("Dodge")) {Dodge (dir);}

		if (Input.GetButtonDown ("Lock")) {
			if (currChosenEnemy == null) {
				currChosenEnemy = ChooseNewEnemy ();
				cam.GetComponent<CameraFollowScript> ().enabled = false;
			} else {
				currChosenEnemy = null;
				cam.GetComponent<CameraFollowScript> ().enabled = true;
			}
		}

		return dir;
	}

	string CycleThroughEnemies()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");
		GameObject currEnemy = GameObject.Find (currChosenEnemy);
		string newEnemyName = null;


		float closesestDist = 0;
		for (int i = 0; i < enemies.Length; i++) {
			if (enemies [i] != currEnemy) {
				float dist = Vector3.Distance (enemies [i].transform.position, currEnemy.transform.position );
				Debug.Log ("Looking for new enemy");
				if (dist < closesestDist || closesestDist == 0) {
					closesestDist = dist;
					newEnemyName = enemies [i].name;
					Debug.Log ("Found new enemy");
				}
			}
		}

		return newEnemyName;
	}

	void AttackInputManager()
	{
		if (Input.GetButtonDown ("Fire1") && timeSinceLastAnimation >= 0.75f) 
		{
			recentAnimationID++;
			if (recentAnimationID > 2) 
			{
				recentAnimationID = 1;
				timeSinceLastAnimation = 0;
			}
			anim.Play ("LightAttack" + recentAnimationID);
		}
		if (Input.GetButtonDown ("Fire2") && timeSinceLastAnimation >= 1) {
			
			anim.Play ("HeavyAttack1");
		}

		if (Input.GetButtonDown ("Fire3") && comboBar > 50) 
		{
			anim.Play ("SpecialAttackStab");
			comboBar -= 50;
			this.GetComponent<PlayerUIManager> ().SetComboUI (comboBar, maxCombo);
		}

		timeSinceLastAnimation += Time.deltaTime;
		if (timeSinceLastAnimation >= 1.5f) 
		{
			recentAnimationID = 0;
			timeSinceLastAnimation = 0;
		}
	}

	public void AddToComboBar(int addition)
	{
		comboBar += addition;
		if (comboBar > maxCombo) {
			comboBar = maxCombo;
		}
	}

	public float GetComboBar()
	{
		return comboBar;
	}

	// Update is called once per frame
	void Update () 
	{
		//Normal movement
		if (!isDodging) {
			Vector3 direction = MovementInputManager ();
			Vector3 rot = RotationInputManager ();
			anim.SetFloat ("movement", direction.magnitude);
			if (direction != Vector3.zero) {
				FaceInCameraDirection ();
				Move (direction, moveSpeed);
			}

			AttackInputManager ();
		} else 
		{
			
		}

		if (canDodge == false) {
			timeSpentDodging -= Time.deltaTime;
			if (timeSpentDodging <= 0) {
				canDodge = true;
			}
		}

		if (currChosenEnemy == null) {
			Turn (RotationInputManager());
		} else {
			LookAtGameObject (currChosenEnemy);
			if (Input.GetButtonDown ("ToggleEnemy")) {currChosenEnemy = CycleThroughEnemies (); Debug.Log ("Togglin");}
		}

		Debug.Log (timeSpentDodging);

	}
}
