using UnityEngine;

public class TimeScaleSetter : MonoBehaviour 
{
	public void SetNewTimescale (float Timescale)
	{
		Time.timeScale = Timescale;
	}
}