using UnityEngine;

public class SwitchScript : MonoBehaviour
{
	public PlayerController playerControllerScript;
	public GameController gameControllerScript;
	public Animator SwitchAnim;
	public Light[] RedLights;
	public Light[] GreenLights;
	public AudioSource SwitchMoveSound;
	public AudioSource SwitchActivateSound;
	public ParticleSystem SwitchActivateParticles;
	public bool activated;

	public string Name = "Power box";
	public LineRenderer line;

	void Start ()
	{	
		ToggleRedLights (true);
		ToggleGreenLights (false);
	}

	// This will be called on the raycast use action.
	public void ActivateSwitch ()
	{
		SwitchAnim.SetTrigger ("ActivateSwitch");
		SwitchMoveSound.Play ();
		gameControllerScript.SwitchesActivated++;
		activated = true;
	}

	// Called on animation to set lights
	public void SetLights ()
	{
		ToggleRedLights (false);
		ToggleGreenLights (true);
		SwitchActivateSound.Play ();
		SwitchActivateParticles.Play ();
		SetLineToRadioTower ();
		gameControllerScript.UpdateBackgroundAmbience ();
		gameControllerScript.RefreshSwitchesLines ();
		SetObjectiveText (gameControllerScript.SwitchesActivated + "/" + gameControllerScript.MaxiumSwitchesToActivate);
	}

	void ToggleGreenLights (bool active)
	{
		foreach (Light light in GreenLights) 
		{
			light.enabled = active;
		}
	}

	void ToggleRedLights (bool active)
	{
		foreach (Light light in RedLights) 
		{
			light.enabled = active;
		}
	}

	void SetLineToRadioTower ()
	{
		line.SetPosition (0, transform.position);
		line.SetPosition (1, gameControllerScript.RadioTowerLink.position);
		line.enabled = true;
	}

	void SetObjectiveText (string Objective)
	{
		playerControllerScript.UIObjectiveAnim.SetTrigger ("Objective");

		if (gameControllerScript.SwitchesActivated < gameControllerScript.MaxiumSwitchesToActivate)
		{
			playerControllerScript.ObjectiveText.text = Objective;
		} 

		else 
		{
			playerControllerScript.ObjectiveText.text = "Run to the bunker";
		}
	}
}