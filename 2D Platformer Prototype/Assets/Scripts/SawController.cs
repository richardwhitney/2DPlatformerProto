using UnityEngine;
using System.Collections;

public class SawController : MonoBehaviour {

	public Vector3[] localWaypoints;
	public Vector3[] globalWaypoints;
	
	public float speed;
	public bool cyclic;
	public float waitTime;
	[Range(0, 2)]
	public float easeAmount;
	
	int fromWaypointIndex;
	float percentBetweenWaypoints;
    float nextMoveTime;

	// Use this for initialization
	void Start() {

		globalWaypoints = new Vector3[localWaypoints.Length];
		for (int i = 0; i < localWaypoints.Length; i++) {
			globalWaypoints[i] = localWaypoints[i] + transform.position;
        }
	}
	
	// Update is called once per frame
	void Update() {
		Vector3	velocity = CalculateSawMovement();

		transform.Translate(velocity);
	}

	float Ease(float x) {
		float a = easeAmount + 1;
		return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

	Vector3 CalculateSawMovement() {
		
		if (Time.time < nextMoveTime) {
			return Vector3.zero;
		}
		
		fromWaypointIndex %= globalWaypoints.Length;
		int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
		float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
		percentBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;
		percentBetweenWaypoints = Mathf.Clamp01(percentBetweenWaypoints);
		float easePercentBetweenWaypoints = Ease(percentBetweenWaypoints);
		
		
		Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], easePercentBetweenWaypoints);
		
		if (percentBetweenWaypoints >= 1) {
			percentBetweenWaypoints = 0;
			fromWaypointIndex++;
			
			if (!cyclic) {
				if (fromWaypointIndex >= globalWaypoints.Length-1) {
					fromWaypointIndex = 0;
                    System.Array.Reverse(globalWaypoints);
                }
            }
            nextMoveTime = Time.time + waitTime;
        }
        
        return newPos - transform.position;
    }

	void OnDrawGizmos() {
		if (localWaypoints != null) {
			Gizmos.color = Color.red;
			float size = 0.3f;
			
			for (int i = 0; i < localWaypoints.Length; i++) {
				Vector3 globalWaypointsPos = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + transform.position;
				Gizmos.DrawLine(globalWaypointsPos - Vector3.up * size, globalWaypointsPos + Vector3.up * size);
				Gizmos.DrawLine(globalWaypointsPos - Vector3.left * size, globalWaypointsPos + Vector3.left * size);
            }
        }
    }
}
