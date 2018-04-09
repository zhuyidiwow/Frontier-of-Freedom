using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	[SerializeField] private float damage;
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

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Boss")) {
			other.GetComponent<Boss>().TakeDamage(damage);
			
			GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
			Destroy(particle, 3f);
			Destroy(gameObject);
		}

		
	}
}
