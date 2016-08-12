using UnityEngine;
using System.Collections;

public class zoomOut : MonoBehaviour
{
    public Camera camera1;

    public Camera camera2;

    public Camera camera3;

    public Camera camera4;

    private bool timeCheck = true;

    public float zoomSpeed = 1.0f;

    public float zoomAmount = 1.0f;

    public float firstCameraSize = 1.0f;

    public float finalCameraSize = 5.0f;

    void Start ()
    {
        camera1.GetComponent<Camera>().orthographicSize = firstCameraSize;
        camera2.GetComponent<Camera>().orthographicSize = firstCameraSize;
        camera3.GetComponent<Camera>().orthographicSize = firstCameraSize;
        camera4.GetComponent<Camera>().orthographicSize = firstCameraSize;
    }


    void Update()
    {
        if (timeCheck && camera1.GetComponent<Camera>().orthographicSize <= finalCameraSize)
        {
            StartCoroutine("cameraZoomOut");
        }
    }

    IEnumerator cameraZoomOut()
    {

        timeCheck = false;

        camera1.GetComponent<Camera>().orthographicSize += zoomAmount;
        camera2.GetComponent<Camera>().orthographicSize += zoomAmount;
        camera3.GetComponent<Camera>().orthographicSize += zoomAmount;
        camera4.GetComponent<Camera>().orthographicSize += zoomAmount;

        yield return new WaitForSeconds(zoomSpeed);

        timeCheck = true;
    }
}
