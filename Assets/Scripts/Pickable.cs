using UnityEngine;

public abstract class Pickable : MonoBehaviour {
	private void Update() {
		transform.Rotate(Vector3.forward, 120f * Time.deltaTime);
	}
	
	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			OnPickUp();
		}
	}
	
	protected abstract void OnPickUp();

}
