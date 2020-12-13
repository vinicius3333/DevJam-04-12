using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{   
    [Header("Musicas")]
    public AudioSource music;
    public AudioClip icebrokenBg;

    [Header("FX")]
    public AudioSource fx;
    public AudioClip ballBounce;
    public AudioClip batAtentention;
    public AudioClip batFly;
    public AudioClip batDie;
    public AudioClip focaDie;
    public AudioClip iceHardBroken; //qnd colidir na parede
    public AudioClip smokeExplosion;
    public AudioClip smokeNormal;
    public AudioClip uiClick;

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }
    public void ChangeMusic(AudioClip newMusic)
    {

    }

    public void PlayFX(AudioClip newFX, float volume)
    {
        fx.PlayOneShot(newFX);
        fx.volume = volume;
    }
}
