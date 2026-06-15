using System.Globalization;
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
        Transform target = EditedTransform();
        if (changePositionInputs.Length > 0)
        {
            changePositionInputs[0].text = Fmt(target.position.x);
            changePositionInputs[1].text = Fmt(target.position.y);
            changePositionInputs[2].text = Fmt(target.position.z);
            currentPositionForReset = target.position;
        }
        if(changeRotationInputs.Length > 0)
        {
            changeRotationInputs[0].text = Fmt(target.eulerAngles.x);
            changeRotationInputs[1].text = Fmt(target.eulerAngles.y);
            changeRotationInputs[2].text = Fmt(target.eulerAngles.z);
            currentRotationForReset = target.eulerAngles;
        }
        if(nameEditorInput)
            nameEditorInput.text = idManager.names[idManager.idObjectToEdit];
    }

    // Update is called once per frame
    void Update()
    {
        Transform target = EditedTransform();
        if (changeRotationInputs.Length > 0)
        {
            if (TryReadVector(changeRotationInputs, out Vector3 euler))
                target.eulerAngles = euler;
        }
        else if (changePositionInputs.Length > 0)
        {
            if (TryReadVector(changePositionInputs, out Vector3 pos))
                target.position = pos;
        }
    }

    Transform EditedTransform()
    {
        return idManager.gameObjects[idManager.idObjectToEdit].transform;
    }

    // Reads three input fields into a Vector3. Returns false (and leaves the object
    // untouched) if any field is empty or mid-edit, so parsing never throws.
    static bool TryReadVector(TMP_InputField[] inputs, out Vector3 result)
    {
        result = Vector3.zero;
        if (!float.TryParse(inputs[0].text, NumberStyles.Float, CultureInfo.InvariantCulture, out float x)) return false;
        if (!float.TryParse(inputs[1].text, NumberStyles.Float, CultureInfo.InvariantCulture, out float y)) return false;
        if (!float.TryParse(inputs[2].text, NumberStyles.Float, CultureInfo.InvariantCulture, out float z)) return false;
        result = new Vector3(x, y, z);
        return true;
    }

    static string Fmt(float value)
    {
        return value.ToString(CultureInfo.InvariantCulture);
    }

    static float ParseOrZero(string text)
    {
        float.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out float value);
        return value;
    }

    void Step(TMP_InputField[] inputs, int axis, float delta)
    {
        inputs[axis].text = Fmt(ParseOrZero(inputs[axis].text) + delta);
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
            changePositionInputs[0].text = Fmt(currentPositionForReset.x);
            changePositionInputs[1].text = Fmt(currentPositionForReset.y);
            changePositionInputs[2].text = Fmt(currentPositionForReset.z);
        }
        else
        {
            changeRotationInputs[0].text = Fmt(currentRotationForReset.x);
            changeRotationInputs[1].text = Fmt(currentRotationForReset.y);
            changeRotationInputs[2].text = Fmt(currentRotationForReset.z);
        }
    }
    public void IncreasePosX() { Step(changePositionInputs, 0, .1f); }
    public void DecreasePosX() { Step(changePositionInputs, 0, -.1f); }
    public void IncreasePosY() { Step(changePositionInputs, 1, .1f); }
    public void DecreasePosY() { Step(changePositionInputs, 1, -.1f); }
    public void IncreasePosZ() { Step(changePositionInputs, 2, .1f); }
    public void DecreasePosZ() { Step(changePositionInputs, 2, -.1f); }
    public void IncreaseRotX() { Step(changeRotationInputs, 0, 5); }
    public void DecreaseRotX() { Step(changeRotationInputs, 0, -5); }
    public void IncreaseRotY() { Step(changeRotationInputs, 1, 5); }
    public void DecreaseRotY() { Step(changeRotationInputs, 1, -5); }
    public void IncreaseRotZ() { Step(changeRotationInputs, 2, 5); }
    public void DecreaseRotZ() { Step(changeRotationInputs, 2, -5); }
    public void Finish()
    {
        userInterfaceManager.BackgroundPanelActive.SetActive(true);
        Destroy(this.gameObject);
    }
}
