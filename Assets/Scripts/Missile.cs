using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

	[SerializeField] private GameObject particlePrefab;

	private void OnCollisionEnter(Collision other) {
		GameObject otherObj = other.gameObject;
		if (otherObj.CompareTag("Breakable")) {
			otherObj.GetComponent<Breakable>().Break();
		}
		
		if (otherObj.CompareTag("Enemy")) {
			GameManager.Instance.Score(otherObj.GetComponent<Enemy>().Score);
			otherObj.GetComponent<Enemy>().Break();
		}

		
		
		GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
		Destroy(particle, 3f);
		Destroy(gameObject);
	}
}
