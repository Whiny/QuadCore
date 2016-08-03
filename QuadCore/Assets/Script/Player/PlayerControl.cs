using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	private int direction;
	private float time;
	private float speed;
	private float jump;
	private bool isStun;
	private bool isPlaying;
	private bool isJumpping;

	// Use this for initialization
	void Start ()
	{
		time = 0;
		speed = 200;
		jump = 400;

		isStun = false;
		isJumpping = false;
		isPlaying = true;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		Move();
	}

	void Move()
	{

	}
}
