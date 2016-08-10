using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	public string p_Name;
	public Animator anim;
	public GameObject m_AttackCollider, m_DefaultCollider;
	public GameObject m_Camera;

	private GameObject temp_Collider;
	private int angle;
	private int direction; // 플레이어 방향값
	private float timer_Charging, timer_Stun; // 타이머
	private float speed, jump; // 플레이어 이동 변수
	private float power;
	private bool isPlaying;
	private bool isJumpping,isAttack; // 조작 상태 변수
	private bool isCharging, isCharged, isStunned; // 행동 상태 변수
	public bool isIgnored;	

	void Start ()
	{
		power = 4f;

		angle = 0;
		speed = 2;
		jump = 6;
		isPlaying = true;
		isIgnored = true;

		timer_Charging = timer_Stun = 0;
		isJumpping = isAttack = false;
		isCharging = isCharged = isStunned = false;
	}

	void Update ()
	{
		if (!isStunned) InputManager();

		if ((timer_Charging >= 2) && !isCharged)
		{
			isCharged = true;
			power *= 2;
			anim.SetBool("P1_Charged", true);
		}

		if (isCharging) timer_Charging += Time.deltaTime;
	}
	
	void FixedUpdate ()
	{
		Debug.Log(GetComponent<Rigidbody2D>().velocity.y);
		if (isIgnored && GetComponent<Rigidbody2D>().velocity.y < 0)
		{
			
			m_DefaultCollider.GetComponent<BoxCollider2D>().enabled = true;
			isIgnored = false;
		}

		if (!isStunned) Move();
		else if (timer_Stun > 0) timer_Stun -= Time.deltaTime;

		else
		{
			isStunned = false;
			anim.SetBool("P1_Damaged", false);
		}
	}

	private void InputManager()
	{
		if (Input.GetButtonDown(p_Name + "_A Button") && !isAttack)
		{
			isAttack = true;
			isCharging = true;

			Attack();
		}

		else if (Input.GetButtonUp(p_Name + "_A Button"))
		{
			isCharging = false;
			timer_Charging = 0;

			if (isCharged)
			{
				isAttack = true;
				Attack();

				isCharged = false;
				anim.SetBool("P1_Charged", false);
			}
		}

		if (!isJumpping && Input.GetButtonDown(p_Name + "_B Button"))
		{
			isJumpping = true;
			anim.SetBool("P1_Jump", true);
			anim.SetBool("P1_Walk", false);

			m_DefaultCollider.GetComponent<BoxCollider2D>().enabled = false;
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jump);
			Physics2D.IgnoreCollision(temp_Collider.GetComponent<BoxCollider2D>(), m_DefaultCollider.GetComponent<BoxCollider2D>(), false);
		}
	}

	private void Move()
	{
		float tempValue = 0;
		int value = -1; // -1 : 왼쪽, 0 : 정지, 1 : 오른쪽

		if (p_Name != "P0") tempValue = Input.GetAxis(p_Name + "_LeftThumbstickButton");
		else
		{
			if (Input.GetAxis(p_Name + "_Horizontal") > 0) value = 1;
			else if (Input.GetAxis(p_Name + "_Horizontal") < 0) value = -1;
			else value = 0;
		}

		/* 키 입력 <오른쪽 이동> */
		if ((tempValue > 0.7) || value == 1)
		{
			m_Camera.transform.localPosition = new Vector3(0, 1.451504f, -10);
			m_Camera.transform.localEulerAngles = new Vector3(0, 0, 0);

			direction = 1;
			angle = 0;

			if (!isJumpping) anim.SetBool("P1_Walk", true);
		}

		/* 키 입력 <왼쪽 이동> */
		else if ((tempValue < -0.7) || value == -1)
		{
			m_Camera.transform.localPosition = new Vector3(0, 1.451504f, 10);
			m_Camera.transform.localEulerAngles = new Vector3(0, 180, 0);

			direction = -1;
			angle = 180;

			if (!isJumpping) anim.SetBool("P1_Walk", true);
		}

		/* 정지 상태 */
		else
		{
			direction = 0;
			anim.SetBool("P1_Walk", false);
		}

		GetComponent<Rigidbody2D>().velocity = new Vector2(direction * speed, GetComponent<Rigidbody2D>().velocity.y);
		transform.eulerAngles = new Vector3(0, angle, 0);
	}

	private void Attack()
	{
		m_AttackCollider.GetComponent<PolygonCollider2D>().enabled = true;
		anim.SetBool("P1_Attack", true);

		m_AttackCollider.GetComponent<PlayerAttack>().DelayOn();
	}

	public void Damaged(float _power, int _direction)
	{
		anim.SetBool("P1_Damaged", true);
		anim.SetBool("P1_Charged", false);
		isStunned = true;
		isCharged = false;
		isCharging = false;

		timer_Stun = _power * 0.25f;
		GetComponent<Rigidbody2D>().velocity = new Vector2(_direction * _power, 1 * _power);

		///Debug.Log(_direction * _power);

		isCharging = false;
		isCharged = false;
		timer_Charging = 0;
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (!isIgnored && GetComponent<Rigidbody2D>().velocity.y < 0 && col.gameObject.tag == "Ground" && isJumpping)
		{
			Physics2D.IgnoreCollision(col.gameObject.GetComponent<BoxCollider2D>(), m_DefaultCollider.GetComponent<BoxCollider2D>(), true);
			temp_Collider = col.gameObject;
		}

		if (col.gameObject.tag == "Ground" && isJumpping)
		{
			isJumpping = false;
			isIgnored = true;
			anim.SetBool("P1_Jump", false);
		}
	}

	public int Angle
	{
		get
		{
			return angle;
		}

		set
		{
			angle = value;
		}
	}

	public float Power
	{
		get
		{
			return power;
		}

		set
		{
			power = value;
		}
	}

	public bool IsAttack
	{
		get
		{
			return isAttack;
		}

		set
		{
			isAttack = value;
		}
	}
}