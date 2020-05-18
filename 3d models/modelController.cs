using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wikitude;

public class modelController : MonoBehaviour
{
     private InstantTrackingController trackerController;
    // Start is called before the first frame update
    void Start()
    {
        //reference intant tracker controller
        trackerController = GameObject.Find("Controller").gameObject.GetComponent<InstantTrackingController>();
        //disable the grid when rain got instantiated 
        trackerController._gridRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {   //Disable grid
        trackerController._gridRenderer.enabled = false;
    }


    private void OnDisable()
    {   //Enable grid 
        trackerController._gridRenderer.enabled = true;
    }
}
