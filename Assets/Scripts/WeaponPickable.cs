using UnityEngine;


public class WeaponPickable : Pickable {

    public enum EWeapon {
        MISSILE_LAUNCHER
    }

    public EWeapon Type;

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
        Player.Instance.Heal(healAmount);
        Destroy(gameObject);
    }
}