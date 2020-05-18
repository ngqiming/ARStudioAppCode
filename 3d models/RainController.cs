using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wikitude;

public class RainController : MonoBehaviour
{
    public InstantTrackingController trackerController;
    public GameObject buttonParent; 
    // Start is called before the first frame update
    void Start()
    {
        //reference intant tracker controller
        trackerController = GameObject.Find("Controller").gameObject.GetComponent<InstantTrackingController>();
        // ref to  rain button
        buttonParent = GameObject.Find("Buttons Parent");

        //disable the grid when rain got instantiated 
        trackerController._gridRenderer.enabled = false;
        buttonParent.SetActive(false);



    }
    private void OnEnable()
    {
        trackerController._gridRenderer.enabled = false;
        buttonParent.SetActive(false);

    }
    private void OnDisable()
    {
        buttonParent.SetActive(true);
        trackerController._gridRenderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
