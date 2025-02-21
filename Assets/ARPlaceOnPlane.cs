using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.XR.ARCore;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class ARPlaceOnPlane : MonoBehaviour
{
    public ARRaycastManager arRaycaster;
    public GameObject placeObejct;

    GameObject spawnObject;

    public bool readyAR = false;

    public GameObject MessageText;

    public ARPlaneManager planManager;

    IEnumerator DelaySet()
    {
        yield return new WaitForSeconds(0.1f);
        readyAR = true;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            //UpdateCenterObejct();
            if (readyAR)
            {
                Debug.Log("CreateBall");
                PlaceObejcctByTouch();
            }
        }
    }
        
    public void ReTurnToHome()
    {
        // update & save UserData
        Transitioner.Instance.TransitionToScene("Main");
    }

    public void SetAR()
    {
        StartCoroutine(DelaySet());
    }

    public void PlaceObejcctByTouch()
    {
        
        MessageText.SetActive(false);
        Touch touch = Input.GetTouch(0);

        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (arRaycaster.Raycast(touch.position, hits, TrackableType.Planes))
        {
            Pose hitPose = hits[0].pose;

            if (!spawnObject)
            {
                spawnObject = Instantiate(placeObejct, hitPose.position, hitPose.rotation);
                planManager.enabled = false;
                SetAllPlanesActive(false);
            }
            else
            {
                //  spawnObject.transform.position = hitPose.position;
                //  spawnObject.transform.rotation = hitPose.rotation;
            }

        }

       

    }


    void SetAllPlanesActive(bool value)
    {
        foreach(var plane in planManager.trackables)
        {
            plane.gameObject.SetActive(value);
        }

    }

    public void ResetAR()
    {
        readyAR = false;
        planManager.enabled = true;

        SetAllPlanesActive(true);
        if (spawnObject)
        {
            Destroy(spawnObject);
        }
    }



    public void UpdateCenterObejct()
    {
        Vector3 screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        arRaycaster.Raycast(screenCenter, hits, TrackableType.Planes);

        if(hits.Count > 0)
        {
            Pose placementPose = hits[0].pose;
            placeObejct.SetActive(true);
            placeObejct.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        else
        {
            placeObejct.SetActive(false);
        }
    }
}
