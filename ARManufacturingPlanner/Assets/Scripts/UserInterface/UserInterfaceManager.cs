using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInterfaceManager : MonoBehaviour
{
    public GameObject UIparent,MenuPanelPrefab,LoadScenesPanelPrefab,AssetsPanelPrefab,BackgroundPanelActive,objectMenuEditor,
        RobotsPanel,BuildingUtilitiesPanel,ProcessEquipmentPanel,FurniturePanel,
        ABBPanel,placePanel,objectsPanel,assetPanelButton,gripperPanel,saveInputPanel;
    public GameObject editPositionMenu, editRotationMenu, editName,objectToEdit;
    IdManager idManager;
    //PlacingVariables
    public PlaceIndicator placeIndicator;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
        idManager = GetComponent<IdManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //start planificare
    public void StartLayout()
    {
        //creare meniu de navighare pentru creare obiect
        GameObject child = Instantiate(AssetsPanelPrefab, UIparent.transform);
        child.transform.parent = UIparent.transform;
        assetPanelButton.SetActive(false);
    }

    public void MenuPanel()
    {
        GameObject child = Instantiate(MenuPanelPrefab, UIparent.transform);
        child.transform.parent = UIparent.transform;
    }
    public void AssetsPanel()
    {
        GameObject child = Instantiate(AssetsPanelPrefab, UIparent.transform);
        child.transform.parent = UIparent.transform;
    }
    public void LoadScenePanel()
    {
        GameObject child = Instantiate(LoadScenesPanelPrefab, UIparent.transform);
        child.transform.parent = UIparent.transform;
    }
    public void Robots()
    {
        GameObject child = Instantiate(RobotsPanel, UIparent.transform);
        child.transform.parent = UIparent.transform;
    }
    public void BuildingUtilities()
    {
        GameObject child = Instantiate(BuildingUtilitiesPanel, UIparent.transform);
        child.transform.parent = UIparent.transform;
    }
    public void ProcessEquipment()
    {
        GameObject child = Instantiate(ProcessEquipmentPanel, UIparent.transform);
        child.transform.parent = UIparent.transform;
    }
    public void BackButtonRobotsPanel()
    {
        GameObject child = Instantiate(AssetsPanelPrefab, UIparent.transform);
        child.transform.parent = UIparent.transform;
    }
    public void ABB()
    {
        GameObject child = Instantiate(ABBPanel, UIparent.transform);
        child.transform.parent = UIparent.transform;
    }
    public void BackButtonABBPanel()
    {
        GameObject child = Instantiate(RobotsPanel, UIparent.transform);
        child.transform.parent = UIparent.transform;
    }
    public void Furniture()
    {
        GameObject child = Instantiate(FurniturePanel, UIparent.transform);
        child.transform.parent = UIparent.transform;
    }
    public void BackButtonAllPanels()
    {
        GameObject child = Instantiate(AssetsPanelPrefab, UIparent.transform);
        child.transform.parent = UIparent.transform;
    }
    public void FinishToPlace()
    {
        Destroy(placeIndicator.theObject);
        placePanel.SetActive(false);
        BackgroundPanelActive.SetActive(true);
        idManager.Minimize();
    }
    public void EditObject()
    {
        GameObject child = Instantiate(objectMenuEditor, UIparent.transform);
        child.transform.parent = UIparent.transform;
        placePanel.SetActive(false);
        if(placeIndicator.indicator)
            placeIndicator.indicator.SetActive(false);
    }
    public void ShowEditPosition()
    {
        GameObject child = Instantiate(editPositionMenu, UIparent.transform);
        child.transform.parent = UIparent.transform;
    }
    public void ShowEditRotation()
    {
        GameObject child = Instantiate(editRotationMenu, UIparent.transform);
        child.transform.parent = UIparent.transform;
    }
    public void ShowEditName()
    {
        GameObject child = Instantiate(editName, UIparent.transform);
        child.transform.parent = UIparent.transform;
    }
    public void ShowGripperPanel()
    {
        GameObject child = Instantiate(gripperPanel, UIparent.transform);
        child.transform.parent = UIparent.transform;
        child.GetComponent<GripperPanelController>().robot = idManager.gameObjects[idManager.idObjectToEdit];
    }
    public void SaveScene()
    {
        GameObject child = Instantiate(saveInputPanel, UIparent.transform);
        child.transform.parent = UIparent.transform;
    }
}
