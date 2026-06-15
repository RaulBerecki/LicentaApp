using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IdManager : MonoBehaviour
{
    public List<GameObject> gameObjects, userInterfaceElements;
    public List<string> names;
    public GameObject objectsPanel, instantiateButton, contentPanel, contentObjects, minimizeButton;
    public bool isDeleted, isMinimized;
    public int idChanged, objectsCreated;
    public RectTransform contentContainer;
    public UserInterfaceManager userInterfaceManager;
    public int idObjectToEdit;
    bool reindexScheduled;
    // Start is called before the first frame update
    void Start()
    {
        isDeleted = false;
        isMinimized = false;
        objectsCreated = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // Schedule the reindex once per delete instead of every frame.
        if (isDeleted && !reindexScheduled)
        {
            reindexScheduled = true;
            Invoke(nameof(idChange), .2f);
        }
    }
    public void OpenObjectsPanel()
    {
        objectsPanel.SetActive(true);
    }
    public void CloseObjectPanel()
    {
        objectsPanel.SetActive(false);
    }
    public void Minimize()
    {
        RectTransform panelRect = objectsPanel.GetComponent<RectTransform>();
        RectTransform minimizeRect = minimizeButton.GetComponent<RectTransform>();
        if (!isMinimized)
        {
            contentObjects.SetActive(false);
            panelRect.sizeDelta = new Vector2(90, 90);
            panelRect.anchoredPosition = new Vector2(120, -60);
            minimizeRect.anchoredPosition = new Vector2(-45, -45);
            isMinimized = true;
        }
        else
        {
            contentObjects.SetActive(true);
            panelRect.sizeDelta = new Vector2(1000, 500);
            panelRect.anchoredPosition = new Vector2(500, -250);
            minimizeRect.anchoredPosition = new Vector2(-50, -60);
            isMinimized = false;
        }
    }
    public void CreateElement()
    {
        objectsCreated++;
        if (gameObjects.Count == 1)
        {
            OpenObjectsPanel();
        }
        AddObjectButton();
        names.Add("Object" + objectsCreated);
        gameObjects[gameObjects.Count - 1].GetComponent<ObjectUniversalScript>().objectName = names[names.Count - 1];
    }
    // Instantiates a list-entry button for the most recently added object.
    void AddObjectButton()
    {
        GameObject obj = Instantiate(instantiateButton, contentPanel.transform, true);
        obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -60 * gameObjects.Count - 55 * (gameObjects.Count - 1));
        MenuManager menu = obj.GetComponent<MenuManager>();
        menu.idManager = this;
        menu.id = gameObjects.Count;
        userInterfaceElements.Add(obj);
        contentContainer.sizeDelta = new Vector2(0, gameObjects.Count * 115);
    }
    void idChange()
    {
        contentContainer.sizeDelta = new Vector2(0, gameObjects.Count * 115);
        isDeleted = false;
        reindexScheduled = false;
    }
    public void EditObjectMenu(int idObject)
    {
        idObjectToEdit = idObject;
        userInterfaceManager.EditObject();
        Minimize();
    }
    public void HideObjectsPanel()
    {
        objectsPanel.SetActive(false);
    }
    public void ShowObjectsPanel()
    {
        objectsPanel.SetActive(true);
    }
    public void ResetSceneObjects()
    {
        foreach (var obj in gameObjects)
        {
            Destroy(obj);
        }
        gameObjects.Clear();
        foreach(var obj in userInterfaceElements)
            Destroy(obj);
        userInterfaceElements.Clear();
        objectsCreated = 0;
        CloseObjectPanel();
    }
    public void RegisterObject(GameObject theObject)
    {
        objectsCreated++;
        gameObjects.Add(theObject);
        if (gameObjects.Count == 1)
        {
            OpenObjectsPanel();
        }
        AddObjectButton();
        names.Add(theObject.GetComponent<ObjectUniversalScript>().objectName);
    }
}
