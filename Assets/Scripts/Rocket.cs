﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float speedCap;
    [SerializeField] private float moveForce;

    public AudioClip clipExplode;
    
    private Enemy target;
    private Rigidbody rb;
    
    private void Start() {
        target = EnemyManager.Instance.GetNearest(transform.position);
        if (target == null) {
            Explode();
        }
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (target == null) {
            target = EnemyManager.Instance.GetNearest(transform.position);
            if (target == null) {
                Explode();
            }
        }
        rb.AddForce( (target.transform.position - transform.position).normalized * moveForce);
        CapSpeed();
        transform.LookAt(transform.position + rb.velocity);
    }

    private void CapSpeed() {
        if (rb.velocity.magnitude > speedCap) {
            rb.velocity = rb.velocity.normalized * speedCap;
        }
    }

    private void OnTriggerEnter(Collider other) {
        GameObject otherObj = other.gameObject;
        
        if (otherObj.CompareTag("Breakable")) {
            otherObj.GetComponent<Breakable>().Break();
        }
		
        if (otherObj.CompareTag("Enemy")) {
            GameManager.Instance.GetScore(otherObj.GetComponent<Enemy>().Score);
            otherObj.GetComponent<Enemy>().Break();
            Explode();
        }

        if (otherObj.CompareTag("Boss")) {
            Explode();
        }
        
    }

    private void Explode() {
        GameObject particle = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        DeathZone deathZone = Instantiate(PrefabManager.Instance.DeathZone, transform.position, Quaternion.identity);
        Utilities.Audio.PlayAudio(particle.AddComponent<AudioSource>(), clipExplode, 0.3f);
        Destroy(particle, 3f);
        Destroy(deathZone.gameObject, 0.3f);
        Destroy(gameObject);
    }
}
