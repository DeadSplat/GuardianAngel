using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothDampAngle : MonoBehaviour 
{
	public Vector3 Angle;
	public Vector3 smoothTime;
	public Transform Reference;

	public updateMode UpdateMode;
	public enum updateMode
	{
		Update,
		FixedUpdate,
		LateUpdate
	}

	void Update () 
	{
		if (UpdateMode == updateMode.Update) {
			Angle = new Vector3 (
				Mathf.LerpAngle (transform.eulerAngles.x, Reference.eulerAngles.x, smoothTime.x * Time.deltaTime),
				Mathf.LerpAngle (transform.eulerAngles.y, Reference.eulerAngles.y, smoothTime.y * Time.deltaTime),
				Mathf.LerpAngle (transform.eulerAngles.z, Reference.eulerAngles.z, smoothTime.z * Time.deltaTime)
			);

			transform.rotation = Quaternion.Euler (Angle);
		}
	}

	void FixedUpdate () 
	{
		if (UpdateMode == updateMode.FixedUpdate) {
			Angle = new Vector3 (
				Mathf.LerpAngle (transform.eulerAngles.x, Reference.eulerAngles.x, smoothTime.x * Time.deltaTime),
				Mathf.LerpAngle (transform.eulerAngles.y, Reference.eulerAngles.y, smoothTime.y * Time.deltaTime),
				Mathf.LerpAngle (transform.eulerAngles.z, Reference.eulerAngles.z, smoothTime.z * Time.deltaTime)
			);

			transform.rotation = Quaternion.Euler (Angle);
		}
	}

	void LateUpdate () 
	{
		if (UpdateMode == updateMode.LateUpdate) {
			Angle = new Vector3 (
				Mathf.LerpAngle (transform.eulerAngles.x, Reference.eulerAngles.x, smoothTime.x * Time.deltaTime),
				Mathf.LerpAngle (transform.eulerAngles.y, Reference.eulerAngles.y, smoothTime.y * Time.deltaTime),
				Mathf.LerpAngle (transform.eulerAngles.z, Reference.eulerAngles.z, smoothTime.z * Time.deltaTime)
			);

			transform.rotation = Quaternion.Euler (Angle);
		}
	}
}
