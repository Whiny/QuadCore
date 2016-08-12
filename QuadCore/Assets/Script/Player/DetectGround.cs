using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DetectGround : MonoBehaviour
{
	public GameObject m_DefaultCollider;

	private List<GameObject> list_Ground;
	private float timer_DefaultCollider;
	private bool isCollision;

	void Start ()
	{
		list_Ground = new List<GameObject>();
		timer_DefaultCollider = 1;
		isCollision = false;
	}
	
	void FixedUpdate ()
	{
		if (timer_DefaultCollider < 0.5f) timer_DefaultCollider += Time.deltaTime;

		else
		{
			for (int i = 0; i < list_Ground.Count; i++)
			{
				if (list_Ground[i].GetComponent<BoxCollider2D>() != null)
					Physics2D.IgnoreCollision(list_Ground[i].GetComponent<BoxCollider2D>(), m_DefaultCollider.GetComponent<PolygonCollider2D>(), false);

				else if (list_Ground[i].GetComponent<PolygonCollider2D>() != null)
					Physics2D.IgnoreCollision(list_Ground[i].GetComponent<PolygonCollider2D>(), m_DefaultCollider.GetComponent<PolygonCollider2D>(), false);
			}

			list_Ground.Clear();
			this.GetComponent<PolygonCollider2D>().enabled = false;
		}
	}

	public void Check()
	{
		timer_DefaultCollider = 0;
		this.GetComponent<PolygonCollider2D>().enabled = true;
	}

	public void Reset()
	{
		for (int i = 0; i<list_Ground.Count; i++)
		{
			if (list_Ground[i].GetComponent<BoxCollider2D>() != null)
				Physics2D.IgnoreCollision(list_Ground[i].GetComponent<BoxCollider2D>(), m_DefaultCollider.GetComponent<PolygonCollider2D>(), false);

			else if (list_Ground[i].GetComponent<PolygonCollider2D>() != null)
				Physics2D.IgnoreCollision(list_Ground[i].GetComponent<PolygonCollider2D>(), m_DefaultCollider.GetComponent<PolygonCollider2D>(), false);
		}

		list_Ground.Clear();
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Ground")
		{
			isCollision = true;
			list_Ground.Add(col.gameObject);

			if (col.GetComponent<PolygonCollider2D>() != null)
				Physics2D.IgnoreCollision(col.GetComponent<PolygonCollider2D>(), m_DefaultCollider.GetComponent<PolygonCollider2D>(), true);

			else if (col.GetComponent<BoxCollider2D>() != null)
				Physics2D.IgnoreCollision(col.GetComponent<BoxCollider2D>(), m_DefaultCollider.GetComponent<PolygonCollider2D>(), true);
		}
	}

	public bool IsCollision
	{
		get
		{
			return isCollision;
		}

		set
		{
			isCollision = value;
		}
	}
}
