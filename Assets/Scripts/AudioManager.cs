using System.Collections;
using System;
using UnityEngine.Audio;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager s_instance = null;
    [SerializeField] private Sound[] sounds;

    public static AudioManager instance {
        get {
            if (s_instance == null) {
                s_instance = FindObjectOfType(typeof(AudioManager)) as AudioManager;
            }
            if (s_instance == null) {
                GameObject obj = new GameObject("AudioManager");
                s_instance = obj.AddComponent<AudioManager>();
            }
            return s_instance;
        }
    }

    private void OnApplicationQuit() {
        s_instance = null;
    }

    void Awake() {
        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    public Sound GetSound(string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogWarning("Sound with name " + name + " not found.");
        }
        return s;
    }

    public void Play(string name) {
        Sound s = GetSound(name);
        if (s == null) {
            return;
        }
        if (!s.interruptible && s.source.isPlaying) {
            return;
        }
        if (s.paused) {
            s.source.UnPause();
            s.paused = false;
        } else {
            s.source.Play();
        }
    }

    public void Pause(string name) {
        Sound s = GetSound(name);
        if (s == null) {
            return;
        }
        if (!s.source.isPlaying) {
            Debug.LogWarning("Pause() error: sound with name " + name + " is not playing.");
            return;
        }
        s.source.Pause();
        s.paused = true;
    }

    public void Stop(string name) {
        Sound s = GetSound(name);
        if (s == null) {
            return;
        }
        if (s.source.isPlaying) {
            s.source.Stop();
        }
    }

    public IEnumerator FadeOut(string name, float fadeDuration, bool pause) {
        Sound s = GetSound(name);
        if (s != null) {
            if (!s.source.isPlaying) {
                Debug.LogWarning("FadeOut() error: sound with name " + name + " is not playing.");
            } else {
                while(s.source.volume > 0) {
                    s.source.volume -= s.volume * Time.deltaTime / fadeDuration;
                    yield return null;
                }
                if (pause) {
                    Pause(name);
                } else {
                    Stop(name);
                }
                s.source.volume = s.volume;
            }
        }
    }

    public IEnumerator FadeIn(string name, float fadeDuration, bool unpause) {
        Sound s = GetSound(name);
        if (s != null) {
            s.source.volume = 0;
            if (!unpause) {
                s.paused = false;
            }
            Play(name);
            while(s.source.volume < s.volume) {
                s.source.volume += s.volume * Time.deltaTime / fadeDuration;
                yield return null;
            }
        }
    }

    public IEnumerator FadeTransition(string fadeOutName, float fadeOutDuration, bool pause, float transitionDelay, string fadeInName, float fadeInDuration, bool unpause) {
        Sound fadeOutSound = GetSound(fadeOutName);
        if (fadeOutSound != null && fadeOutSound.source.isPlaying) {
            StartCoroutine(FadeOut(fadeOutName, fadeOutDuration, pause));
            yield return new WaitForSeconds(fadeOutDuration);
            yield return new WaitForSeconds(transitionDelay);
        }
        StartCoroutine(FadeIn(fadeInName, fadeInDuration, unpause));
    }
}