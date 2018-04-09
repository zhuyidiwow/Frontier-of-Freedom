using System.Collections;
using System.Collections.Generic;
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
	
	private float health;
	private Rigidbody rb;
	private List<Spike> spikes;
	private int spikeCount;
	private bool justHitPlayer;
	
	public void TakeDamage(float amount) {
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
		Destroy(gameObject);
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
	}

	private void Update() {
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
