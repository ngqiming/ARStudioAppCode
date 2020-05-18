using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    static WebCamTexture phoneCam = null;
    
    // Start is called before the first frame update
    void Start()
    {
        phoneCam = new WebCamTexture();
        GetComponent<Renderer>().material.mainTexture = phoneCam;

        if (!phoneCam.isPlaying) {
            phoneCam.Play();
          } 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
