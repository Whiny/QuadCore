using UnityEngine;
using System.Collections;

public class ControllerTest : MonoBehaviour
{
	private float delta = 0;
    public float PlayerMovementSpeed = 10;
    public float PlayerRotationSpeed = 90;
    void Update()
    {
        UserInputs();
    }
    void UserInputs()
    {
        Debug.Log(Input.GetButtonDown("P1_A Button"));
		Debug.Log(Input.GetAxis("P1_LeftThumbstickButton"));
		Debug.Log(Input.GetButton("P1_A Button"));
	}
}