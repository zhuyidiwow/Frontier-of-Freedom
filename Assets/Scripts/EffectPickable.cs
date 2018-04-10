using UnityEngine;


public class EffectPickable : Pickable {

    protected override void OnPickUp() {
        Player.Instance.Heal(healAmount);
        Player.Instance.PickUpHealthSound();
        Destroy(gameObject);
    }
}