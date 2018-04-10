using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.Build.Player;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour {

	[SerializeField] private GameObject model;
	[SerializeField] private Color fullColor;
	[SerializeField] private Color dieColor;
	[SerializeField] private Slider healthSlider;
	[SerializeField] private Image fill;
	[SerializeField] private float moveForce;
	[SerializeField] private float speedCap;
	[SerializeField] private float rotationSpeed;
	[SerializeField] private GameObject dieParticle;
	
	private float health;
	private Rigidbody rb;
	private List<Spike> spikes;
	private int spikeCount;
	private bool justHitPlayer;
	private float baseRadius = 1.73f;
	private float radius;
	private float scaleFactor;
	private bool isDead;
	
	public void TakeDamage(float amount) {
		if (isDead) return;
		health -= amount;
		if (health <= 0f) {
			Die();	
		}

		healthSlider.value = health / 100f;
		fill.color = Color.Lerp(dieColor, fullColor, health / 100f);
		
		if (health / 100f < (float) spikes.Count / spikeCount && spikes.Count >= 1) {
			Spike spike = spikes[0];
			spikes.RemoveAt(0);
			spike.transform.parent = null;
			Destroy(spike.gameObject);
		}
	}

	private void Die() {
		StartCoroutine(DieCoroutine());
	}

	private IEnumerator DieCoroutine() {
		isDead = true;
		healthSlider.gameObject.SetActive(false);
		rb.isKinematic = true;
		rb.velocity = Vector3.one;
		GetComponent<Collider>().enabled = false;
		
		float startTime = Time.time;

		float blowCount = 12f;
		int i = 0;
		float step = 2f / blowCount;
		while (Time.time - startTime < 2f) {
			yield return new WaitUntil(() => Time.time - startTime > step * i);
			SpawnOneParticle();
			i++;
		}
		SpawnOneParticle();SpawnOneParticle();SpawnOneParticle();
		int score = (int) (50 * DifficultyManager.Instance.Difficulty);
		if (score > 100) score = 100;
		
		Vector3 pos = transform.position;
		Vector3 dir = (pos - Player.Instance.transform.position).normalized;
		ScoreText text = Instantiate(PrefabManager.Instance.ScoreText, pos, Quaternion.identity);
		text.Initialize(score, dir, pos); 
		
		GameManager.Instance.GetScore(score);
		Destroy(gameObject);
	}

	void SpawnOneParticle() {
		GameObject particle = Instantiate(dieParticle, transform.position + new Vector3(Random.Range(-radius, radius), Random.Range(-radius, radius)),
			Quaternion.identity);
		Destroy(particle, 3f);
	}
	
	private void Start() {
		rb = GetComponent<Rigidbody>();
		health = 100f;
		
		spikes = new List<Spike>();
		foreach (Spike spike in model.GetComponentsInChildren<Spike>()) {
			spikes.Add(spike);
		}

		spikeCount = spikes.Count;
		TakeDamage(0f);

		scaleFactor = Mathf.Pow(DifficultyManager.Instance.Difficulty, 0.4f);
		
		radius = baseRadius * scaleFactor;
		moveForce = moveForce * scaleFactor;
		speedCap = speedCap * Mathf.Clamp(scaleFactor, 1f, 1.5f);
		health = health * scaleFactor;
		transform.localScale *= scaleFactor;
	}

	private void Update() {
		if (isDead) return;
		
		float distance = (Player.Instance.transform.position - transform.position).magnitude;
		float actualRotateSpeed = rotationSpeed;
		if (distance < 10f) {
			actualRotateSpeed = rotationSpeed + (10f - distance) / 10f * 720f;
		}
		model.transform.Rotate(Vector3.forward, actualRotateSpeed * Time.deltaTime, Space.Self);
		
		rb.AddForce( (Player.Instance.transform.position - transform.position).normalized * moveForce);
		if (rb.velocity.magnitude > speedCap) {
			rb.velocity = rb.velocity.normalized * speedCap;
		}

		if (distance > 30f) {
			transform.position = Player.Instance.transform.position + (transform.position - Player.Instance.transform.position).normalized * 30f;
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (isDead) return;
		if (other.CompareTag("Breakable")) {
			other.GetComponent<Breakable>().Break();
		}

		if (other.CompareTag("Player")) {
			if (!justHitPlayer) {
				other.GetComponent<Player>().TakeDamage(30f);
				justHitPlayer = true;
				Invoke("ContinueAttacking", 3f);
			}
		}
	}

	private void ContinueAttacking() {
		justHitPlayer = false;
	}
}
