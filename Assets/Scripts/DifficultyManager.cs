using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class DifficultyManager : MonoBehaviour {
    public static DifficultyManager Instance;

    [HideInInspector] public float Difficulty;
    public float DropPerMinute;

    [SerializeField] private AnimationCurve scoreCurve;
    [SerializeField] private AnimationCurve itemCurve;
    
    private float dropPerSec;
    private float startTime;
    private float timeModifier;
    private float scoreModifier;
    private float itemModifier;
    private int itemCount;
    
    public void AddItemModifier(float amount) {
        itemCount++;
        int evaluationPoint = itemCount > 10 ? 10 : itemCount;
        itemModifier += amount * itemCurve.Evaluate(evaluationPoint);
    }

    public float Evaluate(AnimationCurve curve) {
        return curve.Evaluate(Difficulty);
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
        int score = GameManager.Instance.Score;
        Mathf.Clamp(score, 0, 5000);
        
        timeModifier = -dropPerSec * (Time.time - startTime);
        scoreModifier = scoreCurve.Evaluate(score);
        
        Difficulty = 1f + timeModifier + scoreModifier + itemModifier;
        Mathf.Clamp(Difficulty, 0.5f, 5f);
    }
    
    
    
}