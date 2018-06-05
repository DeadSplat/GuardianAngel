using UnityEngine;

public class Flashlight : MonoBehaviour
{
	public SmoothFollowOrig SmoothFollowScript;
	public Transform NormalPos;
	public Transform ClosePos;
	public float resetDelay;

	public CapsuleCollider Col;
	public float MinHeight = 1.2f;
	public float MaxHeight = 2.4f;


	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Enemy" || other.tag == "Untagged") 
		{
			SmoothFollowScript.target = ClosePos;
			Col.height = MaxHeight;

			if (IsInvoking ("ResetFlashlightPos") == true)
			{
				CancelInvoke ("ResetFlashlightPos");
			}
		}
	}
		
	void OnTriggerStay (Collider other)
	{
		if (other.tag == "Enemy" || other.tag == "Untagged") 
		{
			SmoothFollowScript.target = ClosePos;

			if (IsInvoking ("ResetFlashlightPos") == true)
			{
				CancelInvoke ("ResetFlashlightPos");
			}
		}
	}

	void OnTriggerExit (Collider other)
	{
		if (other.tag == "Enemy" || other.tag == "Untagged") 
		{
			Invoke ("ResetFlashlightPos", resetDelay);
		}
	}

	void ResetFlashlightPos ()
	{
		SmoothFollowScript.target = NormalPos;
		Col.height = MinHeight;
	}
}