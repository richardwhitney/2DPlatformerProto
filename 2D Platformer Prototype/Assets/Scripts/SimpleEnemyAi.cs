using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class SimpleEnemyAi : MonoBehaviour, ITakeDamage, IPlayerRespawnListener {

	public float moveSpeed = 1;
	public GameObject destroyedEffect;
	public GameObject textPrefab;
	public int pointsToGivePlayer;
	
	private Controller2D controller;
	private Vector2 direction;
	private Vector2 startPosition;
	public Vector3 velocity;

	public void Start() {
		controller = GetComponent<Controller2D>();
		direction = new Vector2(-1, 0);
		startPosition = transform.position;
		velocity = new Vector3(moveSpeed, 0, 0);
		velocity.x *= direction.x;
	}

	public void Update() {
		controller.Move(velocity * Time.deltaTime, direction);

		if ((direction.x < 0 && controller.collisions.left) || (direction.x > 0 && controller.collisions.right)) {
			Debug.Log("Flip Enemy");
			Flip();
		}
	}

	public void TakeDamage(int damage, GameObject instigator) {
		if (pointsToGivePlayer != 0) {
			Projectile projectile = instigator.GetComponent<Projectile>();
			if (projectile != null && projectile.owner.GetComponent<Player>() != null) {
				GameManager.instance.AddPoints(pointsToGivePlayer);
				FloatingText.Show(textPrefab, "+" + pointsToGivePlayer.ToString(), gameObject.transform, new FromWorldPointTextPositioner(1.0f, 1.0f));
			}
		}

		Instantiate(destroyedEffect, transform.position, transform.rotation);
		gameObject.SetActive(false);
	}

	public void OnPlayerRespawnInThisCheckpoint(Checkpoint checkpoint, Player player) {
		direction = new Vector2(-1, 0);
		transform.localScale = new Vector3(1, 1, 1);
		transform.position = startPosition;
		gameObject.SetActive(true);
	}

	void Flip() {
		direction = -direction;
		Vector3 spriteScale = transform.localScale;
		spriteScale.x *= -1;
		transform.localScale = spriteScale;
		velocity.x = moveSpeed * direction.x;
	}
}
