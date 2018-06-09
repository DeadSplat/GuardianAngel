using UnityEngine;

public class MenuManagement : MonoBehaviour 
{
	public SaveAndLoadScript saveAndLoadScript;

	void Start () 
	{
		saveAndLoadScript = GameObject.Find ("SaveAndLoad").GetComponent<SaveAndLoadScript> ();
	}

	void Update ()
	{
		if (AudioListener.volume < saveAndLoadScript.MasterVolume)
		{
			AudioListener.volume += 0.2f * Time.unscaledDeltaTime;
		}
	}
}