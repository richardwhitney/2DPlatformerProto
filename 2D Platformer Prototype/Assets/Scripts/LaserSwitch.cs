using UnityEngine;
using System.Collections;

public class LaserSwitch : MonoBehaviour {

	public enum Switch { on, off }
	
	public Switch switchState;
	public Sprite onSprite;
	public Sprite offSprite;
	public ContinuousLaser controllableLaser;
	
	private SpriteRenderer sRenderer;

	// Use this for initialization
	void Start () {
		sRenderer = GetComponent<SpriteRenderer>();
		switch (switchState) {
			case Switch.on:
				sRenderer.sprite = onSprite;
				controllableLaser.canFire = true;
				break;
			case Switch.off:
				sRenderer.sprite = offSprite;
				controllableLaser.canFire = false;
				break;
			default: 
				break;
		}
	}
	
	void OnTriggerEnter2D(Collider2D other) {
		Debug.Log("Collision with switch occured");
		Player player = other.GetComponent<Player>();
		if (player) {
			switch (switchState) {
				case Switch.on:
					sRenderer.sprite = offSprite;
					controllableLaser.canFire = false;
					switchState = Switch.off;
					break;
				case Switch.off:
					sRenderer.sprite = onSprite;
					controllableLaser.canFire = true;
					switchState = Switch.on;
					break;
				default:
					break;
			}
		}
	}
}
