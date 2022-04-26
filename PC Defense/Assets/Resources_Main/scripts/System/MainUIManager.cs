using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class MainUIManager : MonoBehaviour
{
    public static MainUIManager instance;
    public GameObject motelFlare;
    public GameObject playing;
    public GameObject clear;
    public GameObject esc;
    public GameObject firstM;
    public GameObject nextM;
    public GameObject bossBar;

    public Text time;
    public Text roundtext;
    public Text scoretext;
    public Text totalText;
    public Text clearText;

    public Image myhp_bar;
    public Image bullet_bar;
    GameObject boss = null;
    int player = -1;

    public Sprite[] playerImage; // myPlayerType
    private GameObject[] playername; // 모든 플레이어 받아오기
    public GameObject teamhpUI; //팀 HP UI들이 들어있는 게임 오브젝트
    public GameObject myImageUI; // 자기 플레이어 사진
    float[] TeamHp;
    float[] TeamHpMax;
    int[] TeamType;

    int myPlayerType = 10;
    float timer = 0;

    private void Awake()
    {
        instance = this;
        //playername = GameObject.FindGameObjectsWithTag("Player"); // 플레이어 4명 집어 넣고 이 중에서 이름 검색

    }
    // Start is called before the first frame update
    void Start()
    {
        //motelFlare.SetActive(true);
        esc.SetActive(false);
        clear.SetActive(false);
        playing.SetActive(true);
        nextM.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        while (myPlayerType == 10 && (0 != PhotonNetwork.CountOfPlayersInRooms));
        playername = new GameObject[PhotonNetwork.CountOfPlayersInRooms];
        TeamHp = new float[PhotonNetwork.CountOfPlayersInRooms];
        TeamHpMax = new float[PhotonNetwork.CountOfPlayersInRooms];
        TeamType = new int[PhotonNetwork.CountOfPlayersInRooms];
        myImageUI.transform.GetComponent<Image>().sprite = playerImage[myPlayerType];
        //for (int i = 0, temp = 0; i < 4; i++) // 팀원 이미지 바꾸기
        //{
        //    if(i != myPlayerType)
        //    {
        //        teamhpUI.transform.GetChild(temp).GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = playerImage[i];
        //        Debug.Log("AAA");
        //        temp++;
        //    }
        //}
        //teamPImage[]
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        time.text = "Time\n" + (int)(timer / 360) + " : " + (int)(timer / 60) + " : " + (int)(timer / 1 - (int)(timer / 360) * 360 - (int)(timer / 60) * 60);
        RoundSet();

        if (GameManager.instance.isPmove == false)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            esc.SetActive(true);
        }
        if (GameManager.instance.isPmove)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            esc.SetActive(false);
        }

        if(boss != null)
        {
            bossBar.transform.GetChild(1).GetComponentInChildren<Image>().fillAmount = boss.GetComponent<EnemyController>().health / boss.GetComponent<EnemyController>().enemy_Status.hp;
        }
        else
        {
            bossBar.SetActive(false);
		}

		for (int i = 0, temp = 0; i < playername.Length - 1; i++)
		{
			if (i != myPlayerType)
			{
				teamhpUI.transform.GetChild(temp).GetChild(0).GetComponentInChildren<Image>().fillAmount = TeamHp[i] / TeamHpMax[i]; // 팀원체력 출력
				temp++;
			}
		}

		Esc();
        Score();
        Clear();
    }


    void Score()
    {
        //score = (int)(totalTime * 100 + (round * 100));
        scoretext.text = "Score : " + GameManager.instance.score;

    }

    public void setHPImage(int type) //Image player)
    {
        myPlayerType = type - 1;
    }

    public void SetHP(int type, float hp, float MaxHp) // 팀원체력 받아오기
    {
        Debug.Log(type);
        TeamHp[type-1] = hp;
        TeamHpMax[type - 1] = MaxHp;
        Debug.Log("Sucess");
    }

    void RoundSet()
    {
        if (GameManager.instance.round == 11 && GameManager.instance.nextMap == false && GameManager.instance.middleBossDeath != 0)
        {
            firstM.SetActive(false);
            nextM.SetActive(true);
            //roundtext.text = "빨간 신호를 따라 이동하십시오";

        }
        else
        {
            roundtext.text = "Round " + GameManager.instance.round;
        }


    }

    public void Boss(GameObject _boss){
        boss = _boss;
        bossBar.SetActive(true);
	}

    void Esc()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.isPmove = GameManager.instance.isPmove = !GameManager.instance.isPmove;
        }
    }

    void Clear() // 2021.11.02 수정
    {
        if (GameManager.instance.isclear)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("[UM]Clear");
            playing.SetActive(false);
            clear.SetActive(true);
            esc.SetActive(false);
            GameManager.instance.isPmove = false;
            if (GameManager.instance.gameover)
            {
                clearText.text = "GameOver";
            }
            else
            {
                Debug.Log("[UM]Clear / escape");
                clearText.text = "Clear";
                EscapeManager.instance.Escape();
            }

            GameManager.instance.totalscore = GameManager.instance.score + GameManager.instance.round * 100;
            totalText.text = "점수\n" + GameManager.instance.score + "+" + GameManager.instance.round * 100 + "\n" + GameManager.instance.totalscore;
        }
    }

    public void SetPlayer(GameObject p)
    {
        int i = 0;
        foreach(GameObject _p in playername)
        {
            if(_p == null){
                playername[i] = p;
                player++;
                break;
			}
            i++;
		}
	}

    public void ToMain()
    {
        SceneManager.LoadScene("StartScene");
    }


}