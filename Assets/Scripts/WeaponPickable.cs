using UnityEngine;


public class WeaponPickable : Pickable {

    public enum EWeapon {
        MISSILE_LAUNCHER
    }

    public EWeapon Type;

    private void Start() {
        if (Random.Range(0f, 1f) < 0.5f) {
            Destroy(gameObject);
        }
    }

    protected override void OnPickUp() {
        Weapon weapon;
        switch (Type) {
            case EWeapon.MISSILE_LAUNCHER:
                weapon = PrefabManager.Instance.MissileLauncher;
                break;
            default:
                weapon = PrefabManager.Instance.MissileLauncher;
                break;
        }
        Player.Instance.PickUpWeapon(weapon);
        Destroy(gameObject);
    }
}