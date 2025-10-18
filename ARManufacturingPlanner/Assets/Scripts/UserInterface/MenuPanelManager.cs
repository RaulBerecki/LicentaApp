using UnityEngine;

public class MenuPanelManager : MonoBehaviour
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
    public void LoadScenes()
    {
        userInterfaceManager.LoadScenePanel();
        Destroy(this.gameObject);
    }
    public void Assets()
    {
        userInterfaceManager.AssetsPanel();
        Destroy(this.gameObject);
    }
    public void ViewMode()
    {
        userInterfaceManager.assetPanelButton.SetActive(true);
        Destroy(this.gameObject);
    }
}
