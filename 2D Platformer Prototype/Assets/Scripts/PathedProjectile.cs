using UnityEngine;
using System.Collections;

public class PathedProjectile : MonoBehaviour {

	private Transform _destination;
	private float _speed;
	public GameObject destroyEffect;

	public void Initalise(Transform destination, float speed) {
		_destination = destination;
		_speed = speed;
	}

	public void Update() {
		transform.position = Vector3.MoveTowards(transform.position, _destination.position, Time.deltaTime * _speed);

		float distanceSquared = (_destination.transform.position - transform.position).sqrMagnitude;
		if (distanceSquared > 0.01f * 0.01f) {
			return;
		}
		if (destroyEffect != null) {
			CreatEffect();
		}
	}

	public void CreatEffect() {
		Instantiate(destroyEffect, transform.position, transform.rotation);
		Destroy(gameObject);
	}
}
