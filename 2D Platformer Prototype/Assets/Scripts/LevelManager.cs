using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public static LevelManager instance { get; private set; }

	public Player player { 
		get; 
		private set; 
	}

	public CameraFollow camera {
		get;
		private set;
	}

	public TimeSpan runningTime {
		get {
			return DateTime.UtcNow - started;
		}
	}

	public int currentTimeBonus {
		get {
			int secondDifference = (int)(bonusCutoffSeconds - runningTime.TotalSeconds);
			return Mathf.Max(0, secondDifference) * bonusSecondMultiplier;
		}
	}

	public GameObject deathParticle;
	public GameObject respawnParticle;

	private List<Checkpoint> checkpoints;
	private int currentCheckpointIndex;
	private DateTime started;
	private int savedPoints;


	public Checkpoint debugSpawn;
	public int bonusCutoffSeconds;
	public int bonusSecondMultiplier;

	public void Awake() {
		instance = this;
	}

	public void Start() {
		checkpoints = FindObjectsOfType<Checkpoint>().OrderBy(t => t.transform.position.x).ToList();
		currentCheckpointIndex = checkpoints.Count > 0 ? 0 : -1;

		player = FindObjectOfType<Player>();
		camera = FindObjectOfType<CameraFollow>();

		started = DateTime.UtcNow;

		var listeners = FindObjectsOfType<MonoBehaviour>().OfType<IPlayerRespawnListener>();
		foreach (IPlayerRespawnListener listener in listeners) {
			for (int i = checkpoints.Count - 1; i >= 0; i--) {
				float distance = ((MonoBehaviour)listener).transform.position.x - checkpoints[i].transform.position.x;
				if (distance < 0) {
					continue;
				}
				checkpoints[i].AssignObjectToCheckpoint(listener);
				break;
			}
		}

#if UNITY_EDITOR
		if (debugSpawn != null) {
			debugSpawn.SpawnPlayer(player);
		} else if (currentCheckpointIndex != -1) {
			checkpoints[currentCheckpointIndex].SpawnPlayer(player);
		}
#else
		if (currentCheckpointIndex != -1) {
			checkpoints[currentCheckpointIndex].SpawnPlayer(player);
		}
#endif
	}

	public void Update() {
		bool isAtLastCheckpoint = currentCheckpointIndex + 1 >= checkpoints.Count;
		if (isAtLastCheckpoint) {
			return;
		}

		float distanceToNextCheckpoint = checkpoints[currentCheckpointIndex + 1].transform.position.x - player.transform.position.x;
		if (distanceToNextCheckpoint >= 0) {
			return;
		}

		checkpoints[currentCheckpointIndex].PlayerLeftCheckpoint();
		currentCheckpointIndex++;
		checkpoints[currentCheckpointIndex].PlayerHitCheckpoint();

		GameManager.instance.AddPoints(currentTimeBonus);
		savedPoints = GameManager.instance.points;
		started = DateTime.UtcNow;

	}

	public void KillPlayer() {
		StartCoroutine(KillPlayerCo());
	}

	private IEnumerator KillPlayerCo() {
		player.Kill();
		Instantiate(deathParticle, player.transform.position, player.transform.rotation);
		// TODO: Stop camera following player
		yield return new WaitForSeconds(2f);

		// TODO: Start camera following player
		if (currentCheckpointIndex != -1) {
			Instantiate(respawnParticle, checkpoints[currentCheckpointIndex].transform.position, checkpoints[currentCheckpointIndex].transform.rotation);
			checkpoints[currentCheckpointIndex].SpawnPlayer(player);

		}

		// TODO: Points
		started = DateTime.UtcNow;
		GameManager.instance.ResetPoints(savedPoints);
	}
}
