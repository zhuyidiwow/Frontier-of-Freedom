using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour {

	[SerializeField] private GameObject breakParticle;


	public virtual void Break() {
		GameObject particle = Instantiate(breakParticle, transform.position, Quaternion.identity);
		Destroy(particle, 3f);
		Destroy(gameObject);
	}
}
