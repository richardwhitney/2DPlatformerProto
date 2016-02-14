using UnityEngine;
using System.Collections;

public class PlayerBounds : MonoBehaviour {

	public enum BoundsBehaviour { Nothing, Constrain, Kill }
	
	public BoxCollider2D bounds;
	public BoundsBehaviour above, below, left, right;
	
	private Player player;
	private BoxCollider2D boxCollider;
	

	// Use this for initialization
	void Start () {
		player = GetComponent<Player>();
		boxCollider = GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (player.isDead) {
			return;
		} 
		
		Vector2 colliderSize = new Vector2(boxCollider.size.x * Mathf.Abs(transform.localScale.x), boxCollider.size.y * Mathf.Abs(transform.localScale.y)) / 2;
		
		if (above != BoundsBehaviour.Nothing && transform.position.y + colliderSize.y > bounds.bounds.max.y) {
			ApplyBoundsBehaviour(above, new Vector2(transform.position.x, bounds.bounds.max.y - colliderSize.y));
		}
		if (below != BoundsBehaviour.Nothing && transform.position.y - colliderSize.y < bounds.bounds.min.y) {
			ApplyBoundsBehaviour(below, new Vector2(transform.position.x, bounds.bounds.min.y + colliderSize.y));
		}
		if (right != BoundsBehaviour.Nothing && transform.position.x + colliderSize.x > bounds.bounds.max.x) {
			ApplyBoundsBehaviour(right, new Vector2(bounds.bounds.max.x - colliderSize.x, transform.position.y));
		}
		if (left != BoundsBehaviour.Nothing && transform.position.x - colliderSize.x < bounds.bounds.min.x) {
			ApplyBoundsBehaviour(left, new Vector2(bounds.bounds.min.x + colliderSize.x, transform.position.y));
		}
	}
	
	void ApplyBoundsBehaviour(BoundsBehaviour behaviour, Vector2 constrainedPosition) {
		if (behaviour == BoundsBehaviour.Kill) {
			LevelManager.instance.KillPlayer();
			return;
		}
		
		transform.position = constrainedPosition;
	}
}
