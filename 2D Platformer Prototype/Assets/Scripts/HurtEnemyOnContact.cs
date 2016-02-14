using UnityEngine;
using System.Collections;

public class HurtEnemyOnContact : MonoBehaviour {

	public int damageToGive;
	public float bounceAmount;
	
	private Player player;
	
	void Start() {
		player = transform.parent.GetComponent<Player>();
	
		if (!player) {
			Debug.Log("Player component was not found.");
		}
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		SimpleEnemyAi enemy = other.GetComponent<SimpleEnemyAi>();
		if (enemy == null) {
			return;
		}
		enemy.TakeDamage(damageToGive, gameObject);
		player.velocity.y = bounceAmount;
	}
}
