using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	public float OffsetMultiplier;
	private Vector3 offset;

	private Player player;
	
	private void Start() {
		player = Player.Instance;
	}

	private void Update() {
		Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		mousePos -= new Vector3(0.5f, 0.5f, 0f);
		offset = mousePos * OffsetMultiplier;
		Vector3 targetPosition = player.transform.position + offset;
		targetPosition.z = transform.position.z;
		transform.position = targetPosition;
	}
}
