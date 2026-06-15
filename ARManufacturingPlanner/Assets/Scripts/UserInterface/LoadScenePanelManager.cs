using System.Collections.Generic;
using UnityEngine;

public class LoadScenePanelManager : MonoBehaviour
{
    public GameObject loadElement,parentContent;
    public List<string> fileNames;
    SaveManager saveManager;
    UserInterfaceManager userInterfaceManager;
    int i;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        i = 0;
        saveManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<SaveManager>();
        userInterfaceManager= GameObject.FindGameObjectWithTag("Manager").GetComponent<UserInterfaceManager>();
        fileNames = saveManager.GetAllSaves();
        foreach (string name in fileNames)
        {
            GameObject element = Instantiate(loadElement,parentContent.transform,true);
            element.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,i*-120-60);
            LoadElementScript elementScript = element.GetComponent<LoadElementScript>();
            elementScript.parent = this.gameObject;
            elementScript.SetFileName(name);
            i++;
        }
    }

    public void BackButton()
    {
        userInterfaceManager.MenuPanel();
        Destroy(this.gameObject);
    }
}
