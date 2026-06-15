using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    UserInterfaceManager userInterfaceManager;
    // Start is called before the first frame update
    void Start()
    {
        userInterfaceManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<UserInterfaceManager>();
    }

    public void StartLayout()
    {
        userInterfaceManager.StartLayout();
        Destroy(this.gameObject); //sterge panou initial
    }
}
