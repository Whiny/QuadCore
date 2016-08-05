using UnityEngine;
using System.Collections;

public class PlayerAttack : PlayerControl
{
	public GameObject root_Object;

	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
	
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		
		if (col.gameObject.tag == "Player")
		{
			if (col.gameObject.name == "CollisionCollider")
			{
				Debug.Log("ASDF");
				int angle = root_Object.GetComponent<PlayerControl>().Angle;
				float power = root_Object.GetComponent<PlayerControl>().Power;
				root_Object.GetComponent<PlayerControl>().Power = 4f;

				if (angle == 0) col.gameObject.GetComponentInParent<PlayerControl>().Damaged(power, 1);
				else col.gameObject.GetComponentInParent<PlayerControl>().Damaged(power, -1);
			}
		}
	}
}
