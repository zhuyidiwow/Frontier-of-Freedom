using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RocketLauncher : Weapon {
    [HideInInspector] public int Count = 0;
    [HideInInspector] public int Level = 0;

    [SerializeField] private GameObject rocketUI;
    [SerializeField] private Slider slider;
    [SerializeField] private Text levelText;
    [SerializeField] private Text countText;

    public AudioClip[] clipsLaunch;
    private AudioSource audioSource;
    
    private int targetCount = 4;

    private void UpdateUI() {
        rocketUI.SetActive(true);
        levelText.text = "Rocket lv. " + Level;
        if (Count == targetCount) {
            countText.text = "Ready!";
        } else {
            countText.text = Count + "/" + targetCount;
        }
        
        slider.value = (float) Count / targetCount;
    }

    public void LevelUp() {
        Level++;

        UpdateUI();
    }

    public override void Shoot(Vector3 dir) {
        if (Level == 0) return;

        Count++;

        if (Count > targetCount) {
            Count = 0;
            StartCoroutine(LaunchCoroutine(dir));
        }

        UpdateUI();
    }

    private IEnumerator LaunchCoroutine(Vector3 dir) {
        for (int i = 0; i < Level; i++) {
            Vector3 offset = dir * 0.5f;
            Instantiate(PrefabManager.Instance.Rocket, transform.position + offset, Quaternion.identity, null);
            if (!audioSource.isPlaying) {
                Utilities.Audio.PlayAudioRandom(audioSource, clipsLaunch);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void Start() {
        rocketUI.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }
}