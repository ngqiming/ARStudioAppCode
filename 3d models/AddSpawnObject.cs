using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSpawnObject : MonoBehaviour
{
    private InstantTrackingController ITCs;
    // Start is called before the first frame update
    void Start()
    {
        ITCs = GameObject.Find("Controller").gameObject.GetComponent<InstantTrackingController>();
        GameObject curModel = this.gameObject;
        ITCs._activeModels.Add(curModel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
