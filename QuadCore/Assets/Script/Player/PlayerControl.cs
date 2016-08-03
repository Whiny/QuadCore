using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	public string p_Name;
	public Animator anim;

	private int angle;
	private int direction;
	private float time;
	private float speed;
	private float jump;
	private bool isStun;
	private bool isPlaying;
	private bool isJumpping;

	void Start ()
	{
		angle = 0;
		time = 0;
		speed = 2;
		jump = 6;

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
		//Debug.Log(Input.GetAxis(p_Name + "_LeftThumbstickButton"));
		if (!isJumpping && Input.GetAxis(p_Name + "_B Button") == 1)
		{
			isJumpping = true;
			anim.SetTrigger("P1_Jump");
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jump);
		}

		if (tempValue > 0.7) 
		{
			direction = 1;
			angle = 0;
		} // 오른쪽

		else if (tempValue < -0.7) 
		{
			direction = -1;
			angle = 180;
		} // 왼쪽

		else direction = 0; // 정지

		/*if (isDash)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(forwardValue * speed, GetComponent<Rigidbody2D>().velocity.y);
			speed = 300;
			isDash = false;
		}*/

		GetComponent<Rigidbody2D>().velocity = new Vector2(direction * speed, GetComponent<Rigidbody2D>().velocity.y);
		SetAnimation();

		transform.eulerAngles = new Vector3(0, angle, 0);
	}

	private void SetAnimation()
	{
		if (direction != 0)
		{
			if (!isJumpping) anim.SetTrigger("P1_Walk");
			else anim.SetTrigger("P1_Jump");
		}

		else
		{
			if (!isJumpping) anim.SetTrigger("P1_Stand");
			else anim.SetTrigger("P1_Jump");
		}
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Ground" && isJumpping)
		{
			isJumpping = false;
			anim.SetTrigger("P1_Stand");
		}
	}

}


