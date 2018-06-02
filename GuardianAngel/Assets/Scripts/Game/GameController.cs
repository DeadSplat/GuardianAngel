using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour 
{
	public SaveAndLoadScript saveLoadScript;
	public Terrain terrain;

	public int SwitchesActivated;
	public int MaxiumSwitchesToActivate = 6;
	public Transform RadioTowerLink;

	[Header ("Switches")]
	public SwitchScript[] Switches;
	public Gradient endGradient;

	[Header ("On all awitches activated")]
	public Collider CellarDoorTrigger;
	public GameObject CellarDoorsClosed;
	public GameObject CellarDoorsOpened;

	[Header ("Background audio")]
	public AudioSource[] BackgroundAmbience;

	[Header ("Level timer")]
	public bool isCountingTime;
	public float LevelTimerElapsed;
	public string thisLevelTime;
	public TextMeshProUGUI LevelTimerEndText;

	void Start ()
	{
		saveLoadScript = GameObject.Find ("SaveAndLoad").GetComponent<SaveAndLoadScript> ();
		saveLoadScript.gameControllerScript = this;
		saveLoadScript.LoadPlayerData ();
		saveLoadScript.LoadSettingsData ();
		AudioListener.volume = saveLoadScript.MasterVolume;
		BackgroundAmbience [0].Play ();
		CellarDoorTrigger.enabled = false;
		CellarDoorsClosed.SetActive (true);
		CellarDoorsOpened.SetActive (false);

		isCountingTime = true; // Set this to false when we have a cutscene ready.
		InvokeRepeating ("LevelTimer", 0, 1);
	}

	public void UpdateBackgroundAmbience ()
	{
		BackgroundAmbience [SwitchesActivated].Play ();
	}

	public void RefreshSwitchesLines ()
	{
		if (SwitchesActivated == MaxiumSwitchesToActivate) 
		{
			for (int i = 0; i < Switches.Length; i++) 
			{
				Switches[i].line.colorGradient = endGradient;
			}

			OpenCellarDoor ();
			isCountingTime = false;
			CancelInvoke ("LevelTimer");
		}
	}

	void OpenCellarDoor ()
	{
		CellarDoorsClosed.SetActive (false);
		CellarDoorsOpened.SetActive (true);
		CellarDoorTrigger.enabled = true;
	}

	void LevelTimer ()
	{
		if (isCountingTime == true) 
		{
			LevelTimerElapsed += 1;
			GetLevelTime ();
		}
	}

	public void GetLevelTime ()
	{
		string thisLevelTime = Mathf.Floor ((LevelTimerElapsed / 60)) + ":" + ((LevelTimerElapsed % 60).ToString ("00"));
		LevelTimerEndText.text = thisLevelTime;
	}
}