using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour {
    public static DifficultyManager Instance;

    [HideInInspector] public float Difficulty;
    public float DropPerMinute;
    public float ScoreBase;
    public float ScorePower;
    
    private float dropPerSec;

    private float startTime;
    private float timeModifier;
    private float scoreModifier;
    private float itemModifier = 0f;

    public void AddItem(float amount) {
        itemModifier += amount;
    }
    
    private void Awake() {
        if (Instance == null) Instance = this;
        Difficulty = 1f;
    }

    private void Start() {
        dropPerSec = DropPerMinute / 60f;
        startTime = Time.time;
    }

    private void Update() {
        timeModifier = -dropPerSec * (Time.time - startTime);
        scoreModifier = Mathf.Pow(GameManager.Instance.Score / ScoreBase, ScorePower);

        Difficulty = 1f + timeModifier + scoreModifier + itemModifier;
        if (Difficulty < 0.5f) Difficulty = 0.5f;
//        Debug.Log("D: " + Difficulty);
    }
    
    
    
}