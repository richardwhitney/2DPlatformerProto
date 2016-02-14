using UnityEngine;
using System.Collections;

public class SimpleProjectile : Projectile, ITakeDamage {

	public int damage;
	public GameObject destroyedEffect;
	public int pointsToGiveToPlayer;
	public float timeToLive;

	public void Update() {
		if ((timeToLive -= Time.deltaTime) <= 0) {
			DestroyProjectile();
			return;
		}
		transform.Translate(direction * ((Mathf.Abs(initialVelocity.x) + speed) * Time.deltaTime), Space.World);
	}

	public void TakeDamage(int damage, GameObject instigator) {
		if (pointsToGiveToPlayer != 0) {
			Projectile proj = instigator.GetComponent<Projectile>();
			Player player = proj.GetComponent<Player>();
			if (proj != null && player != null) {
				GameManager.instance.AddPoints(pointsToGiveToPlayer);
				FloatingText.Show(player.textPrefab, "+" + pointsToGiveToPlayer.ToString(), gameObject.transform, new FromWorldPointTextPositioner(1.5f, 1.5f));
			}
		}
		DestroyProjectile();
	}

	protected override void OnCollideOther(Collider2D other) {
		DestroyProjectile();
	}

	protected override void OnCollideTakeDamage(Collider2D other, ITakeDamage takeDamage) {
		takeDamage.TakeDamage(damage, gameObject);
		DestroyProjectile();
	}

	private void DestroyProjectile() {
		if (destroyedEffect != null) {
			Instantiate(destroyedEffect, transform.position, transform.rotation);
		}
		Destroy(gameObject);
	}
}
