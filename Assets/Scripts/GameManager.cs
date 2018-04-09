using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public static GameManager Instance;

	[SerializeField] private GameObject endGameCanvas;
	[SerializeField] private Text scoreText;

	private int score;
	
	public void Score(int amount) {
		score += amount;
		scoreText.text = score.ToString();
	}

	public void EndGame() {
		Time.timeScale = 0f;
		endGameCanvas.SetActive(true);
	}
	
	private void Awake() {
		Instance = this;
	}

	private void Start() {
		endGameCanvas.SetActive(false);
	}
}
