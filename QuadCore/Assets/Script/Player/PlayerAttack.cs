using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
	public GameObject root_Object;
	public string p_Name;

	private float delay;
	private bool isEnable;

	void Start ()
	{
		delay = 0;
		this.GetComponent<PolygonCollider2D>().enabled = false;
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (delay >= 0.15f)
		{
			isEnable = false;
			delay = 0;
			root_Object.GetComponent<PlayerControl>().IsAttack = false;
		}

		else if (delay >= 0.05f)
		{
			this.GetComponent<PolygonCollider2D>().enabled = false;
			//root_Object.GetComponent<PlayerControl>().Power = 4;

			root_Object.GetComponent<PlayerControl>().anim.SetBool(p_Name + "_Attack", false);
		}

		if (isEnable) delay += Time.deltaTime;
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			if (col.gameObject.name == "CollisionCollider")
			{
				int angle = root_Object.GetComponent<PlayerControl>().Angle;
				float power = root_Object.GetComponent<PlayerControl>().Power;
				Debug.Log(power);

				if (angle == 0) col.gameObject.GetComponentInParent<PlayerControl>().Damaged(power, 1, "Player");
				else col.gameObject.GetComponentInParent<PlayerControl>().Damaged(power, -1, "Player");
			}
		}
	}

	public void DelayOn()
	{
		delay = 0;
		isEnable = true;
	}
}
