using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{   
    public float incremento;

    [Header("Musicas")]
    public AudioSource music;
    public AudioClip intro;
    public AudioClip title;
    public AudioClip level1;
    public AudioClip boss1;
    public AudioClip level2;
    public AudioClip boss2;
    public AudioClip perseguicao;
    public AudioClip gameOver;
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

    private void Start() {
       StartCoroutine("StartMusic");
    }

    public void ChangeMusic(AudioClip newMusic)
    {
        StartCoroutine("FadeMusic", newMusic);
    }

    public void PlayFX(AudioClip newFX, float volume)
    {
        fx.PlayOneShot(newFX);
        fx.volume = volume;
    }

    IEnumerator StartMusic()
    {
        for(float i = 0; i <= 1; i += incremento)
        {
            yield return new WaitForEndOfFrame();
            music.volume = i;
        }
    }

    IEnumerator FadeMusic(AudioClip nMusic)
    {
        for(float i = 1; i >= 0; i -= incremento)
        {
            yield return new WaitForEndOfFrame();
            music.volume = i;
        }print("reduziu");

        music.clip = nMusic;
        music.Play();

        for(float i = 0; i <= 1; i += incremento)
        {
            yield return new WaitForEndOfFrame();
            music.volume = i;
        }print("aumentou");
    }
}
