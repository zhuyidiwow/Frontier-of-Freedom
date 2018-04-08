using UnityEngine;


public class PickableBaseWeapon : Pickable {
    [SerializeField] private Weapon missileLauncher;

    protected override void OnPickUp() {
        Player.Instance.PickUpWeapon(missileLauncher);
        Destroy(gameObject);
    }
}