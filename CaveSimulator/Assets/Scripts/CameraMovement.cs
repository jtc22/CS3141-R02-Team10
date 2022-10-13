using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float zoomStep, minCamSize, maxCamSize;
    private Vector3 dragOrigin;

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
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);

            cam.transform.position += difference;

            // Get the bounding area of the camera view, will be used to deactivate voxels that cant be seen
            // Vector3 lowerBound = cam.ScreenToWorldPoint(new Vector3(0, 0, 0));
            // Vector3 upperBound = cam.ScreenToWorldPoint(new Vector3(1, 1, 0));

            // Debug.Log("Lower: " + lowerBound);
            // Debug.Log("Upper: " + upperBound);
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
