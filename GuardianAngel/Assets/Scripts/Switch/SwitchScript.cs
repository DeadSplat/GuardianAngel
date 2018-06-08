using UnityEngine;

public class SwitchScript : MonoBehaviour
{
	public PlayerController playerControllerScript;
	public GameController gameControllerScript;
	public WeatherSystem weatherSystem;
	public EnemyManager enemyManagerScript;

	public Animator SwitchAnim;
	public Light[] RedLights;
	public Light[] GreenLights;
	public AudioSource SwitchMoveSound;
	public AudioSource SwitchActivateSound;
	public ParticleSystem SwitchActivateParticles;
	public bool activated;

	public string Name = "Power box";
	public LineRenderer line;

	public bool isMasterSwitch;

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
		activated = true;
	}

	// Called on animation to set lights
	public void SetLights ()
	{
		ToggleRedLights (false);
		ToggleGreenLights (true);
		SwitchActivateSound.Play ();
		SwitchActivateParticles.Play ();

		if (enemyManagerScript.enemiesCanSpawn == false && isMasterSwitch == false)
		{
			enemyManagerScript.enemiesCanSpawn = true;
			enemyManagerScript.GetNextSpawnTime ();
			enemyManagerScript.GetNextDeactivateTime ();
		}

		if (isMasterSwitch == false)
		{
			gameControllerScript.SwitchesActivated++;
			SetObjectiveText (gameControllerScript.SwitchesActivated + "/" + gameControllerScript.MaximumSwitchesToActivate);
			SetLineToRadioTower ();
			gameControllerScript.RefreshSwitchesLines ();
			gameControllerScript.UpdateBackgroundAmbience ();
			weatherSystem.UpdateRainEmission ();

			if (gameControllerScript.SwitchesActivated == 2) 
			{
				weatherSystem.enabled = true;
				weatherSystem.SetSunMovement (true);
			}
		}

		else 
		
		{
			SetObjectiveText ("Activate " + gameControllerScript.MaximumSwitchesToActivate + " switches");
			gameControllerScript.SetSwitchState (true);
		}
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

		if (gameControllerScript.SwitchesActivated < gameControllerScript.MaximumSwitchesToActivate)
		{
			playerControllerScript.ObjectiveText.text = Objective;
		} 

		else 
		{
			playerControllerScript.ObjectiveText.text = "Run to the bunker";
		}
	}
}