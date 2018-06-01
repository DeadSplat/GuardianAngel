using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothDampAngle : MonoBehaviour 
{
	public Vector3 Angle;
	public Vector3 smoothTime;
	public Transform Reference;

	void LateUpdate () 
	{
		Angle = new Vector3 
			(
				Mathf.LerpAngle (transform.eulerAngles.x, Reference.eulerAngles.x, smoothTime.x * Time.deltaTime),
				Mathf.LerpAngle (transform.eulerAngles.y, Reference.eulerAngles.y, smoothTime.y * Time.deltaTime),
				Mathf.LerpAngle (transform.eulerAngles.z, Reference.eulerAngles.z, smoothTime.z * Time.deltaTime)
			);

		transform.rotation = Quaternion.Euler(Angle);
	}
}
