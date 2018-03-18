using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float Force;
	
	private Rigidbody rb;
	private Coroutine addForceCoroutine;
	private Plane plane;
	
	void Start() {
		rb = GetComponent<Rigidbody>();
		plane = new Plane(Vector3.back, transform.position);
	}

	private void Update() {
		if (Input.GetMouseButtonDown(0)) {
			float distance;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			plane.Raycast(ray, out distance);
			Vector3 clickedPos = ray.origin + ray.direction * distance;
			Vector3 dir = transform.position - clickedPos;
//			Debug.DrawLine(transform.position, clickedPos, Color.white, 3f);
			AddForce(dir * Force, 0.1f);
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
}
