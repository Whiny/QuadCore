using UnityEngine;
using System.Collections;

public class zoomOut : MonoBehaviour
{
    public Camera camera1;

    public Camera camera2;

    public Camera camera3;

    public Camera camera4;

    void Start ()
    {
	
	}
	
	
	void Update ()
    {
        camera1.transform.Translate(new Vector3(camera1.transform.position.x, camera1.transform.position.y, camera1.transform.position.z - 1));
        camera2.transform.Translate(new Vector3(camera2.transform.position.x, camera2.transform.position.y, camera2.transform.position.z - 1));
        camera3.transform.Translate(new Vector3(camera3.transform.position.x, camera3.transform.position.y, camera3.transform.position.z - 1));
        camera4.transform.Translate(new Vector3(camera4.transform.position.x, camera4.transform.position.y, camera4.transform.position.z - 1));
    }
}
