using Newtonsoft.Json;
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
        List<GameObject> webModels = new List<GameObject>();
        foreach(GameObject obiect in idManager.gameObjects)
        {
            ObjectUniversalScript ous = obiect.GetComponent<ObjectUniversalScript>();
            if(!string.IsNullOrEmpty(ous.webUrl))
                webModels.Add(obiect);
            else if(ous.riser!=null)
                robots.Add(obiect);
            else if(ous.stopper!=null)
                conveyors.Add(obiect);
            else
                basics.Add(obiect);
        }
        Obiecte newDataScene = new Obiecte(fileName,userTransform.position,userTransform.rotation, robots, basics, conveyors, webModels);
        string json = JsonConvert.SerializeObject(newDataScene);
        Debug.Log(json);

        string path = Path.Combine(Application.persistentDataPath, fileName + ".json");

        File.WriteAllText(path, json);

        Debug.Log("Fisier salvat la: " + path);
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
            if (idObject < 0 || idObject >= robotsAbb.Length)
            {
                Debug.LogWarning("Robot prefab index out of range, skipping: " + robotData.objectId);
                continue;
            }
            GameObject newObj = Instantiate(robotsAbb[idObject],new Vector3(robotData.positionX,robotData.positionY,robotData.positionZ),new Quaternion(robotData.rotationX,robotData.rotationY,robotData.rotationZ,robotData.rotationW));
            ObjectUniversalScript robotScript = newObj.GetComponent<ObjectUniversalScript>();
            robotScript.objectName= robotData.name;
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
            if (conveyorsStopperPrefab.Length == 0)
            {
                Debug.LogWarning("No conveyor-with-stopper prefab assigned, skipping conveyor.");
                continue;
            }
            GameObject newObj = Instantiate(conveyorsStopperPrefab[0],new Vector3(conveyorData.positionX,conveyorData.positionY,conveyorData.positionZ),new Quaternion(conveyorData.rotationX,conveyorData.rotationY,conveyorData.rotationZ,conveyorData.rotationW));
            ObjectUniversalScript conveyorScript = newObj.GetComponent<ObjectUniversalScript>();
            conveyorScript.objectName= conveyorData.name;
            if(conveyorData.stopper)
                conveyorScript.ActivateStopper();
            idManager.RegisterObject(newObj);
        }

        foreach (ObiecteBasic basicData in data.basic)
        {
            GameObject[] prefabs = PrefabArrayFor(basicData.objectId);
            int index = basicData.objectId % 10;
            if (prefabs == null || index < 0 || index >= prefabs.Length)
            {
                Debug.LogWarning("Basic prefab not found for id, skipping: " + basicData.objectId);
                continue;
            }
            Vector3 pos = new Vector3(basicData.positionX, basicData.positionY, basicData.positionZ);
            Quaternion rot = new Quaternion(basicData.rotationX, basicData.rotationY, basicData.rotationZ, basicData.rotationW);
            GameObject newObj = Instantiate(prefabs[index], pos, rot);
            newObj.GetComponent<ObjectUniversalScript>().objectName = basicData.name;
            idManager.RegisterObject(newObj);
        }

        if (data.webModels != null && data.webModels.Count > 0)
        {
            RuntimeGltfLoader loader = FindAnyObjectByType<RuntimeGltfLoader>();
            if (loader != null)
            {
                foreach (WebModel wm in data.webModels)
                {
                    Vector3 pos = new Vector3(wm.positionX, wm.positionY, wm.positionZ);
                    Quaternion rot = new Quaternion(wm.rotationX, wm.rotationY, wm.rotationZ, wm.rotationW);
                    // Asynchronous: each model registers with the IdManager once its download finishes.
                    loader.LoadSavedModel(wm.url, pos, rot, wm.name);
                }
            }
            else
            {
                Debug.LogWarning("Saved web models found but no RuntimeGltfLoader in scene; skipping them.");
            }
        }
        Debug.Log("Scena incarcata din: " + path);
    }

    // Maps the first digit of objectId to the matching prefab category array.
    GameObject[] PrefabArrayFor(int objectId)
    {
        switch (objectId / 10)
        {
            case 2: return cabinetPrefabs;
            case 3: return workbenchPrefabs;
            case 4: return buildingUtilitiesElectricalPrefabs;
            case 5: return conveyorPrefabs;
            case 6: return processEquipmentPrefabs;
            default: return null;
        }
    }
}
