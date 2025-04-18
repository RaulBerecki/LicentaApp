using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public IdManager idManager;
    UserInterfaceManager userInterfaceManager;
    public int id;
    bool change;
    public TextMeshProUGUI objectName;
    // Start is called before the first frame update
    void Start()
    {
        change = true;
        userInterfaceManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<UserInterfaceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (idManager.isDeleted && idManager.idChanged<=id)
        {
            if (!change)
            {
                id--;
                this.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(392, -50 * id);
                change = true;
            }
        }
        else
        {
            change = false;
        }
        objectName.text = idManager.names[id-1];
    }
    public void DeleteObject()
    {
        Destroy(idManager.gameObjects[id-1]);
        idManager.gameObjects.Remove(idManager.gameObjects[id - 1]);
        idManager.names.Remove(idManager.names[id - 1]);
        idManager.idChanged = id;
        idManager.isDeleted = true;
        if(idManager.gameObjects.Count==0)
        {
            idManager.CloseObjectPanel();
        }
        Destroy(this.gameObject);
    }
    public void EditObject()
    {
        idManager.EditObjectMenu(id-1);
        userInterfaceManager.assetPanelButton.SetActive(false);
        idManager.HideObjectsPanel();
    }
}
