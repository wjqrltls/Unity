using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int score;
    public Text scoreLabel;

    //start 이전 실행
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }
    public void Score(int num)
    {
        score += num;
        Debug.Log(score);
        scoreLabel.text = "Score : " + score;
    }

    void Start()
    {
        score = 0;
        //scoreLabel = GetComponent<Text>();
    }


    void Update()
    {

    }
}

