using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARTapToPlace : MonoBehaviour
{
    public GameObject objectToPlace;
    private GameObject spawnedObject;
    ARRaycastManager _arRaycastManager;
    Vector2 touchPosition;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

public void SetModel(GameObject model)
{
    Transform trans = objectToPlace.transform;
    objectToPlace = model;
    model.transform.SetPositionAndRotation(trans.position, trans.rotation);
    }
    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        return false;
    }
void Update()
{
    if (!TryGetTouchPosition(out Vector2 touchPosition))
        return;

    if (_arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
    {
        var hitPose = hits[0].pose;

        // check if there's an object already, if so you can move around.
        // you could also just put another one.. depends on your app

        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(objectToPlace, hitPose.position, hitPose.rotation);
        }
        // else
        //{
        //    spawnedObject.transform.position = hitPose.position;
        //}
    }
}
}

