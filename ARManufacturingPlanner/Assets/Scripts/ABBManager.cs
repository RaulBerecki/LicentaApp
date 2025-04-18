using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABBManager : MonoBehaviour
{
    public UserInterfaceManager userInterfaceManager;
    [SerializeField] GameObject[] robots,robotsPlacing;
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
        userInterfaceManager.BackButtonABBPanel();
        Destroy(this.gameObject);
    }
    public void IRB1400()
    {
        SetUpForPlanning(0);
    }
    public void IRB1600()
    {
        SetUpForPlanning(1);
    }
    public void IRB2400()
    {
        SetUpForPlanning(2);
    }
    public void IRB2600()
    {
        SetUpForPlanning(3);
    }
    void SetUpForPlanning(int i)
    {
        placeManager.objectToPlace = robots[i];
        placeIndicator.SetObject(robotsPlacing[i]);
        placeIndicator.indicator.SetActive(true);
        userInterfaceManager.placePanel.SetActive(true);
        userInterfaceManager.BackgroundPanelActive = this.gameObject;
        userInterfaceManager.BackgroundPanelActive.SetActive(false);
    }
}
