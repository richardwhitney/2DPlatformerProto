using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class HealthPack : MonoBehaviour, IPlayerRespawnListener {

	public float moveSpeed = 1;
	public int healthToGivePlayer;
	public GameObject effect;
	
	private Controller2D controller;
	private Vector2 direction;
	private Vector2 startPosition;
	public Vector3 velocity;
	private float gravity;
	private int obstacileLayer = 9;
	private int voidLayer = 13;
	private int obstacileMask;
	private int voidMask;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D>();
		
		direction = new Vector2(1, 0);
		startPosition = transform.position;
		velocity = new Vector3(moveSpeed, 10, 0);
		velocity.x *= direction.x;
		gravity = -25;
		
		obstacileMask = 1 << obstacileLayer;
		voidMask = 1 << voidLayer;
		controller.collisionMask = voidMask;
	}
	
	// Update is called once per frame
	void Update () {
		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime, direction);
		
		if (velocity.y <= 0) {
			ChangeMask();
		}
		
		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}
		if ((direction.x < 0 && controller.collisions.left) || (direction.x > 0 && controller.collisions.right)) {
			Flip();
		}
	}
	
	void Flip() {
		direction = -direction;
		Vector3 spriteScale = transform.localScale;
		spriteScale.x *= -1;
		transform.localScale = spriteScale;
		velocity.x = moveSpeed * direction.x;
	}
	
	void ChangeMask() {
		if (obstacileMask != controller.collisionMask) {
			controller.collisionMask = obstacileMask;
		}
	}
	
	public void OnTriggerEnter2D(Collider2D other) {
		Player player = other.GetComponent<Player>();
		if (player == null) {
			return;
		}
		player.GiveHealth(healthToGivePlayer, gameObject);
		Instantiate(effect, transform.position, transform.rotation);
		
		gameObject.SetActive(false);
	}
	
	public void OnPlayerRespawnInThisCheckpoint(Checkpoint checkpoint, Player player) {
	
	}
}
