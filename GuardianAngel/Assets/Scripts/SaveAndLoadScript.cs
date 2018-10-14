using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

using UnityStandardAssets.ImageEffects;
using UnityStandardAssets.Characters.FirstPerson;

using TMPro.Examples;

public class SaveAndLoadScript : MonoBehaviour 
{
	public static SaveAndLoadScript instance { get; private set; }

	public bool AllowLoading = true;
	public bool AllowSaving = true;

	// Live variables.
	[Header ("Player Data")]
	public string Username = "default";
	public int levelOneDifficulty;
	public LevelOneDifficulty[] LevelOneDifficulties;

	[Header ("Settings Data")]
	public PostProcessingProfile VisualSettings;
	public PostProcessingBehaviour VisualSettingsComponent;
	public Camera cam;

	public TMP_UiFrameRateCounter framerateScript;
	public float LowFpsTime;

	[Space (10)]
	public int QualitySettingsIndex;
	public bool useHdr;
	public bool sunShaftsEnabled;

	[Space (10)]
	public int GrassDensity;

	[Space (10)]
	public float mouseSensitivity = 2;

	[Space (10)]
	public float MasterVolume;
	public float SoundtrackVolume;
	public float EffectsVolume;

	public playerData PlayerData;
	public settingsData SettingsData;

	void Awake ()
	{
		instance = this;
	}

	void Start ()
	{
		if (SceneManager.GetActiveScene ().name != "init")
		{
			Invoke ("StartLoading", 18);
		}

		Invoke ("StartLoading", 1);
	}

	void StartLoading ()
	{
		if (AllowLoading == true) 
		{
			cam = Camera.main;

			if (VisualSettingsComponent != null) {
				VisualSettingsComponent = cam.GetComponent<PostProcessingBehaviour> ();
			}

			CheckPlayerDataFile ();

			LoadPlayerData ();
			LoadSettingsData ();

			CheckUsername ();
		}
	}

	void Update ()
	{
		#if UNITY_EDITOR
		if (Input.GetKeyDown (KeyCode.Tab)) 
		{
			framerateScript.isVisible = !framerateScript.isVisible;
		}
		#endif
	}

	void FixedUpdate ()
	{
		if (GameController.instance != null) 
		{
			if (GameController.instance.isCutsceneComplete == true) 
			{
				if (framerateScript != null) 
				{
					if (framerateScript.averageFramerate < 30) 
					{
						LowFpsTime += Time.fixedDeltaTime;

						if (LowFpsTime > 10)
						{
							QualitySettingsIndex = 0;
							Application.targetFrameRate = -1;

							if (Screen.width > 640 || Screen.height > 360) 
							{
								Screen.SetResolution (640, 360, Screen.fullScreen);
							}

							SaveSettingsData ();
							LoadSettingsData ();
							Debug.Log ("Lowered quality settings because of low average framerate.");
						}
					} 

					else 
					
					{
						if (LowFpsTime != 0)
						{
							LowFpsTime = 0;
						}
					}
				}
			}
		}
	}
		
	void CheckUsername ()
	{
		if (Username == null || Username == "") 
		{
			Username = "default";
		}

		if (Username == "default") 
		{
			Debug.Log (
				"Username is " 
				+ Username + 
				". Consider changing your username in the menu. " +
				"You may not have created a local profile yet."
			);
		}
	}
		
	// Gets variables from this script = variables in other scripts.
	void GetPlayerData ()
	{
		#if !UNITY_EDITOR
			if (GameController.instance != null) 
			{
				if (File.Exists (Application.persistentDataPath + "/" + Username + "_PlayerConfig.dat") == true) 
				{
					
				}
			}

			if (File.Exists (Application.persistentDataPath + "/" + Username + "_PlayerConfig.dat") == false) 
			{
				
			}
		#endif

		#if UNITY_EDITOR
			if (GameController.instance != null) 
			{
				if (File.Exists (Application.persistentDataPath + "/" + Username + "_PlayerConfig_Editor.dat") == true) 
				{
					
				}
			}

			if (File.Exists (Application.persistentDataPath + "/" + Username + "_PlayerConfig_Editor.dat") == false) 
			{
				
			}
		#endif
	}

	// Save PlayerData Main.
	public void SavePlayerData ()
	{
		if (AllowSaving == true) 
		{
			// Refer to GetPlayerData.
			GetPlayerData ();

			// Creates new save file.
			BinaryFormatter bf = new BinaryFormatter ();

			#if !UNITY_EDITOR
				FileStream file = File.Create (Application.persistentDataPath + "/" + Username + "_PlayerConfig.dat");

				Debug.Log (
					"Successfully saved to " +
					Application.persistentDataPath + "/" + Username + "_PlayerConfig.dat"
				); 
			#endif

			#if UNITY_EDITOR
				FileStream file = File.Create (Application.persistentDataPath + "/" + Username + "_PlayerConfig_Editor.dat");

				Debug.Log (
					"Successfully saved to " +
					Application.persistentDataPath + "/" + Username + "_PlayerConfig_Editor.dat"
				); 
			#endif

			// Does the saving
			playerData data = new playerData ();
			SetPlayerData (data);

			// Serializes and closes the file.
			bf.Serialize (file, data);
			file.Close ();
		}
	}

	// Sets data.[variable] = [variable] from this script.
	void SetPlayerData (playerData data)
	{
		data.Username = Username;
		data.levelOneDifficulty = levelOneDifficulty;
	}

	// Load PlayerData main.
	public void LoadPlayerData ()
	{
		#if !UNITY_EDITOR
		if (AllowLoading == true)
		{
			if (File.Exists (Application.persistentDataPath + "/" + Username + "_PlayerConfig.dat") == true) 
			{
				// Opens the save data.
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (Application.persistentDataPath + "/" + Username + "_PlayerConfig.dat", FileMode.Open);

				// Processes the save data into memory.
				playerData data = (playerData)bf.Deserialize (file);
				file.Close ();

				LoadPlayerDataContents (data);
				StorePlayerDataInGame ();

				Debug.Log ("Successfully loaded from " +
				Application.persistentDataPath + "/" + Username + "_PlayerConfig.dat");
			}

			CheckPlayerDataFile ();
		}
		#endif

		#if UNITY_EDITOR
		if (AllowLoading == true)
		{
			if (File.Exists (Application.persistentDataPath + "/" + Username + "_PlayerConfig_Editor.dat") == true) 
			{
				// Opens the save data.
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (Application.persistentDataPath + "/" + Username + "_PlayerConfig_Editor.dat", FileMode.Open);

				// Processes the save data into memory.
				playerData data = (playerData)bf.Deserialize (file);
				file.Close ();

				LoadPlayerDataContents (data);
				StorePlayerDataInGame ();

				Debug.Log ("Successfully loaded from " +
				Application.persistentDataPath + "/" + Username + "_PlayerConfig_Editor.dat");
			}

			CheckPlayerDataFile ();
		}
		#endif
	}

	void CheckPlayerDataFile ()
	{
		#if !UNITY_EDITOR
		if (File.Exists (Application.persistentDataPath + "/" + Username + "_PlayerConfig.dat") == false)
		{
			Debug.LogWarning ("Unable to load from " +
				Application.persistentDataPath + "/" + Username + "_PlayerConfig.dat");

			SavePlayerData ();

			Debug.Log ("Saved new player data to " +
				Application.persistentDataPath + "/" + Username + "_PlayerConfig.dat");
		}
		#endif

		#if UNITY_EDITOR
		if (File.Exists (Application.persistentDataPath + "/" + Username + "_PlayerConfig_Editor.dat") == false)
		{
			Debug.LogWarning ("Unable to load from " +
			Application.persistentDataPath + "/" + Username + "_PlayerConfig_Editor.dat");

			SavePlayerData ();

			Debug.Log ("Saved new player data to " +
			Application.persistentDataPath + "/" + Username + "_PlayerConfig_Editor.dat");
		}
		#endif
	}

	// Sets variables in this script by getting data from save file. 
	void LoadPlayerDataContents (playerData data)
	{
		Username = data.Username;
		levelOneDifficulty = data.levelOneDifficulty;
		Debug.Log ("Loaded difficulty is: " + levelOneDifficulty);
	}

	// Puts new data into relevant scripts.
	public void StorePlayerDataInGame ()
	{
	}
		
	// Gets variables from this script = variables in other scripts.
	void GetSettingsData ()
	{
		if (File.Exists (Application.persistentDataPath + "/" + Username + "_SettingsConfig.dat") == true
		 || File.Exists (Application.persistentDataPath + "/" + Username + "_SettingsConfig_Editor.dat") == true) 
		{
			QualitySettings.SetQualityLevel (QualitySettingsIndex);

			if (QualitySettingsIndex == 0) 
			{
				VisualSettingsComponent.enabled = false;
				sunShaftsEnabled = false;
				useHdr = false;
			}

			if (QualitySettingsIndex > 0) 
			{
				VisualSettingsComponent.enabled = true;
				sunShaftsEnabled = true;
				useHdr = true;
			}

			mouseSensitivity = 0.5f * (
			    FirstPersonController.instance.m_MouseLook.XSensitivity +
			    FirstPersonController.instance.m_MouseLook.YSensitivity
			);

			MasterVolume = Mathf.Clamp (AudioListener.volume, 0, 1);
		}
	}

	// Save Settings Main.
	public void SaveSettingsData ()
	{
		if (AllowSaving == true) 
		{
			// Refer to GetSettingsData.
			GetSettingsData ();

			// Creates new save file.
			BinaryFormatter bf = new BinaryFormatter ();

			#if !UNITY_EDITOR
				FileStream file = File.Create (Application.persistentDataPath + "/" + Username + "_SettingsConfig.dat");
				
				Debug.Log (
					"Successfully saved to " +
					Application.persistentDataPath + "/" + Username + "_SettingsConfig.dat"
				); 
			#endif

			#if UNITY_EDITOR
				FileStream file = File.Create (Application.persistentDataPath + "/" + Username + "_SettingsConfig_Editor.dat");

				Debug.Log (
					"Successfully saved to " +
					Application.persistentDataPath + "/" + Username + "_SettingsConfig_Editor.dat"
				); 
			#endif

			// Does the saving
			settingsData data = new settingsData ();
			SetSettingsData (data);

			// Serializes and closes the file.
			bf.Serialize (file, data);
			file.Close ();
		}
	}

	// Sets data.[variable] = [variable] from this script.
	void SetSettingsData (settingsData data)
	{
		QualitySettingsIndex = QualitySettings.GetQualityLevel ();
		data.QualitySettingsIndex = QualitySettingsIndex;

		if (data.QualitySettingsIndex == 0) 
		{
			data.useHdr = false;
			data.sunShaftsEnabled = false;
		}

		if (data.QualitySettingsIndex > 0) 
		{
			data.useHdr = true;
			data.sunShaftsEnabled = true;
		}

		data.mouseSensitivity = mouseSensitivity;

		data.MasterVolume 	  = Mathf.Clamp (MasterVolume, 	   0, 1);
		data.SoundtrackVolume = Mathf.Clamp (SoundtrackVolume, 0, 1);
		data.EffectsVolume 	  = Mathf.Clamp (EffectsVolume,    0, 1);
	}

	public void LoadSettingsData ()
	{
		if (AllowLoading == true)
		{
			#if !UNITY_EDITOR
				if (File.Exists (Application.persistentDataPath + "/" + Username + "_SettingsConfig.dat") == true) 
				{
					// Opens the save data.
					BinaryFormatter bf = new BinaryFormatter ();

					FileStream file = File.Open (Application.persistentDataPath + "/" + Username + "_SettingsConfig.dat", FileMode.Open);

					Debug.Log ("Successfully loaded from " +
					Application.persistentDataPath + "/" + Username + "_SettingsConfig.dat");

					// Processes the save data into memory.
					settingsData data = (settingsData)bf.Deserialize (file);
					file.Close ();

					LoadSettingsDataContents (data);
					StoreSettingsDataInGame ();
				}

				if (File.Exists (Application.persistentDataPath + "/" + Username + "_SettingsConfig.dat") == false) 
				{
					Debug.LogWarning ("Unable to load from " +
					Application.persistentDataPath + "/" + Username + "_SettingsConfig.dat");

					SaveSettingsData ();

					Debug.Log ("Saved settings data to " +
					Application.persistentDataPath + "/" + Username + "_SettingsConfig.dat");
				}
			#endif

			#if UNITY_EDITOR
				if (File.Exists (Application.persistentDataPath + "/" + Username + "_SettingsConfig_Editor.dat") == true) 
				{
					// Opens the save data.
					BinaryFormatter bf = new BinaryFormatter ();

					FileStream file = File.Open (Application.persistentDataPath + "/" + Username + "_SettingsConfig_Editor.dat", FileMode.Open);

					Debug.Log ("Successfully loaded from " +
					Application.persistentDataPath + "/" + Username + "_SettingsConfig_Editor.dat");

					// Processes the save data into memory.
					settingsData data = (settingsData)bf.Deserialize (file);
					file.Close ();

					LoadSettingsDataContents (data);
					StoreSettingsDataInGame ();
				}

				if (File.Exists (Application.persistentDataPath + "/" + Username + "_SettingsConfig_Editor.dat") == false) 
				{
					Debug.LogWarning ("Unable to load from " +
					Application.persistentDataPath + "/" + Username + "_SettingsConfig_Editor.dat");

					SaveSettingsData ();

					Debug.Log ("Saved settings data to " +
					Application.persistentDataPath + "/" + Username + "_SettingsConfig_Editor.dat");
				}
			#endif
		}
	}

	// Sets variables in this script by getting data from save file. 
	void LoadSettingsDataContents (settingsData data)
	{
		QualitySettingsIndex = QualitySettings.GetQualityLevel ();
		//QualitySettingsIndex = data.QualitySettingsIndex;

		if (QualitySettingsIndex == 0) 
		{
			VisualSettingsComponent.enabled = false;
			useHdr = false;
			sunShaftsEnabled = false;
		}

		if (QualitySettingsIndex > 0) 
		{
			if (VisualSettingsComponent != null)
			{
				VisualSettingsComponent.enabled = true;
			}
			useHdr = true;
			sunShaftsEnabled = true;
		}

		mouseSensitivity = data.mouseSensitivity;

		MasterVolume = data.MasterVolume;
		SoundtrackVolume = data.SoundtrackVolume;
		EffectsVolume = data.EffectsVolume;
	}

	// Puts new data into relevant scripts.
	void StoreSettingsDataInGame ()
	{
		QualitySettings.SetQualityLevel (QualitySettingsIndex);
		CheckAndApplyQualitySettings ();

		if (framerateScript != null) 
		{
			framerateScript.isVisible = false;
		}

		if (PlayerController.instance != null) 
		{
			FirstPersonController.instance.m_MouseLook.XSensitivity = mouseSensitivity;
			FirstPersonController.instance.m_MouseLook.YSensitivity = mouseSensitivity;
		}

		AudioListener.volume = Mathf.Clamp (MasterVolume, 0, 1);
	}

	void CheckAndApplyQualitySettings ()
	{
		if (QualitySettingsIndex == 0) 
		{
			VisualSettingsComponent.enabled = false;
			useHdr = false;
			sunShaftsEnabled = false;

			if (GameController.instance != null) 
			{
				GameController.instance.terrain.detailObjectDensity = 0;
				GameController.instance.terrain.treeMaximumFullLODCount = 0;
				GameController.instance.terrain.treeDistance = 1;
				GameController.instance.terrain.heightmapPixelError = 20;
			}

			cam.GetComponent<VolumetricLightRenderer> ().enabled = false;
		}

		if (QualitySettingsIndex == 1) 
		{
			VisualSettingsComponent.enabled = true;
			useHdr = true;
			sunShaftsEnabled = true;

			if (GameController.instance != null) 
			{
				GameController.instance.terrain.detailObjectDensity = 0.2f;
				GameController.instance.terrain.treeMaximumFullLODCount = 50;
				GameController.instance.terrain.treeDistance = 2000;
				GameController.instance.terrain.heightmapPixelError = 1;
			}

			cam.GetComponent<VolumetricLightRenderer> ().enabled = true;
		}
			
		if (cam != null) 
		{
			cam.allowHDR = useHdr;
			cam.GetComponent<SunShafts> ().enabled = sunShaftsEnabled;
		}
	
	}

	// Variables stored in data files.
	[Serializable]
	public class playerData
	{
		public string Username;
		public int levelOneDifficulty;
	}

	[Serializable]
	public class settingsData
	{
		public int QualitySettingsIndex;
		public bool useHdr;
		public bool sunShaftsEnabled;

		public int GrassDensity;

		public float mouseSensitivity;

		public float MasterVolume;
		public float SoundtrackVolume;
		public float EffectsVolume;
	}
}