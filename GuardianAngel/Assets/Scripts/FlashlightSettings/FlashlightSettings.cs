using UnityEngine;

[CreateAssetMenu(fileName = "New Flashlight Settings", menuName = "Flashlight/Flashlight Settings", order = 0)]
public class FlashlightSettings : ScriptableObject 
{
	public float Range = 45;
	[Range (1.0f, 179.0f)]
	public float SpotAngle = 45;
	public Color color = new Color (1, 1, 1, 1);
	public float Intensity = 1;
}