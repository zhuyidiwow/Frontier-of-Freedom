using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Player : MonoBehaviour {
	public static Player Instance;
	
	[SerializeField] private float missileSpeed;
	[SerializeField] private float speedChange;

	[SerializeField] private GameObject missilePrefab;
	[SerializeField] private GameObject gun;
	
	private Rigidbody rb;
	private Coroutine addForceCoroutine;
	private Plane plane;

	private void Awake() {
		if (Instance == null) Instance = this;
		else Destroy(Instance);
	}

	void Start() {
		rb = GetComponent<Rigidbody>();
		plane = new Plane(Vector3.back, transform.position);
	}

	private void Update() {
		float distance;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		plane.Raycast(ray, out distance);
		Vector3 mousePos = ray.origin + ray.direction * distance;
		Vector3 dir = mousePos - transform.position;
		dir = dir.normalized;
		
		gun.transform.position = 0.5f * dir + transform.position;
		gun.transform.LookAt(mousePos);
		
		if (Input.GetMouseButtonDown(0)) {
			Shoot(dir);
		}
	}

	private void Shoot(Vector3 dir) {
		GameObject missile = Instantiate(missilePrefab, transform.position, Quaternion.identity, null);
		missile.GetComponent<Rigidbody>().velocity = dir * missileSpeed;
		rb.velocity += -dir * speedChange;
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
}
