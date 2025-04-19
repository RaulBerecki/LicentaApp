using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EditObjectScript : MonoBehaviour
{
    public int id;
    public IdManager idManager;
    public UserInterfaceManager userInterfaceManager;
    Vector3 currentPositionForReset,currentRotationForReset;
    //Inputs
    public TMP_InputField nameEditorInput;
    public TMP_InputField[] changePositionInputs;
    public TMP_InputField[] changeRotationInputs;
    // Start is called before the first frame update
    void Start()
    {
        idManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<IdManager>();
        userInterfaceManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<UserInterfaceManager>();
        if (changePositionInputs.Length > 0)
        {
            changePositionInputs[0].text = idManager.gameObjects[idManager.idObjectToEdit].transform.position.x.ToString();
            changePositionInputs[1].text = idManager.gameObjects[idManager.idObjectToEdit].transform.position.y.ToString();
            changePositionInputs[2].text = idManager.gameObjects[idManager.idObjectToEdit].transform.position.z.ToString();
            currentPositionForReset = new Vector3(float.Parse(changePositionInputs[0].text), float.Parse(changePositionInputs[1].text), float.Parse(changePositionInputs[2].text));
        }
        if(changeRotationInputs.Length > 0)
        {
            changeRotationInputs[0].text = idManager.gameObjects[idManager.idObjectToEdit].transform.eulerAngles.x.ToString();
            changeRotationInputs[1].text = idManager.gameObjects[idManager.idObjectToEdit].transform.eulerAngles.y.ToString();
            changeRotationInputs[2].text = idManager.gameObjects[idManager.idObjectToEdit].transform.eulerAngles.z.ToString();
            currentRotationForReset = new Vector3(float.Parse(changeRotationInputs[0].text), float.Parse(changeRotationInputs[1].text), float.Parse(changeRotationInputs[2].text));
        }
        if(nameEditorInput)
            nameEditorInput.text = idManager.names[idManager.idObjectToEdit];
    }

    // Update is called once per frame
    void Update()
    {
        if(changeRotationInputs.Length > 0)
            idManager.gameObjects[idManager.idObjectToEdit].transform.eulerAngles = new Vector3(float.Parse(changeRotationInputs[0].text), float.Parse(changeRotationInputs[1].text), float.Parse(changeRotationInputs[2].text));
        else if(changePositionInputs.Length >0)
            idManager.gameObjects[idManager.idObjectToEdit].transform.position = new Vector3(float.Parse(changePositionInputs[0].text), float.Parse(changePositionInputs[1].text), float.Parse(changePositionInputs[2].text));
    }
    public void ChangeName()
    {
        idManager.names[idManager.idObjectToEdit] = nameEditorInput.text;
        Finish();
    }
    public void ResetTransform()
    {
        if (changePositionInputs.Length > 0)
        {
            changePositionInputs[0].text = currentPositionForReset.x.ToString();
            changePositionInputs[1].text = currentPositionForReset.y.ToString();
            changePositionInputs[2].text = currentPositionForReset.z.ToString();
        }
        else
        {
            changeRotationInputs[0].text = currentRotationForReset.x.ToString();
            changeRotationInputs[1].text = currentRotationForReset.y.ToString();
            changeRotationInputs[2].text = currentRotationForReset.z.ToString();
        }
    }
    public void IncreasePosX()
    {
        float currentValue = float.Parse(changePositionInputs[0].text);
        currentValue += 1;
        changePositionInputs[0].text = currentValue.ToString();
    }
    public void DecreasePosX()
    {
        float currentValue = float.Parse(changePositionInputs[0].text);
        currentValue -= 1;
        changePositionInputs[0].text = currentValue.ToString();
    }
    public void IncreasePosY()
    {
        float currentValue = float.Parse(changePositionInputs[1].text);
        currentValue += 1;
        changePositionInputs[1].text = currentValue.ToString();
    }
    public void DecreasePosY()
    {
        float currentValue = float.Parse(changePositionInputs[1].text);
        currentValue -= 1;
        changePositionInputs[1].text = currentValue.ToString();
    }
    public void IncreasePosZ()
    {
        float currentValue = float.Parse(changePositionInputs[2].text);
        currentValue += 1;
        changePositionInputs[2].text = currentValue.ToString();
    }
    public void DecreasePosZ()
    {
        float currentValue = float.Parse(changePositionInputs[2].text);
        currentValue -= 1;
        changePositionInputs[2].text = currentValue.ToString();
    }
    public void IncreaseRotX()
    {
        float currentValue = float.Parse(changeRotationInputs[0].text);
        currentValue += 5;
        changeRotationInputs[0].text = currentValue.ToString();
    }
    public void DecreaseRotX()
    {
        float currentValue = float.Parse(changeRotationInputs[0].text);
        currentValue -= 5;
        changeRotationInputs[0].text = currentValue.ToString();
    }
    public void IncreaseRotY()
    {
        float currentValue = float.Parse(changeRotationInputs[1].text);
        currentValue += 5;
        changeRotationInputs[1].text = currentValue.ToString();
    }
    public void DecreaseRotY()
    {
        float currentValue = float.Parse(changeRotationInputs[1].text);
        currentValue -= 5;
        changeRotationInputs[1].text = currentValue.ToString();
    }
    public void IncreaseRotZ()
    {
        float currentValue = float.Parse(changeRotationInputs[2].text);
        currentValue += 5;
        changeRotationInputs[2].text = currentValue.ToString();
    }
    public void DecreaseRotZ()
    {
        float currentValue = float.Parse(changeRotationInputs[2].text);
        currentValue -= 5;
        changeRotationInputs[2].text = currentValue.ToString();
    }
    public void Finish()
    {
        userInterfaceManager.BackgroundPanelActive.SetActive(true);
        Destroy(this.gameObject);
    }
}
