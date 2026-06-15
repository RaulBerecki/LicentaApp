using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

public class ObjectMenuScript : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI positionCoord, rotationCoord;
    // Renamed from "name" so it no longer shadows UnityEngine.Object.name.
    [FormerlySerializedAs("name")]
    public TextMeshProUGUI nameText;
    IdManager idManager;
    UserInterfaceManager userInterfaceManager;
    public GameObject addGripperButton,addRiserButton,addStopperButton;

    // Start is called before the first frame update
    void Start()
    {
        idManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<IdManager>();
        userInterfaceManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<UserInterfaceManager>();

        ObjectUniversalScript ous = EditedObject();
        if (ous.riser != null)
        {
            SetButtonLabel(addRiserButton, ous.GetRiserStatus() ? "RISER ON" : "RISER OFF");
            addGripperButton.SetActive(true);
            addRiserButton.SetActive(true);
        }
        if (ous.stopper != null)
        {
            SetButtonLabel(addStopperButton, ous.GetStopperStatus() ? "STOPPER ON" : "STOPPER OFF");
            addStopperButton.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Transform t = idManager.gameObjects[idManager.idObjectToEdit].transform;
        positionCoord.text = t.position.x + "\n" + t.position.y + "\n" + t.position.z;
        rotationCoord.text = t.eulerAngles.x + "\n" + t.eulerAngles.y + "\n" + t.eulerAngles.z;
        nameText.text = idManager.names[idManager.idObjectToEdit];
    }

    ObjectUniversalScript EditedObject()
    {
        return idManager.gameObjects[idManager.idObjectToEdit].GetComponent<ObjectUniversalScript>();
    }

    static void SetButtonLabel(GameObject button, string text)
    {
        button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
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
    public void RiserClick()
    {
        ObjectUniversalScript ous = EditedObject();
        if (!ous.GetRiserStatus())
        {
            ous.RiserOn();
            SetButtonLabel(addRiserButton, "RISER ON");
        }
        else
        {
            ous.RiserOff();
            SetButtonLabel(addRiserButton, "RISER OFF");
        }
    }
    public void StopperClick()
    {
        ObjectUniversalScript ous = EditedObject();
        if (!ous.GetStopperStatus())
        {
            ous.ActivateStopper();
            SetButtonLabel(addStopperButton, "STOPPER ON");
        }
        else
        {
            ous.DeactivateStopper();
            SetButtonLabel(addStopperButton, "STOPPER OFF");
        }
    }
}
