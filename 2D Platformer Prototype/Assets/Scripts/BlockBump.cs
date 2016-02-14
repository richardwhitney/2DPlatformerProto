using UnityEngine;
using System.Collections;

public class BlockBump : MonoBehaviour {

	public enum BlockType  { blockBounce, blockCoin, blockBreakable, blockSolid, blockQuestion }
	public enum PickUpType { pickupHealth, pickupGun, pickupStar }
	public enum BreakType  { breakableGeometry, breakableParticles }

	public BlockType blockState;
	public BlockType blockStateAfter;
	public PickUpType pickupState;
	public BreakType breakState;

	public int blockCoinAmount = 3;
	public float blockQuestionScrollSpeed = 0.5f;

	public Sprite blockSprite1;		
	public Sprite blockSprite2;
	public Sprite blockSprite3;
	public Sprite blockSprite4;

	public Transform pickupCoin;
	public Transform pickupHealth;
	public Transform pickupGun;
	public Transform pickupStar;
	public Transform breakableGeometry;
	public Transform breakableParticles;

	public AudioClip soundBump;
	public AudioClip soundPickup;

	private Vector3 breakablePos; 
	private Vector3 pickupPos;
	private Vector3 cointPos;
	private Vector3 starPos;

	private bool blockAnimation = false;
	private bool coinMove = false;

	private int blockCoinAmountReset;

	private Animator animator;
	private SpriteRenderer sRenderer;


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		sRenderer = GetComponent<SpriteRenderer>();
		cointPos = transform.position;
		pickupPos = transform.position;
		blockCoinAmountReset = blockCoinAmount;
	}

	// Update is called once per frame
	void Update () {
		switch (blockState) {
			case BlockType.blockBounce:
				sRenderer.sprite = blockSprite1;
				if (blockAnimation) {
					animator.SetTrigger("Hit");
					blockAnimation = false;
					// Play Audio
				}
				break;
			case BlockType.blockCoin:
				if (blockAnimation) {
					sRenderer.sprite = blockSprite2;
					animator.SetTrigger("Hit");
					GameObject coin = Instantiate(pickupCoin, cointPos, transform.rotation) as GameObject;
					blockAnimation = false;
					blockCoinAmount--;
					// Play Audio
				}
				if (blockCoinAmount == 0 && blockStateAfter == BlockType.blockBounce) {
					blockState = blockStateAfter;
				}
				if (blockCoinAmount == 0 && blockStateAfter == BlockType.blockCoin) {
					blockState = blockStateAfter;
					blockStateAfter = BlockType.blockBreakable;
				}
				if (blockCoinAmount == 0 && blockStateAfter == BlockType.blockBreakable) {
					blockState = blockStateAfter;
				}
				if (blockCoinAmount == 0 && blockStateAfter == BlockType.blockSolid) {
					blockState = blockStateAfter;
				}
				if (blockCoinAmount == 0 && blockStateAfter == BlockType.blockQuestion) {
					blockState = blockStateAfter;
				}
				break;
			case BlockType.blockBreakable:
				sRenderer.sprite = blockSprite3;
				if (blockAnimation) {
					animator.SetTrigger("Hit");
					if (breakState == BreakType.breakableGeometry) {
						Instantiate(breakableGeometry, transform.position, transform.rotation);
					}
					if (breakState == BreakType.breakableParticles) {
						Instantiate(breakableParticles, transform.position, transform.rotation);
					}
					Destroy(transform.parent.gameObject);
					blockAnimation = false;
				}
				break;
			case BlockType.blockSolid:
				sRenderer.sprite = blockSprite3;
				if (blockAnimation) {
					// Play Audio
					blockAnimation = false;
				}
				break;
			case BlockType.blockQuestion:
				sRenderer.sprite = blockSprite4;
				if (blockAnimation && pickupState == PickUpType.pickupHealth) {
					animator.SetTrigger("Hit");
					Instantiate(pickupHealth, transform.position, transform.rotation);
					// Play Audio
					blockAnimation = false;
					blockState = blockStateAfter;
				}
				if (blockAnimation && pickupState == PickUpType.pickupGun) {
					animator.SetTrigger("Hit");
					Instantiate(pickupGun, transform.position, transform.rotation);
					// Play Audio
					blockAnimation = false;
					blockState = blockStateAfter;
				}
				if (blockAnimation && pickupState == PickUpType.pickupStar) {
					animator.SetTrigger("Hit");
					Instantiate(pickupStar, transform.position, transform.rotation);
					// Play Audio
					blockAnimation = false;
					blockState = blockStateAfter;
				} 
				break;
			default:
				break;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Head Box") {
			blockAnimation = true;
		}
	}
}
