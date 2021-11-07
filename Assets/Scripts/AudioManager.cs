using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    [SerializeField]
    AudioSource moveSound = default;
    [SerializeField]
    AudioClip[] moveClips = default;
    [SerializeField]
    AudioSource blossomSound = default;
    [SerializeField]
    AudioClip blossomClip = default;
    [SerializeField]
    AudioSource collectSound = default;
    [SerializeField]
    AudioClip collectClip = default;
    [SerializeField]
    AudioSource buttonSound = default;
    [SerializeField]
    AudioSource gameBGM = default;
    [SerializeField]
    AudioSource menuBGM = default;

    IList<AudioSource> bgmSources = new List<AudioSource>();
    void Start() {
        bgmSources.Add(gameBGM);
        bgmSources.Add(menuBGM);
    }

    public void PlayMoveSFX() {
        var randomClip = moveClips[Random.Range(0, moveClips.Length)];
        moveSound.PlayOneShot(randomClip);
    }
    public void PlayBlossomSFX() {
        Debug.Log("Should play audio");
        blossomSound.PlayOneShot(blossomClip);
    }
    public void PlayCollectSFX() {
        collectSound.PlayOneShot(collectClip);
    }
    public void PlayButtonSFX() {
        buttonSound.Play();
    }
    public void PlayGameBGM() {
        foreach (var bgm in bgmSources) {
            bgm.Stop();
        }
        gameBGM.Play();
    }
    public void PlayMenuBGM() {
        foreach (var bgm in bgmSources) {
            bgm.Stop();
        }
        menuBGM.Play();
    }

    public void StopBGM() {
        foreach (var bgm in bgmSources) {
            bgm.Stop();
        }
    }
}
