using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
	public static Player Instance;
	
	[SerializeField] private Slider slider;
	[SerializeField] private Image fill;
	[SerializeField] private Color lowHealthColor;
	[SerializeField] private Color fullHealthColor;
	
	[SerializeField] private float baseSpeedChange;
	[SerializeField] private float maxHealth = 100f;
	
	private Rigidbody rb;
	private Coroutine addForceCoroutine;
	private Plane plane;
	private List<Weapon> weapons;
	private float health = 100f;

	public void PickUpWeapon(Weapon newWeapon) {
		newWeapon = Instantiate(newWeapon).GetComponent<Weapon>();
		newWeapon.transform.parent = transform;
		weapons.Add(newWeapon);
	}

	public void TakeDamage(float amount) {
		health -= amount;
		if (health <= 0f) {
			Die();
		}
		UpdateUI();
	}

	public void Heal(float amount) {
		health += amount;
		if (health >= maxHealth) {
			health = maxHealth;
		}
		UpdateUI();
	}

	private void UpdateUI() {
		slider.value = health / maxHealth;

		fill.color = Color.Lerp(lowHealthColor, fullHealthColor, health / maxHealth);
	}
	
	private void Awake() {
		if (Instance == null) Instance = this;
		else Destroy(Instance);
	}

	void Start() {
		rb = GetComponent<Rigidbody>();
		plane = new Plane(Vector3.back, transform.position);
		weapons = new List<Weapon> {Instantiate(PrefabManager.Instance.MissileLauncher).GetComponent<Weapon>()};
		UpdateUI();
	}

	private void Update() {
		float distance;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		plane.Raycast(ray, out distance);
		Vector3 mousePos = ray.origin + ray.direction * distance;
		Vector3 dir = mousePos - transform.position;
		dir = dir.normalized;

		int weaponCount = weapons.Count;
		
		Vector3 baseDir = Quaternion.AngleAxis(-45f, Vector3.back) * dir;
		float angleStep = 90f / (weaponCount + 1);
		
		List<Vector3> dirList = new List<Vector3>();
		
		for (int i = 0; i < weaponCount; i++) {
			dirList.Add(Quaternion.AngleAxis(angleStep * (i + 1), Vector3.back) * baseDir);
			Weapon weapon = weapons[i];
			weapon.transform.position = 0.5f * dirList[i] + transform.position;
			weapon.transform.LookAt(transform.position + dirList[i]);
		}
		
		if (Input.GetMouseButtonDown(0)) {
			for (int i = 0; i < weaponCount; i++) {
				weapons[i].Shoot(dirList[i]);
			}

			float speedChange = baseSpeedChange * Mathf.Sqrt(weaponCount);
			rb.velocity += -dir * speedChange;
		}
	}

	private void AddForce(Vector3 force, float duration) {
		if (addForceCoroutine != null) StopCoroutine(addForceCoroutine);
		addForceCoroutine = StartCoroutine(AddForceCoroutine(force, duration));
	}

	private IEnumerator AddForceCoroutine(Vector3 force, float duration) {
		float elapsedTime = 0;
		while (elapsedTime < duration) {
			rb.AddForce(force, ForceMode.Force);
			yield return null;
			elapsedTime += Time.deltaTime;
		}

		addForceCoroutine = null;
	}

	private void Die() {
		GameManager.Instance.EndGame();
	}
}
