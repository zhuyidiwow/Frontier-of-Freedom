using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ScoreText : MonoBehaviour {

    [SerializeField] private float gravity;
    [SerializeField] private float lifeTime;
    private float baseScale;
    [SerializeField] private float baseSpeed;
    
    private Vector3 velocity;
    private bool isInitialized;
    private float startTime;
    
    public void Initialize(float value, Vector3 initialDir, Vector3 pos) {
        GetComponent<RectTransform>().position = pos;
        GetComponent<TextMeshPro>().text = "+" + value + "!";
        velocity = initialDir * baseSpeed;
        isInitialized = true;
        startTime = Time.time;
        baseScale = value / 3f;
    }

    private void Update() {
        if (!isInitialized) return;
        
        velocity += Vector3.down * gravity * Time.deltaTime;
        transform.Translate(velocity * Time.deltaTime, Space.World);
        float elapsedTime = Time.time - startTime;

        if (elapsedTime < lifeTime / 3f) {
            transform.localScale = baseScale * Vector3.one * elapsedTime / lifeTime * 3f;
        } else {
            transform.localScale = baseScale * Vector3.one * (1f - (elapsedTime - lifeTime / 3f) / (lifeTime * 2f / 3f));
        }

        if (elapsedTime > lifeTime) {
            Destroy(gameObject);
        }
    }
}
