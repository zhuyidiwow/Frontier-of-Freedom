using UnityEngine;


public class PrefabManager : MonoBehaviour {
    public static PrefabManager Instance;

    public Weapon MissileLauncher;
    
    private void Awake() {
        if (Instance == null) Instance = this;
    }
}