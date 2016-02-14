using UnityEngine;
using System.Collections;

public class BlockBreakable : MonoBehaviour {

	public float breakableTime = 2.5f;
	

	// Use this for initialization
	void Start () {
		
	}
	
	IEnumerator BreakableWait() {
		yield return new WaitForSeconds(breakableTime);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		GetComponent<Rigidbody2D>().AddForce(Vector3.up * 2);
		StartCoroutine(BreakableWait());
		//GetComponent<Rigidbody2D>().AddForce(Vector3.up * -200);
		Destroy(this.gameObject, breakableTime);
	}
}
