using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour {
	[SerializeField] private GameObject breakParticle;

	public virtual void Break() {
		//TODO: fix this
//		if (gameObject.name.Contains("Cube")) {
//			Vector3 pos = transform.position;
//			Vector3 dir = (pos - Player.Instance.transform.position).normalized;
//			ScoreText text = Instantiate(PrefabManager.Instance.ScoreText, pos, Quaternion.identity);
//			text.Initialize(1, dir, pos); 
//		}
		
		GameObject particle = Instantiate(breakParticle, transform.position, Quaternion.identity);
		Destroy(particle, 3f);
		Destroy(gameObject);
	}
}
