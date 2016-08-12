using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	public string p_Name;
	public Animator anim;
	public GameObject m_AttackCollider, m_DefaultCollider, m_DetectCollider;
	public GameObject m_Camera;
	public int angle;

	private GameObject temp_Collider;
	private int direction; // 플레이어 방향값
	private int jump_MAX, jump_Count;
	private float timer_Charging, timer_Stun, time_Detect; // 타이머
	private float speed, jump; // 플레이어 이동 변수
	private float power;
	private bool isPlaying;
	private bool isJumpping,isAttack, isTriggerOn; // 조작 상태 변수
	private bool isCharging, isCharged, isStunned; // 행동 상태 변수
	public bool isIgnored;	

	void Start ()
	{
		jump_MAX = 3;
		jump_Count = 0;

		power = 4f;
		speed = 2;
		jump = 6;
		isPlaying = true;

		timer_Charging = timer_Stun = time_Detect = 0;
		isJumpping = isAttack = isTriggerOn = false;
		isCharging = isCharged = isStunned  = false;
	}

	void Update ()
	{
		if (!isStunned) InputManager();

		if ((timer_Charging >= 2) && !isCharged)
		{
			isCharged = true;
			anim.SetBool(p_Name + "_Charged", true);
		}

		if (isCharging) timer_Charging += Time.deltaTime;
	}
	
	void FixedUpdate ()
	{
		if (time_Detect > 0.2f)
		{
			m_DetectCollider.GetComponent<PolygonCollider2D>().enabled = false;
			m_DefaultCollider.GetComponent<PolygonCollider2D>().enabled = true;
			time_Detect = 0;
		}

		if ((GetComponent<Rigidbody2D>().velocity.y < 0) && !isIgnored && isJumpping)
		{
			m_DetectCollider.GetComponent<PolygonCollider2D>().enabled = true;
			isIgnored = true;
		}

		if (isIgnored) time_Detect += Time.deltaTime;

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

			power = 4;
			Attack();
		}

		else if (Input.GetButtonUp(p_Name + "_A Button"))
		{
			isCharging = false;
			timer_Charging = 0;

			if (isCharged)
			{
				isAttack = true;

				power = 12;
				Attack();

				isCharged = false;
				anim.SetBool(p_Name + "_Charged", false);
			}
		}

		/* 점프 */
		if ((jump_Count < jump_MAX) && (Input.GetButtonDown(p_Name + "_B Button") || ((p_Name != "P0") && Input.GetAxis(p_Name + "_Triggers") > 0.9f && !isTriggerOn)))
		{
			jump_Count++;
			Jump(false);
		}

		if ((p_Name != "P0") && isTriggerOn && Input.GetAxis(p_Name + "_Triggers") < 0.5f) isTriggerOn = false;
	}

	public void Jump(bool isSpring)
	{
		m_DetectCollider.GetComponent<DetectGround>().Reset();
		isIgnored = false;
		isTriggerOn = true;
		isJumpping = true;
		anim.SetBool(p_Name + "_Jump", true);
		anim.SetBool(p_Name + "_Walk", false);

		m_DefaultCollider.GetComponent<PolygonCollider2D>().enabled = false;
		if (!isSpring) GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jump);
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
		anim.SetBool(p_Name + "_Charged", false);
		anim.SetBool(p_Name + "_Damaged", true);
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

	public void Die()
	{
		isPlaying = false;
		transform.gameObject.SetActive(false);

		GameObject.Find("GameManager").GetComponent<GameManager>().NoticeOfDeath(p_Name);
		Debug.Log(this.name);
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Ground" )
		{
			jump_Count = 0;
			anim.SetBool(p_Name + "_Jump", false);
			isJumpping = false;

			m_DetectCollider.GetComponent<DetectGround>().Reset();
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