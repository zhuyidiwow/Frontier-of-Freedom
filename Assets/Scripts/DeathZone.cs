using UnityEngine;


public class DeathZone : MonoBehaviour {
    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Breakable")) {
            other.GetComponent<Breakable>().Break();
        }

        if (other.CompareTag("Enemy")) {
            GameManager.Instance.Score(other.GetComponent<Enemy>().Score);
            other.GetComponent<Enemy>().Break();
        }
    }
}