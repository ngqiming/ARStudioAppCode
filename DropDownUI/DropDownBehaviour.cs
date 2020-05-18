using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Wikitude;

public class DropDownBehaviour: MonoBehaviour
{

    //List<string> names = new List<string>() { "Weather", "Vehicles", "Furniture", "Animals", "SolarSystem" };
    public  Dropdown dropDown;
    private GameObject buttonParent;
    private InstantTrackingController furnitureButton;
    private InstantTrackingController weatherButtons;
    private InstantTrackingController buttonDock;
    private InstantTrackingController VehicleButton;
    private InstantTrackingController AnimalButton;
    private InstantTrackingController SolarSystemButton; 


    // Start is called before the first frame update
    void Start()
    {
        //PopulateList();
        buttonParent = GameObject.Find("Buttons Parent");
        buttonDock = GameObject.Find("Controller").gameObject.GetComponent<InstantTrackingController>();
        furnitureButton = GameObject.Find("Controller").gameObject.GetComponent<InstantTrackingController>();
        weatherButtons = GameObject.Find("Controller").gameObject.GetComponent<InstantTrackingController>();
        VehicleButton = GameObject.Find("Controller").gameObject.GetComponent<InstantTrackingController>();
        AnimalButton = GameObject.Find("Controller").gameObject.GetComponent<InstantTrackingController>();
        SolarSystemButton = GameObject.Find("Controller").gameObject.GetComponent<InstantTrackingController>();
    }
   /*public void PopulateList() {
        var options = new List<string>();
        foreach (var name in names) {

            options.Add(name);
        }
        dropDown.AddOptions(options);
    } */

 

    // Update is called once per frame
    void Update()
    {
        switch (dropDown.value)
        {   // Change the UI buttons depending on which category is selected in the droop down menu.
            case 1:
                buttonDock.ButtonDock.SetActive(true);
                weatherButtons.wButtons.SetActive(true);
                furnitureButton.furnitureButtons.SetActive(false);
                VehicleButton.vButtons.SetActive(false);
                AnimalButton.aButtons.SetActive(false);
                SolarSystemButton.ssButtons.SetActive(false);
                break;
            case 2:
                buttonDock.ButtonDock.SetActive(true);
                VehicleButton.vButtons.SetActive(true);
                weatherButtons.wButtons.SetActive(false);
                furnitureButton.furnitureButtons.SetActive(false);
                AnimalButton.aButtons.SetActive(false);
                SolarSystemButton.ssButtons.SetActive(false);
                break;
            case 3:
                buttonDock.ButtonDock.SetActive(true);
                furnitureButton.furnitureButtons.SetActive(true);
                weatherButtons.wButtons.SetActive(false);
                VehicleButton.vButtons.SetActive(false);
                AnimalButton.aButtons.SetActive(false);
                SolarSystemButton.ssButtons.SetActive(false);
                break;
            case 4:
                buttonDock.ButtonDock.SetActive(true);
                AnimalButton.aButtons.SetActive(true);
                weatherButtons.wButtons.SetActive(false);
                VehicleButton.vButtons.SetActive(false);
                furnitureButton.furnitureButtons.SetActive(false);
                SolarSystemButton.ssButtons.SetActive(false);
                break;

            case 5:
                buttonDock.ButtonDock.SetActive(true);
                SolarSystemButton.ssButtons.SetActive(true);
                weatherButtons.wButtons.SetActive(false);
                VehicleButton.vButtons.SetActive(false);
                furnitureButton.furnitureButtons.SetActive(false);
                AnimalButton.aButtons.SetActive(false);
                break;
                



        }
    }
}
