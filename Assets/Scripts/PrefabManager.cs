﻿using UnityEngine;


public class PrefabManager : MonoBehaviour {
    public static PrefabManager Instance;

    public Weapon MissileLauncher;
    public GameObject Enemy;
    public Pickable WeaponPickable;
    public Pickable HealthPickable;
    
    
    private void Awake() {
        if (Instance == null) Instance = this;
    }
}