using UnityEngine;
using System.Collections;

public class DetectGround : MonoBehaviour
{
	public GameObject m_DefaultCollider;
	public GameObject temp_Collider;

	void Start ()
	{
		temp_Collider = null;
	}
	
	void Update ()
	{
	
	}

	public void Reset()
	{
		if (temp_Collider != null)
		{
			if (temp_Collider.GetComponent<BoxCollider2D>() != null)
				Physics2D.IgnoreCollision(temp_Collider.GetComponent<BoxCollider2D>(), m_DefaultCollider.GetComponent<PolygonCollider2D>(), false);

			else if (temp_Collider.GetComponent<PolygonCollider2D>() != null)
				Physics2D.IgnoreCollision(temp_Collider.GetComponent<PolygonCollider2D>(), m_DefaultCollider.GetComponent<PolygonCollider2D>(), false);

		}
		temp_Collider = null;
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Ground")
		{
			temp_Collider = col.gameObject;

			if (temp_Collider.GetComponent<BoxCollider2D>() != null)
				Physics2D.IgnoreCollision(temp_Collider.GetComponent<BoxCollider2D>(), m_DefaultCollider.GetComponent<PolygonCollider2D>(), true);

			else if (temp_Collider.GetComponent<PolygonCollider2D>() != null)
				Physics2D.IgnoreCollision(temp_Collider.GetComponent<PolygonCollider2D>(), m_DefaultCollider.GetComponent<PolygonCollider2D>(), true);

			this.GetComponent<PolygonCollider2D>().enabled = false;
		}
	}
}
