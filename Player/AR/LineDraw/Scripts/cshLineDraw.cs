using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class cshLineDraw : MonoBehaviourPun
{
    public GameObject lineDrawPrefabs; // this is where we put the prefabs object
    public Camera projectionCam;

    private bool isMousePressed;
    private GameObject lineDrawPrefab;
    private LineRenderer lineRenderer;
    private List<Vector3> drawPoints = new List<Vector3>();

    public bool isCaputreCam;
    // Use this for initialization
    void Start()
    {
        isMousePressed = false;
    }

    public void ClearText(bool capture)
    {
        if(capture)
            GetComponent<cshCaptureCamera>().CaptureText();
        GameObject[] delete = GameObject.FindGameObjectsWithTag("LineDraw");
        int deleteCount = delete.Length;
        for (int i = deleteCount - 1; i >= 0; i--)
            Destroy(delete[i]);
        //PhotonNetwork.Destroy(delete[i]);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // Capture Camera
            if(isCaputreCam)
                GetComponent<cshCaptureCamera>().CaptureText();
            // delete the LineRenderers when right mouse down
            GameObject[] delete = GameObject.FindGameObjectsWithTag("LineDraw");
            int deleteCount = delete.Length;
            for (int i = deleteCount - 1; i >= 0; i--)
                Destroy(delete[i]);
        }

        if (Input.GetMouseButtonDown(0))
        {
            // left mouse down, make a new line renderer
            isMousePressed = true;
            lineDrawPrefab = GameObject.Instantiate(lineDrawPrefabs) as GameObject;
            //lineDrawPrefab = PhotonNetwork.Instantiate(lineDrawPrefabs.name, Vector3.zero, Quaternion.identity) as GameObject;
            lineRenderer = lineDrawPrefab.GetComponent<LineRenderer>();
            //lineRenderer.SetVertexCount(0);
            lineRenderer.positionCount = 0;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            // left mouse up, stop drawing
            isMousePressed = false;
            drawPoints.Clear();
        }

        if (isMousePressed)
        {
            // when the left mouse button pressed
            // continue to add vertex to line renderer
            //Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Vector3 mousePos = projectionCam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 mousePos = projectionCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -projectionCam.transform.position.z));

            mousePos.z = 0.0f;
           // Debug.Log(Input.mousePosition);
            //Debug.Log(mousePos);
            if (!drawPoints.Contains(mousePos))
            {
                drawPoints.Add(mousePos);
                //lineRenderer.SetVertexCount(drawPoints.Count);
                lineRenderer.positionCount = drawPoints.Count;
                lineRenderer.SetPosition(drawPoints.Count - 1, mousePos);
            }
        }
    }
}
