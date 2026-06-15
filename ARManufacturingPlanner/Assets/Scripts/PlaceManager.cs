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
        placeIndicator = FindAnyObjectByType<PlaceIndicator>();
    }

    public void ClickToPlace()
    {
        newPlacedObject = Instantiate(objectToPlace, placeIndicator.transform.position, placeIndicator.transform.rotation);
        newPlacedObject.SetActive(true); // web templates are kept inactive; no-op for normal prefabs
        if (idManager.gameObjects.Count == 0)
        {
            idManager.OpenObjectsPanel();
        }
        idManager.gameObjects.Add(newPlacedObject);
        idManager.CreateElement();
    }
}
