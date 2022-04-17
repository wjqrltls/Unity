using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public GameObject IntroObject;
    public GameObject StartObject;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayTime(3));
        SoundManager.Intance.mainSong();
    }

    IEnumerator DelayTime(float time)
    {
        yield return new WaitForSeconds(time);

        IntroObject.SetActive(false);
        StartObject.SetActive(true);
    }


    public void SceneChagne()
    {
        SceneManager.LoadScene("Solitaire");
    }
}
