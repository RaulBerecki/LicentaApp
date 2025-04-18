using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IdManager : MonoBehaviour
{
    public List<GameObject> gameObjects;
    public List<string> names;
    public GameObject objectsPanel, instantiateButton, contentPanel, contentObjects;
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
            objectsPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(60, 60);
            objectsPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(50, -35);
            isMinimized = true;
        }
        else
        {
            contentObjects.SetActive(true);
            objectsPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 400);
            objectsPanel.GetComponent<RectTransform>().anchoredPosition = new Vector2(400, -200);
            isMinimized = false;
        }
    }
    public void CreateButton()
    {
        objectsCreated++;
        if(gameObjects.Count ==1) {
            OpenObjectsPanel();
        }
        GameObject obj=Instantiate(instantiateButton, contentPanel.transform, true);
        obj.GetComponent<RectTransform>().anchoredPosition = new Vector2(392 ,-50*gameObjects.Count);
        obj.GetComponent<MenuManager>().idManager = this.gameObject.GetComponent<IdManager>();
        obj.GetComponent<MenuManager>().id = gameObjects.Count;
        names.Add("Object"+objectsCreated);
        contentContainer.sizeDelta = new Vector2(0, 30+ gameObjects.Count * 50);
    }
    void idChange()
    {
        contentContainer.sizeDelta = new Vector2(0, 30 + gameObjects.Count * 50);
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
