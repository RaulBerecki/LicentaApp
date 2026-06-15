using UnityEngine;
using TMPro;

public class SaveUIController : MonoBehaviour
{
    public TMP_InputField inputFileName;
    SaveManager saveManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        saveManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<SaveManager>();
    }

    public void SaveFile()
    {
        if (inputFileName.text != "")
        {
            saveManager.Save(inputFileName.text);
            Destroy(this.gameObject);
        }
    }
    public void BackButton()
    {
        Destroy(this.gameObject);
    }
}
