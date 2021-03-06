﻿using UnityEngine;


public class PrefabManager : MonoBehaviour {
    public static PrefabManager Instance;

    public Weapon MissileLauncher;
    public Enemy Enemy;
    public Enemy Attacker;
    public Pickable WeaponPickable;
    public Pickable HealthPickable;
    public Pickable RocketPickable;
    public DeathZone DeathZone;
    public Rocket Rocket;
    public EnemyBullet EnemyBullet;
    public ScoreText ScoreText;
    public Boss Boss;
    
    private void Awake() {
        if (Instance == null) Instance = this;
    }
}