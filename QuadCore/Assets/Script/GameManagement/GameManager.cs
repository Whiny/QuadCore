using UnityEngine;
using UnityEngine.UI;
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
		finally{ Debug.Log("Index Error"); index = -1; }

		//에러면 게임 종료
		if (index == -1) { GameOver(index); return; }

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
		if (winner > 3 || winner < 0)
		{
			Debug.Log("Winner Number Error");
			GameObject.Find("/Canvas/Text").GetComponent<Text>().text = "It's Error";
		}
		else
		{
			//게임에서 승리한 플레이어의 카메라를 전체로 확대하고 승리문구 출력
			//GameObject.Find(/*오브젝트 이름*/).GetComponentInChildren<Camera>().rect = new Rect (0, 0, 1, 1);
			GameObject.Find("/Canvas/Text").GetComponent<Text>().text = winner + " Palyer\nWin!";
		}

		//게임 종료
		Application.LoadLevel("MainMenu");
	}
}
