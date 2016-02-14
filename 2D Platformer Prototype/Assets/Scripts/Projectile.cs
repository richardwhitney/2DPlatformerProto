using UnityEngine;
using System.Collections;

public abstract class Projectile : MonoBehaviour {

	public float speed;
	public LayerMask collisionMask;
	public GameObject owner {
		get;
		private set;
	}
	public Vector2 direction {
		get;
		private set;
	}
	public Vector2 initialVelocity {
		get;
		private set;
	}

	public void Initialise(GameObject owner, Vector2 direction, Vector2 initialVelocity) {
		transform.right = direction;

		this.owner = owner;
		this.direction = direction;
		this.initialVelocity = initialVelocity;
		OnInitialised();
	}

	protected virtual void OnInitialised() {

	}

	public virtual void OnTriggerEnter2D(Collider2D other) {
		if ((collisionMask.value & (1 << other.gameObject.layer)) == 0) {
			OnNotCollideWith(other);
			return;
		}
		bool isOwner = other.gameObject == owner;
		if (isOwner) {
			OnCollideOwner();
			return;
		}
		ITakeDamage takeDamage = (ITakeDamage)other.GetComponent(typeof(ITakeDamage));
		if (takeDamage != null) {
			OnCollideTakeDamage(other, takeDamage);
			return;
		}
		OnCollideOther(other);
	}

	protected virtual void OnNotCollideWith(Collider2D other) {

	}

	protected virtual void OnCollideOwner() {

	}

	protected virtual void OnCollideTakeDamage(Collider2D other, ITakeDamage takeDamage) {

	}

	protected virtual void OnCollideOther(Collider2D other) {

	}
}
