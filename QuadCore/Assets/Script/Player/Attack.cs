using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour
{
	private float charge = 0;					//차징 시간 저장
	private bool currentSwingIsPower = false;	//최근 공격이 차징공격인지

	// Use this for initialization
	void Start ()
	{
	
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
        Swing();
        Charging();
	}
    private void Swing()
    {
        if(Input.GetButtonDown("P1_A Button"))
        {
			currentSwingIsPower = false;

			/*애니메이션 작업*/

			
        }
    }

    private void Charging()
	{
		if(Input.GetButton("P1_A Button"))
		{
			charge += Time.deltaTime;
		}
		else
		{
			if(charge > 2)
			{
				SuperPowerSwing();
			}
			charge = 0;
		}
    }

	private void SuperPowerSwing()
	{
		currentSwingIsPower = true;

		/*애니메이션 작업*/


	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "player")
		{
			if (/*플레이어가 공격상태이면*/true)
			{
				//other.GetComponent<PlayerControl>().Damaged(currentSwingIsPower);
			}
		}
	}
}