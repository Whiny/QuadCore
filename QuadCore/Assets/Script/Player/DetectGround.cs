using UnityEngine;
using System.Collections;

public class DetectGround : MonoBehaviour
{
	public GameObject m_DefaultCollider;
	private GameObject temp_Collider;

	void Start ()
	{
		temp_Collider = null;
	}
	
	void Update ()
	{
	
	}

	public void Reset()
	{
		if (temp_Collider != null) Physics2D.IgnoreCollision(temp_Collider.GetComponent<BoxCollider2D>(), m_DefaultCollider.GetComponent<BoxCollider2D>(), false);
		temp_Collider = null;
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Ground")
		{
			temp_Collider = col.gameObject;
			Physics2D.IgnoreCollision(temp_Collider.GetComponent<BoxCollider2D>(), m_DefaultCollider.GetComponent<BoxCollider2D>(), true);
			this.GetComponent<PolygonCollider2D>().enabled = false;
		}
	}
}
