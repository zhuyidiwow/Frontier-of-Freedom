using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public static GameManager Instance;
	
	public int EnemyCount;
	[HideInInspector] public int MaxEnemy;
	
	[SerializeField] private Text scoreText;

	private int score;

	public bool ShouldSpawnEnemies() {
		return EnemyCount < MaxEnemy;
	}
	
	public void Score(int amount) {
		score += amount;
		scoreText.text = score.ToString();
	}

	private void Start() {
		Instance = this;
	}
}
