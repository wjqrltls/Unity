using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

public class PlayerCamController_Main : MonoBehaviour
{
    public float mouseSensitivity = 100.0f;
    public Transform playerBody;
    public Transform cameraPos;

    float xRotation = 0.0f;

    //네트워크
    public PhotonView pV;

	private void Awake()
	{
        if (!pV.IsMine)
        {
            this.gameObject.tag = "Camera";
            transform.GetComponent<Camera>().enabled = false;
        }
    }

	private void Start()
    {
        Setup();

    }

    void Update()
    {
        if (pV.IsMine)
        {
            transform.position = cameraPos.position;
            if (GameManager.instance.isPmove == true)
            {
                float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
                float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -75, 50);

                transform.localRotation = Quaternion.Euler(xRotation, 0.0f, 0.0f);
                playerBody.Rotate(Vector3.up, mouseX);
            }
        }
    }


    void Setup()
    {
        
    }
}