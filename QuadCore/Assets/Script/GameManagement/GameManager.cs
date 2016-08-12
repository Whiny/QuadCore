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
		/*try { index = Convert.ToInt16(p_Name[1]); Debug.Log(index); }
		finally{ Debug.Log("Index Error"); index = -1; }

		Debug.Log(p_Name);
		Debug.Log(index);*/

		if (p_Name == "P0")
			index = 0;
		else if (p_Name == "P1")
			index = 1;
		else if (p_Name == "P2")
			index = 2;
		else if (p_Name == "P3")
			index = 3;
		else
			index = -1;

		//에러면 게임 종료
		if (index == -1) { StartCoroutine("GameOver", -1); return; }

		isPlaying[index] = false;

		for(int i = 0; i < 4; i++)
		{
			if (isPlaying[i])
			{
				count++;
				winner = i;
			}
		}

		if (count == 1) { StartCoroutine("GameOver", winner); }
		else if (count < 1) { return; }
    }
	private IEnumerator GameOver(int winner)
	{
		if (winner > 3 || winner < 0)
		{
			Debug.Log("Winner Number Error");
			GameObject.Find("Canvas/Text").GetComponent<Text>().text = "It's Error";
		}
		else
		{
			string playerColor;
			if (winner == 1)
				playerColor = "Green";
			else if (winner == 2)
				playerColor = "Red";
			else if (winner == 3)
				playerColor = "Yellow";
			else // winner == 0
				playerColor = "Blue";
			//게임에서 승리한 플레이어의 카메라를 전체로 확대하고 승리문구 출력
			GameObject.Find("Player " + playerColor).GetComponentInChildren<Camera>().rect = new Rect(0, 0, 1, 1);
			GameObject.Find("Canvas/Text").GetComponent<Text>().text = playerColor + " Player\nWin!";
		}
		GameObject.Find("Canvas/Text").GetComponent<Text>().enabled = true;

		yield return new WaitForSeconds(5);

		//게임 종료
		Application.LoadLevel("MainMenu");
	}
}
