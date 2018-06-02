using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;
using UnityStandardAssets.Characters.FirstPerson;
using InControl;
using TMPro;

public class PlayerController : MonoBehaviour
{
	public SaveAndLoadScript saveLoadScript;
	public LocalSceneLoader localSceneLoaderScript;
	public PostProcessingProfile postProcess;

	[Header ("Input")]
	public FirstPersonController fpsController;
	public PlayerActions playerActions;

	[Range (-1, 1)]
	public float MovementX;
	[Range (-1, 1)]
	public float MovementY;
	public bool WalkState;
	private float initalRunSpeed;
	//public bool JumpState;

	[Header ("Sprint")]
	public float SprintTimeRemaining = 28;
	public float SprintTimeDuration = 28;
	public float RecoverMultiplier_Walk = 2;
	public float RecoverMultiplier_Idle = 3;
	public ParticleSystem SprintParticles;

	public bool SprintRanOut;
	public float SprintCooldownTime = 5;

	[Header ("Camera")]
	public Camera PlayerCam;
	public Animator CamAnim;
	public float IdleFov = 55;
	public float WalkFov = 60;
	public float SprintFov = 65;
	public float FovSmoothing = 60;

	[Header ("UI Actions")]
	public Animator UIActionAnim;
	public TextMeshProUGUI ActionText;
	public Animator UIDescriptionAnim;
	public TextMeshProUGUI DescriptionText;
	public Animator UIObjectiveAnim;
	public TextMeshProUGUI ObjectiveText;

	[Header ("EnemyDetection")]
	public Collider EnemyChecker;

	public int CurrentHealth;
	public int StartingHealth = 100;

	[Header ("Flashlight Settings")]
	public Light Flashlight;
	public FlashlightSettings NormalFlashlightSettings;
	public FlashlightSettings FocussedFlashlightSettings;
	public AudioSource NormalFlashlightSound;
	public AudioSource FocusFlashlightSound;
	public float FlashlightSettingSmoothing = 1;
	public Animator FlashlightAnim;

	void Start ()
	{
		saveLoadScript = GameObject.Find ("SaveAndLoad").GetComponent<SaveAndLoadScript> ();
		saveLoadScript.playerControllerScript_P1 = this;
		saveLoadScript.cam = PlayerCam;
		CreatePlayerActions ();
		CurrentHealth = StartingHealth;
		initalRunSpeed = fpsController.m_RunSpeed;
	}

	void Update ()
	{
		CheckPlayerInput ();
		CheckEnemies ();
		CheckSprint ();
		CheckUse ();
		CheckActionMenu ();
		CheckRestart ();
	}

	void CheckRestart ()
	{
		if (Input.GetKeyDown (KeyCode.R)) 
		{
			if (localSceneLoaderScript.SceneLoadCommit == false) 
			{
				localSceneLoaderScript.LoadScene (SceneManager.GetActiveScene ().name);
				localSceneLoaderScript.SceneLoadCommit = true;
			}
		}
	}

	void CheckActionMenu ()
	{
		RaycastHit hit;
		Ray ray = PlayerCam.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray, out hit, 2f)) 
		{
			Transform objectHit = hit.transform;

			if (objectHit.tag == "Switch")
			{
				SwitchScript switchScript = objectHit.GetComponent<SwitchScript> ();

				if (switchScript.activated == false)
				{
					if (UIDescriptionAnim.GetBool ("Visible") == false) 
					{
						UIDescriptionAnim.SetBool ("Visible", true);
					}

					if (UIActionAnim.GetBool ("Visible") == false) 
					{
						UIActionAnim.SetBool ("Visible", true);
					}

					DescriptionText.text = switchScript.Name;
					ActionText.text = "E / F";
				}
			}
		}

		else // Did not find anything to raycast on.
		{
			if (UIDescriptionAnim.GetBool ("Visible") == true) 
			{
				UIDescriptionAnim.SetBool ("Visible", false);
			}

			if (UIActionAnim.GetBool ("Visible") == true) 
			{
				UIActionAnim.SetBool ("Visible", false);
			}
		}
	}

	void CheckUse ()
	{
		// Executes once when player pressed use key.
		if (playerActions.Use.WasPressed) 
		{
			RaycastHit hit;
			Ray ray = PlayerCam.ScreenPointToRay (Input.mousePosition);

			if (Physics.Raycast (ray, out hit, 2f)) 
			{
				Transform objectHit = hit.transform;

				if (objectHit.tag == "Switch")
				{
					SwitchScript switchScript = objectHit.GetComponent<SwitchScript> ();

					if (switchScript.activated == false) 
					{
						switchScript.ActivateSwitch ();
					}

					Debug.Log ("Activated a switch."); 
				}
					
				if (objectHit.tag == "Flashlight") 
				{
					Debug.Log ("Used/picked up flashlight.");

				}
			}

			return;
		}
	}

	void SprintRecover ()
	{
		SprintRanOut = false;
	}

	void CheckSprint ()
	{
		var motionBlurSettings = postProcess.motionBlur.settings;
		float blurFrameBlend = -0.2f * SprintTimeRemaining + 1;
		motionBlurSettings.frameBlending = Mathf.Clamp (blurFrameBlend, 0, 1);
		postProcess.motionBlur.settings = motionBlurSettings;

		var vignetteSettings = postProcess.vignette.settings;
		float vignetteIntensity = -0.2f * SprintTimeRemaining + 0.35f;
		vignetteSettings.intensity = Mathf.Clamp (vignetteIntensity, 0.2f, 1);
		postProcess.vignette.settings = vignetteSettings;

		// If walking.
		if (playerActions.Move.Value.magnitude > 0) 
		{
			// Is sprinting.
			if (playerActions.Sprint.IsPressed) 
			{
				// Has sprint time.
				if (SprintTimeRemaining > 0 && SprintRanOut == false) // Punish the player if they have sprint ran out.
				{
					WalkState = false;
					CamAnim.SetBool ("Walking", false);
					CamAnim.SetBool ("Running", true);
					SprintTimeRemaining -= Time.deltaTime;
					FlashlightAnim.SetBool ("FlashlightWalk", false);
					FlashlightAnim.SetBool ("FlashlightRun", true);

					if (SprintParticles.isPlaying == false) 
					{
						SprintParticles.Play ();
					}

					PlayerCam.fieldOfView = Mathf.Lerp (PlayerCam.fieldOfView, SprintFov, FovSmoothing * Time.deltaTime);

					// Allow faster sprint with special key.
					if (Input.GetKeyDown (KeyCode.C)) 
					{
						fpsController.m_RunSpeed *= 5;
					}

					if (Input.GetKeyUp (KeyCode.C)) 
					{
						fpsController.m_RunSpeed = initalRunSpeed;
					}
				} 

				else // Run out of sprinting.
				
				{
					if (SprintRanOut == false) // Punish the player if they have sprint ran out.
					{
						Invoke ("SprintRecover", SprintCooldownTime);
						SprintRanOut = true;
					}

					WalkState = true;
					CamAnim.SetBool ("Walking", true);
					CamAnim.SetBool ("Running", false);
					SprintTimeRemaining = 0;
					FlashlightAnim.SetBool ("FlashlightWalk", true);
					FlashlightAnim.SetBool ("FlashlightRun", false);

					if (SprintParticles.isPlaying == true) 
					{
						SprintParticles.Stop (true, ParticleSystemStopBehavior.StopEmitting);
					}

					PlayerCam.fieldOfView = Mathf.Lerp (PlayerCam.fieldOfView, WalkFov, FovSmoothing * Time.deltaTime);
				}
			} 

			else // Not sprinting.
			
			{
				WalkState = true;
				CamAnim.SetBool ("Walking", true);
				CamAnim.SetBool ("Running", false);
				FlashlightAnim.SetBool ("FlashlightWalk", true);
				FlashlightAnim.SetBool ("FlashlightRun", false);

				if (SprintParticles.isPlaying == true) 
				{
					SprintParticles.Stop (true, ParticleSystemStopBehavior.StopEmitting);
				}

				// Can reover sprint time.
				if (SprintTimeRemaining < SprintTimeDuration) 
				{
					if (SprintRanOut == false) // Punish the player if they have sprint ran out.
					{
						SprintTimeRemaining += Time.deltaTime * RecoverMultiplier_Walk;
					}
				} 

				else // Fully recovered.
				{
					SprintTimeRemaining = SprintTimeDuration;
				}

				PlayerCam.fieldOfView = Mathf.Lerp (PlayerCam.fieldOfView, WalkFov, FovSmoothing * Time.deltaTime);
			}
		} 

		else // Not walking, (Idling).
		
		{
			CamAnim.SetBool ("Walking", false);
			CamAnim.SetBool ("Running", false);
			CamAnim.SetTrigger ("Idle");
			FlashlightAnim.SetBool ("FlashlightWalk", false);
			FlashlightAnim.SetBool ("FlashlightRun", false);
			FlashlightAnim.SetTrigger ("FlashlightIdle");

			if (SprintParticles.isPlaying == true) 
			{
				SprintParticles.Stop (true, ParticleSystemStopBehavior.StopEmitting);
			}

			// Can recover sprint time.
			if (SprintTimeRemaining < SprintTimeDuration) 
			{
				if (SprintRanOut == false) // Punish the player if they have sprint ran out.
				{
					SprintTimeRemaining += Time.deltaTime * RecoverMultiplier_Idle;
				}
			} 

			else // Fully recovered.
			{
				SprintTimeRemaining = SprintTimeDuration;
			}

			PlayerCam.fieldOfView = Mathf.Lerp (PlayerCam.fieldOfView, IdleFov, FovSmoothing * Time.deltaTime);
			fpsController.m_RunSpeed = initalRunSpeed;
		}

		/*
		if (playerActions.Jump.WasPressed) 
		{
			CamAnim.SetTrigger ("Idle");
		}
		*/
	}

	public void TakeDamage (int Damage)
	{
		CurrentHealth -= Damage;
	}

	void CheckEnemies ()
	{
		// Player pressed flashlight.
		if (playerActions.Flashlight.IsPressed) 
		{
			// If enemy checker is off.
			if (EnemyChecker.enabled == false) 
			{
				EnemyChecker.enabled = true; // Turn on enemy checker.
			}

			// Set flashlight settings to focussed.
			Flashlight.range = Mathf.Lerp (Flashlight.range, FocussedFlashlightSettings.Range, FlashlightSettingSmoothing * Time.deltaTime);
			Flashlight.spotAngle = Mathf.Lerp (Flashlight.spotAngle, FocussedFlashlightSettings.SpotAngle, FlashlightSettingSmoothing * Time.deltaTime);
			Flashlight.color = Color.Lerp (Flashlight.color, FocussedFlashlightSettings.color, FlashlightSettingSmoothing * Time.deltaTime);
			Flashlight.intensity = Mathf.Lerp (Flashlight.intensity, FocussedFlashlightSettings.Intensity, FlashlightSettingSmoothing * Time.deltaTime);
		} 

		else 
		
		{
			// If enemy checker is on.
			if (EnemyChecker.enabled == true) 
			{
				EnemyChecker.enabled = false; // Turn of enemy checker.
			}

			// Set flashlight settings to normal.
			Flashlight.range = Mathf.Lerp (Flashlight.range, NormalFlashlightSettings.Range, FlashlightSettingSmoothing * Time.deltaTime);
			Flashlight.spotAngle = Mathf.Lerp (Flashlight.spotAngle, NormalFlashlightSettings.SpotAngle, FlashlightSettingSmoothing * Time.deltaTime);
			Flashlight.color = Color.Lerp (Flashlight.color, NormalFlashlightSettings.color, FlashlightSettingSmoothing * Time.deltaTime);
			Flashlight.intensity = Mathf.Lerp (Flashlight.intensity, NormalFlashlightSettings.Intensity, FlashlightSettingSmoothing * Time.deltaTime);
		}

		if (playerActions.Flashlight.WasPressed) 
		{
			FocusFlashlightSound.Play ();
		}

		if (playerActions.Flashlight.WasReleased) 
		{
			NormalFlashlightSound.Play ();
		}
	}

	void CheckPlayerInput ()
	{
		MovementX = playerActions.Move.Value.x;
		MovementY = playerActions.Move.Value.y;

		fpsController.m_MouseLook.xRot = playerActions.Look.Value.y * fpsController.m_MouseLook.XSensitivity;
		fpsController.m_MouseLook.yRot = playerActions.Look.Value.x * fpsController.m_MouseLook.YSensitivity;

		//JumpState = playerActions.Jump.WasPressed;
		//fpsController.m_Jump = JumpState;

		fpsController.m_IsWalking = WalkState;

		fpsController.horizontal = MovementX;
		fpsController.vertical = MovementY;
	}

	void CreatePlayerActions ()
	{
		playerActions = new PlayerActions ();

		playerActions.MoveLeft.AddDefaultBinding (Key.A);
		playerActions.MoveLeft.AddDefaultBinding (InputControlType.LeftStickLeft);

		playerActions.MoveRight.AddDefaultBinding (Key.D);
		playerActions.MoveRight.AddDefaultBinding (InputControlType.LeftStickRight);

		playerActions.MoveForward.AddDefaultBinding (Key.W);
		playerActions.MoveForward.AddDefaultBinding (Key.UpArrow);
		playerActions.MoveForward.AddDefaultBinding (InputControlType.LeftStickUp);

		playerActions.MoveBackward.AddDefaultBinding (Key.S);
		playerActions.MoveBackward.AddDefaultBinding (Key.DownArrow);
		playerActions.MoveBackward.AddDefaultBinding (InputControlType.LeftStickDown);

		playerActions.TurnLeft.AddDefaultBinding (Key.LeftArrow);
		playerActions.TurnLeft.AddDefaultBinding (Mouse.NegativeX);
		playerActions.TurnLeft.AddDefaultBinding (InputControlType.RightStickLeft);

		playerActions.TurnRight.AddDefaultBinding (Key.RightArrow);
		playerActions.TurnRight.AddDefaultBinding (Mouse.PositiveX);
		playerActions.TurnRight.AddDefaultBinding (InputControlType.RightStickRight);

		playerActions.LookUp.AddDefaultBinding (InputControlType.RightStickUp);
		playerActions.LookUp.AddDefaultBinding (Mouse.PositiveY);

		playerActions.LookDown.AddDefaultBinding (InputControlType.RightStickDown);
		playerActions.LookDown.AddDefaultBinding (Mouse.NegativeY);

		//playerActions.Jump.AddDefaultBinding (Key.Space);
		//playerActions.Jump.AddDefaultBinding (InputControlType.Action1);

		playerActions.Sprint.AddDefaultBinding (Key.LeftShift);
		playerActions.Sprint.AddDefaultBinding (InputControlType.LeftStickButton);

		playerActions.Use.AddDefaultBinding (Key.E);
		playerActions.Use.AddDefaultBinding (Key.F);
		playerActions.Use.AddDefaultBinding (InputControlType.Action3);

		playerActions.Flashlight.AddDefaultBinding (Mouse.LeftButton);
		playerActions.Flashlight.AddDefaultBinding (InputControlType.RightTrigger);

		playerActions.Pause.AddDefaultBinding (Key.Escape);
		playerActions.Pause.AddDefaultBinding (InputControlType.Command);
	}
}