using UnityEngine;
using InControl;

public class PlayerActions : PlayerActionSet
{
	public PlayerAction MoveLeft;
	public PlayerAction MoveRight;
	public PlayerAction MoveForward;
	public PlayerAction MoveBackward;
	public PlayerTwoAxisAction Move;

	public PlayerAction TurnLeft;
	public PlayerAction TurnRight;
	public PlayerAction LookUp;
	public PlayerAction LookDown;
	public PlayerTwoAxisAction Look;

	public PlayerAction Jump;
	public PlayerAction Sprint;
	public PlayerAction Use;
	public PlayerAction Flashlight;
	public PlayerAction Pause;

	public PlayerActions ()
	{
		MoveLeft 	 = CreatePlayerAction ("MoveLeft");
		MoveRight 	 = CreatePlayerAction ("MoveRight");
		MoveForward  = CreatePlayerAction ("MoveForward");
		MoveBackward = CreatePlayerAction ("MoveBack");

		Move = CreateTwoAxisPlayerAction (MoveLeft, MoveRight, MoveBackward, MoveForward);

		TurnLeft  = CreatePlayerAction ("TurnLeft");
		TurnRight = CreatePlayerAction ("TurnRight");
		LookUp 	  = CreatePlayerAction ("LookUp");
		LookDown  = CreatePlayerAction ("LookDown");

		Look = CreateTwoAxisPlayerAction (TurnLeft, TurnRight, LookDown, LookUp);

		Jump 	   = CreatePlayerAction ("Jump");
		Sprint 	   = CreatePlayerAction ("Sprint");
		Use 	   = CreatePlayerAction ("Use");
		Flashlight = CreatePlayerAction ("Flashlight");
		Pause 	   = CreatePlayerAction ("Pause");
	}
}