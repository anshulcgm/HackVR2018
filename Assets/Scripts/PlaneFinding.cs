﻿using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
public class PlaneFinding : MonoBehaviour {

    public Transform boundingBoxTransform;
    public Vector3 boundingBoxExtents;

    //draws this object on queried planes
    public GameObject planeVisualization;

   [BitMask(typeof(MLWorldPlanesQueryFlags))]public MLWorldPlanesQueryFlags queryFlags;    

    //The frequency with which to request planes
    private float requestTime = 5.0f;
    private float timeSinceLastReq = 0.0f;


    private MLWorldPlanesQueryParams _params = new MLWorldPlanesQueryParams();

    //Collected planes list 
    private List<GameObject> _planesList = new List<GameObject>();

    //Horizontal planes above floors list 
    private List<GameObject> _aboveFloorList = new List<GameObject>();


    private void Start()
    {
        MLWorldPlanes.Start();
        Debug.Log("Started MLWorldPlanes");
    }

    private void OnDestroy()
    {
        MLWorldPlanes.Stop();
    }

    private void Update()
    {
        //Code to request planes every 5 seconds 
        timeSinceLastReq += Time.deltaTime;
        if(timeSinceLastReq > requestTime)
        {
            requestPlanes();
        }
    }

    private void requestPlanes()
    {
        //Sets parameters for planes searching 
        //Debug.Log("Requesting planes");
        _params.Flags = queryFlags;
        _params.MaxResults = 100;
        _params.BoundsCenter = boundingBoxTransform.position;
        _params.BoundsExtents = boundingBoxExtents;

        Debug.Log("Query params are " + _params.ToString());
        Debug.Log("Trying to get planes");
        MLWorldPlanes.GetPlanes(_params, HandleReceivedPlanes);
        Debug.Log("Finished GetPlanes() method");
    }

    private void HandleReceivedPlanes(MLResult result, MLWorldPlane[] planes)
    {
        //Removes current cache of collected planes
        Debug.Log("In HandleRecievedPlanes function");
        Debug.Log("There are currently " + _planesList.Count + " planes in the environment");
        for(int i = _planesList.Count -1; i >=0; i--)
        {
            Destroy(_planesList[i]);
            _planesList.Remove(_planesList[i]);
        }

        for(int i = _aboveFloorList.Count - 1; i >= 0; i--)
        {
            Destroy(_aboveFloorList[i]);
            _aboveFloorList.Remove(_aboveFloorList[i]);
        }

        //Creates new cache of planes
        Debug.Log("There were " + planes.Length + " planes found");
        GameObject newPlane;
        List<float> yValues = new List<float>();
        for(int i = 0; i < planes.Length; i++)
        {
            newPlane = Instantiate(planeVisualization);
            newPlane.transform.position = planes[i].Center;
            newPlane.transform.rotation = planes[i].Rotation;
            newPlane.transform.localScale = new Vector3(planes[i].Width, planes[i].Height, 1.0f);
            _planesList.Add(newPlane);
            yValues.Add(newPlane.transform.position.y);
        }

        float minY = yValues.Min();
        Debug.Log("Minimum y-value is " + minY);
        for(int i = 0; i < _planesList.Count; i++)
        {
            if(_planesList[i].transform.position.y >= minY + 0.5f)
            {
                _aboveFloorList.Add(_planesList[i]);
            }
        }
        Debug.Log("There are currently " + _aboveFloorList.Count + " non-floor planes visible");
    }
}