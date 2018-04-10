using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {
    public static Player Instance;

    [SerializeField] private AnimationCurve weaponToSpeedCurve;
    [SerializeField] private AudioClip clipHit;
    [SerializeField] private AudioClip[] clipsShootBullet;
    [SerializeField] private AudioClip clipHealth;
    [SerializeField] private AudioClip clipBullet;
    [SerializeField] private AudioClip clipRocket;

    [SerializeField] private Color normalColor;
    [SerializeField] private Color hitColor;
    [SerializeField] private Color normalEmissionColor;
    [SerializeField] [ColorUsageAttribute(true, true, 0.5f, 3f, 0f, 1f)] private Color hitEmissionColor;
    [SerializeField] private Color lowHealthColor;
    [SerializeField] private Color fullHealthColor;
    
    [SerializeField] private Slider slider;
    [SerializeField] private Image fill;

    [SerializeField] private float baseSpeedChange;
    [SerializeField] private float maxHealth = 100f;

    private Rigidbody rb;
    private Coroutine addForceCoroutine;
    private Plane plane;
    private List<Weapon> baseWeapons;
    private RocketLauncher rocketLauncher;
    private Coroutine hitCoroutine;

    private AudioSource sourceHit;
    private AudioSource sourceShootBullet;
    private AudioSource sourceHealth;
    private AudioSource sourceBullet;
    private AudioSource sourceRocket;

    public float Health = 100f;

    public void PickUpHealthSound() {
        Utilities.Audio.PlayAudio(sourceHealth, clipHealth);
    }

    public void PickUpBulletSound() {
        Utilities.Audio.PlayAudio(sourceBullet, clipBullet);
    }

    public void PickUpRocketSound() {
        Utilities.Audio.PlayAudio(sourceRocket, clipRocket);
    }

    public void PickUpWeapon(Weapon newWeapon) {
        newWeapon = Instantiate(newWeapon).GetComponent<Weapon>();
        newWeapon.transform.parent = transform;
        baseWeapons.Add(newWeapon);
    }

    public void TakeDamage(float amount) {
        Health -= amount;
        if (Health <= 0f) {
            Die();
        }

        Utilities.Audio.PlayAudio(sourceHit, clipHit);
        UpdateUI();

        if (hitCoroutine != null) StopCoroutine(hitCoroutine);
        hitCoroutine = StartCoroutine(HitCoroutine());
    }

    public void Heal(float amount) {
        Health += amount;
        if (Health >= maxHealth) {
            Health = maxHealth;
        }

        UpdateUI();
    }

    private void UpdateUI() {
        slider.value = Health / maxHealth;
        fill.color = Color.Lerp(lowHealthColor, fullHealthColor, Health / maxHealth);
    }

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(Instance);
    }

    void Start() {
        rb = GetComponent<Rigidbody>();
        rocketLauncher = GetComponent<RocketLauncher>();

        plane = new Plane(Vector3.back, transform.position);
        baseWeapons = new List<Weapon> {Instantiate(PrefabManager.Instance.MissileLauncher).GetComponent<Weapon>()};
        UpdateUI();

        sourceHit = gameObject.AddComponent<AudioSource>();
        sourceShootBullet = gameObject.AddComponent<AudioSource>();
        sourceHealth = gameObject.AddComponent<AudioSource>();
        sourceBullet = gameObject.AddComponent<AudioSource>();
        sourceRocket = gameObject.AddComponent<AudioSource>();
    }

    private void Update() {
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        plane.Raycast(ray, out distance);
        Vector3 mousePos = ray.origin + ray.direction * distance;
        Vector3 dir = mousePos - transform.position;
        dir = dir.normalized;

        int weaponCount = baseWeapons.Count;

        Vector3 baseDir = Quaternion.AngleAxis(-45f, Vector3.back) * dir;
        float angleStep = 90f / (weaponCount + 1);

        List<Vector3> dirList = new List<Vector3>();

        for (int i = 0; i < weaponCount; i++) {
            dirList.Add(Quaternion.AngleAxis(angleStep * (i + 1), Vector3.back) * baseDir);
            Weapon weapon = baseWeapons[i];
            weapon.transform.position = 0.5f * dirList[i] + transform.position;
            weapon.transform.LookAt(transform.position + dirList[i]);
        }

        if (Input.GetMouseButtonDown(0)) {
            for (int i = 0; i < weaponCount; i++) {
                baseWeapons[i].Shoot(dirList[i]);
            }

            Utilities.Audio.PlayAudioRandom(sourceShootBullet, clipsShootBullet, 0.75f);

            rocketLauncher.Shoot(dir);
            float speedChange = baseSpeedChange * weaponToSpeedCurve.Evaluate(weaponCount > 20 ? 20 : weaponCount);
            rb.velocity += -dir * speedChange;
        }

        float speedCap = Mathf.Pow(DifficultyManager.Instance.Difficulty, 0.33f) * 15f;
        if (rb.velocity.magnitude > speedCap) {
            rb.velocity = rb.velocity.normalized * speedCap;
        }
    }

    private void Die() {
        GameManager.Instance.EndGame();
    }

    private IEnumerator HitCoroutine() {
        float elapsedTime = 0f;
        const float duration = 0.2f;
        Material material = GetComponent<Renderer>().material;

        while (elapsedTime < duration) {
            material.SetColor("_EmissionColor",
                Color.Lerp(material.GetColor("_EmissionColor"), hitEmissionColor, Mathf.Pow(elapsedTime / duration, 2f)));
            material.SetColor("_Color", Color.Lerp(material.GetColor("_Color"), hitColor, Mathf.Pow(elapsedTime / duration, 2f)));
            yield return null;
            elapsedTime += Time.deltaTime;
        }

        elapsedTime = 0f;
        while (elapsedTime < duration) {
            material.SetColor("_EmissionColor",
                Color.Lerp(material.GetColor("_EmissionColor"), normalEmissionColor, Mathf.Pow(elapsedTime / duration, 2f)));
            material.SetColor("_Color", Color.Lerp(material.GetColor("_Color"), normalColor, Mathf.Pow(elapsedTime / duration, 2f)));
            yield return null;
            elapsedTime += Time.deltaTime;
        }
    }
}