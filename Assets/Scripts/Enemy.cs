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
	private bool shouldMove;
	
	private void Start() {
		player = Player.Instance;
		rb = GetComponent<Rigidbody>();
		maxSpeed *= Random.Range(0.5f, 1.5f);
		StartCoroutine(DistanceCheck());
		shouldMove = true;
	}

	private void FixedUpdate() {
		if (shouldMove) {
			MoveToPlayer();
		}

		transform.LookAt(player.transform.position);
	}

	private void MoveToPlayer() {
		Vector3 dir = (player.transform.position - transform.position).normalized;
		dir.z = 0f;
		rb.AddForce(moveForce * dir, ForceMode.Force);
		CapSpeed();
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

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player")) {
			shouldMove = false;
			rb.velocity = (transform.position - player.transform.position).normalized * 12f;
			rb.drag = 5f;
			Invoke("ContinueMoving", 3f);
			player.TakeDamage(10f);
		}
	}

	private void ContinueMoving() {
		shouldMove = true;
		rb.drag = 0f;
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
