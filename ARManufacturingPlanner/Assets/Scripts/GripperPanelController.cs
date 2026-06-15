using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripperPanelController : MonoBehaviour
{
    public GameObject robot;
    UserInterfaceManager userInterfaceManager;
    // Start is called before the first frame update
    void Start()
    {
        userInterfaceManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<UserInterfaceManager>();
    }

    public void NoneGripper()
    {
        robot.GetComponent<ObjectUniversalScript>().NoneGripper();
    }
    public void Gripper1()
    {
        robot.GetComponent<ObjectUniversalScript>().Gripper1();
    }
    public void Gripper2()
    {
        robot.GetComponent<ObjectUniversalScript>().Gripper2();
    }
    public void Finish()
    {
        userInterfaceManager.BackgroundPanelActive.SetActive(true);
        Destroy(this.gameObject);
    }
}
