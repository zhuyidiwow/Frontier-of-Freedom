using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
	public static CameraManager Instance;
	
	public float OffsetMultiplier;

	[HideInInspector] public Vector2 ViewRange;
	[HideInInspector] public Vector2 ScreenSize;
	
	private Vector3 offset;
	private Player player;
	
	private void Awake() {
		if (Instance == null) Instance = this;
		ViewRange = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, 14.13f)) - Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 14.13f));
		ScreenSize = Camera.main.ViewportToScreenPoint(new Vector3(1f, 1f, 14.13f)) - Camera.main.ViewportToScreenPoint(new Vector3(0f, 0f, 14.13f));
		
	}
	
	private void Start() {
		player = Player.Instance;
	}

	private void Update() {
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
			return;
		}
		Vector3 mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
		mousePos -= new Vector3(0.5f, 0.5f, 0f);
		offset = mousePos * OffsetMultiplier;
		Vector3 targetPosition = player.transform.position + offset;
		targetPosition.z = transform.position.z;
		transform.position = targetPosition;
	}
}
