using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {

	public GameObject FollowedGameObject;
	public float LerpFactor;
	
	private void Update() {
		Vector3 targetPosition = FollowedGameObject.transform.position;
		targetPosition.z = transform.position.z;
//		transform.position = Vector3.Lerp(transform.position, targetPosition, LerpFactor * Time.deltaTime);
		transform.position = targetPosition;
	}
}
