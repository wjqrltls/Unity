using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Intance = null;

    public AudioClip cardswap;
    public AudioClip cardclick;

    public AudioClip LogoSong;
    public AudioClip Mainsong;

    public float LowPitchRange = 0.95f;
    public float HighPitchRange = 1.05f;

    public AudioSource audioSource;
    public AudioSource bgm;
    // Start is called before the first frame update
    void Start()
    {
        if (Intance == null)
        {
            Intance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

    }

    public void CardSwap()
    {
        audioSource.clip = cardswap;
        audioSource.Play();
    }

    public void CardClick()
    {
        audioSource.clip = cardclick;
        audioSource.Play();
    }

    public void mainSong()
    {
        bgm.clip = Mainsong;
        bgm.Play();
    }
}
