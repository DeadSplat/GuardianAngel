using UnityEngine;
using UnityEngine.Events;

public class BunkerZone : MonoBehaviour 
{
	public UnityEvent events;

	void OnTriggerEnter (Collider other)
	{
		if (other.name == "Player")
		{
			events.Invoke ();
		}
	}
}