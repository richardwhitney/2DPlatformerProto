using UnityEngine;
using System.Collections;

public class GivePlayerDamage : MonoBehaviour {

	public int damageToGive = 10;

	private Vector2 lastPosition, velocity;

	// Update is called once per frame
	void LateUpdate () {
		velocity = (lastPosition - (Vector2)transform.position) / Time.deltaTime;
		lastPosition = transform.position;
	}

	public void OnTriggerEnter2D(Collider2D other) {
		Player player = other.GetComponent<Player>();
		if (player == null) {
			return;
		}
		PathedProjectile projectile = gameObject.GetComponent<PathedProjectile>();
		if (projectile != null) {
			projectile.CreatEffect();
		}

		Debug.Log("Hit Player");
		player.TakeDamage(damageToGive);
		Controller2D controller = player.GetComponent<Controller2D>();
		Vector2 totalVelocity = (Vector2)player.velocity + velocity;

		ImpactReciever impactReciever = player.GetComponent<ImpactReciever>();
		Vector3 direction = player.transform.position - transform.position;
		impactReciever.AddImpact(direction, Mathf.Clamp(Mathf.Abs(totalVelocity.magnitude), 10, 20));

	}
}
