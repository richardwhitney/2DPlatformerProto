using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Controller2D))]
public class Player : MonoBehaviour {

	public float maxJumpHeight = 4;
	public float minJumpHeight = 1;
	public float timeToJumpApex = 0.4f;
	public int maxHealth = 100;
	public GameObject textPrefab;

	public int health {
		get;
		private set;
	}

	public GameObject OuchEffect;
	public Projectile projectile;
	public float fireRate;
	public Transform ProjectileFireLocation;
	private float canFireIn;
	public bool knockBack = false;
	public bool onLadder = false;
	bool ignoreLadderForJump = false;

	float accelerationTimeAirborn = 0.2f;
	float accelerationTimeGrounded = 0.1f;
	float accelerationTimeLadder = 0.1f;
	float moveSpeed = 6;

	public Vector2 wallJumpClimb;
	public Vector2 wallJumpOff;
	public Vector2 wallLeap;

	public float wallSlideSpeedMax = 3;
	public float wallStickTime = 0.25f;
	float timeToWallUnstick;

	float gravity;
	float maxJumpVelocity;
	float minJumpVelocity;
	public Vector3 velocity;
	float velocityXSmoothing;
	float velocityYSmoothing;
	bool facingRight;
	public float climbSpeed = 3;
	float climbVelocity;
	float gravityStore;

	Controller2D controller;
	Animator animator;

	public bool isDead {
		get;
		private set;
	}

	// Use this for initialization
	public void Start () {
		controller = GetComponent<Controller2D>();
		animator = GetComponent<Animator>();

		gravity = -(2 * maxJumpHeight)/Mathf.Pow(timeToJumpApex, 2);
		maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
		minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);

		health = maxHealth;
		
		gravityStore = gravity;
		
		Debug.Log("Gravity: " + gravity);
	}
	
	// Update is called once per frame
	public void Update () {

		Vector2 input;

		canFireIn -= Time.deltaTime;

		if (!isDead) {
			input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
			ControllerInput(input);
		} else {
			input = Vector2.zero;
			velocity.x = 0;
		}

		if (knockBack) {
			velocity = Vector2.zero;
		}

		velocity.y += gravity * Time.deltaTime;
		
		controller.Move(velocity * Time.deltaTime, input);

		if (controller.collisions.above || controller.collisions.below) {
			velocity.y = 0;
		}

		// Note - using velocity.x makes the run animation play too long after input key released (caused by smooth damp)
		animator.SetFloat("Velocity", Mathf.Abs(input.x));
		animator.SetBool("Grounded", controller.collisions.below);
	}

	public void Kill() {
		gameObject.GetComponent<Renderer>().enabled = false;
		health = 0;
		isDead = true;
	}

	public void RespawnAt(Transform spawnPoint) {
		gameObject.GetComponent<Renderer>().enabled = true;
		if (!facingRight) {
			Flip();
		}

		isDead = false;
		health = maxHealth;
		transform.position = spawnPoint.position;
	}

	public void TakeDamage(int damage) {
		FloatingText.Show(textPrefab, "-" + damage.ToString(), gameObject.transform, new FromWorldPointTextPositioner(1.5f, 1.5f));
		if (OuchEffect != null) {
			Instantiate(OuchEffect, transform.position, transform.rotation);
		}
		health -= damage;

		if (health <= 0) {
			LevelManager.instance.KillPlayer();
		}
	}
	
	public void GiveHealth(int healthToGive, GameObject instagator) {
		FloatingText.Show(textPrefab, "+" + health.ToString(), gameObject.transform, new FromWorldPointTextPositioner(1.0f, 1.0f));
		health = Mathf.Min(health + healthToGive, maxHealth);
	}

	void ControllerInput(Vector2 input) {
		int wallDirX = (controller.collisions.left) ? -1 : 1;
		
		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborn);
		
		if (input.x < 0 && !facingRight) {
			Flip();
		} else if (input.x > 0 && facingRight) {
			Flip();
		}
		
		bool wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0) {
			wallSliding = true;
			
			if (velocity.y < -wallSlideSpeedMax) {
				velocity.y = - wallSlideSpeedMax;
			}
			
			if (timeToWallUnstick > 0) {
				velocityXSmoothing = 0;
				velocity.x = 0;
				
				if (input.x != wallDirX && input.x != 0) {
					timeToWallUnstick -= Time.deltaTime;
				} else {
					timeToWallUnstick = wallStickTime;
				}
			} else {
				timeToWallUnstick = wallStickTime;
			}
		}
		
		if (onLadder) {
			if (!controller.collisions.below) {
				gravity = 0.0f;
			} else {
				gravity = gravityStore;
			}
			if (!ignoreLadderForJump) {
				velocity.y = input.y * climbSpeed;
			}
			animator.SetBool("On Ladder", true);
			if (input.y != 0) {
				animator.SetBool("Is Climbing", true);
			} else {
				animator.SetBool("Is Climbing", false);
			}

			//velocity.y = Mathf.SmoothDamp(velocity.y, targetVelocityY, ref velocityYSmoothing, accelerationTimeLadder);
		}
		if (!onLadder) {
			gravity = gravityStore;
			ignoreLadderForJump = false;
			animator.SetBool("On Ladder", false);
		}
		
		if (Input.GetKeyDown(KeyCode.Space)) {
			if (wallSliding) {
				if (wallDirX == input.x) {
					velocity.x = -wallDirX * wallJumpClimb.x;
					velocity.y = wallJumpClimb.y;
				} else if (input.x == 0) {
					velocity.x = -wallDirX * wallJumpOff.x;
					velocity.y = wallJumpOff.y;
				} else {
					velocity.x = -wallDirX * wallLeap.x;
					velocity.y = wallLeap.y;
				}
			}
			if (onLadder) {
				ignoreLadderForJump = true;
				velocity.y = maxJumpVelocity;
			}
			if (controller.collisions.below) {
				velocity.y = maxJumpVelocity;
			}
		}
		if (Input.GetKeyUp(KeyCode.Space)) {
			if (velocity.y > minJumpVelocity) 
				velocity.y = minJumpVelocity;
		}
		if (Input.GetMouseButtonDown(0)) {
			FireProjectile();
		}
	}

	void FireProjectile() {
		if (canFireIn >0) {
			return;
		}
		Vector2 direction = facingRight ? -Vector2.right : Vector2.right;
		Projectile projectile = (Projectile)Instantiate(this.projectile, ProjectileFireLocation.position, ProjectileFireLocation.rotation);
		projectile.Initialise(gameObject, direction, velocity);
		canFireIn = fireRate;
	}

	void Flip() {
		facingRight = !facingRight;
		Vector3 spriteScale = transform.localScale;
		spriteScale.x *= -1;
		transform.localScale = spriteScale;
	}
	
	bool canLadderJump(Vector2 input) {
		if (onLadder && input.x != 0) {
			return true;
		}
		return false;
	}
}
