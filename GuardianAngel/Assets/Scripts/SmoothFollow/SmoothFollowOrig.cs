using UnityEngine;

public class SmoothFollowOrig : MonoBehaviour
{
	public updateMode UpdateMode;
	public enum updateMode
	{
		Update,
		FixedUpdate,
		LateUpdate
	}

	public bool OnlyInStart;
	public float SMOOTH_TIME = 0.3f;
	public Vector3 offset;
	
	#region Public Properties
	public bool LockX;
	public bool LockY;
	public bool LockZ;
	public bool useSmoothing;
	public Transform target;
	#endregion
	
	#region Private Properties
	private Transform thisTransform;
	public Vector3 velocity;
	#endregion

	public float xMin, xMax;
	public float yMin, yMax;
	public float zMin, zMax;
	public bool useRB;
	public float lockedPos;
	public bool hasLockedPos;

	private void Start ()
	{
		thisTransform = transform;
		velocity = new Vector3(0.5f, 0.5f, 0.5f);

		if (target == null) 
		{
			target = GameObject.FindGameObjectWithTag ("PlayerOne").transform;
		}
	}

	void start ()
	{
		if (target == null) 
		{
			target = GameObject.FindGameObjectWithTag ("Player").transform;
		}

		if (OnlyInStart == true) 
		{
			if (useRB == true) 
			{
				GetComponent<Transform> ().position = new Vector3
				(
					Mathf.Clamp (GetComponent<Rigidbody> ().position.x, xMin, xMax),
					Mathf.Clamp (GetComponent<Rigidbody> ().position.y, yMin, yMax),
					Mathf.Clamp (GetComponent<Rigidbody> ().position.z, zMin, zMax)
				);
			}

			var newPos = Vector3.zero;
			
			if (useSmoothing) 
			{
				newPos.x = Mathf.SmoothDamp (thisTransform.position.x, target.position.x, ref velocity.x, SMOOTH_TIME);
				newPos.y = Mathf.SmoothDamp(thisTransform.position.y, target.position.y, ref velocity.y, SMOOTH_TIME);
				newPos.z = Mathf.SmoothDamp (thisTransform.position.z, target.position.z, ref velocity.z, SMOOTH_TIME);
			} 

			else 
			
			{
				thisTransform.position = new Vector3 (target.position.x, target.position.y, target.position.z);
			}
			
			#region Locks
			if (LockX) 
			{
				newPos.x = thisTransform.position.x + offset.x;
			}
			
			if (LockY) 
			{
				newPos.y = thisTransform.position.y + offset.y;
			}
			
			if (LockZ) 
			{
				newPos.z = thisTransform.position.z + offset.z;
			}
			#endregion
		}
	}

	public void Update ()
	{
		if (UpdateMode == updateMode.Update) {
			if (hasLockedPos == true) {
				gameObject.GetComponent<Transform> ().position = new Vector3 (lockedPos, gameObject.transform.position.y, gameObject.transform.position.z);
			}
		
			if (OnlyInStart == false) {
				if (useRB == true) {
					GetComponent<Transform> ().position = new Vector3 (
						Mathf.Clamp (GetComponent<Rigidbody> ().position.x, xMin, xMax),
						Mathf.Clamp (GetComponent<Rigidbody> ().position.y, yMin, yMax),
						Mathf.Clamp (GetComponent<Rigidbody> ().position.z, zMin, zMax)
					);
				}

				var newPos = Vector3.zero;
			
				if (useSmoothing) {
					if ((target.position.x >= xMin && target.position.x <= xMax) ||
					   (target.position.y >= yMin && target.position.y <= yMax) ||
					   (target.position.z >= zMin && target.position.z <= zMax)) {
						newPos.x = Mathf.SmoothDamp (Mathf.Clamp (thisTransform.position.x, xMin, xMax), target.position.x, ref velocity.x, SMOOTH_TIME) + offset.x;
						newPos.y = Mathf.SmoothDamp (Mathf.Clamp (thisTransform.position.y, yMin, yMax), target.position.y, ref velocity.y, SMOOTH_TIME) + offset.y;
						newPos.z = Mathf.SmoothDamp (Mathf.Clamp (thisTransform.position.z, zMin, zMax), target.position.z, ref velocity.z, SMOOTH_TIME) + offset.z;
					} else { 
						newPos.x = thisTransform.position.x;
						newPos.y = thisTransform.position.y;
						newPos.z = thisTransform.position.z;
					}
				} else {
					newPos.x = target.position.x + offset.x;
					newPos.y = target.position.y + offset.y;
					newPos.z = target.position.z + offset.z;
				}
			
				#region Locks
				if (LockX) {
					newPos.x = thisTransform.position.x + offset.x;
				}
			
				if (LockY) {
					newPos.y = thisTransform.position.y + offset.y;
				}
			
				if (LockZ) {
					newPos.z = thisTransform.position.z + offset.z;
				}
				#endregion
			
				transform.position = Vector3.Slerp (transform.position, newPos, Time.time);
			}
		}
	}

	public void FixedUpdate ()
	{
		if (UpdateMode == updateMode.FixedUpdate) {
			if (hasLockedPos == true) {
				gameObject.GetComponent<Transform> ().position = new Vector3 (lockedPos, gameObject.transform.position.y, gameObject.transform.position.z);
			}

			if (OnlyInStart == false) {
				if (useRB == true) {
					GetComponent<Transform> ().position = new Vector3 (
						Mathf.Clamp (GetComponent<Rigidbody> ().position.x, xMin, xMax),
						Mathf.Clamp (GetComponent<Rigidbody> ().position.y, yMin, yMax),
						Mathf.Clamp (GetComponent<Rigidbody> ().position.z, zMin, zMax)
					);
				}

				var newPos = Vector3.zero;

				if (useSmoothing) {
					if ((target.position.x >= xMin && target.position.x <= xMax) ||
						(target.position.y >= yMin && target.position.y <= yMax) ||
						(target.position.z >= zMin && target.position.z <= zMax)) {
						newPos.x = Mathf.SmoothDamp (Mathf.Clamp (thisTransform.position.x, xMin, xMax), target.position.x, ref velocity.x, SMOOTH_TIME) + offset.x;
						newPos.y = Mathf.SmoothDamp (Mathf.Clamp (thisTransform.position.y, yMin, yMax), target.position.y, ref velocity.y, SMOOTH_TIME) + offset.y;
						newPos.z = Mathf.SmoothDamp (Mathf.Clamp (thisTransform.position.z, zMin, zMax), target.position.z, ref velocity.z, SMOOTH_TIME) + offset.z;
					} else { 
						newPos.x = thisTransform.position.x;
						newPos.y = thisTransform.position.y;
						newPos.z = thisTransform.position.z;
					}
				} else {
					newPos.x = target.position.x + offset.x;
					newPos.y = target.position.y + offset.y;
					newPos.z = target.position.z + offset.z;
				}

				#region Locks
				if (LockX) {
					newPos.x = thisTransform.position.x + offset.x;
				}

				if (LockY) {
					newPos.y = thisTransform.position.y + offset.y;
				}

				if (LockZ) {
					newPos.z = thisTransform.position.z + offset.z;
				}
				#endregion

				transform.position = Vector3.Slerp (transform.position, newPos, Time.time);
			}
		}
	}

	public void LateUpdate ()
	{
		if (UpdateMode == updateMode.LateUpdate) {
			if (hasLockedPos == true) {
				gameObject.GetComponent<Transform> ().position = new Vector3 (lockedPos, gameObject.transform.position.y, gameObject.transform.position.z);
			}

			if (OnlyInStart == false) {
				if (useRB == true) {
					GetComponent<Transform> ().position = new Vector3 (
						Mathf.Clamp (GetComponent<Rigidbody> ().position.x, xMin, xMax),
						Mathf.Clamp (GetComponent<Rigidbody> ().position.y, yMin, yMax),
						Mathf.Clamp (GetComponent<Rigidbody> ().position.z, zMin, zMax)
					);
				}

				var newPos = Vector3.zero;

				if (useSmoothing) {
					if ((target.position.x >= xMin && target.position.x <= xMax) ||
						(target.position.y >= yMin && target.position.y <= yMax) ||
						(target.position.z >= zMin && target.position.z <= zMax)) {
						newPos.x = Mathf.SmoothDamp (Mathf.Clamp (thisTransform.position.x, xMin, xMax), target.position.x, ref velocity.x, SMOOTH_TIME) + offset.x;
						newPos.y = Mathf.SmoothDamp (Mathf.Clamp (thisTransform.position.y, yMin, yMax), target.position.y, ref velocity.y, SMOOTH_TIME) + offset.y;
						newPos.z = Mathf.SmoothDamp (Mathf.Clamp (thisTransform.position.z, zMin, zMax), target.position.z, ref velocity.z, SMOOTH_TIME) + offset.z;
					} else { 
						newPos.x = thisTransform.position.x;
						newPos.y = thisTransform.position.y;
						newPos.z = thisTransform.position.z;
					}
				} else {
					newPos.x = target.position.x + offset.x;
					newPos.y = target.position.y + offset.y;
					newPos.z = target.position.z + offset.z;
				}

				#region Locks
				if (LockX) {
					newPos.x = thisTransform.position.x + offset.x;
				}

				if (LockY) {
					newPos.y = thisTransform.position.y + offset.y;
				}

				if (LockZ) {
					newPos.z = thisTransform.position.z + offset.z;
				}
				#endregion

				transform.position = Vector3.Slerp (transform.position, newPos, Time.time);
			}
		}
	}

	public void SetTarget (GameObject newTarget)
	{
		target = newTarget.transform;
	}

	public void setSmoothTime (float smoothTime)
	{
		SMOOTH_TIME = smoothTime;
	}
}