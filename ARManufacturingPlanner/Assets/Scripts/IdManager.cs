using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IdManager : MonoBehaviour
{
    public List<GameObject> gameObjects;
    public List<string> names;
    public GameObject objectsPanel, instantiateButton, contentPanel, contentObjects, minimizeButton, objectSpawner;
    public bool isDeleted, isMinimized;
    public int idChanged, objectsCreated;
    public RectTransform contentContainer;
    public UserInterfaceManager userInterfaceManager;
    public int idObjectToEdit;
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
        if (isDeleted)
        {
            Invoke("idChange", .2f);
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
        if (!isMinimized)
        {
            contentObjects.SetActive(false);
            objectsPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(90, 90);
            objectsPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(120, -60);
            minimizeButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-45, -45);
            isMinimized = true;
        }
        else
        {
            contentObjects.SetActive(true);
            objectsPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(1000, 500);
            objectsPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(500, -250);
            minimizeButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-50, -60);
            isMinimized = false;
        }
    }
    public void CreateElement()
    {
        objectsCreated++;
        if(gameObjects.Count ==1) {
            OpenObjectsPanel();
        }
        GameObject obj=Instantiate(instantiateButton, contentPanel.transform, true);
        obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0 ,-60*gameObjects.Count-55*(gameObjects.Count-1));
        obj.GetComponent<MenuManager>().idManager = this.gameObject.GetComponent<IdManager>();
        obj.GetComponent<MenuManager>().id = gameObjects.Count;
        names.Add("Object"+objectsCreated);
        contentContainer.sizeDelta = new Vector2(0, gameObjects.Count * 115);
    }
    void idChange()
    {
        contentContainer.sizeDelta = new Vector2(0, gameObjects.Count * 115);
        isDeleted = false;
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
}
