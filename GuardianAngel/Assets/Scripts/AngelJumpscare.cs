using UnityEngine;

public class AngelJumpscare : MonoBehaviour 
{
	public Renderer[] Eyes;

	void Start ()
	{
		Invoke ("UpdateEyes", 3);
	}

	void UpdateEyes ()
	{
		foreach (Renderer eye in Eyes)
		{
			eye.material = 
				SaveAndLoadScript.instance.LevelOneDifficulties [SaveAndLoadScript.instance.levelOneDifficulty].EyeMaterial;
		}
	}
}
