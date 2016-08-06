using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NewPlayerControl : MonoBehaviour
{
	public string p_Name;
	public Animator anim;
	public GameObject attackCollider;
	public GameObject defaultCollider;

	private int angle;
	private int direction;
	private float speed;
	private float power;
	private float jump;
	private bool isStun;
	private bool isPlaying;
	public bool isJumpping;
	public bool isFalling;
	private bool isCharging;
	public bool isCharged;

	private float timer_Stun;    //isStun을 대체해주길 바람
	private float timer_Charging;
	void Start()
	{
		angle = 0;
		power = 4;
		speed = 2;
		jump = 6;

		isJumpping = false;
		isFalling = false;
		isPlaying = true;
		isCharging = false;
		isCharged = false;

		timer_Stun = 0f;
		timer_Charging = 0f;

		attackCollider.GetComponent<PolygonCollider2D>().enabled = false;
		attackCollider.GetComponent<NewPlayerAttack>().enabled = false;
	}

	void Update()
	{
		ControllerInput();

		if (isCharging && !isCharged)
		{
			timer_Charging += Time.deltaTime;
			if (timer_Charging > 2)
			{
				isCharged = true;
				anim.SetBool("isCharged", true);
			}
		}
		if (timer_Stun > 0)
		{
			timer_Stun -= Time.deltaTime;
			anim.SetFloat("timer_Stun", timer_Stun);
		}
	}

	void FixedUpdate()
	{
		if(!(timer_Stun > 0))
			Move();

		//떨어지는 중으로 상태 변경
		if (GetComponent<Rigidbody2D>().velocity.y < -0.5)
		{
			isJumpping = false;
			isFalling = true;
			anim.SetBool("isJumpping", false);
			anim.SetBool("isFalling", true);
			defaultCollider.GetComponent<BoxCollider2D>().enabled = true;
		}
	}

	private void ControllerInput()
	{
		if (Input.GetButtonDown(p_Name + "_A Button"))
		{
			power = 4;	//일반공격 파워
			Attack();
			isCharging = true;
			anim.SetBool("isCharging", true);
		}
		if (Input.GetButtonUp(p_Name + "_A Button"))
		{
			if (isCharged)
			{
				power = 10;	//차징공격 파워
				Attack();
				isCharged = false;
				anim.SetBool("isCharged", false);
			}
			timer_Charging = 0f;
			isCharging = false;
			anim.SetBool("isCharging", false);
		}
		if (Input.GetButtonDown(p_Name + "_B Button") || Input.GetAxis(p_Name + "_Triggers") > 0.9f)
		{
			if(!isFalling && !isJumpping)
				Jump();
		}
	}

	private void Move()
	{
		float tempValue = Input.GetAxis(p_Name + "_LeftThumbstickButton");

		if (tempValue > 0.7)	//오른
		{
			direction = 1;
			angle = 0;
			anim.SetBool("isWalking", true);
		}
		else if (tempValue < -0.7)	//왼
		{
			direction = -1;
			angle = 180;
			anim.SetBool("isWalking", true);
		}
		else
		{
			direction = 0;
			anim.SetBool("isWalking", false);
		}

		GetComponent<Rigidbody2D>().velocity = new Vector2(direction * speed, GetComponent<Rigidbody2D>().velocity.y);
		transform.eulerAngles = new Vector3(0, angle, 0);
	}

	private void Jump()
	{
		GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jump);
		isJumpping = true;
		anim.SetBool("isJumpping", true);
		defaultCollider.GetComponent<BoxCollider2D>().enabled = false;
	}

	public void Damaged(float power, int direction)
	{
		timer_Stun = power * 0.25f;
		anim.SetFloat("timer_Stun", timer_Stun);

		GetComponent<Rigidbody2D>().velocity = new Vector2(direction * power, 1 * power);

		Debug.Log(direction * power);

		isCharging = false;
		isCharged = false;
		anim.SetBool("isCharging", false);
		anim.SetBool("isCharged", false);
		timer_Charging = 0;
	}

	private void Attack()
	{
		attackCollider.GetComponent<NewPlayerAttack>().enabled = true;
		anim.SetTrigger("trig_Attack");
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Ground" && (isJumpping || isFalling))
		{
			isJumpping = false;
			isFalling = false;
			anim.SetBool("isJumpping", false);
			anim.SetBool("isFalling", false);
			defaultCollider.GetComponent<BoxCollider2D>().enabled = true;
		}
	}

	public int Angle
	{
		get {return angle;}
		set {angle = value;}
	}

	public float Power
	{
		get {return power;}
		set {power = value;}
	}
}
	/*void Update()
	{
		if (Input.GetButtonDown(p_Name + "_A Button") == true)
		{
			Attack();
			isCharging = true;
			anim.SetBool("isCharging", true);
		}
		if (Input.GetButton(p_Name + "_A Button") == false)
		{
			isCharging = false;
			anim.SetBool("isCharging", false);
			time = 0;

			if (isCharged)
			{
				Attack();

				isCharged = false;
				anim.SetBool("isCharged", false);

			}
		}

		//테스트용
		if (Input.GetButtonDown(p_Name + "_Y Button"))
			Damaged(4, true); //일반 공격 테스트
		if (Input.GetButtonDown(p_Name + "_X Button"))
			Damaged(10, true); //차징 공격 테스트

		//차징 카운트
		if (isCharging) time += Time.deltaTime;
		if (time >= 2)
		{
			isCharged = true;
			anim.SetBool("isCharged", true);

		}
		//스턴이면 이동불가
		if (stunTimer < 0) Move();
		else stunTimer -= Time.deltaTime;
	}

	private void Move()
	{

		float tempValue = Input.GetAxis(p_Name + "_LeftThumbstickButton");

		if (!isJumpping && Input.GetAxis(p_Name + "_B Button") == 1)
		{
			isJumpping = true;
			anim.SetBool("isJumpping", true);

			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jump);
		}

		if (tempValue > 0.7)
		{
			direction = 1;
			angle = 0;
			anim.SetBool("isWalking", true);

		} // 오른쪽

		else if (tempValue < -0.7)
		{
			direction = -1;
			angle = 180;
			anim.SetBool("isWalking", true);
		} // 왼쪽

		else
		{
			direction = 0; // 정지
			anim.SetBool("isWalking", false);
		}
		GetComponent<Rigidbody2D>().velocity = new Vector2(direction * speed, GetComponent<Rigidbody2D>().velocity.y);
		transform.eulerAngles = new Vector3(0, angle, 0);
	}

	private void Attack()
	{
		//anim.SetBool("isAttack", true);
		anim.SetTrigger("AttackTrigger");
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Ground" && isJumpping)
		{
			isJumpping = false;
			anim.SetBool("isJumpping", false);
		}
	}

	//힘의 크기와 방향(좌우)
	public void Damaged(int power, bool throwRight)
	{
		stunTimer = power * 0.15f;

		if (throwRight)
			GetComponent<Rigidbody2D>().velocity = new Vector2(1 * power, 1 * power);
		else
			GetComponent<Rigidbody2D>().velocity = new Vector2(-1 * power, 1 * power);

		isJumpping = true;
		anim.SetBool("isJumpping", true);
	}*/