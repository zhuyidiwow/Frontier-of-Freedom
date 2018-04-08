using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour {
	private void Update() {
		transform.Rotate(Vector3.forward, 120f * Time.deltaTime);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {

			Destroy(this);
		}
	}

}
