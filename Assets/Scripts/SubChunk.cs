using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubChunk : MonoBehaviour {

	public float SurvivalRate;

	private void Start() {
		if (Random.Range(0f, 1f) >= SurvivalRate) {
			Destroy(gameObject);
		}
	}
}
