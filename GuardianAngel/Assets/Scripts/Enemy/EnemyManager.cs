using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	public GameController gameControllerScript;
	public LevelOneDifficulty[] LevelOneDifficulty;
	public Transform Player;
	public GameObject[] Enemies;
	public bool enemiesCanSpawn;

	[Header ("Activation")]
	public float nextSpawnTimeRemaining;
	public Vector2[] SpawnTimes;
	public Vector2[] SpawnRadii;

	[Header ("Deactivation")]
	public float nextDeactivateTimeRemaining;
	public Vector2[] DeactivateTimes;

	void Start ()
	{
		SpawnTimes = LevelOneDifficulty [gameControllerScript.thisDifficulty].SpawnTimes;
		SpawnRadii = LevelOneDifficulty [gameControllerScript.thisDifficulty].SpawnRadii;
		DeactivateTimes = LevelOneDifficulty [gameControllerScript.thisDifficulty].DeactivateTimes;
	}

	void Update ()
	{
		SpawnTimer ();
		DeactivateTimer ();
	}

	void SpawnTimer ()
	{
		if (enemiesCanSpawn == true) 
		{
			if (nextSpawnTimeRemaining > 0) 
			{
				nextSpawnTimeRemaining -= Time.deltaTime;
				return;
			} 

			else		
			{
				GetNextSpawnTime ();
				ActivateEnemy ();
			}
		}
	}

	void DeactivateTimer ()
	{
		if (enemiesCanSpawn == true) 
		{
			if (nextDeactivateTimeRemaining > 0) 
			{
				nextDeactivateTimeRemaining -= Time.deltaTime;
				return;
			} 

			else		
			{
				GetNextDeactivateTime ();
				DeactivateEnemy ();
			}
		}
	}

	public void GetNextSpawnTime ()
	{
		nextSpawnTimeRemaining = Random.Range (
			SpawnTimes[gameControllerScript.SwitchesActivated].x, 
			SpawnTimes[gameControllerScript.SwitchesActivated].y
		);
	}

	void ActivateEnemy ()
	{
		int enemyId = Random.Range (0, Enemies.Length); // Get next enemyId.

		float nextSpawnOffsetX = Random.Range (
			SpawnRadii[gameControllerScript.SwitchesActivated].x, 
			SpawnRadii[gameControllerScript.SwitchesActivated].y
		);

		float nextSpawnOffsetZ = Random.Range (
			SpawnRadii[gameControllerScript.SwitchesActivated].x, 
			SpawnRadii[gameControllerScript.SwitchesActivated].y
		);

		// Get new spawn position based on player origin with offset.
		Enemies [enemyId].transform.position = new Vector3 (
			Player.position.x + nextSpawnOffsetX,
			Player.position.y,
			Player.position.z + nextSpawnOffsetZ
		);

		Enemies [enemyId].SetActive (true);
	}

	public void GetNextDeactivateTime ()
	{
		nextDeactivateTimeRemaining = Random.Range (
			DeactivateTimes[gameControllerScript.SwitchesActivated].x, 
			DeactivateTimes[gameControllerScript.SwitchesActivated].y
		);
	}

	void DeactivateEnemy ()
	{
		int enemyId = Random.Range (0, Enemies.Length); // Get next enemyId.
		Enemies [enemyId].SetActive (false);
	}
}