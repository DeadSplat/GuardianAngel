using UnityEngine;
using UnityStandardAssets.Utility;
using UnityStandardAssets.ImageEffects;

public class WeatherSystem : MonoBehaviour 
{
	public GameController gameControllerScript;
	public SunShafts sunShaftsScript;

	[Header ("Lightning")]
	public Animator LightFlash;
	public ParticleSystem Lightning;

	public Vector2 LightningTime = new Vector2 (30, 60);
	public float NewLightningTime;

	public AudioSource Thunder;
	public AudioClip[] ThunderSounds;
	public Vector2 ThunderPitchRange = new Vector2 (0.5f, 1.25f);

	[Header ("Rain and clouds")]
	public ParticleSystem Rain;
	public AudioSource RainLoop;

	[Header ("Sun")]
	public Light Sun;
	public AutoMoveAndRotate SunAutoRotateScript;

	void Start ()
	{
		ResetLightningTime ();
		UpdateRainEmission ();
	}

	void Update ()
	{
		CheckSun ();
		CheckLightning ();
	}

	void CheckSun ()
	{
		if (Sun.transform.eulerAngles.x > 180) 
		{
			Sun.intensity = Mathf.Lerp (Sun.intensity, 0, 0.1f * Time.deltaTime);

			if (sunShaftsScript == true)
			{
				sunShaftsScript.enabled = false;
			}
		}

		if (Sun.intensity < 0.02f && SunAutoRotateScript.enabled == true) 
		{
			SunAutoRotateScript.enabled = false;
			Sun.enabled = false;
			gameControllerScript.MasterAmbience.Play ();
		}
	}

	void CheckLightning ()
	{
		if (Sun.transform.eulerAngles.x > 180)
		{
			if (NewLightningTime > 0)
			{
				NewLightningTime -= Time.deltaTime;
				float targetRainVol = Mathf.Clamp (0.125f * gameControllerScript.SwitchesActivated, 0, 1);
				RainLoop.volume = Mathf.Lerp (RainLoop.volume, targetRainVol, 3 * Time.deltaTime);
				return;
			}

			else 
			
			{
				DoLightning ();
				ResetLightningTime ();
			}
		}
	}

	public void DoLightning ()
	{
		LightFlash.SetTrigger ("Flash");
		Lightning.Play ();

		if (Thunder.isPlaying == false) 
		{
			Thunder.clip = ThunderSounds [Random.Range (0, ThunderSounds.Length)];
			Thunder.pitch = Random.Range (ThunderPitchRange.x, ThunderPitchRange.y);
			Thunder.Play ();
		}
	}

	public void UpdateRainEmission ()
	{
		var RainEmission = Rain.emission;
		RainEmission.rateOverTime = 166.66f * gameControllerScript.SwitchesActivated;
	}

	void ResetLightningTime ()
	{
		NewLightningTime = Random.Range (LightningTime.x, LightningTime.y);
	}

	public void SetSunMovement (bool move)
	{
		SunAutoRotateScript.enabled = move;
	}
}