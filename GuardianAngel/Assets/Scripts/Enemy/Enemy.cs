using UnityEngine;

public class Enemy : MonoBehaviour 
{
	public PlayerController playerControllerScript;
	public GameController gameControllerScript;
	public SaveAndLoadScript saveAndLoadScript;
	public EnemyManager enemyManagerScript;
	public Transform Player;
	public Camera PlayerCam;
	public LevelOneDifficulty[] levelOneDifficulty;

	public MeshRenderer[] Eyes;

	[Header ("Attacking")]
	public MeshRenderer AttackMesh;
	public GameObject AttackingPose;
	public GameObject DefendingPose;
	public bool isDefending;
	public float DefendingTimerRemain;
	public float DefendingTimerDuration;

	public Collider AngelAttackCol;
	public Collider EnemyChecker;

	public Animator RedVignette;

	[Header ("Movement")]
	public bool onScreen;
	public float MovementSpeed = 1;
	public float BufferDistance;
	public Vector2[] FluidAttackSpeeds;
	[Tooltip ("Current fluid movement for Angels when facing away from them.")]

	public Vector2[] TeleportAttackSpeeds;
	[Tooltip ("Current teleporting movement for Angels when facing towards them.")]
	public float TeleportAttackSpeed = 2;
	public float TeleportTimeDuration;
	public float TeleportTimeRemaining;
	public AudioSource TeleportSound;

	public int Damage = 2;
	public float AttackSpeed = 0.5f;

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

		saveAndLoadScript = GameObject.Find ("SaveAndLoad").GetComponent<SaveAndLoadScript> ();
		RefreshDifficulty ();

		InvokeRepeating ("GetNewMovementSpeed", 0, 3);
	}

	void OnEnable ()
	{
		if (saveAndLoadScript == null) 
		{
			saveAndLoadScript = GameObject.Find ("SaveAndLoad").GetComponent<SaveAndLoadScript> ();
		} 
	}

	void RefreshDifficulty ()
	{
		FluidAttackSpeeds = levelOneDifficulty [saveAndLoadScript.levelOneDifficulty].FluidAttackSpeeds;
		TeleportAttackSpeeds = levelOneDifficulty [saveAndLoadScript.levelOneDifficulty].TeleportAttackSpeeds;
		AttackSpeed = levelOneDifficulty [saveAndLoadScript.levelOneDifficulty].AttackSpeed;
		Damage = levelOneDifficulty [saveAndLoadScript.levelOneDifficulty].Damage;

		// Updates the eyes.
		foreach (MeshRenderer eye in Eyes) 
		{
			eye.material = levelOneDifficulty [saveAndLoadScript.levelOneDifficulty].EyeMaterial;
		}
	}

	void Update ()
	{
		CheckDefendingTimer ();

		if (saveAndLoadScript != null) 
		{
			GenerateNextTeleportMovement ();
		}

		CheckIfVisible ();
		CheckPlayerHealth ();
	}

	void CheckPlayerHealth ()
	{
		if (playerControllerScript.CurrentHealth <= 0) 
		{
			if (enemyManagerScript.enabled == true) 
			{
				enemyManagerScript.enabled = false;
				gameControllerScript.BackgroundAmbienceParent.SetActive (false);
				playerControllerScript.HeartBeatSound.Stop ();
				playerControllerScript.BreathingSound.Stop ();
			}

			gameObject.SetActive (false);
		}
	}

	void GetNewMovementSpeed ()
	{
		MovementSpeed = Random.Range (
			FluidAttackSpeeds [gameControllerScript.SwitchesActivated].x, 
			FluidAttackSpeeds [gameControllerScript.SwitchesActivated].y
		);
	}

	void GetNewTeleportSpeed ()
	{
		TeleportAttackSpeed = Random.Range (
			TeleportAttackSpeeds [gameControllerScript.SwitchesActivated].x, 
			TeleportAttackSpeeds [gameControllerScript.SwitchesActivated].y
		);
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
						TeleportSound.Play ();
						GetNewTeleportSpeed ();
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
			Vector3.Distance (Player.transform.position, transform.position) <= 5) 
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

			RedVignette.Play ("RedVignette");
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