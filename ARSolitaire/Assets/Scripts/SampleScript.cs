using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SampleScript : MonoBehaviour
{
    public ARRaycastManager arRaycaster;
    public GameObject placeObject;
    GameObject spawnObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        PlaceObjectByTouch();
    }

    private void PlaceObjectByTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (arRaycaster.Raycast(touch.position, hits, TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;
                if (!spawnObject)
                {
                    spawnObject = Instantiate(placeObject, hitPose.position, hitPose.rotation);
                    spawnObject.transform.localRotation = Quaternion.identity;
                }
                else
                {
                    //spawnObject.transform.position = hitPose.position;
                    //spawnObject.transform.rotation = hitPose.rotation;
                }
            }
            //placeObject.SetActive(true);
        }
    }

    public void PressTouchPanel()
    {
        if (Input.touchCount > 0)
        {
            //Ray r = Camera.current.ScreenPointToRay(Input.GetTouch(0).position);
            //RaycastHit hit;

            Vector2 pos = Camera.current.ScreenToWorldPoint(Input.GetTouch(0).position); ;
            Ray2D r2d = new Ray2D(pos, Vector2.zero);
            RaycastHit2D hit2D = Physics2D.Raycast(r2d.origin,r2d.direction, Mathf.Infinity);


            //if (Physics.Raycast(r, out hit, Mathf.Infinity))
            //{
            //    Debug.Log("[SampleScript] collider name = " + hit.collider.name);
            //    if (hit.collider.CompareTag("Cubes__"))
            //    {
            //        Debug.Log("[SapleScript] Cubes__ Tag!");
            //    }
            //}

            if (hit2D)
            {
                Debug.Log("[SampleScript]2d hit!!");
            }
        }
    }
}
