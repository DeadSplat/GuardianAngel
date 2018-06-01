using UnityEngine;

public class WeatherSystem : MonoBehaviour 
{
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

	void Start ()
	{
		ResetLightningTime ();
	}

	void Update ()
	{
		if (NewLightningTime > 0) 
		{
			NewLightningTime -= Time.deltaTime;
			return;
		} 

		else 
		
		{
			DoLightning ();
			ResetLightningTime ();
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

		//Debug.Log ("Did lightning.");
	}

	void ResetLightningTime ()
	{
		NewLightningTime = Random.Range (LightningTime.x, LightningTime.y);
	}
}