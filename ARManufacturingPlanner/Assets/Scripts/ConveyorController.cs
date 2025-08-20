using UnityEngine;

public class ConveyorController : MonoBehaviour
{
    public GameObject stopper;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ActivateStopper()
    {
        stopper.SetActive(true);
    }
    public void DeactivateStopper()
    {
        stopper.SetActive(false);
    }
}
