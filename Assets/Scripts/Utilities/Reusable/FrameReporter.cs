using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameReporter : MonoBehaviour {

	private Text text;
	
	void Start () {
		text = GetComponent<Text>();
		StartCoroutine(ReportCoroutine());
	}


	IEnumerator ReportCoroutine() {
		while (true) {
			text.text = (1f / Time.deltaTime).ToString().Substring(0, 2);
			yield return new WaitForSeconds(0.5f);
		}
		
	}
	

}
