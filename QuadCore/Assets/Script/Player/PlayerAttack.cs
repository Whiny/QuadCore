using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour
{
	public GameObject root_Object;

	private float delay;
	private bool isEnable;

	void Start ()
	{
		delay = 0;
		this.GetComponent<PolygonCollider2D>().enabled = false;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if(delay >= 0.1f)
		{
			delay = 0;
			isEnable = false;

			this.GetComponent<PolygonCollider2D>().enabled = false;
			root_Object.GetComponent<PlayerControl>().Power = 4;
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

				if (angle == 0) col.gameObject.GetComponentInParent<PlayerControl>().Damaged(power, 1);
				else col.gameObject.GetComponentInParent<PlayerControl>().Damaged(power, -1);
			}
		}
	}

	public void DelayOn()
	{
		delay = 0;
		isEnable = true;
	}
}
