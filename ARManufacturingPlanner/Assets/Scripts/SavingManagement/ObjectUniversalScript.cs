using UnityEngine;

public class ObjectUniversalScript : MonoBehaviour
{
    [Header("BasicObjects")]
    public int id;
    public string name;
    [Header("RobotController")]
    public GameObject[] grippers;
    public GameObject riser;
    [Header("ConveyorController")]
    public GameObject stopper;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Robot Functions!!!
    public void NoneGripper()
    {
        for (int i = 0; i < grippers.Length; i++)
        {
            grippers[i].SetActive(false);
        }
    }
    public void Gripper1()
    {
        for (int i = 0; i < grippers.Length; i++)
        {
            if (i == 0)
                grippers[i].SetActive(true);
            else
                grippers[i].SetActive(false);
        }
    }
    public void Gripper2()
    {
        for (int i = 0; i < grippers.Length; i++)
        {
            if (i == 1)
                grippers[i].SetActive(true);
            else
                grippers[i].SetActive(false);
        }
    }
    public void RiserOn()
    {
        riser.SetActive(true);
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.55f, transform.position.z);
    }
    public void RiserOff()
    {
        riser.SetActive(false);
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.55f, transform.position.z);
    }
    public int GetGripperStatus()
    {
        for (int i = 0; i < grippers.Length; i++)
        {
            if (grippers[i].activeSelf)
                return i;
        }
        return -1;
    }
    public bool GetRiserStatus()
    {
        return riser.activeSelf;
    }
    //Conveyor Functions!!!
    public void ActivateStopper()
    {
        stopper.SetActive(true);
    }
    public void DeactivateStopper()
    {
        stopper.SetActive(false);
    }
    public bool GetStopperStatus()
    {
        return stopper.activeSelf;
    }
}
