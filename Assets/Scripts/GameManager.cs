using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    [SerializeField] private GameObject endGameCanvas;
    [SerializeField] private Text scoreText;

    [Header("Timer")]
    [SerializeField] private Slider timerSlider;
    [SerializeField] private Text timerText;
    [SerializeField] private float timerCap;
    
    [HideInInspector] public float Timer;
    
    private int score;

    public void Score(int amount) {
        score += amount;
        scoreText.text = score.ToString();
    }

    public void PickExtraTime(float amount) {
        Timer += amount;
        Mathf.Clamp(Timer, 0f, timerCap);
    }

    public void EndGame() {
        Time.timeScale = 0.1f;
        endGameCanvas.SetActive(true);
    }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        endGameCanvas.SetActive(false);
        Timer = timerCap;
    }

    private void Update() {
        Timer -= Time.deltaTime;
        if (Timer <= 0f) {
            EndGame();
        }
        UpdateTimerUI();
    }

    private void UpdateTimerUI() {
        timerSlider.value = Timer / timerCap;
        timerText.text = Timer.ToString().Substring(0, 4);
    }
}