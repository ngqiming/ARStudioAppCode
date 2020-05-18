using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private float minTurnAngle = 1;
    private float oldAngle = 0;
    Vector2 startVector = Vector2.zero;
    public float globalDeltaAngle = 0;
    private InstantTrackingController _controller;
    public Transform _activeObject = null;

    private Vector3 _touch1StartGroundPosition;
    private Vector3 _touch2StartGroundPosition;
    private Vector3 _startObjectRotate;
    public bool isRotating = false;
    public bool rotateComplete = false;
    // Start is called before the first frame update
    void Start()
    {
        _controller = GameObject.Find("Controller").gameObject.GetComponent<InstantTrackingController>();
        //Debug.Log("start!");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 2)
        {
            /* We need at least two touches to perform a rotation gesture */
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            Transform hitTransform;
            /* If we're currently not scaling any augmentation, do a raycast for each touch position to find one. */
            if (_activeObject == null)
            {
                if (GetTouchObject(touch1.position, out hitTransform))
                {
                    //get the activeObject from touchinput 
                    SetTouchObject(hitTransform);
                    startVector = touch1.position - touch2.position;
                    //Debug.Log("touch1 input!");
                    //oldAngle = Mathf.Atan2(startVector.y, startVector.x) * Mathf.Rad2Deg - 90;
                }
                else if (GetTouchObject(touch2.position, out hitTransform))
                {   //get the activeObject from touchinput 
                    SetTouchObject(hitTransform);
                    startVector = touch1.position - touch2.position;
                    //oldAngle = Mathf.Atan2(startVector.y, startVector.x) * Mathf.Rad2Deg - 90;
                    //Debug.Log("touch2 input!");
                }
                else if (GetTouchObject((touch1.position + touch2.position) / 2, out hitTransform))
                {
                    //get the activeObject from touchinput 
                    SetTouchObject(hitTransform);
                    startVector = touch1.position - touch2.position;
                    //oldAngle = Mathf.Atan2(startVector.y, startVector.x) * Mathf.Rad2Deg - 90;
                }
                if (_activeObject != null)
                {   //convert touch position (pixel) to ground position of a plane
                    _touch1StartGroundPosition = GetGroundPosition(touch1.position);
                    _touch2StartGroundPosition = GetGroundPosition(touch2.position);
                    startVector = touch1.position - touch2.position;
                    //oldAngle = Mathf.Atan2(startVector.y, startVector.x) * Mathf.Rad2Deg - 90;
                }

            }
            if (_activeObject != null)
            {
                //isRotating = true;
                //Debug.Log("Active object not null!");
                //obtain current vector
                Vector2 curVector = touch1.position - touch2.position;
                //compute delta angle
                float deltaAngle = 0;
                deltaAngle = Vector3.SignedAngle(curVector, startVector, _activeObject.transform.up);
                var LR = Vector3.Cross(startVector, curVector);
                //set values of x,y,z
                float x = _activeObject.localRotation.eulerAngles.x;
                float y = _activeObject.localRotation.eulerAngles.y;
                float z = _activeObject.localRotation.eulerAngles.z;

                if (deltaAngle > minTurnAngle)
                {
                    //clockwise rotation
                    if (LR.z > 0)
                    {
                        deltaAngle = deltaAngle * -1;
                        globalDeltaAngle = deltaAngle;
                        y = y + deltaAngle;
                        _activeObject.localRotation = Quaternion.Euler(x, y, z);
                        rotateComplete = true;
                        //Debug.Log("Object turned right!");
                    }
                    //anti-clockwise rotation 
                    else if (LR.z < 0)
                    {
                        deltaAngle = deltaAngle * 1;
                        globalDeltaAngle = deltaAngle;
                        y = y + deltaAngle;

                        _activeObject.localRotation = Quaternion.Euler(x, y, z);
                        rotateComplete = true;
                        //Debug.Log("Object turned left!");
                    }
                    //set previous vector as current vector
                    startVector = curVector;


                }
                else
                {
                    //Debug.Log("active object is null!");
                    deltaAngle = 0;
                }


                //Set rotateComplete as true

                /* //calculate delta 
                 float angleOffset = Mathf.Atan2(curVector.y, curVector.x) * Mathf.Rad2Deg - 90;
                 var deltaAngle = Mathf.DeltaAngle(angleOffset, oldAngle);
                 oldAngle = angleOffset; 


                 if (deltaAngle > minTurnAngle)
                 {
                     Vector3 LR = Vector3.Cross(startVector, curVector);
                     //get current rotation scale
                     float x = _activeObject.localRotation.eulerAngles.x;
                     float y = _activeObject.localRotation.eulerAngles.y;
                     float z = _activeObject.localRotation.eulerAngles.z;


                       if (LR.z > 0)
                       {
                           deltaAngle = deltaAngle * 1;
                           y = y + deltaAngle;
                           //_activeObject.Rotate (x, y, z);
                           _activeObject.localRotation = Quaternion.Euler(x, y, z);
                       }
                       else if (LR.z < 0)
                       {
                           deltaAngle = deltaAngle * -1;
                           y = y + deltaAngle;
                           //_activeObject.Rotate(x, y, z);
                           _activeObject.localRotation = Quaternion.Euler(x, y, z);
                       }


                 }
                 else {
                     deltaAngle = 0;
                 } */

            }

        }
        else {
            _activeObject = null;
        }
       
        
    }
    private bool GetTouchObject(Vector2 touchPosition, out Transform hitTransform)
    {
        var touchRay = Camera.main.ScreenPointToRay(touchPosition);
        touchRay.origin -= touchRay.direction * 100.0f;

        RaycastHit hit;
        if (Physics.Raycast(touchRay, out hit))
        {
            hitTransform = hit.transform;
            return true;
        }

        hitTransform = null;
        return false;
    }

    private void SetTouchObject(Transform newObject)
    {
        //if ray hit contains an object 
        if (_controller.ActiveModels.Contains(newObject.gameObject))
        {
            _activeObject = newObject;  //set active coordinate  = rayhit coordinate
        }
    }

    private Vector3 GetGroundPosition(Vector2 touchPosition)
    { //get ground position
        var groundPlane = new Plane(Vector3.up, Vector3.zero); //set ground plane
        var touchRay = Camera.main.ScreenPointToRay(touchPosition);
        float enter;
        if (groundPlane.Raycast(touchRay, out enter))
        { //intersects the camera ray with the plane 
            return touchRay.GetPoint(enter); //return coordinate for ground position 
        }
        return Vector3.zero;
    }

}
