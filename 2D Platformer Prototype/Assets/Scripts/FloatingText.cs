using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FloatingText : MonoBehaviour {

	private IFloatingTextPositioner positioner;
	private Vector2 position;

	public static FloatingText Show(GameObject text, string textToShow, Transform t, IFloatingTextPositioner positioner) {
		Debug.Log("Show Text");		
		GameObject go = Instantiate(text, t.position, t.rotation) as GameObject;
		FloatingText floatingText = go.AddComponent<FloatingText>();
		Text test = go.GetComponentInChildren<Text>();
		test.text = textToShow;
		floatingText.position = (Vector2)go.transform.position;
		floatingText.positioner = positioner;

		return floatingText;
	}

	// Update is called once per frame
	void Update () {
		if (!positioner.GetPosition(ref position)) {
			Destroy(gameObject);
			return;
		}
		gameObject.transform.position = new Vector3(position.x, position.y, gameObject.transform.position.z);
	}
}
