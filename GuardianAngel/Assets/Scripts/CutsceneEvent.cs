using UnityEngine;
using UnityEngine.Events;

public class CutsceneEvent : MonoBehaviour 
{
	public static CutsceneEvent instance { get; private set; }
	public GameController gameControllerScript;
	public UnityEvent OnBeginCutscene;
	public UnityEvent OnEndCutscene;
	public bool cutsceneEnded;

	void Awake ()
	{
		instance = this;
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Space)) 
		{
			EndCutscene ();
		}
	}

	public void BeginCutscene ()
	{
		cutsceneEnded = false;
		OnBeginCutscene.Invoke ();
	}

	public void EndCutscene ()
	{
		OnEndCutscene.Invoke ();
		cutsceneEnded = true;
	}

	public void CompleteCutscene ()
	{
		gameControllerScript.isCutsceneComplete = true;
	}
}