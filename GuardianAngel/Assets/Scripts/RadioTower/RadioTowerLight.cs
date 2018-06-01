using UnityEngine;
using System.Collections;

public class RadioTowerLight : MonoBehaviour 
{
	public Light RadioLight;

	public float CurrentFlashPeriod = 1;

	public float PeriodDecreaseRate = 0.05f;

	public AudioSource RadioPingSound;

	void Start ()
	{
		StartCoroutine (RadioLightFlash ());
	}

	public IEnumerator RadioLightFlash ()
	{
		RadioLight.enabled = false;
		yield return new WaitForSeconds (CurrentFlashPeriod);
		RadioLight.enabled = true;
		RadioPingSound.Play ();
		yield return new WaitForSeconds (0.5f * CurrentFlashPeriod);
		StartCoroutine (RadioLightFlash ());
	}
}