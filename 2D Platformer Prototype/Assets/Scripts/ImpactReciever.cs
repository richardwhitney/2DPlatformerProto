using UnityEngine;
using System.Collections;

public class ImpactReciever : MonoBehaviour {

	float mass = 3.0f;
	Vector3 impact = Vector3.zero;
	private Controller2D controller;
	private Player player;

	// Use this for initialization
	void Start () {
		controller = GetComponent<Controller2D>();
		player = GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
		if (impact.magnitude > 0.2f) {
			controller.Move(impact * Time.deltaTime, controller.collisions.below);
		} else {
			player.knockBack = false;
		}
		impact = Vector3.Lerp(impact, Vector3.zero, 5 * Time.deltaTime);
	}

	public void AddImpact(Vector3 direction, float force) {
		direction.Normalize();
		player.knockBack = true;

		if (direction.y < 0) {
			direction.y = - direction.y;
		}
		impact += direction.normalized * force / mass;
	}
}
