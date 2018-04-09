using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    [SerializeField] private GameObject explosionPrefab;

    private Enemy target;
    private Rigidbody rb;
    
    private void Start() {
        target = EnemyManager.Instance.GetNearest(transform.position);
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (target == null) {
            target = EnemyManager.Instance.GetNearest(transform.position);
        }
        rb.AddForce( (target.transform.position - transform.position).normalized * 6f);
        CapSpeed();
        transform.LookAt(transform.position + rb.velocity);
    }

    private void CapSpeed() {
        if (rb.velocity.magnitude > 20f) {
            rb.velocity = rb.velocity.normalized * 20f;
        }
    }

    private void OnTriggerEnter(Collider other) {
        GameObject otherObj = other.gameObject;
        
        if (otherObj.CompareTag("Breakable")) {
            otherObj.GetComponent<Breakable>().Break();
        }
		
        if (otherObj.CompareTag("Enemy")) {
            GameManager.Instance.Score(otherObj.GetComponent<Enemy>().Score);
            otherObj.GetComponent<Enemy>().Break();
            GameObject particle = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            
            DeathZone deathZone = Instantiate(PrefabManager.Instance.DeathZone, transform.position, Quaternion.identity);
            Destroy(particle, 3f);
            Destroy(deathZone.gameObject, 0.3f);
            Destroy(gameObject);
        }

        
        
    }
}
