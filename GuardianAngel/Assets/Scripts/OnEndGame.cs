using UnityEngine;
using UnityEngine.Events;

public class OnEndGame : MonoBehaviour 
{
	public UnityEvent OnEndLevel;

	void Start ()
	{
		OnEndLevel.Invoke ();
	}
}