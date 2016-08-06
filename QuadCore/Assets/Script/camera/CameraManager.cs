using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour 
{
    public Camera mainCamera;
    public Camera[] cameraList;
    public float zoomScale;

    private bool isMain;

	void Start () 
    {
        isMain = false;

        cameraList[0].rect = new Rect(0, 0, 0.5f, 0.5f);
        cameraList[1].rect = new Rect(0.5f, 0, 0.5f, 0.5f);
        cameraList[2].rect = new Rect(0, 0.5f, 0.5f, 0.5f);
        cameraList[3].rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
	}
	
	void Update () 
    {
        if (Input.GetKeyDown(KeyCode.Space))
            ScreenSwitch();
	}

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            foreach(Camera camera in cameraList)
            {
                ZoomIn(zoomScale, camera);
            }
        }
        if (Input.GetKey(KeyCode.S))
        {
            foreach (Camera camera in cameraList)
            {
                ZoomOut(zoomScale, camera);
            }
        }          
    }

    public void ScreenSwitch()
    {
        isMain = !isMain;

        foreach (Camera camera in cameraList) // 분할 화면 카메라 리스트
            camera.enabled = !isMain;
 
        mainCamera.enabled = isMain; // 메인 카메라

    }

    public void ZoomIn(float scale, Camera camera)
    {
        camera.fieldOfView += scale;
    }

    public void ZoomOut(float scale, Camera camera)
    {
        camera.fieldOfView -= scale;
    }
}
