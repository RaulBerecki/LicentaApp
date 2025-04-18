using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalObjectPanelController : MonoBehaviour
{
    public UserInterfaceManager userInterfaceManager;
    [SerializeField] GameObject[] objects,objectsPlacing;
    PlaceManager placeManager;
    PlaceIndicator placeIndicator;

    // Start is called before the first frame update
    void Start()
    {
        userInterfaceManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<UserInterfaceManager>();
        placeManager = GameObject.FindGameObjectWithTag("PlaceManager").GetComponent<PlaceManager>();
        placeIndicator = GameObject.FindGameObjectWithTag("PlaceIndicator").GetComponent<PlaceIndicator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void BackButton()
    {
        userInterfaceManager.BackButtonAllPanels();
        Destroy(this.gameObject);
    }
    public void Object1()
    {
        SetUpForPlanning(0);
    }
    public void Object2()
    {
        SetUpForPlanning(1);
    }
    public void Object3()
    {
        SetUpForPlanning(2);
    }
    public void Object4()
    {
        SetUpForPlanning(3);
    }
    public void Object5()
    {
        SetUpForPlanning(4);
    }
    public void Object6()
    {
        SetUpForPlanning(5);
    }
    public void Object7()
    {
        SetUpForPlanning(6);
    }
    void SetUpForPlanning(int i)
    {
        placeManager.objectToPlace = objects[i];
        placeIndicator.SetObject(objectsPlacing[i]);
        placeIndicator.indicator.SetActive(true);
        userInterfaceManager.placePanel.SetActive(true);
        userInterfaceManager.BackgroundPanelActive = this.gameObject;
        userInterfaceManager.BackgroundPanelActive.SetActive(false);
    }
}
