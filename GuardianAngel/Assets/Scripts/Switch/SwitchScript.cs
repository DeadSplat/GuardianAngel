using UnityEngine;

public class SwitchScript : MonoBehaviour
{
	public ParticleSystem Rings;

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
	public GameObject StaticAngels;

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
		WeatherSystem.instance.GlobalWind.windMain += 16;
	}

	// Called on animation to set lights
	public void SetLights ()
	{
		ToggleRedLights (false);
		ToggleGreenLights (true);
		SwitchActivateSound.Play ();
		SwitchActivateParticles.Play ();

		if (EnemyManager.instance.enemiesCanSpawn == false && isMasterSwitch == false)
		{
			EnemyManager.instance.enemiesCanSpawn = true;
			EnemyManager.instance.GetNextSpawnTime ();
			EnemyManager.instance.GetNextDeactivateTime ();
		}

		if (isMasterSwitch == false)
		{
			GameController.instance.SwitchesActivated++;
			SetObjectiveText (GameController.instance.SwitchesActivated + "/" + GameController.instance.MaximumSwitchesToActivate);
			SetLineToRadioTower ();
			GameController.instance.RefreshSwitchesLines ();
			GameController.instance.UpdateBackgroundAmbience ();
			WeatherSystem.instance.UpdateRainEmission ();

			if (GameController.instance.SwitchesActivated == 2) 
			{
				WeatherSystem.instance.enabled = true;
				WeatherSystem.instance.SetSunMovement (true);
			}
		}

		else 
		
		{
			SetObjectiveText ("Activate " + GameController.instance.MaximumSwitchesToActivate + " switches");
			GameController.instance.SetSwitchState (true);
			StaticAngels.SetActive (false);
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
		line.SetPosition (1, GameController.instance.RadioTowerLink.position);
		line.enabled = true;
	}

	void SetObjectiveText (string Objective)
	{
		PlayerController.instance.UIObjectiveAnim.SetTrigger ("Objective");

		if (GameController.instance.SwitchesActivated < GameController.instance.MaximumSwitchesToActivate)
		{
			PlayerController.instance.ObjectiveText.text = Objective;
		} 

		else 
		{
			PlayerController.instance.ObjectiveText.text = "Run to the bunker";
			Rings.Play ();
		}
	}
}