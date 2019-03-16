using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TMPro.Examples;
using UnityEngine.PostProcessing;

public class GameController : MonoBehaviour 
{
	public static GameController instance { get; private set; }
	 
	public Terrain terrain;
	public int thisDifficulty;
	public bool isCutsceneComplete;
	public bool EnteredBunker;

	public int SwitchesActivated;
	public int MaximumSwitchesToActivate = 6;
	public Transform RadioTowerLink;

	[Header ("Switches")]
	public GameObject[] SwitchObjects;
	public SwitchScript[] Switches;
	public Gradient endGradient;
	public AudioSource MasterAmbience;

	[Header ("On all awitches activated")]
	public Collider CellarDoorTrigger;
	public GameObject CellarDoorsClosed;
	public GameObject CellarDoorsOpened;

	[Header ("Background audio")]
	public GameObject BackgroundAmbienceParent;
	public AudioSource[] BackgroundAmbience;
	public Slider AudioSlider;
	public TextMeshProUGUI AudioValueText;

	[Header ("Level timer")]
	public bool isCountingTime;
	public float LevelTimerElapsed;
	public string thisLevelTime;
	public TextMeshProUGUI LevelTimerEndText;
	public TMP_UiFrameRateCounter framrateCounter;

	void Awake ()
	{
		instance = this;
	}

	void Start ()
	{
		Invoke ("LoadStuff", 18);

		thisDifficulty = SaveAndLoadScript.instance.levelOneDifficulty;

		BackgroundAmbience [0].Play ();

		CellarDoorTrigger.enabled = false;
		CellarDoorsClosed.SetActive (true);
		CellarDoorsOpened.SetActive (false);

		isCountingTime = true; // Set this to false when we have a cutscene ready.
		InvokeRepeating ("LevelTimer", 0, 1);

		SetSwitchState (false);
		WeatherSystem.instance.enabled = false;

		Invoke ("LoadAudioValue", 2);
		float AudioVolVal = (float)System.Math.Round (AudioSlider.value * 100, 0);
		AudioValueText.text = "" + AudioVolVal + "%";

		SaveAndLoadScript.instance.cam = PlayerController.instance.PlayerCam;
		SaveAndLoadScript.instance.VisualSettingsComponent = 
			PlayerController.instance.PlayerCam.GetComponent<PostProcessingBehaviour> ();
		SaveAndLoadScript.instance.framerateScript = framrateCounter;
	}

	public void EnterBunker ()
	{
		EnteredBunker = true;
	}

	void LoadAudioValue ()
	{
		AudioSlider.value = SaveAndLoadScript.instance.MasterVolume;
		SetAudioSliderValue ();
	}

	void LoadStuff ()
	{
		SaveAndLoadScript.instance.LoadPlayerData ();
		SaveAndLoadScript.instance.LoadSettingsData ();
	}

	public void UpdateBackgroundAmbience ()
	{
		BackgroundAmbience [SwitchesActivated].Play ();
	}

	public void RefreshSwitchesLines ()
	{
		if (SwitchesActivated == MaximumSwitchesToActivate) 
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

	public void SetSwitchState (bool switchIsInteractable)
	{
		foreach (GameObject switchObject in SwitchObjects) 
		{
			switchObject.SetActive (switchIsInteractable);
		}
	}

	public void SetAudioSliderValue ()
	{
		AudioListener.volume = AudioSlider.value;
		float AudioVolVal = (float)System.Math.Round (AudioSlider.value * 100, 0);
		AudioValueText.text = "" + AudioVolVal + "%";
	}

	public void SaveAudioSliderValue ()
	{
		SaveAndLoadScript.instance.SaveSettingsData ();
	}
}