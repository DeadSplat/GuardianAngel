using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LocalSceneLoader : MonoBehaviour 
{
	[Tooltip ("Scene is going to laod another scene.")]
	public bool SceneLoadCommit;
		
	public void LoadScene (string sceneName)
	{
		if (SceneLoadCommit == false)
		{
			SceneLoadCommit = true;
			SceneLoader.instance.SceneName = sceneName;
			StartCoroutine (SceneLoadSequence ());
		}

		return;
	}

	IEnumerator SceneLoadSequence ()
	{
		SceneLoader.instance.StartLoadSequence ();
		yield return null;
	}
}