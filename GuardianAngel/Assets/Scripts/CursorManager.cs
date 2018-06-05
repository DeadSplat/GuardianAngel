using UnityEngine;

public class CursorManager : MonoBehaviour
{
	public void ShowCursor ()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
	}
}