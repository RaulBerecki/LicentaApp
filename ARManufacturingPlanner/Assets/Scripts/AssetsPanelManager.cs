using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsPanelManager : MonoBehaviour
{
    public UserInterfaceManager userInterfaceManager;
    // Start is called before the first frame update
    void Start()
    {
        userInterfaceManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<UserInterfaceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Robots()
    {
        userInterfaceManager.Robots();
        Destroy(this.gameObject);
    }
    public void BuildingUtilities()
    {
        userInterfaceManager.BuildingUtilities();
        Destroy(this.gameObject);
    }
    public void ProcessEquipment()
    {
        userInterfaceManager.ProcessEquipment();
        Destroy(this.gameObject);
    }
    public void Furniture()
    {
        userInterfaceManager.Furniture();
        Destroy(this.gameObject);
    }
    public void ViewMode()
    {
        userInterfaceManager.assetPanelButton.SetActive(true);
        Destroy(this.gameObject);
    }
}
