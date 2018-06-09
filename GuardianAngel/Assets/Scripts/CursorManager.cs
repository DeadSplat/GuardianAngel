using UnityEngine;

public class CursorManager : MonoBehaviour
{
	void Start ()
	{
		HideCursor ();
	}

	public void ShowCursor ()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}

	public void HideCursor ()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
}