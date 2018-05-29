using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using InControl;

public class PlayerController : MonoBehaviour
{
	[Header ("Input")]
	public FirstPersonController fpsController;
	public PlayerActions playerActions;

	[Range (-1, 1)]
	public float MovementX;
	[Range (-1, 1)]
	public float MovementY;
	public bool WalkState;
	public bool JumpState;

	[Header ("Camera")]
	public Animator CamAnim;

	[Header ("EnemyDetection")]
	public Collider EnemyChecker;

	public int CurrentHealth;
	public int StartingHealth = 100;

	void Start ()
	{
		CreatePlayerActions ();
		CurrentHealth = StartingHealth;
	}

	void Update ()
	{
		CheckPlayerInput ();
		CheckCamAnim ();
		CheckEnemies ();
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
		} 

		else 
		
		{
			// If enemy checker is on.
			if (EnemyChecker.enabled == true) 
			{
				EnemyChecker.enabled = false; // Turn of enemy checker.
			}
		}
	}

	void CheckCamAnim ()
	{
		if (playerActions.MoveForward.Value > 0 && playerActions.Sprint.Value <= 0)
		{
			CamAnim.SetBool ("Walking", true);
		}

		if (playerActions.MoveForward.Value <= 0)
		{
			CamAnim.SetBool ("Walking", false);
			CamAnim.SetTrigger ("Idle");
		}

		if (playerActions.MoveForward.Value > 0 && playerActions.Sprint.Value > 0)
		{
			CamAnim.SetBool ("Walking", true);
			CamAnim.SetBool ("Running", true);
		}

		if (playerActions.Sprint.Value <= 0) 
		{
			CamAnim.SetBool ("Running", false);
		}

		if (playerActions.Jump.WasPressed) 
		{
			CamAnim.SetTrigger ("Idle");
		}
	}

	void CheckPlayerInput ()
	{
		MovementX = playerActions.Move.Value.x;
		MovementY = playerActions.Move.Value.y;

		fpsController.m_MouseLook.xRot = playerActions.Look.Value.y * fpsController.m_MouseLook.XSensitivity;
		fpsController.m_MouseLook.yRot = playerActions.Look.Value.x * fpsController.m_MouseLook.YSensitivity;

		JumpState = playerActions.Jump.WasPressed;
		fpsController.m_Jump = JumpState;

		WalkState = !playerActions.Sprint.IsPressed;
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

		playerActions.Jump.AddDefaultBinding (Key.Space);
		playerActions.Jump.AddDefaultBinding (InputControlType.Action1);

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