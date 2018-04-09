using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Attacker : Enemy {

    public enum EEnemyMode {
        ATTACK, MOVE
    }
    
    [SerializeField] private float moveForce;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float continueChasingDistance;
    
    private Player player;
    private Rigidbody rb;
    private EEnemyMode enemyMode;
    
    private void Start() {
        player = Player.Instance;
        rb = GetComponent<Rigidbody>();
        maxSpeed *= Random.Range(0.5f, 1.5f);
        StartCoroutine(DistanceCheck());
    }

    private void FixedUpdate() {
        switch (enemyMode) {
            case EEnemyMode.ATTACK:
                break;
            case EEnemyMode.MOVE:
                break;
            default:
                break;
        }
        
        if (Vector3.Distance(player.transform.position, transform.position) > stoppingDistance) {
            MoveToPlayer();    
        }
        else {
            ShootAtPlayer();
        }
        
        transform.LookAt(player.transform.position);
    }

    private void ShootAtPlayer() {
        
    }
    
    private void MoveToPlayer() {
        Vector3 dir = (player.transform.position - transform.position).normalized;
        dir.z = 0f;
        rb.AddForce(moveForce * dir, ForceMode.Force);
        CapSpeed();
    }

    private void CapSpeed() {
        if (rb.velocity.magnitude > maxSpeed) {
            rb.velocity = maxSpeed * rb.velocity.normalized;
        }
    }

    private IEnumerator DistanceCheck() {
        while (true) {
            if (Vector3.Distance(Player.Instance.transform.position, transform.position) > 50f) {
                EnemyManager.Instance.SpawnOne();
                Break();
            }

            yield return new WaitForSeconds(1f);
        }
    }
}