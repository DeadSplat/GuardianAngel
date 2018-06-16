using UnityEngine;
using UnityEngine.Events;

public class SimpleEvent : MonoBehaviour 
{
	public UnityEvent Events;

	public void DoEvents ()
	{
		Events.Invoke ();
	}
}