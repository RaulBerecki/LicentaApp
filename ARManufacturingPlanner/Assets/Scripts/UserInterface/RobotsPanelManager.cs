using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotsPanelManager : MonoBehaviour
{
    public UserInterfaceManager userInterfaceManager;
    // Start is called before the first frame update
    void Start()
    {
        userInterfaceManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<UserInterfaceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ABB()
    {
        userInterfaceManager.ABB();
        Destroy(this.gameObject);
    }
    public void BackButton()
    {
        userInterfaceManager.BackButtonRobotsPanel();
        Destroy(this.gameObject);
    }
}
