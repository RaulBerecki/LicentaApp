using UnityEngine;
using TMPro;

public class LoadElementScript : MonoBehaviour
{
    public string fileName;
    public GameObject parent;
    SaveManager saveManager;
    public TextMeshProUGUI elementText;
    UserInterfaceManager userInterfaceManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<SaveManager>();
        userInterfaceManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<UserInterfaceManager>();
    }

    public void LoadSceneNow()
    {
        saveManager.LoadScene(fileName);
        userInterfaceManager.assetPanelButton.SetActive(true);
        Destroy(parent);
    }
    public void SetFileName(string newFileName)
    {
        fileName = newFileName;
        elementText.text = fileName;
    }
}
