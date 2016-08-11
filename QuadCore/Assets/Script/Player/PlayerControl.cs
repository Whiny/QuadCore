using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	public string p_Name;
	public Animator anim;
	public GameObject m_AttackCollider, m_DefaultCollider;
	public GameObject m_Camera;
	public int angle;

	//private GameObject temp_Collider;
	private int direction; // 플레이어 방향값
	private int jump_MAX, jump_Count;
	private float timer_Charging, timer_Stun; // 타이머
	private float speed, jump; // 플레이어 이동 변수
	private float power;
	private bool isPlaying;
	private bool isJumpping,isAttack; // 조작 상태 변수
	private bool isCharging, isCharged, isStunned, isFalling; // 행동 상태 변수
	//public bool isIgnored;	

	void Start ()
	{
		jump_MAX = 3;
		jump_Count = 0;

		power = 4f;
		speed = 2;
		jump = 6;
		isPlaying = true;
		//isIgnored = true;

		timer_Charging = timer_Stun = 0;
		isJumpping = isAttack = false;
		isCharging = isCharged = isStunned = isFalling = false;
	}

	void Update ()
	{
		if (!isStunned) InputManager();

		if ((timer_Charging >= 2) && !isCharged)
		{
			isCharged = true;
			power *= 2;
			anim.SetBool(p_Name + "_Charged", true);
		}

		if (isCharging) timer_Charging += Time.deltaTime;
	}
	
	void FixedUpdate ()
	{
		Debug.Log(GetComponent<Rigidbody2D>().velocity.y);

		if ((GetComponent<Rigidbody2D>().velocity.y < -2) && !isFalling) isFalling = true;

		/*if (isIgnored && GetComponent<Rigidbody2D>().velocity.y < 0)
		{
			
			m_DefaultCollider.GetComponent<BoxCollider2D>().enabled = true;
			isIgnored = false;
		}*/

		if (!isStunned) Move();
		else if (timer_Stun > 0) timer_Stun -= Time.deltaTime;

		else
		{
			isStunned = false;
			anim.SetBool(p_Name + "_Damaged", false);
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
				anim.SetBool(p_Name + "_Charged", false);
			}
		}

		if ((jump_Count < jump_MAX) && Input.GetButtonDown(p_Name + "_B Button"))
		{
			//isJumpping = true;
			//isIgnored = true;
			anim.SetBool(p_Name + "_Jump", true);
			anim.SetBool(p_Name + "_Walk", false);
			jump_Count++;

			//m_DefaultCollider.GetComponent<BoxCollider2D>().enabled = false;
			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jump);
			//Physics2D.IgnoreCollision(temp_Collider.GetComponent<BoxCollider2D>(), m_DefaultCollider.GetComponent<BoxCollider2D>(), false);
		}
	}

	private void Move()
	{
		float tempValue = 0;
		int value = -10; // -1 : 왼쪽, 0 : 정지, 1 : 오른쪽

		/* 조이스틱 입력 */
		if (p_Name != "P0") tempValue = Input.GetAxis(p_Name + "_LeftThumbstickButton");

		/* 키보드 입력 */
		else
		{
			if (Input.GetAxis(p_Name + "_LeftThumbstickButton") > 0.05) value = 1; // 오른쪽 이동
			else if (Input.GetAxis(p_Name + "_LeftThumbstickButton") < -0.05) value = -1; // 왼족 이동
			else value = 0; // 정지
		}

		/* 키 입력 <오른쪽 이동> */
		if ((tempValue > 0.7) || value == 1)
		{
			m_Camera.transform.localPosition = new Vector3(0, 1.451504f, -10);
			m_Camera.transform.localEulerAngles = new Vector3(0, 0, 0);

			direction = 1;
			angle = 0;

			if (!isJumpping) anim.SetBool(p_Name + "_Walk", true);
		}

		/* 키 입력 <왼쪽 이동> */
		else if ((tempValue < -0.7) || value == -1)
		{
			m_Camera.transform.localPosition = new Vector3(0, 1.451504f, 10);
			m_Camera.transform.localEulerAngles = new Vector3(0, 180, 0);

			direction = -1;
			angle = 180;

			if (!isJumpping) anim.SetBool(p_Name + "_Walk", true);
		}

		/* 정지 상태 */
		else
		{
			direction = 0;
			anim.SetBool(p_Name + "_Walk", false);
		}

		GetComponent<Rigidbody2D>().velocity = new Vector2(direction * speed, GetComponent<Rigidbody2D>().velocity.y);
		transform.eulerAngles = new Vector3(0, angle, 0);
	}

	private void Attack()
	{
		m_AttackCollider.GetComponent<PolygonCollider2D>().enabled = true;
		anim.SetBool(p_Name + "_Attack", true);

		m_AttackCollider.GetComponent<PlayerAttack>().DelayOn();
	}

	public void Damaged(float _power, int _direction)
	{
		anim.SetBool(p_Name + "_Damaged", true);
		anim.SetBool(p_Name + "_Charged", false);
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
		/*if (!isIgnored && GetComponent<Rigidbody2D>().velocity.y < 0 && col.gameObject.tag == "Ground" && isJumpping)
		{
			Physics2D.IgnoreCollision(col.gameObject.GetComponent<BoxCollider2D>(), m_DefaultCollider.GetComponent<BoxCollider2D>(), true);
			temp_Collider = col.gameObject;
		}*/

		if (col.gameObject.tag == "Ground" && isFalling)
		{
			//isJumpping = false;
			jump_Count = 0;
			isFalling = false;
			anim.SetBool(p_Name + "_Jump", false);
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