using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshSelectedTrash : MonoBehaviour
{
    private Vector3 firstPosition;
    private Quaternion firstRotation;
    private Transform trashPos;
    private Transform vrUser;

    public bool isSelected;
    // Start is called before the first frame update
    void Start()
    {
        firstPosition = gameObject.transform.position;
        firstRotation = gameObject.transform.rotation;
        isSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        {
            vrUser = GameObject.Find("VRUser(Clone)").transform;
            trashPos = GameObject.Find("VRUser(Clone)").transform.Find("OVRCameraRig").Find("TrackingSpace").Find("CenterEyeAnchor").Find("TrashPos");
            gameObject.transform.position = trashPos.position;
            gameObject.transform.LookAt(vrUser);
        }
        else
        {
            gameObject.transform.position = firstPosition;
            gameObject.transform.rotation = firstRotation;
        }
    }
}
