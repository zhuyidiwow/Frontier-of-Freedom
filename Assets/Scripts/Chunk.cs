using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
    private ChunkManager chunkManager;
    private Transform player;

    private Chunk top;
    private Chunk topRight;
    private Chunk right;
    private Chunk btmRight;
    private Chunk btm;
    private Chunk btmLeft;
    private Chunk left;
    private Chunk topLeft;

    private float topBound;
    private float rightBound;
    private float btmBound;
    private float leftBound;
    private float threshold;

    private float verMin;
    private float verMax;
    private float horiMin;
    private float horiMax;

    private void Start() {
        chunkManager = ChunkManager.Instance;
        player = Player.Instance.transform;

        topBound = transform.position.y + chunkManager.VerStep.y / 2f;
        rightBound = transform.position.x + chunkManager.HoriStep.x / 2f;
        btmBound = transform.position.y - chunkManager.VerStep.y / 2f;
        leftBound = transform.position.x - chunkManager.HoriStep.x / 2f;
        threshold = chunkManager.Threshold;

        verMin = -chunkManager.VerStep.y / 2f;
        verMax = chunkManager.VerStep.y / 2f;
        horiMin = -chunkManager.HoriStep.x / 2f;
        horiMax = chunkManager.HoriStep.x / 2f;

        if (EnemyManager.Instance.ShouldSpawnEnemies()) {
            SpawnEnemies();
        }

        SpawnItems();
    }

    private void SpawnItems() {
        if (Random.Range(0f, 1f) > 0.25f) {
            float rand = Random.Range(0f, 1f);
            Vector3 offset = new Vector3(Random.Range(horiMin, horiMax), Random.Range(verMin, verMax), 0f);
            if (rand > 0.3f) {
                Instantiate(PrefabManager.Instance.WeaponPickable, transform.position + offset, Quaternion.identity, transform.Find("Items"));
            } else {
                Instantiate(PrefabManager.Instance.HealthPickable, transform.position + offset, Quaternion.identity, transform.Find("Items"));
            }
        }
    }

    private void SpawnEnemies() {
        int enemyCount = chunkManager.AverageEnemy;

        for (int i = 0; i < enemyCount; i++) {
            Vector3 offset = new Vector3(Random.Range(horiMin, horiMax), Random.Range(verMin, verMax), 0f);
            EnemyManager.Instance.SpawnOne(transform.position + offset);
        }
    }

    private void Update() {
        if (player.position.x < rightBound &&
            player.position.x > leftBound &&
            player.position.y < topBound &&
            player.position.y > btmBound) {
            if (topBound - player.transform.position.y < threshold) {
                if (top == null) SpawnTop();

                if (rightBound - player.position.x < threshold) {
                    if (topRight == null) SpawnTopRight();
                }

                if (player.position.x - leftBound < threshold) {
                    if (topLeft == null) SpawnTopLeft();
                }
            }

            if (rightBound - player.position.x < threshold) {
                if (right == null) SpawnRight();
            }

            if (player.position.y - btmBound < threshold) {
                if (btm == null) SpawnBtm();

                if (rightBound - player.position.x < threshold) {
                    if (btmRight == null) SpawnBtmRight();
                }

                if (player.position.x - leftBound < threshold) {
                    if (btmLeft == null) SpawnBtmLeft();
                }
            }

            if (player.position.x - leftBound < threshold) {
                if (left == null) SpawnLeft();
            }
        }
    }

    private void SetChunks(Chunk topChunk, Chunk topRightChunk, Chunk rightChunk, Chunk btmRightChunk, Chunk btmChunk, Chunk btmLeftChunk,
        Chunk leftChunk, Chunk topLeftChunk) {
        top = topChunk;
        topRight = topRightChunk;
        right = rightChunk;
        btmRight = btmRightChunk;
        btm = btmChunk;
        btmLeft = btmLeftChunk;
        left = leftChunk;
        topLeft = topLeftChunk;
    }

    private void UpdateAllNeighbours() {
        if (top != null)
            top.SetChunks(top.top, top.topRight, topRight, right, this, left, topLeft, top.topLeft);
        if (topRight != null)
            topRight.SetChunks(topRight.top, topRight.topRight, topRight.right, topRight.btmRight, right, this, top, topRight.topLeft);
        if (right != null)
            right.SetChunks(topRight, right.topRight, right.right, right.btmRight, btmRight, btm, this, top);
        if (btmRight != null)
            btmRight.SetChunks(right, btmRight.topRight, btmRight.right, btmRight.btmRight, btmRight.btm, btmRight.btmLeft, btm, this);
        if (btm != null)
            btm.SetChunks(this, right, btmRight, btm.btmRight, btm.btm, btm.btmLeft, btmLeft, left);
        if (btmLeft != null)
            btmLeft.SetChunks(left, this, btm, btmLeft.btmRight, btmLeft.btm, btmLeft.btmLeft, btmLeft.left, btmLeft.topLeft);
        if (left != null)
            left.SetChunks(topLeft, top, this, btm, btmLeft, left.btmLeft, left.left, left.topLeft);
        if (topLeft != null)
            topLeft.SetChunks(topLeft.top, topLeft.topRight, top, this, left, topLeft.btmLeft, topLeft.left, topLeft.topLeft);
    }

    private void SpawnTop() {
        GameObject chunk = Instantiate(chunkManager.ChunkPrefab, transform.position + chunkManager.VerStep, Quaternion.identity,
            chunkManager.transform);
        top = chunk.GetComponent<Chunk>();
        UpdateAllNeighbours();
    }

    private void SpawnTopRight() {
        GameObject chunk = Instantiate(chunkManager.ChunkPrefab, transform.position + chunkManager.HoriStep + chunkManager.VerStep,
            Quaternion.identity, chunkManager.transform);
        topRight = chunk.GetComponent<Chunk>();
        UpdateAllNeighbours();
    }

    private void SpawnRight() {
        GameObject chunk = Instantiate(chunkManager.ChunkPrefab, transform.position + chunkManager.HoriStep, Quaternion.identity,
            chunkManager.transform);
        right = chunk.GetComponent<Chunk>();
        UpdateAllNeighbours();
    }

    private void SpawnBtmRight() {
        GameObject chunk = Instantiate(chunkManager.ChunkPrefab, transform.position + chunkManager.HoriStep - chunkManager.VerStep,
            Quaternion.identity, chunkManager.transform);
        btmRight = chunk.GetComponent<Chunk>();
        UpdateAllNeighbours();
    }

    private void SpawnBtm() {
        GameObject chunk = Instantiate(chunkManager.ChunkPrefab, transform.position - chunkManager.VerStep, Quaternion.identity,
            chunkManager.transform);
        btm = chunk.GetComponent<Chunk>();
        UpdateAllNeighbours();
    }

    private void SpawnBtmLeft() {
        GameObject chunk = Instantiate(chunkManager.ChunkPrefab, transform.position - chunkManager.VerStep - chunkManager.HoriStep,
            Quaternion.identity, chunkManager.transform);
        btmLeft = chunk.GetComponent<Chunk>();
        UpdateAllNeighbours();
    }

    private void SpawnLeft() {
        GameObject chunk = Instantiate(chunkManager.ChunkPrefab, transform.position - chunkManager.HoriStep, Quaternion.identity,
            chunkManager.transform);
        left = chunk.GetComponent<Chunk>();
        UpdateAllNeighbours();
    }

    private void SpawnTopLeft() {
        GameObject chunk = Instantiate(chunkManager.ChunkPrefab, transform.position - chunkManager.HoriStep + chunkManager.VerStep,
            Quaternion.identity, chunkManager.transform);
        topLeft = chunk.GetComponent<Chunk>();
        UpdateAllNeighbours();
    }
}