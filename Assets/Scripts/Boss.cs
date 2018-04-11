using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour {

	[SerializeField] private AnimationCurve healthCurve;
	[SerializeField] private AnimationCurve damageModifierCurve;
	[SerializeField] private AnimationCurve scaleFactorCurve;
	[SerializeField] private AnimationCurve moveModifierCurve;

	[SerializeField] private float distanceResetThreshold;
	[SerializeField] private float damage;
	[SerializeField] private GameObject model;
	[SerializeField] private Color fullColor;
	[SerializeField] private Color dieColor;
	[SerializeField] private Slider healthSlider;
	[SerializeField] private Image fill;
	[SerializeField] private float moveForce;
	[SerializeField] private float speedCap;
	[SerializeField] private float rotationSpeed;
	[SerializeField] private GameObject dieParticle;
	[SerializeField] private AudioClip moveClip;
	[SerializeField] private AudioClip hitPlayerClip;
	[SerializeField] private AudioClip[] explodeClips;
	
	private float health;
	private Rigidbody rb;
	private List<Spike> spikes;
	private int spikeCount;
	private bool justHitPlayer;
	private float baseRadius = 1.73f;
	private float radius;
	private bool isDead;
	private AudioSource source;
	private AudioSource explodeSource;
	private AudioSource hitPlayerSource;
	private float maxHealth;
	
	public void TakeDamage(float amount) {
		if (isDead) return;
		health -= amount;
		if (health <= 0f) {
			Die();	
		}

		healthSlider.value = health / maxHealth;
		fill.color = Color.Lerp(dieColor, fullColor, health / maxHealth);
		
		if (health / maxHealth < (float) spikes.Count / spikeCount && spikes.Count >= 1) {
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
		if (!explodeSource.isPlaying) Utilities.Audio.PlayAudioRandom(explodeSource, explodeClips);
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
		float scaleFactor = DifficultyManager.Instance.Evaluate(scaleFactorCurve);
		float moveModifier = DifficultyManager.Instance.Evaluate(moveModifierCurve);
		
		radius = baseRadius * scaleFactor;
		maxHealth = health * DifficultyManager.Instance.Evaluate(healthCurve);
		health = maxHealth;
		transform.localScale *= scaleFactor;
		
		moveForce = moveForce * moveModifier;
		speedCap = speedCap * moveModifier;
		
		damage *= DifficultyManager.Instance.Evaluate(damageModifierCurve);
		
		source = GetComponent<AudioSource>();
		explodeSource = gameObject.AddComponent<AudioSource>();
		hitPlayerSource = gameObject.AddComponent<AudioSource>();
		Utilities.Audio.PlayAudio(source, moveClip, 1f, true);
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

		if (distance > distanceResetThreshold) {
			transform.position = Player.Instance.transform.position +
			                     (transform.position - Player.Instance.transform.position).normalized * distanceResetThreshold;
		}
	}

	private void OnTriggerEnter(Collider other) {
		if (isDead) return;
		if (other.CompareTag("Breakable")) {
			other.GetComponent<Breakable>().Break();
		}

		if (other.CompareTag("Player")) {
			if (!justHitPlayer) {
				other.GetComponent<Player>().TakeDamage(damage);
				justHitPlayer = true;
				Invoke("ContinueAttacking", 3f);
				Utilities.Audio.PlayAudio(hitPlayerSource, hitPlayerClip);
			}
		}
	}

	private void ContinueAttacking() {
		justHitPlayer = false;
	}
}
