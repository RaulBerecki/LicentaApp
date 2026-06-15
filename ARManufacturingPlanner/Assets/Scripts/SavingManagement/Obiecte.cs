using System.Collections.Generic;
using UnityEngine;

public class Obiecte
{
    public int sceneId;
    public string sceneName;
    public float userPositionX, userPositionY,userPositionZ;
    // User transform rotation is stored as Euler angles (loaded back via eulerAngles).
    public float userRotationX, userRotationY, userRotationZ;
    public List<Robot> robots;
    public List<ObiecteBasic> basic;
    public List<Conveyor> conveyors;
    public List<WebModel> webModels;

    public Obiecte(string sceneNameInput, Vector3 userPositionCoord, Quaternion userRotationCoord, List<GameObject> robotsGameObjects, List<GameObject> basicGameObjects, List<GameObject> conveyorsGameObjects, List<GameObject> webModelGameObjects)
    {
        this.sceneName = sceneNameInput;
        this.userPositionX = userPositionCoord.x;
        this.userPositionY = userPositionCoord.y;
        this.userPositionZ = userPositionCoord.z;
        this.userRotationX = userRotationCoord.eulerAngles.x;
        this.userRotationY = userRotationCoord.eulerAngles.y;
        this.userRotationZ = userRotationCoord.eulerAngles.z;
        robots = new List<Robot>();
        basic = new List<ObiecteBasic>();
        conveyors = new List<Conveyor>();
        webModels = new List<WebModel>();
        if (robotsGameObjects != null)
        {
            foreach (GameObject robot in robotsGameObjects)
            {
                ObjectUniversalScript rc = robot.GetComponent<ObjectUniversalScript>();
                Robot newRobot = new Robot(rc.id, rc.objectName, robot.transform.position.x,robot.transform.position.y,robot.transform.position.z,robot.transform.rotation.x,robot.transform.rotation.y,robot.transform.rotation.z,robot.transform.rotation.w, rc.GetGripperStatus(), rc.GetRiserStatus());
                robots.Add(newRobot);
            }
        }
        if (basicGameObjects != null)
        {
            foreach (GameObject basicGameObject in basicGameObjects)
            {
                ObjectUniversalScript bc = basicGameObject.GetComponent<ObjectUniversalScript>();
                ObiecteBasic newObject = new ObiecteBasic(bc.id, bc.objectName, basicGameObject.transform.position.x, basicGameObject.transform.position.y, basicGameObject.transform.position.z, basicGameObject.transform.rotation.x, basicGameObject.transform.rotation.y, basicGameObject.transform.rotation.z, basicGameObject.transform.rotation.w);
                basic.Add(newObject);
            }
        }
        if (conveyorsGameObjects != null)
        {
            foreach (GameObject conveyor in conveyorsGameObjects)
            {
                ObjectUniversalScript cc = conveyor.GetComponent<ObjectUniversalScript>();
                Conveyor newConveyor = new Conveyor(cc.id, cc.objectName, conveyor.transform.position.x,conveyor.transform.position.y,conveyor.transform.position.z,conveyor.transform.rotation.x,conveyor.transform.rotation.y,conveyor.transform.rotation.z,conveyor.transform.rotation.w, cc.GetStopperStatus());
                conveyors.Add(newConveyor);
            }
        }
        if (webModelGameObjects != null)
        {
            foreach (GameObject webModel in webModelGameObjects)
            {
                ObjectUniversalScript wc = webModel.GetComponent<ObjectUniversalScript>();
                WebModel newWebModel = new WebModel(wc.webUrl, wc.objectName, webModel.transform.position.x, webModel.transform.position.y, webModel.transform.position.z, webModel.transform.rotation.x, webModel.transform.rotation.y, webModel.transform.rotation.z, webModel.transform.rotation.w);
                webModels.Add(newWebModel);
            }
        }
    }
}
