using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager Instance;

    public GameObject Canvas;
    [SerializeField] private GameObject[] hideWhenEnd;
    [SerializeField] private AnimationCurve bossWaitScoreCurve;
    [SerializeField] private GameObject endGameCanvas;
    [SerializeField] private Text endGameText;
    [SerializeField] private Text scoreText;

    [Header("Timer")] [SerializeField] private Slider timerSlider;
    [SerializeField] private Text timerText;
    [SerializeField] private float timerCap;

    [HideInInspector] public float Timer;

    [HideInInspector] public int Score = 0;
    private bool isRunning;

    [SerializeField] private AudioClip clipBrickBreak;


    public void PlayBrickBreakAudio() {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        Utilities.Audio.PlayAudio(source, clipBrickBreak, 0.3f, false, Random.Range(1f, 1.2f));
        Destroy(source, 0.5f);
    }
    
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
        foreach (GameObject o in hideWhenEnd) {
            o.SetActive(false);
        }

        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
            endGameText.text = "Or press with 2 fingers to continue";
        }
    }

    private void Awake() {
        Instance = this;
        isRunning = true;
    }

    private void Start() {
        endGameCanvas.SetActive(false);
        Timer = timerCap;
        StartCoroutine(SpawnBossCoroutine());
        StartCoroutine(SpawnItemCoroutine());
    }

    private void Update() {
        if (!isRunning ) {
            if (Application.platform == RuntimePlatform.Android ||
                Application.platform == RuntimePlatform.IPhonePlayer) {

                if (Input.touchCount >= 3) {
                    Time.timeScale = 1f;
                    SceneManager.LoadScene("Main");
                }
            }
            else {
                if (Input.GetKeyDown(KeyCode.R)) {
                    Time.timeScale = 1f;
                    SceneManager.LoadScene("Main");
                }
                
            }
            
        } 
    }

    private void UpdateTimerUI() {
        timerSlider.value = Timer / timerCap;
        timerText.text = Timer.ToString().Substring(0, 4);
    }

    private IEnumerator SpawnBossCoroutine() {
        int count = 0;
        int waitScore = (int) bossWaitScoreCurve.Evaluate(count);
        while (isRunning) {
            var score = waitScore;
            yield return new WaitUntil(()=> Score > score);
            Instantiate(PrefabManager.Instance.Boss,
                Player.Instance.transform.position + new Vector3(CameraManager.Instance.ViewRange.x, CameraManager.Instance.ViewRange.y, 0f),
                Quaternion.identity);
            count++;
            waitScore += (int) bossWaitScoreCurve.Evaluate(count > 10 ? 10 : count);
        }
    }
    
    //TODO: balance this

    private IEnumerator SpawnItemCoroutine() {
        while (isRunning) {
            float waitTime = Random.Range(3f, 6f);
            Pickable pickable;
            if (Player.Instance.Health < 60f) {
                pickable = PrefabManager.Instance.HealthPickable;
                waitTime -= 2f;
            } else {
                float ran = Random.Range(0f, 1f);
                if (ran > 0.67f) {
                    pickable = PrefabManager.Instance.WeaponPickable;
                } else if (ran > 0.33f) {
                    pickable = PrefabManager.Instance.RocketPickable;
                } else {
                    pickable = PrefabManager.Instance.HealthPickable;
                }
            }

            Vector3 playerVelocity = Player.Instance.GetComponent<Rigidbody>().velocity;
            Vector3 offset = playerVelocity.normalized * 20f;

            offset += new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0f);
            Pickable newPickable = Instantiate(pickable, Player.Instance.transform.position + offset, Quaternion.identity);
            yield return new WaitForSeconds(waitTime);
        }
    }
}