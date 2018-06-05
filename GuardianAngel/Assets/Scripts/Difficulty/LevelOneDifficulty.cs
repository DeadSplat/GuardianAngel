using UnityEngine;

[CreateAssetMenu (fileName = "New Level One Difficulty", menuName = "Difficulty Modes/Level 1/Level One Difficulty", order = 1)]
public class LevelOneDifficulty : ScriptableObject
{
	public int Difficulty;

	public Vector2[] FluidAttackSpeeds;
	public Vector2[] TeleportAttackSpeeds;

	public float AttackSpeed = 0.5f;
	public int Damage;
	public Material EyeMaterial;
}