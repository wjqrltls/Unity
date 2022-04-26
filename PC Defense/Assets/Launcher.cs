using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//networking : PUN
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{

    private void Start()
    {
        //ConnectStart();
        DontDestroyOnLoad(gameObject);
    }

    public void ConnectStart()
    {
        Debug.Log("연걸 시작");
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // 2 
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected Master");
        PhotonNetwork.JoinOrCreateRoom("text", new RoomOptions { MaxPlayers = 4 }, null);
    }

    // 3
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");

        LetsGameStart();
        //플레이어 선택씬으로 이동
        PhotonNetwork.LoadLevel(2);
    }

    //4
    public void LetsGameStart()
    {
        //게임씬으로 이동
        PhotonNetwork.LoadLevel(3);

        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.Log("현 클라이언트에서 에너미 스포너 삭제");
            Destroy(GameManager.instance.spawn1);
            Destroy(GameManager.instance.spawn2);
        }
    }

    private void Update()
    {

    }
}