using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : Breakable {

	public int Score;
	
	[SerializeField] private float moveForce;
	[SerializeField] private float maxSpeed;
	
	private Player player;
	private Rigidbody rb;
	
	private void Start() {
		player = Player.Instance;
		rb = GetComponent<Rigidbody>();
		maxSpeed *= Random.Range(0.5f, 1.5f);
		StartCoroutine(DistanceCheck());
	}

	private void FixedUpdate() {
		Vector3 dir = (player.transform.position - transform.position).normalized;
		dir.z = 0f;
		
		rb.AddForce(moveForce * dir, ForceMode.Force);
		CapSpeed();
		
		transform.LookAt(player.transform.position);
	}

	private void CapSpeed() {
		if (rb.velocity.magnitude > maxSpeed) {
			rb.velocity = maxSpeed * rb.velocity.normalized;
		}
	}

	public override void Break() {
		base.Break();
		EnemyManager.Instance.Delete(this);
	}

	private IEnumerator DistanceCheck() {
		while (true) {
			if (Vector3.Distance(Player.Instance.transform.position, transform.position) > 50f) {
				EnemyManager.Instance.SpawnOne();
				Break();
			}
			yield return new WaitForSeconds(1f);
		}
	}
}
