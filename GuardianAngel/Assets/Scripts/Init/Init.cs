using UnityEngine;

public class Init : MonoBehaviour 
{
	[Tooltip ("Managers Prefab")]
	public GameObject ManagersPrefab;

	void Start ()
	{
		Time.timeScale = 1;
		DetectManagers ();
	}

	public void DetectManagers ()
	{
		// If there are no MANAGERS GameObject present,
		// Create one and make it not destroy on load.
		if (GameObject.Find ("MANAGERS") == null)
		{
			GameObject managers = Instantiate (ManagersPrefab, Vector3.zero, Quaternion.identity);
			managers.name = "MANAGERS";
			DontDestroyOnLoad (managers.gameObject);
		}
	}
}