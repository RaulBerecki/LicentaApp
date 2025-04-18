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
        raycastManager = FindObjectOfType<ARRaycastManager>();
        //indicator = transform.GetChild(0).gameObject;
        //indicator.SetActive(false);
        isStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        var ray = new Vector2(Screen.width / 2, Screen.height / 2);
        if (raycastManager.Raycast(ray, hits, TrackableType.Planes))
        {
            Debug.Log("merge");
            Pose hitPose = hits[0].pose;
            transform.position = hitPose.position;
            transform.rotation = hitPose.rotation;
            if (!GameObject.FindGameObjectWithTag("StartPanel") && !isStarted)
            {
                GameObject child = Instantiate(startPanel, UIparent.transform);
                child.transform.parent = UIparent.transform;
                isStarted = true;
                //debugText.text = child.GetComponent<RectTransform>().offsetMax.ToString()+" "+child.GetComponent<RectTransform>().offsetMin.ToString();
            }
            /*if (!indicator.activeInHierarchy)
            {
                indicator.SetActive(true);
            }*/
        }
    }
    public void SetObject(GameObject obj)
    {
        theObject = Instantiate(obj,transform.position,transform.rotation);
        theObject.transform.parent = this.transform;
        indicator = theObject;
    }
}
