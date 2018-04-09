using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public static GameManager Instance;
	
	[SerializeField] private Text scoreText;

	private int score;
	
	public void Score(int amount) {
		score += amount;
		scoreText.text = score.ToString();
	}

	public void EndGame() {
		
	}
	
	private void Awake() {
		Instance = this;
	}
}
