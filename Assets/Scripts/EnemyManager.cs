using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void SpawnOne() {
        Vector3 camPos = CameraManager.Instance.transform.position;
        camPos.z = 0f;
        Vector3 offset = new Vector3 {
            x = CameraManager.Instance.ViewRange.x * Random.Range(0.5f, 1f) * (Random.Range(0f, 1f) > 0.5f ? 1f : -1f),
            y = CameraManager.Instance.ViewRange.y * Random.Range(0.5f, 1f) * (Random.Range(0f, 1f) > 0.5f ? 1f : -1f),
            z = 0f
        };
        Vector3 pos = camPos + offset;

        SpawnOne(pos);
    }
    
    public void SpawnOne(Vector3 pos) {
        GameObject enemy = Instantiate(PrefabManager.Instance.Enemy, pos, Quaternion.identity, transform);
        Enemies.Add(enemy.GetComponent<Enemy>());
        enemy.transform.localScale *= Random.Range(0.75f, 1.25f);
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
                SpawnOne();
                yield return null;
            }
            
            
        }
    }
}
