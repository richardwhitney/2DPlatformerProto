using UnityEngine;
using System.Collections;

public class BackgroundParallax : MonoBehaviour {

    public Transform[] backgrounds;
	public float parallaxScale;
	public float parallaxReductionFactor;
	public float smoothing;

	private Vector2 lastPosition;

	public void Start() {
		lastPosition = transform.position;
	}

	public void Update() {
		float parallax = (lastPosition.x - transform.position.x) * parallaxScale;

		for (int i = 0; i < backgrounds.Length; i++) {
			float backgroundTargetPosition = backgrounds[i].position.x + parallax * (i * parallaxReductionFactor + 1);
			backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, new Vector3(backgroundTargetPosition, backgrounds[i].position.y, backgrounds[i].position.z), smoothing * Time.deltaTime);
		}

		lastPosition = transform.position;
	}
}
