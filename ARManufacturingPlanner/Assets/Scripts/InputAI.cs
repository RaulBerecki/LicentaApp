using UnityEngine;

[System.Serializable]
public class InputAI
{
    public string code;
    public string status;

    public InputAI(string newCode)
    {
        code = newCode;
        status = "pending";
    }
}
