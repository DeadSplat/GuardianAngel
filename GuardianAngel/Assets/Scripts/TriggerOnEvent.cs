using UnityEngine;
using UnityEngine.Events;

public class TriggerOnEvent : MonoBehaviour
{
	public string Tag = "Player";
	public UnityEvent OnTriggerEnterEvents;
	public UnityEvent OnTriggerStayEvents;
	public UnityEvent OnTriggerExitEvents;

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == Tag) 
		{
			OnTriggerEnterEvents.Invoke ();
		}
	}

	void OnTriggerStay (Collider other)
	{
		if (other.tag == Tag)
		{
			OnTriggerStayEvents.Invoke ();
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == Tag)
		{
			OnTriggerExitEvents.Invoke ();
		}
	}
}
