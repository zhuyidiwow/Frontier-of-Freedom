using UnityEngine;


    public class EnemyBullet : MonoBehaviour {
        [SerializeField] private GameObject particle;
        private Rigidbody rb;
        private float damage;
        
        public void Initialize(float damageAmount, float speed) {
            rb = GetComponent<Rigidbody>();
            damage = damageAmount;

            rb.velocity = (Player.Instance.transform.position - transform.position).normalized * speed;
            
        }

        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("Breakable")) {
                other.GetComponent<Breakable>().Break();
                Destroy(Instantiate(particle, transform.position, Quaternion.identity), 3f);
                Destroy(gameObject);
            }
            
            if (other.CompareTag("Player")) {
                Player.Instance.TakeDamage(damage);
                Destroy(Instantiate(particle, transform.position, Quaternion.identity), 3f);
                Destroy(gameObject);
            }
        }
    }
