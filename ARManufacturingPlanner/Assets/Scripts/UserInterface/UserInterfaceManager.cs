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

    // Instantiates a UI panel under UIparent and returns it.
    GameObject OpenPanel(GameObject prefab)
    {
        return Instantiate(prefab, UIparent.transform);
    }

    //start planificare
    public void StartLayout()
    {
        //creare meniu de navighare pentru creare obiect
        OpenPanel(AssetsPanelPrefab);
        assetPanelButton.SetActive(false);
    }

    public void MenuPanel() { OpenPanel(MenuPanelPrefab); }
    public void AssetsPanel() { OpenPanel(AssetsPanelPrefab); }
    public void LoadScenePanel() { OpenPanel(LoadScenesPanelPrefab); }
    public void Robots() { OpenPanel(RobotsPanel); }
    public void BuildingUtilities() { OpenPanel(BuildingUtilitiesPanel); }
    public void ProcessEquipment() { OpenPanel(ProcessEquipmentPanel); }
    public void BackButtonRobotsPanel() { OpenPanel(AssetsPanelPrefab); }
    public void ABB() { OpenPanel(ABBPanel); }
    public void BackButtonABBPanel() { OpenPanel(RobotsPanel); }
    public void Furniture() { OpenPanel(FurniturePanel); }
    public void BackButtonAllPanels() { OpenPanel(AssetsPanelPrefab); }
    public void ShowEditPosition() { OpenPanel(editPositionMenu); }
    public void ShowEditRotation() { OpenPanel(editRotationMenu); }
    public void ShowEditName() { OpenPanel(editName); }
    public void SaveScene() { OpenPanel(saveInputPanel); }

    public void FinishToPlace()
    {
        Destroy(placeIndicator.theObject);
        placePanel.SetActive(false);
        BackgroundPanelActive.SetActive(true);
        idManager.Minimize();
    }
    public void EditObject()
    {
        OpenPanel(objectMenuEditor);
        placePanel.SetActive(false);
        if(placeIndicator.indicator)
            placeIndicator.indicator.SetActive(false);
    }
    public void ShowGripperPanel()
    {
        GameObject child = OpenPanel(gripperPanel);
        child.GetComponent<GripperPanelController>().robot = idManager.gameObjects[idManager.idObjectToEdit];
    }
}
