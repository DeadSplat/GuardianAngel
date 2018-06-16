using UnityEngine;
using UnityEngine.Events;

public class CutsceneEvent : MonoBehaviour 
{
	public GameController gameControllerScript;
	public UnityEvent OnBeginCutscene;
	public UnityEvent OnEndCutscene;

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			EndCutscene ();
		}
	}

	public void BeginCutscene ()
	{
		OnBeginCutscene.Invoke ();
	}

	public void EndCutscene ()
	{
		OnEndCutscene.Invoke ();
	}

	public void CompleteCutscene ()
	{
		gameControllerScript.isCutsceneComplete = true;
	}
}