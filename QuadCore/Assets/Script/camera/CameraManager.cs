using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour 
{
    public Camera mainCamera; // 메인 카메라
    public Camera[] cameraList; // 분할 카메라 리스트
    public float zoomScale; // 줌 크기

    private bool isMain;

	void Start () 
    {
        isMain = false;

        Init();
	}
	
    public void Init() // 카메라 초기화
    {
        float length = 0.5f;

        cameraList[0].rect = new Rect(0, 0, length, length);
        cameraList[1].rect = new Rect(length, 0, length, length);
        cameraList[2].rect = new Rect(0, length, length, length);
        cameraList[3].rect = new Rect(length, length, length, length);
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

    public void ScreenSwitch() // 메인 카메라로 화면 전환
    {
        isMain = !isMain;

        foreach (Camera camera in cameraList) // 분할 화면 카메라 리스트
            camera.enabled = !isMain;
 
        mainCamera.enabled = isMain; // 메인 카메라

    }

    public void ZoomOut(float scale, Camera camera)
    {
        if(camera.fieldOfView < 175) // 최대 크기
            camera.fieldOfView += scale;
    }

    public void ZoomIn(float scale, Camera camera)
    {
        if (camera.fieldOfView > 10) // 최소 크기
            camera.fieldOfView -= scale;
    }
}
