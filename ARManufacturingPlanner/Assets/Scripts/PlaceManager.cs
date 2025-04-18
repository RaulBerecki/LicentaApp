using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceManager : MonoBehaviour
{
    private PlaceIndicator placeIndicator;
    public GameObject objectToPlace;

    private GameObject newPlacedObject;
    [SerializeField] IdManager idManager;
    // Start is called before the first frame update
    void Start()
    {
        placeIndicator = FindObjectOfType<PlaceIndicator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ClickToPlace()
    {
        newPlacedObject = Instantiate(objectToPlace, placeIndicator.transform.position, placeIndicator.transform.rotation);
        if (idManager.gameObjects.Count == 0)
        {
            idManager.OpenObjectsPanel();
        }
        idManager.gameObjects.Add(newPlacedObject);
        idManager.CreateButton();
    }
}
