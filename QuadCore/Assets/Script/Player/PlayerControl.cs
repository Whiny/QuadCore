using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	public string p_Name;
	public Animator anim;
	public GameObject[] p_DefaultCollider, p_AttackCollider, p_CollisionCollider;
	public GameObject m_AttackCollider;

	private int angle;
	private float timer_Charging, timer_Stun; // 타이머
	private float speed, jump; // 플레이어 이동 변수
	private bool isPlaying;
	private bool isJumpping,isAttack; // 조작 상태 변수
	private bool isCharging, isCharged, isStunned; // 행동 상태 변수

	protected int direction; // 플레이어 방향값
	protected float power;

	void Awake()
	{
		power = 2f;
	}

	void Start ()
	{
		angle = 0;
		speed = 2;
		jump = 6;
		isPlaying = true;

		timer_Charging = timer_Stun = 0;
		isJumpping = isAttack = false;
		isCharging = isCharged = isStunned = false;

		Physics2D.IgnoreCollision(p_DefaultCollider[0].GetComponent<BoxCollider2D>(), p_DefaultCollider[1].GetComponent<BoxCollider2D>(), true);
		Physics2D.IgnoreCollision(p_AttackCollider[0].GetComponent<PolygonCollider2D>(), p_AttackCollider[1].GetComponent<PolygonCollider2D>(), true);
		Physics2D.IgnoreCollision(p_CollisionCollider[0].GetComponent<PolygonCollider2D>(), p_CollisionCollider[1].GetComponent<PolygonCollider2D>(), true);
		m_AttackCollider.GetComponent<PolygonCollider2D>().enabled = false;
	}
	
	void FixedUpdate ()
	{
		if (isAttack)
		{
			anim.SetBool("P1_Attack", false);
			m_AttackCollider.GetComponent<PolygonCollider2D>().enabled = false;
			isAttack = false;
		}

		if (Input.GetButton(p_Name + "_A Button") && !isAttack && !isCharging)
		{
			isAttack = true;
			isCharging = true;

			Attack();
		}

		else if (!(Input.GetButton(p_Name + "_A Button")))
		{
			isCharging = false;
			power = 2f;
			timer_Charging = 0;

			if (isCharged)
			{
				isAttack = true;
				Attack();

				isCharged = false;
				anim.SetBool("P1_Charged", false);
			}
		}

		if (isCharging) timer_Charging += Time.deltaTime;

		if (timer_Charging >= 2)
		{
			isCharged = true;
			power *= 2;
			anim.SetBool("P1_Charged", true);
		}	

		if (!isStunned) Move();
		else if (timer_Stun > 0) timer_Stun -= Time.deltaTime;
		else isStunned = false;
	}

	private void Move()
	{
		float tempValue = Input.GetAxis(p_Name + "_LeftThumbstickButton");

		if (!isJumpping && Input.GetButton(p_Name + "_B Button"))
		{
			isJumpping = true;
			anim.SetBool("P1_Jump", true);
			anim.SetBool("P1_Walk", false);

			GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, jump);
		}

		/* 키 입력 <오른쪽 이동> */
		if (tempValue > 0.7)
		{
			direction = 1;
			angle = 0;

			if (!isJumpping) anim.SetBool("P1_Walk", true);
		}

		/* 키 입력 <왼쪽 이동> */
		else if (tempValue < -0.7)
		{
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
	}

	public void Damaged(float power, int direction)
	{
		isStunned = true;
		timer_Stun = power * 0.15f;
		GetComponent<Rigidbody2D>().velocity = new Vector2(direction * power, 1 * power);
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Ground" && isJumpping)
		{
			isJumpping = false;
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
}