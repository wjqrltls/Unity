using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_PlayerCamController : MonoBehaviour
{
    public float cameraSensitivity = 0f;
    public Transform playerBody;

    float xRotation = 0.0f;

    private void Start()
    {
        Setup();

    }

    void Update()
    {
        if (GameManager.instance.isPmove == true)
        {
            playerBody.rotation = Quaternion.Euler(playerBody.rotation.x, this.transform.localEulerAngles.y* cameraSensitivity, playerBody.rotation.z);
            //playerBody.Rotate(Vector3.up, transform.rotation.y * Time.deltaTime);
            //Debug.Log(transform.localRotation.y);
        }
    }


    void Setup()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
