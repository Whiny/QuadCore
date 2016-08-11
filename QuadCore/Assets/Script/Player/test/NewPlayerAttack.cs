using UnityEngine;
using System.Collections;

public class NewPlayerAttack : MonoBehaviour
{
	public GameObject root_Object;

	public float timer_Attack = 0f;

	void Start()
	{

	}

	void OnEnable()
	{
		GetComponent<PolygonCollider2D>().enabled =true;
		timer_Attack = 0.1f;
	}
	void OnDisable()
	{
		root_Object.GetComponent<NewPlayerControl>().Power = 4f;
	}
	void FixedUpdate()
	{
		timer_Attack -= Time.deltaTime;
		if (timer_Attack < 0)
		{
			GetComponent<PolygonCollider2D>().enabled = false;
			GetComponent<NewPlayerAttack>().enabled = false;
		}
	}
	private void OnTriggerEnter2D(Collider2D col)
	{
		Debug.Log("fire1");
		if (col.gameObject.tag == "Player")
		{
			Debug.Log("fire2");
			if (col.gameObject.name == "CollisionCollider")
			{
				Debug.Log("fire3");
				int angle = root_Object.GetComponent<NewPlayerControl>().Angle;
				float power = root_Object.GetComponent<NewPlayerControl>().Power;

				if (angle == 0) col.gameObject.GetComponentInParent<NewPlayerControl>().Damaged(power, 1);
				else col.gameObject.GetComponentInParent<NewPlayerControl>().Damaged(power, -1);
			}
		}
	}
}
