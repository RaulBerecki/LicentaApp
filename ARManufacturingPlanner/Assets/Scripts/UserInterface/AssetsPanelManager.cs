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
    public void BackButton()
    {
        userInterfaceManager.MenuPanel();
        Destroy(this.gameObject);
    }
    // Downloads a .glb model from the configured GitHub URL and enters ground-placement
    // mode (hologram preview + place panel). Wire a new button on this panel to this method.
    public void LoadGithubModel()
    {
        RuntimeGltfLoader loader = FindAnyObjectByType<RuntimeGltfLoader>();
        if (loader == null)
        {
            Debug.LogWarning("LoadGithubModel: no RuntimeGltfLoader found in the scene. Add a 'Loader' GameObject with the WebModelLoader script.");
            return;
        }
        // Hide this panel while placing; FinishToPlace re-activates it (same as the category panels).
        userInterfaceManager.BackgroundPanelActive = this.gameObject;
        userInterfaceManager.BackgroundPanelActive.SetActive(false);
        loader.LoadModel();
    }
}
