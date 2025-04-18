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

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartLayout()
    {
        userInterfaceManager.StartLayout();
        Destroy(this.gameObject);
    }
}
