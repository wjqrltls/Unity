using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PressTouchPanel();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void PressTouchPanel()
    {
        if (Input.touchCount > 0)
        {
            Ray r = Camera.current.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            /*Vector2 pos = Camera.current.ScreenToWorldPoint(Input.GetTouch(0).position); ;
            Ray2D r2d = new Ray2D(pos, Vector2.zero);
            RaycastHit2D hit2D = Physics2D.Raycast(r2d.origin, r2d.direction, Mathf.Infinity);
            */

            if (Physics.Raycast(r, out hit, Mathf.Infinity))
            {
                Debug.Log("[SampleScript] collider name = " + hit.collider.name);
                if (hit.collider.CompareTag("Card"))
                {
                    Debug.Log("[SapleScript] Card!");
                }
            }

            /*if (hit2D)
            {
                Debug.Log("[SampleScript]2d hit!!");

            }*/
        }
    }
}
