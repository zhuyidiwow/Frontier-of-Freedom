using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Enemy : Breakable {

	public int Score;
	public float Damage;
	
	[SerializeField] protected float moveForce;
	[SerializeField] protected float maxSpeed;
	
	protected Player player;
	protected Rigidbody rb;
	private bool justHurtPlayer;
	
	private void Start() {
		player = Player.Instance;
		rb = GetComponent<Rigidbody>();
		maxSpeed *= Random.Range(0.5f, 1.5f);
		StartCoroutine(DistanceCheck());
	}

	private void FixedUpdate() {
		if (!justHurtPlayer) {
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
		
		Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);		
		pos.x *= CameraManager.Instance.ScreenSize.x;
		pos.y *= CameraManager.Instance.ScreenSize.y;
		
		ScoreText text = Instantiate(PrefabManager.Instance.ScoreText, pos, Quaternion.identity, GameManager.Instance.Canvas.transform);
		
		text.Initialize(Score, (Vector3.up + Vector3.right).normalized, pos); 
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player") && !justHurtPlayer) {
			justHurtPlayer = true;
			rb.velocity = (transform.position - player.transform.position).normalized * 15f;
			rb.drag = 5f;
			Invoke("ContinueMoving", 2f);
			player.TakeDamage(Damage);
		}
	}

	private void ContinueMoving() {
		justHurtPlayer = false;
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
