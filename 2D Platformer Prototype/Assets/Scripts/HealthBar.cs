using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public Player player;
	public Transform foregroundSprite;
	public SpriteRenderer foregroundRenderer;
	public Color maxHealthColor = new Color(255/255f, 63/255f, 63/255f);
	public Color minHealthColor = new Color(64/255f, 137/255f, 255/255f);

	// Use this for initialization
	void Start () {

	}

	public void Update() {
		float healthPercent = player.health / (float)player.maxHealth;

		foregroundSprite.localScale = new Vector3(healthPercent, 1, 1);
		foregroundRenderer.color = Color.Lerp(maxHealthColor, minHealthColor, healthPercent);
	}
	
}
