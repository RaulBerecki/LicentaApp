using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectMenuScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI positionCoord, rotationCoord;
    public TextMeshProUGUI name;
    IdManager idManager;
    UserInterfaceManager userInterfaceManager;
    public GameObject addGripperButton;
    // Start is called before the first frame update
    void Start()
    {
        idManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<IdManager>();
        userInterfaceManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<UserInterfaceManager>();
        if (idManager.gameObjects[idManager.idObjectToEdit].GetComponent<RobotsController>())
            addGripperButton.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        positionCoord.text = idManager.gameObjects[idManager.idObjectToEdit].transform.position.x.ToString() + "\n" + idManager.gameObjects[idManager.idObjectToEdit].transform.position.y.ToString() + "\n" + idManager.gameObjects[idManager.idObjectToEdit].transform.position.z.ToString();
        rotationCoord.text = idManager.gameObjects[idManager.idObjectToEdit].transform.eulerAngles.x.ToString() + "\n" + idManager.gameObjects[idManager.idObjectToEdit].transform.eulerAngles.y.ToString() + "\n" + idManager.gameObjects[idManager.idObjectToEdit].transform.eulerAngles.z.ToString();
        name.text = idManager.names[idManager.idObjectToEdit];
    }
    public void EditPosition()
    {
        userInterfaceManager.ShowEditPosition();
        userInterfaceManager.BackgroundPanelActive = this.gameObject;
        userInterfaceManager.BackgroundPanelActive.SetActive(false);
    }
    public void EditRotation()
    {
        userInterfaceManager.ShowEditRotation();
        userInterfaceManager.BackgroundPanelActive = this.gameObject;
        userInterfaceManager.BackgroundPanelActive.SetActive(false);
    }
    public void EditName()
    {
        userInterfaceManager.ShowEditName();
        userInterfaceManager.BackgroundPanelActive = this.gameObject;
        userInterfaceManager.BackgroundPanelActive.SetActive(false);
    }
    public void BackButton()
    {
        idManager.ShowObjectsPanel();
        userInterfaceManager.assetPanelButton.SetActive(true);
        Destroy(this.gameObject);
    }
    public void GripperAddButton()
    {
        userInterfaceManager.ShowGripperPanel();
        userInterfaceManager.BackgroundPanelActive = this.gameObject;
        userInterfaceManager.BackgroundPanelActive.SetActive(false);
    }
}
