using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlaceOnPlane : MonoBehaviour
{
    public ARRaycastManager arRaycaster;
    public GameObject placeObject;
    GameObject spawnObject;
    UserInput userinput;

    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
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
                /*else
                {
                    //spawnObject.transform.position = hitPose.position;
                    //spawnObject.transform.rotation = hitPose.rotation;
                }*/
            }
            placeObject.SetActive(true);
        }
    }

    private void UpdateCenterObject()
    {
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        arRaycaster.Raycast(screenCenter, hits, TrackableType.Planes);

        if (hits.Count > 0)
        {
            Pose placementPose = hits[0].pose;
            placeObject.SetActive(true);
            placeObject.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }

    }

    public void PressTouchPanel()
    {
        if (Input.touchCount > 0)
        {
            Ray r = Camera.current.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(r, out hit, Mathf.Infinity))
            {
                if (hit.collider.CompareTag("Deck"))
                {
                    GameObject.Find("Deck").GetComponent<UserInput>().Deck();
                }
            }
        }
    }

}
