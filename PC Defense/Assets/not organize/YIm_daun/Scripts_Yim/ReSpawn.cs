using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class ReSpawn : MonoBehaviour
{
    public GameObject[] charPrefabs;
    public GameObject player;

    // Start is called before the first frame update
    public void Start()
    {
        PhotonNetwork.Instantiate(charPrefabs[(int)DataMgr.instance.currentCharacter].name, transform.position, transform.rotation);
    }
}
