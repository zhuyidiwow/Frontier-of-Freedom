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
	[SerializeField] protected GameObject trail;
	[SerializeField] protected AudioClip dieClip;
	
	protected Player player;
	protected Rigidbody rb;
	private bool justHurtPlayer;
	
	private void Start() {
		player = Player.Instance;
		rb = GetComponent<Rigidbody>();
		StartCoroutine(DistanceCheck());
		
		float randomFactor = Random.Range(0.75f, 1.5f);
		transform.localScale *= randomFactor;
		
		float difficulty = DifficultyManager.Instance.Evaluate(EnemyManager.Instance.DifficultyCurve);
		moveForce = moveForce * difficulty * randomFactor;
		maxSpeed = maxSpeed * difficulty;
		Score = (int) (Score * difficulty * randomFactor);
		Damage = Damage * DifficultyManager.Instance.Evaluate(EnemyManager.Instance.DamageCurve);
		trail.SetActive(difficulty > 1.25f);
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
		EnemyManager.Instance.Delete(this);

		Vector3 pos = transform.position;
		Vector3 dir = (pos - player.transform.position).normalized;
		ScoreText text = Instantiate(PrefabManager.Instance.ScoreText, pos, Quaternion.identity);
		text.Initialize(Score, dir, pos);
		
		Utilities.Audio.PlayAudio(text.gameObject.AddComponent<AudioSource>(), dieClip, 0.3f);
		base.Break();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.CompareTag("Player") && !justHurtPlayer) {
			justHurtPlayer = true;
			float speed = player.GetComponent<Rigidbody>().velocity.magnitude > 15f ? player.GetComponent<Rigidbody>().velocity.magnitude + 5f : 15f;
			rb.velocity = (transform.position - player.transform.position).normalized * speed;
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
				EnemyManager.Instance.Delete(this);
				Destroy(gameObject);
			}
			yield return new WaitForSeconds(1f);
		}
	}
}
