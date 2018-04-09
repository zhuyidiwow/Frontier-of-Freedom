using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class Attacker : Enemy {

    public enum EEnemyMode {
        ATTACK, MOVE
    }

    public float ShootInterval;

    [SerializeField] private float bulletSpeed;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float continueChasingDistance;
    
    private EEnemyMode enemyMode;
    private float lastShotTime;
    
    private void Start() {
        player = Player.Instance;
        rb = GetComponent<Rigidbody>();
        maxSpeed *= Random.Range(0.5f, 1.5f);
        StartCoroutine(SelfDestroyCheck());
        enemyMode = EEnemyMode.MOVE;
    }

    private void FixedUpdate() {
        float distance = Vector3.Distance(player.transform.position, transform.position);
        
        switch (enemyMode) {
            case EEnemyMode.ATTACK:
                ShootAtPlayer();
                if (distance >= continueChasingDistance) {
                    enemyMode = EEnemyMode.MOVE;
                    rb.drag = 0f;
                }
                break;
            case EEnemyMode.MOVE:
                MoveToPlayer();
                if (distance <= stoppingDistance) {
                    enemyMode = EEnemyMode.ATTACK;
                    rb.drag = 10f;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        transform.LookAt(player.transform.position);
    }

    private void ShootAtPlayer() {
        if (Time.time - lastShotTime > ShootInterval) {
            lastShotTime = Time.time;
            EnemyBullet enemyBullet = Instantiate(PrefabManager.Instance.EnemyBullet, transform.position + transform.forward * 0.5f,
                Quaternion.identity);
            enemyBullet.Initialize(Damage, bulletSpeed);
        }
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

    private IEnumerator SelfDestroyCheck() {
        while (true) {
            if (Vector3.Distance(Player.Instance.transform.position, transform.position) > 50f) {
                EnemyManager.Instance.SpawnOne(PrefabManager.Instance.Attacker);
                Break();
            }

            yield return new WaitForSeconds(1f);
        }
    }
}