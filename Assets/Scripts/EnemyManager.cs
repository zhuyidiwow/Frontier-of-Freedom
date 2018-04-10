using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class EnemyManager : MonoBehaviour {
    public static EnemyManager Instance;

    public int MaxEnemy;
    [HideInInspector] public List<Enemy> Enemies;

    public bool ShouldSpawnEnemies() {
        return Enemies.Count < MaxEnemy;
    }

    private void Awake() {
        if (Instance == null) Instance = this;
        Enemies = new List<Enemy>();
    }

    private void Start() {
        StartCoroutine(SpawnEnemyCoroutine());
    }

    public void SpawnOne(Enemy enemy = null) {
        Vector3 camPos = CameraManager.Instance.transform.position;
        camPos.z = 0f;
        Vector3 offset = new Vector3 {
            x = CameraManager.Instance.ViewRange.x * Random.Range(0.5f, 1f) * (Random.Range(0f, 1f) > 0.5f ? 1f : -1f),
            y = CameraManager.Instance.ViewRange.y * Random.Range(0.5f, 1f) * (Random.Range(0f, 1f) > 0.5f ? 1f : -1f),
            z = 0f
        };
        Vector3 pos = camPos + offset;

        if (enemy != null) {
            SpawnOne(pos, enemy);
        } else {
            SpawnOne(pos, PrefabManager.Instance.Enemy);
        }
    }

    public Enemy GetNearest(Vector3 pos) {
        int index = 0;
        float shortestDis = 100f;
        for (int i = 0; i < Enemies.Count; i++) {
            float distance = Vector3.Distance(Enemies[i].transform.position, pos);
            if (distance < shortestDis) {
                shortestDis = distance;
                index = i;
            }
        }

        return Enemies[index];
    }

    public void SpawnOne(Vector3 pos, Enemy enemy) {
        Enemy newEnemy = Instantiate(enemy, pos, Quaternion.identity, transform);
        Enemies.Add(newEnemy);
    }

    public void Delete(Enemy enemy) {
        if (Enemies.Contains(enemy)) {
            Enemies.Remove(enemy);
        }
    }

    private IEnumerator SpawnEnemyCoroutine() {
        while (Application.isPlaying) {
            yield return new WaitForSeconds(3f);

            while (ShouldSpawnEnemies()) {
                float attackerRate = (Mathf.Pow(DifficultyManager.Instance.Difficulty, 0.5f) - 1f);
                if (attackerRate > 0.75f) attackerRate = 0.75f;
                
                if (Random.Range(0f, 1f) > attackerRate) {
                    SpawnOne();
                } else {
                    SpawnOne(PrefabManager.Instance.Attacker);
                }

                yield return null;
            }
        }
    }
}