using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//Networking
using Photon.Pun;
using Photon.Realtime;

public class EnemySpawner : MonoBehaviour
{
    //PlayTime pT;

    public GameObject defalt_EnemyPrefabs; // 생성할 원본 
    public GameObject aerial_EnemyPrefabs; // 생성할 원본2 
    public GameObject physical_EnemyPrefabs; // 생성할 원본3
    public GameObject speed_EnemyPrefabs; // 생성할 원본4
    public GameObject explosion_EnemyPrefabs; // 생성할 원본5
    public GameObject reinforced_EnemyPrefabs; // 생성할 원본6
    public GameObject middle_EnemyPrefabs; // 생성할 원본7
    public GameObject final_EnemyPrefabs; // 생성할 원본8
    public float spawnRateMin = 0.5f; // 최소 생성 주기
    public float spawnRateMax = 3f; //최대 생성 주기


    public Transform[] spawnPoints;
    public GameObject[] round5;
    public GameObject[] round7;
    public GameObject[] round10;
    public GameObject[] round13;
    public GameObject[] round16;

    /// <summary>
    PhotonView PV;
    /// </summary>

    //public int round;
    //private Transform target; // 추적당할 대상 
    private float spanwRate; //생성주기
    private float timeAfterSpawn; //최근 생성 시점에서 지난 시간

    private void Awake()
    {
    }

    void Start()
    {
        timeAfterSpawn = 0f; // 누적 시간 초기화
        //pT = FindObjectOfType<PlayTime>();
        spanwRate = Random.Range(spawnRateMin, spawnRateMax);
        //round = 1;

    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (GameManager.instance.round < 21)
            {
                if (timeAfterSpawn >= spanwRate && GameManager.instance.enemyCount < GameManager.instance.round_enemy[GameManager.instance.round] && GameManager.instance.round <= 10 && GameManager.instance.playercreate == true) // 누적된 시간이 생성주기와 같거나 크다면   
                {
                    int x = Random.Range(0, spawnPoints.Length);
                    int y = Random.Range(0, 9);
                    //Debug.Log("[ES]Update / round_enemy : " + GameManager.instance.round_enemy[0]);
                    Spawn(x, y);
                }

                if (timeAfterSpawn >= spanwRate && GameManager.instance.enemyCount < GameManager.instance.round_enemy[GameManager.instance.round] && GameManager.instance.nextMap == true) // 누적된 시간이 생성주기와 같거나 크다면
                {
                    int x = Random.Range(0, spawnPoints.Length);
                    int y = Random.Range(0, 9);
                    //Debug.Log("[ES]Update / round_enemy : " + GameManager.instance.round_enemy[0]);
                    Spawn2(x, y);
                }
                timeAfterSpawn += Time.deltaTime;// 갱신
            }

        }


    }

    //거점1 스폰
    void Spawn(int ranNumx, int ranNumy)
    {
        // Debug.Log("[ES]Spawn / test");

        timeAfterSpawn = 0f; //리셋  
        if (GameManager.instance.round > 0 && GameManager.instance.round < 6)
        {
            //GameObject defalt = Instantiate(defalt_EnemyPrefabs, spawnPoints[ranNumx]);
            PhotonNetwork.Instantiate("Defalt_Enemy (1)", spawnPoints[ranNumx].position, Quaternion.identity);
            /*GameObject defalt2 = PhotonNetwork.Instantiate("Aerial_Enemy (1)", spawnPoints[ranNumx].position, Quaternion.identity);
            GameObject defalt3 = PhotonNetwork.Instantiate("Physical_Enemy (1)", spawnPoints[ranNumx].position, Quaternion.identity);
            GameObject defalt4 = PhotonNetwork.Instantiate("Speed_Enemy (1)", spawnPoints[ranNumx].position, Quaternion.identity);
            GameObject defalt5 = PhotonNetwork.Instantiate("Explosion_Enemy (1)", spawnPoints[ranNumx].position, Quaternion.identity);
            GameObject defalt6 = PhotonNetwork.Instantiate("Reinforced_Enemy (1)", spawnPoints[ranNumx].position, Quaternion.identity);
            GameObject defalt7 = PhotonNetwork.Instantiate("Middle_Enemy (1)", spawnPoints[ranNumx].position, Quaternion.identity);
            GameObject defalt8 = PhotonNetwork.Instantiate("Final_Enemy (1)", spawnPoints[ranNumx].position, Quaternion.identity);*/
            GameManager.instance.enemyCount++;
        }
        if (GameManager.instance.round > 5 && GameManager.instance.round <= 7)
        {
            //GameObject aerial = Instantiate(round5[ranNumy], spawnPoints[ranNumx]);
            PhotonNetwork.Instantiate(round5[ranNumy].name, spawnPoints[ranNumx].position, Quaternion.identity);
            GameManager.instance.enemyCount++;
        }
        if (GameManager.instance.round > 7 && GameManager.instance.round <= 10)
        {
            if (GameManager.instance.round == 10 && GameManager.instance.middleBossCount == 1)
            {
                //GameObject middle = Instantiate(middle_EnemyPrefabs, spawnPoints[ranNumx]);//
                GameObject boss = PhotonNetwork.Instantiate("Middle_Enemy (1)", spawnPoints[ranNumx].position, Quaternion.identity);
                MainUIManager.instance.Boss(boss);
                MainUIManager.instance.bossBar.GetComponentInChildren<Image>().fillAmount = boss.GetComponent<EnemyController>().health / boss.GetComponent<EnemyController>().enemy_Status.hp;
                GameManager.instance.middleBossCount++;
            }

            //GameObject physical = Instantiate(round7[ranNumy], spawnPoints[ranNumx]);
            PhotonNetwork.Instantiate(round7[ranNumy].name, spawnPoints[ranNumx].position, Quaternion.identity);
            GameManager.instance.enemyCount++;

        }

        //GameObject physical = Instantiate(physical_EnemyPrefabs, transform.position, transform.rotation);//

        spanwRate = Random.Range(spawnRateMin, spawnRateMax);


    }

    //거점2 스폰
    void Spawn2(int ranNumx, int ranNumy)
    {
        timeAfterSpawn = 0f; //리셋  

        if (GameManager.instance.round > 10 && GameManager.instance.round <= 13)
        {
            //GameObject speeed = Instantiate(round10[ranNumy], spawnPoints[ranNumx]);
            PhotonNetwork.Instantiate(round10[ranNumy].name, spawnPoints[ranNumx].position, Quaternion.identity);
            GameManager.instance.enemyCount++;
        }
        if (GameManager.instance.round > 13 && GameManager.instance.round <= 15)
        {
            //GameObject reinforced = Instantiate(round13[ranNumy], spawnPoints[ranNumx]);
            PhotonNetwork.Instantiate(round13[ranNumy].name, spawnPoints[ranNumx].position, Quaternion.identity);
            GameManager.instance.enemyCount++;
        }
        if (GameManager.instance.round > 15 && GameManager.instance.round <= 20)
        {
            if (GameManager.instance.round == 20 && GameManager.instance.finalBossCount == 1)
            {
                //GameObject final = Instantiate(final_EnemyPrefabs, spawnPoints[ranNumx]);
                PhotonNetwork.Instantiate("Final_Enemy (1)", spawnPoints[ranNumx].position, Quaternion.identity);
                GameManager.instance.finalBossCount++;
            }
            //GameObject speed = Instantiate(round16[ranNumy], spawnPoints[ranNumx]);
            PhotonNetwork.Instantiate(round16[ranNumy].name, spawnPoints[ranNumx].position, Quaternion.identity);
            GameManager.instance.enemyCount++;
        }
        spanwRate = Random.Range(spawnRateMin, spawnRateMax);
    }



    //IEnumerator Boss()
    //{

    //}
}