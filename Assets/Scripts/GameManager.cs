using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    public GameObject Canvas;
    [SerializeField] private GameObject endGameCanvas;
    [SerializeField] private Text scoreText;

    [Header("Timer")] [SerializeField] private Slider timerSlider;
    [SerializeField] private Text timerText;
    [SerializeField] private float timerCap;

    [HideInInspector] public float Timer;

    [HideInInspector] public int Score = 0;
    private bool isRunning;
    
    public void GetScore(int amount) {
        if (!isRunning) return;
        
        Score += amount;
        scoreText.text = Score.ToString();
        
    }

    public void PickExtraTime(float amount) {
        Timer += amount;
        Mathf.Clamp(Timer, 0f, timerCap);
    }

    public void EndGame() {
        Time.timeScale = 0.15f;
        endGameCanvas.SetActive(true);
        isRunning = false;
    }

    private void Awake() {
        Instance = this;
        isRunning = true;
    }

    private void Start() {
        endGameCanvas.SetActive(false);
        Timer = timerCap;
        StartCoroutine(SpawnBossCoroutine());
    }

//    private void Update() {
//        Timer -= Time.deltaTime;
//        if (Timer <= 0f) {
//            EndGame();
//        }
//        UpdateTimerUI();
//    }

    private void UpdateTimerUI() {
        timerSlider.value = Timer / timerCap;
        timerText.text = Timer.ToString().Substring(0, 4);
    }

    private IEnumerator SpawnBossCoroutine() {
        int count = 0;
        while (isRunning) {
            yield return new WaitUntil(()=> Score > 200f * (count + 1));
            Instantiate(PrefabManager.Instance.Boss,
                Player.Instance.transform.position + new Vector3(CameraManager.Instance.ViewRange.x, CameraManager.Instance.ViewRange.y, 0f),
                Quaternion.identity);
            count++;
        }
    }
}