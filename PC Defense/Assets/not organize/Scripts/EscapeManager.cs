using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeManager : MonoBehaviour
{
    public static EscapeManager instance;

    public GameObject Helicopter;
    public Transform main;
    public Transform rotor;
    public GameObject camera1;
    float speed = 3000f;

    bool end = true;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        camera1.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //rotor.Rotate(0f, 1f * speed * Time.deltaTime, 0f);
        //Helicopter.transform.position = new Vector3(Helicopter.transform.position.x, Helicopter.transform.position.y+1f * Time.deltaTime, Helicopter.transform.position.z);
    }
    public void Escape()
    {
        camera1.SetActive(true);
        /*while (end)
        {
            StartCoroutine(End());
            rotor.Rotate(0f, 0f * speed * Time.deltaTime, 0f);
            Helicopter.transform.position = new Vector3(Helicopter.transform.position.x, Helicopter.transform.position.y + speed * Time.deltaTime, Helicopter.transform.position.z);
        }
        */
    }
    IEnumerator End()
    {
        yield return new WaitForSeconds(7f);
        end = false;
    }
}
