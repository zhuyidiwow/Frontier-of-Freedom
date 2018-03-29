using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

	[SerializeField] private GameObject particlePrefab;

	private void OnCollisionEnter(Collision other) {
		GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
		Destroy(particle, 3f);
		Destroy(gameObject);
	}
}
