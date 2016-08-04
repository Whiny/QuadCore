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
	public bool isJumpping;
	private bool isCharging;
	public bool isCharged;

	private float stunTimer;	//isStun을 대체해주길 바람

	void Start ()
	{
		angle = 0;
		time = 0;
		speed = 2;
		jump = 6;

		isStun = false;
		isJumpping = false;
		isPlaying = true;
		isCharging = false;
		isCharged = false;

		stunTimer = 0;
	}
	
	void FixedUpdate ()
	{
		if (Input.GetButtonDown(p_Name + "_A Button"))
		{
			isCharging = true;
			Attack();
		}

		if (Input.GetButtonUp(p_Name + "_A Button"))
		{

			isCharging = false;
			time = 0;

			if (isCharged)
			{
				Attack();

				isCharged = false;
			}
		}

		//테스트용
		if (Input.GetButtonDown(p_Name + "_Y Button"))
			Damaged(4, true); //일반 공격 테스트
		if (Input.GetButtonDown(p_Name + "_X Button"))
			Damaged(10, true); //차징 공격 테스트

		if (isCharging) time += Time.deltaTime;
		if (time >= 2) isCharged = true;

		//스턴이면 이동불가
		if (stunTimer < 0) Move();
		else stunTimer -= Time.deltaTime;

		SetAnimation();
	}

	private void Move()
	{

		float tempValue = Input.GetAxis(p_Name + "_LeftThumbstickButton");

		if (!isJumpping && Input.GetAxis(p_Name + "_B Button") == 1)
		{
			isJumpping = true;

			if (!isCharged) anim.SetTrigger(p_Name + "_Jump"); // 일반 점프
			else if (isCharged) anim.SetTrigger(p_Name + "_JumpCharging"); // 차징 점프
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

		GetComponent<Rigidbody2D>().velocity = new Vector2(direction * speed, GetComponent<Rigidbody2D>().velocity.y);
		transform.eulerAngles = new Vector3(0, angle, 0);
	}

	private void Attack()
	{
		if (!isJumpping)
		{
			anim.SetTrigger(p_Name + "_Attack"); // 일반 공격
			anim.SetTrigger(p_Name + "_Stand"); // 일반 스탠드
		}

		else
		{
			anim.SetTrigger(p_Name + "_JumpAttack"); // 점프 공격
			anim.SetTrigger(p_Name + "_Jump"); // 점프
		}
	}

	private void SetAnimation()
	{
		if (direction != 0)
		{
			if (!isJumpping && !isCharged) anim.SetTrigger(p_Name + "_Walk"); // 일반 걸음
			else if (!isJumpping && isCharged) anim.SetTrigger(p_Name + "_ChargingWalk"); // 차징 걸음
			else if (isJumpping && !isCharged) anim.SetTrigger(p_Name + "_Jump"); // 일반 점프
			else if (isJumpping && isCharged) anim.SetTrigger(p_Name + "_JumpCharging"); // 차징 점프
		}

		else
		{
			if (!isJumpping && !isCharged) anim.SetTrigger(p_Name + "_Stand"); // 일반 스탠드
			else if (!isJumpping && isCharged) anim.SetTrigger(p_Name + "_ChargingStand"); // 차징 스탠드
			else if (isJumpping && !isCharged) anim.SetTrigger(p_Name + "_Jump"); // 일반 점프
			else if (isJumpping && isCharged) anim.SetTrigger(p_Name + "_JumpCharging"); // 차징 점프
		}
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Ground" && isJumpping)
		{
			isJumpping = false;
			if (!isCharged) anim.SetTrigger(p_Name + "_Stand"); // 일반 스탠드
			else if (isCharged)anim.SetTrigger(p_Name + "_ChargingStand"); // 차징 스텐드
		}
	}

	//힘의 크기와 방향(좌우)
	public void Damaged(int power,bool throwRight)
	{
		stunTimer = power * 0.15f;

		if(throwRight)
			GetComponent<Rigidbody2D>().velocity = new Vector2(1 * power, 1 * power);
		else
			GetComponent<Rigidbody2D>().velocity = new Vector2(-1 * power, 1 * power);

		isJumpping = true;
	}
}


