using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float zoomStep, minCamSize;
    [SerializeField] public MapRenderer mr;
    private Vector3 dragOrigin;
    private float maxCamSize;

    void Start()
    {
        // Moves camera from the corner to the center of the map
        cam.transform.position += new Vector3(mr.map.width / 2, mr.map.height / 2);
        cam.orthographicSize = mr.map.height / 4;
        maxCamSize = mr.map.width / 2;
    }

    // Update is called once per frame
    void Update()
    {
        PanCamera();
    }

    private void PanCamera()
    {
        // Save position of mouse on mouse click
        if(Input.GetMouseButtonDown(0))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);

            Collider2D[] results;
            results = Physics2D.OverlapPointAll(dragOrigin);
            foreach (Collider2D col in results)
            {
                MineralHover mh = col.GetComponent<MineralHover>();
                mh.MouseOver();
            }
        }

        // If mouse is unclicked, move camera
        if (Input.GetMouseButton(0))
        {
            // Get new mouse position
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);

            // Set camera to new position
            cam.transform.position += difference;
        }

    }

    public void ZoomIn()
    {
        float newSize = cam.orthographicSize - zoomStep;
        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);
    }

    public void ZoomOut()
    {
        float newSize = cam.orthographicSize + zoomStep;
        cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);
    }
}
