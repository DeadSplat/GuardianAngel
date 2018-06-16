using System.Collections;
using UnityEngine;

public class MenuJumpscare : MonoBehaviour 
{
	public Vector2 JumpscareTimes = new Vector2 (30, 60);
	public float ThisJumpscareTime;
	public GameObject JumpscareObject;
	public SimpleEvent JumpScareEvents;

	void Start ()
	{
		StartCoroutine (Jumpscare ());
	}

	IEnumerator Jumpscare ()
	{
		ThisJumpscareTime = Random.Range (JumpscareTimes.x, JumpscareTimes.y);
		yield return new WaitForSecondsRealtime (ThisJumpscareTime);
		JumpscareObject.SetActive (true);
		JumpScareEvents.DoEvents ();
		yield return new WaitForSecondsRealtime (1);
		JumpscareObject.SetActive (false);
		StartCoroutine (Jumpscare ());
	}
}