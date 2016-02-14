using UnityEngine;
using System.Collections;

public class PathedProjectileSpawner : MonoBehaviour {

	public Transform destination;
	public PathedProjectile projectile;
	public GameObject spawnEffect;
	public float speed;
	public float fireRate;

	private float nextShotInSeconds;

	// Use this for initialization
	void Start () {
		nextShotInSeconds = fireRate;
	}
	
	// Update is called once per frame
	void Update () {
		if ((nextShotInSeconds -= Time.deltaTime) > 0) {
			return;
		}
		Animator a = GetComponentInParent<Animator>();
		if (a != null) {
			Debug.Log("Play Animation");
			a.SetTrigger("Shoot");
		}
		nextShotInSeconds = fireRate;
		Debug.Log("Shoot");
		PathedProjectile p = (PathedProjectile)Instantiate(projectile, transform.position, transform.rotation);
		p.Initalise(destination, speed);

		if (spawnEffect != null) {
			///Instantiate(spawnEffect, transform.position, transform.rotation);
		}
	}

	public void OnDrawGizmos() {
		if (destination == null) {
			return;
		}
		Gizmos.color = Color.red;
		Gizmos.DrawLine(transform.position, destination.position);
	}
}
