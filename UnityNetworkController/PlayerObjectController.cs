using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class PlayerObjectController : NetworkBehaviour
{
    //get the condition of onBeginDrag so that we can instantiate the models to the server and boardcast it
    private InstantTrackingController isTracking;
    private InstantTrackingController isOnDrag;
    private InstantTrackingController spawnPrefab;
    private InstantTrackingController ITC;
    private Transform activeObject = null;
    //touch position for moving object
    private Vector2 _startTouchPosition;
    private Vector2 _touchOffset;
    private Vector3 _startObjectPosition;
    //Set a variable for moving objects
    public MoveController _moveController;
    private MoveController iActiveObject;
    private MoveController iActiveObjectNull;
    private MoveController oFingerTouch;
    private MoveController sMoveFuction;
    //Scale Controller 
    private ScaleController SC;
    private Rotate RC;
    private Text messageBox;
    private InstantTrackingController receiveModel;
    public GameObject activePrefab;
    public GameObject aActivePrefab;
    private int countObjects = 0;
    List<GameObject> activeObjects;
    public List<GameObject> Models;
    // Start is called before the first frame update
    void Start()
    {
        activeObjects = new List<GameObject>();
        if (isLocalPlayer == false)
        {
            return;
        }

        isTracking = GameObject.Find("Controller").gameObject.GetComponent<InstantTrackingController>();
        isOnDrag = GameObject.Find("Controller").gameObject.GetComponent<InstantTrackingController>();
        spawnPrefab = GameObject.Find("Controller").gameObject.GetComponent<InstantTrackingController>();
        receiveModel = GameObject.Find("Controller").gameObject.GetComponent<InstantTrackingController>();
        _moveController = GameObject.Find("Controller").gameObject.GetComponent<MoveController>();
        ITC = GameObject.Find("Controller").gameObject.GetComponent<InstantTrackingController>();
        SC = GameObject.Find("Controller").gameObject.GetComponent<ScaleController>();
        RC = GameObject.Find("Controller").gameObject.GetComponent<Rotate>();
    }
    public Transform ActiveObject
    {
        get
        {
            return activeObject;
        }
    }

    /*  public void SetMoveObject(Transform newMoveObject)
      {
          if (_controller.ActiveModels.Contains(newMoveObject.gameObject))
          { //Check for active object in the scene
              activeObject = newMoveObject;                      //set activeObject transform = touch object transfrom 
              _startObjectPosition = activeObject.position;      //3d world position
              _startTouchPosition = Input.GetTouch(0).position; // obtain the pixels corrdinate from touch input
              _touchOffset = Camera.main.WorldToScreenPoint(_startObjectPosition);
          }
      } */
    // Update is called once per frame
    void Update()
    {
        //Check if dragging is true 
        if (ITC.isOnDragFuction == true)
        {
            if (isLocalPlayer)
            {
                ITC.isOnDragFuction = false;

                if (isServer)
                {
                    Debug.Log("I am host!");
                    CmdSpawnMyUnits(ITC.clientIndex);
                    Debug.Log("Object Spawned");
                }
                else
                {
                    Debug.Log("I am the client");
                    //Spawn from the client side 

                    Debug.Log("Object Spawned");
                }
            }

        }
        //if is tracking and drag is true, spawn the 3d object
        //if ( isOnDrag.isOnDragFuction == true)
        //{
        //    //Debug.Log("On Drag is true!");
        //    CmdSpawnMyUnit();

        //    isOnDrag.isOnDragFuction = false;
        //    //Debug.Log("On Drag is false");
        //}
        ////Check for reset button
        //if (ITC.resetButtonActive == true) {
        //    CmdDestoryAllObject();
        //    ITC.resetButtonActive = false;
        //}


        //if one input touch and there is object found
        if (Input.touchCount == 1 && _moveController.moveDone == true)
        {
            //reset moveDone
            _moveController.moveDone = false;
            getActivePrefab();
            //Debug.Log("OneFingerTouch is true and active object found!");
            if (activePrefab == null) {
                Debug.Log("Object is empty!");
            }
            //set active prefab position to new position obtained from _moveController Class
            activePrefab.transform.position = _moveController._activeObject.transform.position;
            //set gameobject and its coordinates to the server
            CmdMoveObject(activePrefab, activePrefab.transform.position.x, activePrefab.transform.position.y, activePrefab.transform.position.z);
            activePrefab = null;
        }
        if (SC.scaleDone == true)
        {
            //reset scale done 
            SC.scaleDone = false;
            getActivePrefab();
            if (activePrefab == null)
            {
                Debug.Log("Object is empty!");
            }
            //set our object localscale as scale controller object control scale 
            GameObject buffer = SC._activeObject.gameObject;
            activePrefab.transform.localScale = buffer.transform.localScale;
            //Send to the host 
            CmdScaleObject(activePrefab, activePrefab.transform.localScale.x, activePrefab.transform.localScale.y, activePrefab.transform.localScale.z);

        }
        if (RC.rotateComplete == true)
        {
            //Reset rotate complete 
            RC.rotateComplete = false;
            getActivePrefab();
            //Set our object local rotation as RC's local rotation 
            //GameObject buffer = RC._activeObject.gameObject;
            if (RC.globalDeltaAngle != 0)
            {
                activePrefab.transform.localRotation = RC._activeObject.localRotation;
                CmdRotateObject(activePrefab, activePrefab.transform.localRotation);
            }
        }


    }

    [Command]
    public void CmdSpawnMyUnit()
    {


        if (isOnDrag.onDrag == false)
        { //after dragging on the host has taken place

            //Obtain the model prefab from InstantTrackingController
            activePrefab = spawnPrefab.spawnModelPrefab;
            //obtain touch input coordinate from InstantTrackingController class
            Transform model = receiveModel.sendModel.transform;

            //Assign it's position and rotation
            activePrefab.transform.position = model.transform.position;
            activePrefab.transform.rotation = model.transform.rotation;

            activeObjects.Add(activePrefab);
            //Spawn the object      
            NetworkServer.Spawn(activePrefab);
            //Debug.Log("SPAWNED on client side!");

        }




    }
    [Command]
    public void CmdDestoryAllObject()
    {
        GameObject bufferObject = null;
        //check if gameobject is empty
        if (activeObjects.Count == 0)
        {
            return;
        }
        else
        {
            for (int i = 0; i < activeObjects.Count; i++)
            {
                bufferObject = activeObjects[i];
                NetworkServer.Destroy(bufferObject);
                Debug.Log("Models destroyed! ");
            }
            countObjects = 0;
        }
    }

    [Command]
    public void CmdSetMoveObject(GameObject model)
    {
        if (model == null)
        {
            return;
        }

        else
        {
            RpcSetMoveObject(model);
        }

    }
    [ClientRpc]
    public void RpcSetMoveObject(GameObject newModel)
    {
        if (newModel == null)
        {
            return;
        }
        else
        {
            // _moveController.SetMoveObject(newModel.transform);
        }

    }
    [Command]
    public void CmdMoveObject(GameObject active_Object, float x, float y, float z)
    {
        //check for null object
        if (active_Object != null)
        {
            //Debug.Log("Call RPC Move Object!");
            RpcMoveObject(active_Object, x, y, z);
        }


    }



    [ClientRpc]
    public void RpcMoveObject(GameObject active_Object, float x, float y, float z)
    {
        Vector3 posBuffer;
        //check if object is null 
        if (active_Object == null)
        {
            Debug.Log("No object found");
            return;
        }
        if (isLocalPlayer == false)
        {
            posBuffer.x = x;
            posBuffer.y = y;
            posBuffer.z = z;
            active_Object.transform.position = posBuffer;
            //messageBox.text = "object moved";
        }


    }
    [Command]
    public void CmdScaleObject(GameObject active_Object, float x, float y, float z)
    {
        //Check for null object 
        if (active_Object == null)
        {
            return;
        }
        else if (active_Object != null)
        {
            //Call for RPC Scale Object function
            RpcScaleObject(active_Object, x, y, z);
        }
    }
    [ClientRpc]
    public void RpcScaleObject(GameObject active_Object, float x, float y, float z)
    {
        Vector3 posBuffer;
        if (active_Object == null)
        {
            //Debug.Log("No objects found");
            return;
        }
        if (isLocalPlayer == false)
        {
            posBuffer.x = x;
            posBuffer.y = y;
            posBuffer.z = z;
            active_Object.transform.localScale = posBuffer;
            //Debug.Log("Object scaled!");

        }
        else
        {
            //Debug.Log("Object not scaled");
            return;
        }
    }
    [Command]
    public void CmdRotateObject(GameObject active_Object, Quaternion rotation)
    {
        if (active_Object == null)
        {
            Debug.Log("Object is null!");
            return;
        }
        else
        {
            RpcRotateObject(active_Object, rotation);
        }
    }
    [ClientRpc]
    public void RpcRotateObject(GameObject active_Object, Quaternion rotation)
    {

        if (active_Object == null)
        {
            Debug.Log("Object is null!");
            return;
        }
        //if im not the one who spawn the object 
        else
        {

            active_Object.transform.localRotation = rotation;
            //Debug.Log("Object rotated!");

        }
        //else {
        //    Debug.Log("Object not rotated!");
        //}
    }

    [Command]
    public void CmdSpawnMyUnitsFromClientSide(int modelIndex)
    {
        Debug.Log("Server Side's index: " + modelIndex);
        var selected = Models[modelIndex];
        Debug.Log(selected?.name);
        activePrefab = Instantiate(Models[modelIndex]);
        Transform model = activePrefab.transform;

        /* Set model position at touch position */
        // Set model position by casting a ray from the touch position and finding where it intersects with the ground plane
        var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane p = new Plane(Vector3.up, Vector3.zero);
        float enter;
        if (p.Raycast(cameraRay, out enter))
        {
            model.position = cameraRay.GetPoint(enter);
        }
        /* Set model orientation to face toward the camera */
        Quaternion modelRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(-Camera.main.transform.forward, Vector3.up), Vector3.up);
        model.rotation = modelRotation;
        _moveController.SetMoveObject(model);

        activePrefab.transform.position = model.transform.position;
        activePrefab.transform.rotation = model.transform.rotation;

        GameObject owner = this.gameObject;
        NetworkServer.SpawnWithClientAuthority(activePrefab, owner);
        Debug.Log("Spawned: " + activePrefab.name);
        activePrefab = null;
    }
    [Command]
    public void CmdClientSideSpawn(int clientIndex) {

        //Getting the object from ITC
        GameObject buffer = Instantiate(Models[clientIndex]);
        activePrefab = buffer;
        Transform model = buffer.transform;

        /* Set model position at touch position */
        // Set model position by casting a ray from the touch position and finding where it intersects with the ground plane
        var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane p = new Plane(Vector3.up, Vector3.zero);
        float enter;
        if (p.Raycast(cameraRay, out enter))
        {
            model.position = cameraRay.GetPoint(enter);
        }
        /* Set model orientation to face toward the camera */
        Quaternion modelRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(-Camera.main.transform.forward, Vector3.up), Vector3.up);
        model.rotation = modelRotation;
        _moveController.SetMoveObject(model);

        activePrefab.transform.position = model.transform.position;
        activePrefab.transform.rotation = model.transform.rotation;

        //GameObject owner = this.gameObject;
        NetworkServer.Spawn(activePrefab);
        activePrefab = null;




    }
    [Command]
    public void CmdSpawnMyUnits(int clientIndex)
    {
        GameObject buffer = Instantiate(Models[clientIndex]);
        activePrefab = buffer;
        Transform model = buffer.transform;
        //ITC._activeModels.Add(model.gameObject);

        /* Set model position at touch position */
        // Set model position by casting a ray from the touch position and finding where it intersects with the ground plane
        var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane p = new Plane(Vector3.up, Vector3.zero);
        float enter;
        if (p.Raycast(cameraRay, out enter))
        {
            model.position = cameraRay.GetPoint(enter);
        }
        /* Set model orientation to face toward the camera */
        Quaternion modelRotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(-Camera.main.transform.forward, Vector3.up), Vector3.up);
        model.rotation = modelRotation;
        _moveController.SetMoveObject(model);

        activePrefab.transform.position = model.transform.position;
        activePrefab.transform.rotation = model.transform.rotation;

        GameObject owner = this.gameObject;
        NetworkServer.SpawnWithClientAuthority(activePrefab, owner);
        activePrefab = null;
    }
    public void FindObject(Transform newMoveObject)
    {
        if (ITC.ActiveModels.Contains(newMoveObject.gameObject))
        { //Check for active object in the scene
            activePrefab = newMoveObject.gameObject;                  
            
            
        }
    }
    public void getActivePrefab() {
        if (Input.touchCount == 1) {
            Touch touch = Input.GetTouch(0);
            RaycastHit hit;
            if (activePrefab == null)
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(touch.position), out hit))
                { //if there is an raycast hit to an AR object,  SetMoveObject.
                    FindObject(hit.transform);

                }
            }
            else {
                Debug.Log("Active prefab is not null!");
            }

        }
    }
}