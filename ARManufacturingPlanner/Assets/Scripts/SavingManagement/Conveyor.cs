using UnityEngine;

public class Conveyor
{
    public int objectId;
    public string name;
    public float positionX, positionY, positionZ;
    public float rotationX, rotationY, rotationZ, rotationW;
    public bool stopper;

    public Conveyor(int objectId, string nameObject, float xP, float yP, float zP, float xR, float yR, float zR, float wR, bool stopperStatus)
    {
        this.objectId = objectId;
        this.name = nameObject;
        this.positionX = xP;
        this.positionY = yP;
        this.positionZ = zP;
        this.rotationX = xR;
        this.rotationY = yR;
        this.rotationZ = zR;
        this.rotationW = wR;
        this.stopper = stopperStatus;
    }
}
