using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class ConnectToLauncher : MonoBehaviour
{

    public GameObject b;
    void Start()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            b.SetActive(false);
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void NextRoom()
    {
        GameObject.Find("Launcher").GetComponent<Launcher>().LetsGameStart();
	}
}
