using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	public string p_Name;
	public Animator anim;
	public GameObject m_AttackCollider;
	public GameObject m_Camera;

	private int angle;
	private float timer_Charging, timer_Stun; // 타이머
	private float speed, jump; // 플레이어 이동 변수
	private bool isPlaying;
	private bool isJumpping,isAttack; // 조작 상태 변수
	private bool isCharging, isCharged, isStunned; // 행동 상태 변수

	protected int direction; // 플레이어 방향값
	public float power;

	void Start ()
	{
		power = 4f;

		angle = 0;
		speed = 2;
		jump = 6;
		isPlaying = true;

		timer_Charging = timer_Stun = 0;
		isJumpping = isAttack = false;
		isCharging = isCharged = isStunned = false;
	}
	
	void FixedUpdate ()
	{
		if (isAttack && !isStunned)
		{
			anim.SetBool(p_Name + "_Attack", false);
			isAttack = false;
		}

		if (Input.GetButton(p_Name + "_A Button") && !isAttack && !isCharging && !isStunned)
		{
			isAttack = true;
			isCharging = true;

			Attack();
		}

		else if (!(Input.GetButton(p_Name + "_A Button")))
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

		if (isCharging) timer_Charging += Time.deltaTime;

		if ((timer_Charging >= 2) && !isCharged)
		{
			isCharged = true;
			power *= 2;
			anim.SetBool(p_Name + "_Charged", true);
		}

		if (!isStunned) Move();
		else if (timer_Stun > 0) timer_Stun -= Time.deltaTime;

		else
		{
			isStunned = false;
			anim.SetBool(p_Name + "_Damaged", false);
		}
	}

	private void Move()
	{
		float tempValue = Input.GetAxis(p_Name + "_LeftThumbstickButton");

		if (!isJumpping && Input.GetButton(p_Name + "_B Button"))
		{
			isJumpping = true;
			anim.SetBool(p_Name + "_Jump", true);
			anim.SetBool(p_Name + "_Walk", false);

			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jump);
		}

		/* 키 입력 <오른쪽 이동> */
		if (tempValue > 0.7)
		{
			m_Camera.transform.localPosition = new Vector3(0, 1.451504f, -10);
			m_Camera.transform.localEulerAngles = new Vector3(0, 0, 0);

			direction = 1;
			angle = 0;

			if (!isJumpping) anim.SetBool(p_Name + "_Walk", true);
		}

		/* 키 입력 <왼쪽 이동> */
		else if (tempValue < -0.7)
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
		if (col.gameObject.tag == "Ground" && isJumpping)
		{
			isJumpping = false;
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
}