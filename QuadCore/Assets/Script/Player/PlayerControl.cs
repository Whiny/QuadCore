using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	public Animator anim;
	public string p_Name;

	private int direction;
	private float time;
	private float speed;
	private float jump;
	private bool isStun;
	private bool isPlaying;
	private bool isJumpping;

	void Start ()
	{
		time = 0;
		speed = 2;
		jump = 400;

		isStun = false;
		isJumpping = false;
		isPlaying = true;
	}
	
	void FixedUpdate ()
	{
		Move();
	}

	private void Move()
	{
		float tempValue = Input.GetAxis(p_Name + "_LeftThumbstickButton");

		if (tempValue > 0.7) direction = 1; // 오른쪽
		else if (tempValue < -0.7) direction = -1; // 왼쪽
		else direction = 0; // 정지

		//direction = Input.GetAxis(p_Name[p_number] + "_LeftThumbstickButton") > 0 ? 1 : -1;
		//Debug.Log(Input.GetAxis(p_Name[p_number] + "_LeftThumbstickButton"));

		/*if (isDash)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(forwardValue * speed, GetComponent<Rigidbody2D>().velocity.y);
			speed = 300;
			isDash = false;
		}*/

		GetComponent<Rigidbody2D>().velocity = new Vector2(direction * speed, GetComponent<Rigidbody2D>().velocity.y);

		if (direction == 1) transform.eulerAngles = new Vector3(0, 0, 0);
		else if (direction == -1) transform.eulerAngles = new Vector3(0, 180, 0);
	}
}


