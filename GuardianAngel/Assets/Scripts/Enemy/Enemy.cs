using UnityEngine;

public class Enemy : MonoBehaviour 
{
	public PlayerController playerControllerScript;
	public Transform Player;
	public Camera PlayerCam;

	[Header ("Attacking")]
	public MeshRenderer AttackMesh;
	public GameObject AttackingPose;
	public GameObject DefendingPose;
	public bool isDefending;
	public float DefendingTimerRemain;
	public float DefendingTimerDuration;

	public Collider AngelAttackCol;
	public Collider EnemyChecker;

	[Header ("Movement")]
	public bool onScreen;
	public float MovementSpeed = 1;
	public float BufferDistance;
	[Tooltip ("Current fluid movement for Angels when facing away from them.")]
	public float AttackSpeed = 0.5f;
	[Tooltip ("Current teleporting movement for Angels when facing towards them.")]
	public float TeleportAttackSpeed = 2;

	public float TeleportTimeDuration;
	public float TeleportTimeRemaining;

	public int Damage = 2;

	[Header ("Jump scares")]
	public Animator JumpScareCam;
	public Transform OnScreenChecker;
	public Collider JumpScareVisibleTrigger;
	public AudioSource JumpScareInvisible;
	public AudioClip[] JumpScaresInvisibleClips;
	[Space (10)]
	public AudioSource JumpScareVisible;
	public AudioClip[] JumpScaresVisibleClips;

	void Start () 
	{
		DefendingTimerRemain = 0;
		isDefending = false;

		InvokeRepeating ("CheckAngelAttackCol", 0, AttackSpeed);
	}

	void Update ()
	{
		CheckDefendingTimer ();
		GenerateNextTeleportMovement ();
		CheckIfVisible ();
	}

	void GenerateNextTeleportMovement ()
	{
		if (AttackingPose.activeSelf == true) 
		{
			if (TeleportTimeRemaining > 0) 
			{
				TeleportTimeRemaining -= Time.deltaTime;
				return;
			}

			if (TeleportTimeRemaining <= 0) 
			{
				// Calculate next teleport speed range.
				TeleportAttackSpeed = Random.Range (250, 500);

				// Calculate next teleport duration.
				TeleportTimeDuration = Random.Range (0.1f, 0.75f);

				TeleportTimeRemaining = TeleportTimeDuration;

				// If is seen by the camera.
				if (onScreen == true) 
				{
					if (Vector3.Distance (Player.transform.position, transform.position) > BufferDistance) 
					{
						// Do some teleport movement.
						transform.position = 
							Vector3.MoveTowards (
								transform.position, 
								Player.transform.position,
								TeleportAttackSpeed * Time.deltaTime
							);

						transform.LookAt (Player.transform);
					}

					if (Vector3.Distance (Player.transform.position, transform.position) <= 0.1f) 
					{
						// Do some teleport movement.
						transform.position = 
							Player.transform.position + transform.TransformDirection (new Vector3 (0, 0, -1f));
					}
				}
			}
		}
	}

	void CheckAngelAttackCol ()
	{
		if (AttackingPose.activeSelf == true && 
			Vector3.Distance (Player.transform.position, transform.position) <= BufferDistance) 
		{
			AngelAttackCol.enabled = !AngelAttackCol.enabled;
			playerControllerScript.TakeDamage (Damage);

			// Off screen jump scare.
			if (onScreen == false) 
			{
				if (JumpScareInvisible.isPlaying == false)
				{
					JumpScareInvisible.clip = JumpScaresInvisibleClips [Random.Range (0, JumpScaresInvisibleClips.Length)];
					JumpScareInvisible.Play ();
					JumpScareCam.SetTrigger ("Jumpscare");
				}
			}
		}
	}

	void CheckIfVisible ()
	{
		if (AttackingPose.activeSelf == true)
		{
			// Get screenpoint in viewport space by camera.
			Vector3 screenPoint = PlayerCam.WorldToViewportPoint (OnScreenChecker.position);

			// Check is on screen by normalised points.
			onScreen = 
				screenPoint.z > -1 &&
				screenPoint.x > 0 &&
				screenPoint.x < 1 &&
				screenPoint.y > 0 &&
				screenPoint.y < 1;

			// If not on screen.
			if (onScreen == false)
			{
				Vector3 lastPos = transform.position;

				if (Vector3.Distance (Player.transform.position, transform.position) > BufferDistance) 
				{
					// Do some fluid movement.
					transform.position = Vector3.MoveTowards (
						transform.position, 
						Player.transform.position,
						MovementSpeed * Time.deltaTime
					);

					transform.LookAt (Player.transform);
				}

				if (Vector3.Distance (Player.transform.position, transform.position) <= BufferDistance)
				{
					transform.position = lastPos;
				}
			}	
		}
	}

	void CheckDefendingTimer ()
	{
		if (isDefending == true && DefendingTimerRemain > 0) 
		{
			DefendingTimerRemain -= Time.deltaTime;
		}

		if (DefendingTimerRemain <= 0 && isDefending == true) 
		{
			isDefending = false;
			AttackingPose.SetActive (true);
			DefendingPose.SetActive (false);
			transform.LookAt (Player.transform);

			Vector3 LookRotation = new Vector3 (0, transform.eulerAngles.y, transform.eulerAngles.z);
			transform.eulerAngles = LookRotation;
		}
	}

	void OnTriggerEnter (Collider other)
	{
		// Enemy is exposed to the collider light.
		if (other == EnemyChecker)
		{
			AttackingPose.SetActive (false);
			DefendingPose.SetActive (true);
			isDefending = true;
			DefendingTimerRemain = DefendingTimerDuration;  // Reset defendng timer.
		}

		if (other == JumpScareVisibleTrigger) 
		{
			if (onScreen == true) 
			{
				if (JumpScareVisible.isPlaying == false)
				{
					JumpScareVisible.clip = JumpScaresVisibleClips [Random.Range (0, JumpScaresVisibleClips.Length)];
					JumpScareVisible.Play ();
				}
			}
		}
	}

	void OnTriggerStay (Collider other)
	{
		// Enemy is exposed to the collider light.
		if (other == EnemyChecker)
		{
			DefendingTimerRemain = DefendingTimerDuration; // Reset defendng timer.
		}
	}

	void OnTriggerExit (Collider other)
	{
		// Enemy is not exposed to the collider light.
		if (other == EnemyChecker)
		{
			DefendingTimerRemain = DefendingTimerDuration; // Reset defendng timer.
		}
	}
}