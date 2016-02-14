using UnityEngine;
using System.Collections;

public class ContinuousLaser : MonoBehaviour {

	public Transform destination;
	public Transform laserEndEffect;
	public bool canFire;
	public LayerMask collisionMask;

	private LineRenderer lineRenderer;
	private GameObject laserEnd;
	private float distance;

	// Use this for initialization
	void Start () {
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.enabled = true;
		lineRenderer.useWorldSpace = true;
		lineRenderer.sortingLayerName = "Foreground";
		lineRenderer.SetPosition(0, transform.position);
		lineRenderer.SetPosition(1, destination.position);
		laserEnd = Instantiate(laserEndEffect, destination.position, destination.rotation) as GameObject;
		canFire = true;
		distance = Vector3.Distance(transform.position, destination.position);
	}
	
	// Update is called once per frame
	void Update () {
		Debug.DrawLine(transform.position, destination.position);
		if (canFire) {
			RaycastHit2D hit = Physics2D.Linecast(transform.position, destination.position, collisionMask);
			if (hit) {
				Player player = hit.collider.GetComponent<Player>();
				if (player) {
					LevelManager.instance.KillPlayer();
				}
			}
		}
		lineRenderer.enabled = canFire;
		//laserEnd.SetActive(canFire);
	}
}
