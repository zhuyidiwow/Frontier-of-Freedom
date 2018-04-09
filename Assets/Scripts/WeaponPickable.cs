using UnityEngine;


public class WeaponPickable : Pickable {

    public enum EWeapon {
        BULLET_LAUNCHER,
        ROCKET_LAUNCHER
    }

    public EWeapon Type;

    protected override void OnPickUp() {
        switch (Type) {
            case EWeapon.BULLET_LAUNCHER:
                Player.Instance.PickUpWeapon(PrefabManager.Instance.MissileLauncher);
                break;
            case EWeapon.ROCKET_LAUNCHER:
                Player.Instance.GetComponent<RocketLauncher>().LevelUp();
                break;
            default:
                Debug.LogError("Error in weapon pickable");
                break;
        }
        
        Destroy(gameObject);
    }
}