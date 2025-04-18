using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotsController : MonoBehaviour
{
    public GameObject[] grippers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NoneGripper()
    {
        for(int i = 0; i < grippers.Length; i++)
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
}
