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

    [SerializeField] private GameObject weapon;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float continueChasingDistance;
    
    private EEnemyMode enemyMode;
    private float lastShotTime;
    private Vector3 originalWeaponScale;
    
    private void Start() {
        player = Player.Instance;
        rb = GetComponent<Rigidbody>();
        maxSpeed *= Random.Range(0.5f, 1.5f);
        StartCoroutine(SelfDestroyCheck());
        enemyMode = EEnemyMode.MOVE;
        originalWeaponScale = weapon.transform.localScale;
        weapon.transform.localScale = originalWeaponScale * 2f;
        
        float randomFactor = Random.Range(0.75f, 1.5f);
        float difficulty = Mathf.Pow(DifficultyManager.Instance.Difficulty, 0.5f);
        moveForce = moveForce * difficulty;
        maxSpeed = maxSpeed * difficulty * randomFactor;
        Score = (int) (Score * difficulty);
        Damage = Damage * difficulty * randomFactor * 0.75f;

        bulletSpeed = bulletSpeed * difficulty * randomFactor;
        if (bulletSpeed > 25f) bulletSpeed = 25f;
        ShootInterval = ShootInterval / difficulty;
        stoppingDistance = stoppingDistance * difficulty;
        if (stoppingDistance > 10f) stoppingDistance = 10f;
        trail.SetActive(difficulty > 1.5f);
        
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
        weapon.transform.Rotate(Vector3.forward, 360f * Time.deltaTime, Space.Self);
        
        transform.LookAt(player.transform.position);
    }

    private void ShootAtPlayer() {
        float elapsedTime = Time.time - lastShotTime;
        
        if (elapsedTime > ShootInterval) {
            lastShotTime = Time.time;
            EnemyBullet enemyBullet = Instantiate(PrefabManager.Instance.EnemyBullet, transform.position + transform.forward * 0.5f,
                Quaternion.identity);
            enemyBullet.Initialize(Damage, bulletSpeed);
        } else {
            weapon.transform.localScale = originalWeaponScale * (1f + elapsedTime / ShootInterval);
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
               
                EnemyManager.Instance.Delete(this);
                Destroy(gameObject);
            }

            yield return new WaitForSeconds(1f);
        }
    }
}