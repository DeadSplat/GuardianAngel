using UnityEngine;

public class AudioSourcePitchByTimeScale : MonoBehaviour
{
	public AudioSource source;
	public Vector2 PitchLimits = new Vector2 (0, 2);
	public float PitchMultiplier = 1;
	public float PitchOffset;

	void Start ()
	{
		if (source == null) 
		{
			if (GetComponent <AudioSource> () != null) 
			{
				source = GetComponent <AudioSource> ();
			}
		}
	}

	void Update ()
	{
		float sourcePitch = PitchMultiplier * Time.timeScale + PitchOffset;
		source.pitch = Mathf.Clamp (sourcePitch, PitchLimits.x, PitchLimits.y);
	}
}