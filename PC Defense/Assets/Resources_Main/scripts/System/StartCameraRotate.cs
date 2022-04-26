using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCameraRotate : MonoBehaviour
{
    public float speed;
    int sign = -1;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.rotation.y <= sign)
        {
            speed *= sign;
        }
        transform.Rotate(Vector3.up, (transform.rotation.y + speed) * Time.deltaTime, Space.World);
    }
}
