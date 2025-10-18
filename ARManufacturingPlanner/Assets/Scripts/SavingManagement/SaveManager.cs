using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] IdManager idManager;
    [SerializeField] Transform userTransform;
    List<GameObject> robots, conveyors, basics;
    [Header("Robots")]
    [SerializeField] GameObject[] robotsAbb;
    [Header("Conveyors with Stopper")]
    [SerializeField] GameObject[] conveyorsStopperPrefab;
    [Header("Basics")]
    [SerializeField]
    GameObject[] cabinetPrefabs,workbenchPrefabs,buildingUtilitiesElectricalPrefabs,conveyorPrefabs,processEquipmentPrefabs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetAllSaves();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public List<string> GetAllSaves()
    {
        List<string> saves = new List<string>();
        foreach (var file in Directory.GetFiles(Application.persistentDataPath, "*.json"))
        {
            saves.Add(Path.GetFileNameWithoutExtension(file));
        }
        return saves;
    }
    public void Save(string fileName)
    {
        robots = new List<GameObject>();
        conveyors = new List<GameObject>();
        basics = new List<GameObject>();
        foreach(GameObject obiect in idManager.gameObjects)
        {
            ObjectUniversalScript ous = obiect.GetComponent<ObjectUniversalScript>();
            if(ous.riser!=null)
                robots.Add(obiect);
            else if(ous.stopper!=null)
                conveyors.Add(obiect);
            else
                basics.Add(obiect);
        }
        Obiecte newDataScene = new Obiecte(fileName,userTransform.position,userTransform.rotation, robots, basics, conveyors);
        string json = JsonConvert.SerializeObject(newDataScene);
        Debug.Log(json);

        string path = Path.Combine(Application.persistentDataPath, fileName + ".json");

        File.WriteAllText(path, json);

        Debug.Log("Fisier salvat la: " + path);
        //idManager.ResetSceneObjects();
    }
    public void LoadScene(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName + ".json");

        if (!File.Exists(path))
        {
            Debug.LogWarning("Fisierul nu exista: " + path);
            return;
        }

        string json = File.ReadAllText(path);
        Debug.Log(json);
        Obiecte data = JsonConvert.DeserializeObject<Obiecte>(json);

        idManager.ResetSceneObjects();

        userTransform.position = new Vector3(data.userPositionX,data.userPositionY,data.userPositionZ);
        userTransform.eulerAngles = new Vector3(data.userRotationX,data.userRotationY,data.userRotationZ);

        foreach (Robot robotData in data.robots)
        {
            int idObject = robotData.objectId % 10;
            GameObject newObj = Instantiate(robotsAbb[idObject],new Vector3(robotData.positionX,robotData.positionY,robotData.positionZ),new Quaternion(robotData.rotationX,robotData.rotationY,robotData.rotationZ,robotData.rotationW));
            ObjectUniversalScript robotScript = newObj.GetComponent<ObjectUniversalScript>();
            robotScript.name= robotData.name;
            if (robotData.riser)
                robotScript.RiserOn();
            if (robotData.gripper > 0)
            {
                if (robotData.gripper == 1)
                    robotScript.Gripper1();
                else
                    robotScript.Gripper2();
            }
            idManager.RegisterObject(newObj);
        }

        foreach (Conveyor conveyorData in data.conveyors)
        {
            GameObject newObj = Instantiate(conveyorsStopperPrefab[0],new Vector3(conveyorData.positionX,conveyorData.positionY,conveyorData.positionZ),new Quaternion(conveyorData.rotationX,conveyorData.rotationY,conveyorData.rotationZ,conveyorData.rotationW));
            ObjectUniversalScript conveyorScript = newObj.GetComponent<ObjectUniversalScript>();
            conveyorScript.name= conveyorData.name;
            if(conveyorData.stopper)
                conveyorScript.ActivateStopper();
            idManager.RegisterObject(newObj);
        }

        foreach (ObiecteBasic basicData in data.basic)
        {
            GameObject newObj=null;
            if(basicData.objectId/10==2)
                newObj = Instantiate(cabinetPrefabs[basicData.objectId % 10], new Vector3(basicData.positionX, basicData.positionY, basicData.positionZ), new Quaternion(basicData.rotationX, basicData.rotationY, basicData.rotationZ, basicData.rotationW));
            else if(basicData.objectId/10==3)
                newObj = Instantiate(workbenchPrefabs[basicData.objectId % 10], new Vector3(basicData.positionX, basicData.positionY, basicData.positionZ), new Quaternion(basicData.rotationX, basicData.rotationY, basicData.rotationZ, basicData.rotationW));
            else if(basicData.objectId/10==4)
                newObj = Instantiate(buildingUtilitiesElectricalPrefabs[basicData.objectId % 10], new Vector3(basicData.positionX, basicData.positionY, basicData.positionZ), new Quaternion(basicData.rotationX, basicData.rotationY, basicData.rotationZ, basicData.rotationW));
            else if (basicData.objectId/10==5)
                newObj = Instantiate(conveyorPrefabs[basicData.objectId % 10], new Vector3(basicData.positionX, basicData.positionY, basicData.positionZ), new Quaternion(basicData.rotationX, basicData.rotationY, basicData.rotationZ, basicData.rotationW));
            else if(basicData.objectId/10==6)
                newObj = Instantiate(processEquipmentPrefabs[basicData.objectId % 10], new Vector3(basicData.positionX, basicData.positionY, basicData.positionZ), new Quaternion(basicData.rotationX, basicData.rotationY, basicData.rotationZ, basicData.rotationW));
            if (newObj != null) {
                newObj.GetComponent<ObjectUniversalScript>().name=basicData.name;
                idManager.RegisterObject(newObj);
            }
        }
        Debug.Log("Scena incarcata din: " + path);
    }
}
