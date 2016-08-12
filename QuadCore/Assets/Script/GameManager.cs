using UnityEngine;
using System.Collections;
using System;

public class GameManager : MonoBehaviour
{
	private bool[] isPlaying = new bool [4];

	void Start ()
	{
		for (int i = 0; i < 4; i++)
			isPlaying[i] = true;
	}

	public void NoticeOfDeath(string p_Name)
	{
		int index;
		int count = 0;
		int winner = -1;
		//혹시나 에러 날까봐(사실은 그냥 써보고 싶어서)
		try { index = Convert.ToInt16(p_Name[1]); }
		finally{ Debug.Log("Index Error"); index = 0; }

		isPlaying[index] = false;

		for(int i = 0; i < 4; i++)
		{
			if (isPlaying[i])
			{
				count++;
				winner = i;
			}
		}

		if(count == 1)
		{
			GameOver(winner);
		}
    }
	private void GameOver(int winner)
	{
		if (winner == -1) { Debug.Log("Winner Number Error"); }
		else
		{
			 //게임 승리시
		}
	}
}
