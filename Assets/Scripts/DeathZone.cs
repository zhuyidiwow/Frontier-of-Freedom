using UnityEngine;


public class DeathZone : MonoBehaviour {
    [SerializeField] private float damage;
    private bool hasHitBoss = false;
    
    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Breakable")) {
            other.GetComponent<Breakable>().Break();
        }

        if (other.CompareTag("Enemy")) {
            GameManager.Instance.GetScore(other.GetComponent<Enemy>().Score);
            other.GetComponent<Enemy>().Break();
        }

        if (other.CompareTag("Boss")) {
            if (!hasHitBoss) {
                other.GetComponent<Boss>().TakeDamage(damage);
                hasHitBoss = true;
            }
        }
        
        
    }
}