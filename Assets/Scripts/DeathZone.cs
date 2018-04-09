using UnityEngine;


public class DeathZone : MonoBehaviour {
    [SerializeField] private float damage;
    
    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Breakable")) {
            other.GetComponent<Breakable>().Break();
        }

        if (other.CompareTag("Enemy")) {
            GameManager.Instance.Score(other.GetComponent<Enemy>().Score);
            other.GetComponent<Enemy>().Break();
        }

        if (other.CompareTag("Boss")) {
            other.GetComponent<Boss>().TakeDamage(damage);
        }
        
        
    }
}