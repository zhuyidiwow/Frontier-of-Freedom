using DefaultNamespace;
using UnityEngine;


public class PrefabManager : MonoBehaviour {
    public static PrefabManager Instance;

    public Weapon MissileLauncher;
    public Enemy Enemy;
    public Pickable WeaponPickable;
    public Pickable HealthPickable;
    public DeathZone DeathZone;
    public Rocket Rocket;
    
    
    private void Awake() {
        if (Instance == null) Instance = this;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            
        }
    }
}