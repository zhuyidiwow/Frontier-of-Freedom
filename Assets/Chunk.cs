using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour {
	[SerializeField] private GameObject chunkPrefab;
	
	private bool topSpawned;
	private bool topRightSpawned;
	private bool rightSpawned;
	private bool btmRightSpawned;
	private bool btmSpawned;
	private bool btmLeftSpawned;
	private bool leftSpawned;
	private bool topLeftSpawned;
	private ChunkManager chunkManager;
	
	private Chunk chunkTop;
	private Chunk chunkTopRight;
	private Chunk chunkRight;
	private Chunk chunkBtmRight;
	private Chunk chunkBtm;
	private Chunk chunkBtmLeft;
	private Chunk chunkLeft;
	private Chunk chunkTopLeft;
	
	public void SetSpawned(bool top, bool topRight, bool right, bool btmRight, bool btm, bool btmLeft, bool left, bool topLeft) {
		topSpawned = top;
		topRightSpawned = topRight;
		rightSpawned = right;
		btmRightSpawned = btmRight;
		btmSpawned = btm;
		btmLeftSpawned = btmLeft;
		leftSpawned = left;
		topLeftSpawned = topLeft;
	}

	private void Start() {
		chunkManager = ChunkManager.Instance;
	}

	private void SpawnTop() {
		GameObject chunk = Instantiate(chunkPrefab, transform.position + chunkManager.VerStep, Quaternion.identity, chunkManager.transform);
		chunkTop = chunk.GetComponent<Chunk>();
		chunkTop.SetSpawned(false, false, topRightSpawned, rightSpawned, true, leftSpawned, topLeftSpawned, false);
		if (chunkTopRight != null) chunkTopRight.leftSpawned = true;
		
	}

	private void SpawnTopRight() {
		GameObject chunk = Instantiate(chunkPrefab, transform.position + chunkManager.HoriStep + chunkManager.VerStep, Quaternion.identity, chunkManager.transform);
		chunkTopRight = chunk.GetComponent<Chunk>();
		chunkTopRight.SetSpawned(false, false, false, false, rightSpawned, true, topSpawned, false);
	}

	private void SpawnRight() {
		GameObject chunk = Instantiate(chunkPrefab, transform.position + chunkManager.HoriStep, Quaternion.identity, chunkManager.transform);
		chunkRight = chunk.GetComponent<Chunk>();
		chunkRight.SetSpawned(topRightSpawned, false, false, false, btmRightSpawned, btmSpawned, true, topSpawned);
	}

	private void SpawnBtmRight() {
		GameObject chunk = Instantiate(chunkPrefab, transform.position + chunkManager.HoriStep - chunkManager.VerStep, Quaternion.identity, chunkManager.transform);
		chunkBtmRight = chunk.GetComponent<Chunk>();
		chunkBtmRight.SetSpawned(rightSpawned, false, false, false, false, false, btmSpawned, true);
	}

	private void SpawnBtm() {
		GameObject chunk = Instantiate(chunkPrefab, transform.position - chunkManager.VerStep, Quaternion.identity, chunkManager.transform);
		chunkBtm = chunk.GetComponent<Chunk>();
		chunkBtm.SetSpawned(true, rightSpawned, btmRightSpawned, false, false, false, btmLeftSpawned, leftSpawned);
	}

	private void SpawnBtmLeft() {
		GameObject chunk = Instantiate(chunkPrefab, transform.position - chunkManager.VerStep - chunkManager.HoriStep, Quaternion.identity, chunkManager.transform);
		chunkBtmLeft = chunk.GetComponent<Chunk>();
		chunkBtmLeft.SetSpawned(leftSpawned, true, btmSpawned, false, false, false, false, false);
	}

	private void SpawnLeft() {
		GameObject chunk = Instantiate(chunkPrefab, transform.position - chunkManager.HoriStep, Quaternion.identity, chunkManager.transform);
		chunkLeft = chunk.GetComponent<Chunk>();
		chunkLeft.SetSpawned(topLeftSpawned, topSpawned, true, btmSpawned, btmLeftSpawned, false, false, false);
	}

	private void SpawnTopLeft() {
		GameObject chunk = Instantiate(chunkPrefab, transform.position - chunkManager.HoriStep + chunkManager.VerStep, Quaternion.identity, chunkManager.transform);
		chunkTopLeft = chunk.GetComponent<Chunk>();
		chunkTopLeft.SetSpawned(false, false, topSpawned, true, leftSpawned, false, false, false);
	}
	
}
