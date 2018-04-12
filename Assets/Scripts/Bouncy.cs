using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncy : MonoBehaviour {

    public AnimationCurve BounceCurve;
    private Coroutine bounceCoroutine;
   
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            if (bounceCoroutine != null) StopCoroutine(bounceCoroutine);
            bounceCoroutine = StartCoroutine(BounceCoroutine());
        }
    }

    private IEnumerator BounceCoroutine() {
        float elapsedTime = 0f;
        
        while (elapsedTime < BounceCurve.keys[BounceCurve.keys.Length - 1].time) {
            transform.localScale = Vector3.one * BounceCurve.Evaluate(elapsedTime);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        bounceCoroutine = null;
    }

}