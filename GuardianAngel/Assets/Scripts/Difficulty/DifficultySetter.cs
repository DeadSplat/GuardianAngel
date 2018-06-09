using UnityEngine;

public class DifficultySetter : MonoBehaviour
{
	public SaveAndLoadScript saveAndLoadScript;

	void Start ()
	{
		saveAndLoadScript = GameObject.Find ("SaveAndLoad").GetComponent<SaveAndLoadScript> ();
	}

	public void SetDifficultyLevelOne (int Difficulty)
	{
		saveAndLoadScript.levelOneDifficulty = Difficulty;
		saveAndLoadScript.SavePlayerData ();
	}
}