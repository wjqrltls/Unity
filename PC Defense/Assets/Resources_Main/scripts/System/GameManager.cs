using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    float sec;
    int min;

    public int round;
    public int enemy_Death; // 적의 죽음
    public int enemyCount; // 적의 수
    public int[] round_enemy;
    public int middleBossCount = 1;
    public int middleBossDeath = 0;
    public int finalBossCount = 1;
    public int finalBossDeath = 0;


    public bool nextMap;
    public bool nextRound;
    public bool isPmove;
    public bool gameover;
    public bool isclear;
    public bool isnext = true;
    public bool playercreate = false;
    //public bool gameClear = false;

    public GameObject spawn1;
    public GameObject spawn2;
    public GameObject point1;
    public GameObject point2;
    public GameObject flare;
    public GameObject[] players;

    public float totalTime;
    public int score;
    public int totalscore;
    //public Text timetext;
    public Text roundtext;
    public Text scoretext;
    public Text totalText;

    public GameObject sp;

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
        isclear = false;
        //playing.SetActive(true);
        nextRound = false;
        //clear.SetActive(false);
        isPmove = true;
        players = GameObject.FindGameObjectsWithTag("Player");
        //esc.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //TimeSet(); // 시간
        if (!nextRound && round <= 21)
        {
            RoundSet();
        }
        SpawnRound();
        //Esc();

        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
            PhotonNetwork.Instantiate("Alchemist", sp.transform.position, Quaternion.identity);
		}
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            PhotonNetwork.Instantiate("Burglar", sp.transform.position, Quaternion.identity);
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            PhotonNetwork.Instantiate("Doctor", sp.transform.position, Quaternion.identity);
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            PhotonNetwork.Instantiate("Hunter", sp.transform.position, Quaternion.identity);
        }
    }

    void TimeSet()
    {
        //timetext.text = "Time : " + min + "분" + (int)sec + "초"; // 플레이 시간
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

        if (round <= 20)
        {
            if (round_enemy[round] == enemy_Death)
            {

                if (round == 10 && middleBossDeath == 0)
                {
                    // 비워두는거 맞음
                }
                else
                {
                    round++;
                }

                enemy_Death = 0;
                enemyCount = 0;
            }
        }
        if (round > 20 || gameover)
        {
            if (finalBossDeath != 0)
            {
                Clear();
            }
        }

    }


    void SpawnRound()
    {
        if (round == 11 && nextMap == false && middleBossDeath != 0)
        {
            nextRound = true;

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
        Debug.Log("[GM]ESC");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPmove = !isPmove;
        }
    }

    void Clear() // 2021.11.02 수정
    {
        isclear = true;
        Debug.Log("[GM]Clear");
        //playing.SetActive(false);
        //players[0].SetActive(false);
        //players[1].SetActive(false);
        //players[2].SetActive(false);
        //players[3].SetActive(false);
        spawn1.SetActive(false);
        spawn2.SetActive(false);

        //clear.SetActive(true);
        isPmove = false;
    }

}