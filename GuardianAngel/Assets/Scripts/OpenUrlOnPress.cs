using UnityEngine;

public class OpenUrlOnPress : MonoBehaviour 
{
	public void OpenUrl (string site)
	{
		Application.OpenURL (site);
	}
}