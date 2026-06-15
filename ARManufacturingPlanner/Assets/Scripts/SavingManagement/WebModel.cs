// Save-data record for a model downloaded at runtime from a URL.
// Unlike ObiecteBasic/Robot/Conveyor (which store a prefab objectId), this stores
// the source URL so the model can be re-downloaded when the scene is loaded.
public class WebModel
{
    public string url;
    public string name;
    public float positionX, positionY, positionZ;
    public float rotationX, rotationY, rotationZ, rotationW;

    public WebModel(string url, string nameObject, float xP, float yP, float zP, float xR, float yR, float zR, float wR)
    {
        this.url = url;
        this.name = nameObject;
        this.positionX = xP;
        this.positionY = yP;
        this.positionZ = zP;
        this.rotationX = xR;
        this.rotationY = yR;
        this.rotationZ = zR;
        this.rotationW = wR;
    }
}
