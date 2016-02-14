using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Coin : MonoBehaviour, IPlayerRespawnListener {

	public GameObject effect;
	public int pointsToAdd = 10;
	public GameObject textPrefab;

	public void Start() {
		//text.text = pointsToAdd.ToString();
	}

	public void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag != "Player") {
			return;
		}
		GameManager.instance.AddPoints(pointsToAdd);
		Instantiate(effect, transform.position, transform.rotation);

		gameObject.SetActive(false);

		FloatingText.Show(textPrefab, "+" + pointsToAdd.ToString(), gameObject.transform, new FromWorldPointTextPositioner(1.0f, 1f));
	}

	public void OnPlayerRespawnInThisCheckpoint(Checkpoint checkpoint, Player player) {
		Debug.Log("It worked");
		gameObject.SetActive(true);
	}
}
