using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LocalSceneLoader : MonoBehaviour 
{
	[Tooltip ("Scene loader from Init.")]
	public SceneLoader sceneLoaderScript;
	[Tooltip ("Scene is going to laod another scene.")]
	public bool SceneLoadCommit;

	void Start ()
	{
		sceneLoaderScript = GameObject.Find ("SceneLoader").GetComponent<SceneLoader> ();
	}
		
	public void LoadScene (string sceneName)
	{
		if (SceneLoadCommit == false)
		{
			SceneLoadCommit = true;
			sceneLoaderScript.SceneName = sceneName;
			StartCoroutine (SceneLoadSequence ());
		}

		return;
	}

	IEnumerator SceneLoadSequence ()
	{
		sceneLoaderScript.StartLoadSequence ();
		yield return null;
	}
}