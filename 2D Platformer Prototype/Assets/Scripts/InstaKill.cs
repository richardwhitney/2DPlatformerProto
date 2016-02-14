using UnityEngine;
using System.Collections;

public class InstaKill : MonoBehaviour {

	public void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			LevelManager.instance.KillPlayer();
		}
	}
}
