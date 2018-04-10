using System.Collections;
using UnityEngine;

namespace Utilities {
    
    public class Audio : MonoBehaviour{

        public static void StopIfPlaying(AudioSource audioSource) {
            if (audioSource.isPlaying) {
                audioSource.Stop();
            }    
        }
        
        public static void PlayIfNotAlready(AudioSource audioSource, AudioClip audioClip, float volume = 1f, bool loop = false, float pitch = 1f) {
            if (!audioSource.isPlaying) {
                PlayAudio(audioSource, audioClip, volume, loop, pitch);
            }
        }
        
        public static void PlayAudio(AudioSource audioSource, AudioClip audioClip, float volume = 1f, bool loop = false, float pitch = 1f) {
            audioSource.clip = audioClip;
            audioSource.volume = volume;
            audioSource.loop = loop;
            audioSource.pitch = pitch;
            audioSource.Play();
        }

        public static void PlayAudioRandom(AudioSource audioSource, AudioClip[] audioClips, float volume = 1f, bool loop = false, float pitch = 1f) {
            audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
            audioSource.volume = volume;
            audioSource.loop = loop;
            audioSource.pitch = pitch;
            audioSource.Play();
        }
        
    }
}