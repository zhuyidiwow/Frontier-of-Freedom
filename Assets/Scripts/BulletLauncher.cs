using System;
using UnityEngine;


public class BulletLauncher : Weapon {
    [SerializeField] private float missileSpeed;
    [SerializeField] private GameObject missilePrefab;
    
    public override void Shoot(Vector3 dir) {
        Vector3 offset = dir * 0.5f;
        GameObject missile = Instantiate(missilePrefab, transform.position + offset, Quaternion.identity, null);
        missile.GetComponent<Rigidbody>().velocity = dir * missileSpeed;
    }
}