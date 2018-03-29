using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Breakable {

	[SerializeField] private float moveSpeed;
	
	private Player player;

	private void Start() {
		if (Random.Range(0f, 1f) < 0.5f) {
			Destroy(gameObject);
		}
		
		player = Player.Instance;
		
	}

	private void Update() {
		Vector3 dir = (player.transform.position - transform.position).normalized;
		dir.z = 0f;
		transform.Translate(dir * moveSpeed * Time.deltaTime, Space.World);
		transform.LookAt(player.transform.position);
	}

	private void OnCollisionEnter(Collision other) {
		if (other.gameObject.CompareTag("Projectile")) {
			Break();
		}
	}
}
