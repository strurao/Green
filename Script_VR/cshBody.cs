using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshBody : MonoBehaviour
{
    public Transform head;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 bodyPosition = head.position - new Vector3(0f, 0.5f, 0f);
        transform.position = bodyPosition;
    }
}