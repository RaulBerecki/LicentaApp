using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class PlaceIndicator : MonoBehaviour
{
    private ARRaycastManager raycastManager;
    public GameObject indicator;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public GameObject startPanel,UIparent;
    public TextMeshProUGUI debugText;
    bool isStarted;
    public GameObject theObject;
    // Start is called before the first frame update
    void Start()
    {
        raycastManager = FindAnyObjectByType<ARRaycastManager>();
        isStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        var ray = new Vector2(Screen.width / 2, Screen.height / 2); //punct imaginar
        if (raycastManager.Raycast(ray, hits, TrackableType.Planes)) //verificare intersectie cu o suprafata
        {
            Pose hitPose = hits[0].pose;
            transform.position = hitPose.position;
            transform.rotation = hitPose.rotation;
            if (!GameObject.FindGameObjectWithTag("StartPanel") && !isStarted) //verificare intersectie cu o suprafata pentru prima data
            {
                //initializare interfata
                Instantiate(startPanel, UIparent.transform);
                isStarted = true;
            }
        }
    }
    public void SetObject(GameObject obj)
    {
        theObject = Instantiate(obj,transform.position,transform.rotation);
        theObject.transform.parent = this.transform;
        indicator = theObject;
    }
    // Uses an already-instantiated GameObject (e.g. a runtime-built hologram) as the
    // placement preview, parenting it to the indicator so it follows the detected ground.
    public void SetObjectInstance(GameObject instance)
    {
        theObject = instance;
        theObject.transform.SetParent(this.transform, false);
        theObject.transform.localPosition = Vector3.zero;
        theObject.transform.localRotation = Quaternion.identity;
        indicator = theObject;
    }
}
