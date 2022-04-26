using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager1 : MonoBehaviour
{
    public static GameManager1 instance;

    float sec;
    int min;

    public int round;
    public int enemy_Death; // 적의 죽음
    public int enemyCount; // 적의 수
    public int[] round_enemy = new int[] { 0, 1, 1, 1, 1, 1, 7, 8, 9, 10, 11, 12, 13, 14 };
    public int middleBossCount = 1;
    public int finalBossCount = 1;

    public bool nextMap;
    public bool nextRound;
    public bool isPmove;
    public bool gameover;
    //public bool gameClear = false;

    public GameObject spawn1;
    public GameObject spawn2;
    public GameObject point1;
    public GameObject point2;
    public GameObject flare;
    public GameObject playing;
    public GameObject clear;
    public GameObject esc;
    public GameObject[] players;

    public float totalTime;
    public int score;
    public int totalscore;
    public Text timetext;
    public Text roundtext;
    public Text scoretext;
    public Text totalText;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        gameover = false;
        Cursor.visible = false;
        sec = 0;
        min = 0;
        round = 1;
        score = 0;
        nextMap = false;
        spawn1.SetActive(true);
        spawn2.SetActive(false);
        point1.SetActive(true);
        point2.SetActive(false);
        flare.SetActive(false);
        playing.SetActive(true);
        nextRound = false;
        clear.SetActive(false);
        isPmove = true;
        players = GameObject.FindGameObjectsWithTag("Player");
        //esc.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        TimeSet(); // 시간
        if (!nextRound && round < 21)
        {
            RoundSet();
        }
        Score();
        SpawnRound();
        Esc();
        if (!isPmove)
        {
            Cursor.visible = true;
            esc.SetActive(true);
        }
        if (isPmove)
        {
            Cursor.visible = false;
            esc.SetActive(false);
        }
    }

    void TimeSet()
    {
        timetext.text = "Time : " + min + "분" + (int)sec + "초"; // 플레이 시간
        //Debug.Log("[GM]Timeset / 시간" + min + sec);
        sec += Time.deltaTime;
        totalTime += Time.deltaTime;
        //Debug.Log("[GM]Timeset / totalTime" + totalTime);
        if (sec >= 60)
        {
            min += 1;
            sec = 0;
        }
    }
    void RoundSet()
    {
        roundtext.text = "Round " + round;
        if (round <= 20 ) 
        {
            if (round_enemy[round] == enemy_Death)
            {
                round++;
                enemy_Death = 0;
                enemyCount = 0;
            }
        }
        if(round > 20 || gameover)
        {
            //Clear();
        }
    
    }

    void Score()
    {
        //score = (int)(totalTime * 100 + (round * 100));
        scoretext.text = "Score : " + score;
        
    }

    void SpawnRound()
    {
        if(round == 11 && nextMap == false && middleBossCount!=1)
        {
            nextRound = true;
            roundtext.text = "다음 구역으로 이동하십시오";
            spawn1.SetActive(false);
            spawn2.SetActive(true);
            point1.SetActive(false);
            point2.SetActive(true);
            flare.SetActive(true);

        }
        else
        {
            nextRound = false;
        }
    }
    void Esc()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPmove = !isPmove;
        }
    }
    /*
    void Clear() // 2021.11.02 수정
    {
        Debug.Log("[GM]Clear");
        playing.SetActive(false);
        //players[0].SetActive(false);
        //players[1].SetActive(false);
        //players[2].SetActive(false);
        //players[3].SetActive(false);
        spawn1.SetActive(false);
        spawn2.SetActive(false);
        clear.SetActive(true);
        isPmove = false;
        if (gameover)
        {
            clearText.text = "GameOver";
        }
        else
        {
            Debug.Log("[GM]Clear / escape");
            clearText.text = "Clear";
            EscapeManager.instance.Escape();
        }

        totalscore = score + round * 100;
        totalText.text = "점수\n" + score + "+" + round * 100 + "\n" + totalscore;
    }
    */
}
