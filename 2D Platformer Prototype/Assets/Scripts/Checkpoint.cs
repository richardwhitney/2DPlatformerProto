using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Checkpoint : MonoBehaviour {

	Animator animator;
	List<IPlayerRespawnListener> listeners;
	public GameObject textPrefab;

	public void Awake() {
		listeners = new List<IPlayerRespawnListener>();
	}

	public void Start() {
		animator = GetComponent<Animator>();
	}

	public void PlayerHitCheckpoint() {
		StartCoroutine(PlayerHitCheckpointCo(LevelManager.instance.currentTimeBonus));
		animator.SetBool("isActive", true);
	}

	private IEnumerator PlayerHitCheckpointCo(int bonus) {
		FloatingText.Show(textPrefab, "CheckPoint!", gameObject.transform, new FromWorldPointTextPositioner(1.0f, 1f));
		yield return new WaitForSeconds(1f);
		FloatingText.Show(textPrefab, "+" + bonus, gameObject.transform, new FromWorldPointTextPositioner(1.0f, 1f));
	}

	public void PlayerLeftCheckpoint() {
		animator.SetBool("isActive", false);
	}

	public void SpawnPlayer(Player player) {
		player.RespawnAt(transform);

		Debug.Log("Spawn Player");

		foreach(IPlayerRespawnListener listener in listeners) {
			Debug.Log("RespawnListener");
			listener.OnPlayerRespawnInThisCheckpoint(this, player);
		}
	}

	public void AssignObjectToCheckpoint(IPlayerRespawnListener listener) {
		listeners.Add(listener);
	}
	

}
