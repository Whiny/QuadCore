using UnityEngine;
using System.Collections;

public class Damaged : MonoBehaviour
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		if(Input.GetButton("P1_B Button"))
		{
			IsDamaged(true, false);
		}
	}

	void IsDamaged(bool isPower,bool isRight)
	{ 
		if (isPower)
		{
			if (isRight)
				GetComponent<Rigidbody2D>().velocity = new Vector2(1 * 10, 1 * 10);
			else
				GetComponent<Rigidbody2D>().velocity = new Vector2(-1 * 10, 1 * 10);
		}
		else
		{
			if (isRight)
				GetComponent<Rigidbody2D>().velocity = new Vector2(1 * 4, 1 * 4);
			else
			{
				GetComponent<Rigidbody2D>().velocity = new Vector2(-1 * 4, 1 * 4);
				//Debug.Log();
			}
		}
	}
}
