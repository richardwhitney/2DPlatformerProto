using UnityEngine;
using System.Collections;

public class LadderZone : MonoBehaviour {
	
	void OnTriggerEnter2D(Collider2D other) {
		if (other.name == "Player") {
			Player player = other.GetComponent<Player>();
			player.onLadder = true;
		}
	}
	
	void OnTriggerExit2D(Collider2D other) {
		if (other.name == "Player") {
			Player player = other.GetComponent<Player>();
			player.onLadder = false;
		}
	}
}
