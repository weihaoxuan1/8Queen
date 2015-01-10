using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_SelectChessboard : MonoBehaviour 
{
	private CameraMovement cameraMovement;
	
	void Start()
	{
		cameraMovement = Camera.main.GetComponent<CameraMovement> ();
	}

	void OnGUI()
	{
		Button_startChess ();
		Button_ShowPlayerAnswer ();
	}

	/*
	 * 开始游戏的按钮
	 */ 
	private void Button_startChess()
	{
		if(GUI.Button(new Rect(50, 50, 50, 50), "4*4"))
		{
			StartChess(4);
		}

		if(GUI.Button(new Rect(150, 50, 50, 50), "5*5"))
		{
			StartChess(5);
		}

		if(GUI.Button(new Rect(250, 50, 50, 50), "6*6"))
		{
			StartChess(6);
		}

		if(GUI.Button(new Rect(350, 50, 50, 50), "7*7"))
		{
			StartChess(7);
		}

		if(GUI.Button(new Rect(450, 50, 50, 50), "8*8"))
		{
			StartChess(8);
		}
	}

	/*
	 * 查看玩家答案的按钮
	 */ 
	private void Button_ShowPlayerAnswer()
	{
		if(GUI.Button(new Rect(50, 150, 50, 50), "答案"))
		{
			ShowPlayerAnswer(4);
		}
		
		if(GUI.Button(new Rect(150, 150, 50, 50), "答案"))
		{
			ShowPlayerAnswer(5);
		}
		
		if(GUI.Button(new Rect(250, 150, 50, 50), "答案"))
		{
			ShowPlayerAnswer(6);
		}
		
		if(GUI.Button(new Rect(350, 150, 50, 50), "答案"))
		{
			ShowPlayerAnswer(7);
		}
		
		if(GUI.Button(new Rect(450, 150, 50, 50), "答案"))
		{
			ShowPlayerAnswer(8);
		}
	}

	/*
	 * 开始游戏
	 */ 
	private void StartChess(int chessboardSize)
	{
		ChessRoot.Instance.CreateChessboard(chessboardSize);
		ChessRoot.Instance.AllowSetChessman ();
		cameraMovement.RefreshCameraPos ();
		MainDirector.Instance.ToGamingUI ();
	}

	/*
	 * 展示玩家解出的答案
	 */ 
	private void ShowPlayerAnswer(int chessboardSize)
	{
		List<ChessmanInfo[]> playerAnswer = PlayerAnswerRecorder.Instance.GetPlayerAnswer (chessboardSize);

		//玩家尚未解出答案
		if (playerAnswer == null) 
		{
			Debug.Log("尚未解出" + chessboardSize + "*" + chessboardSize + "棋盘规模的答案");
			return;
		}

		ChessRoot.Instance.CreateChessboard(chessboardSize);
		ChessRoot.Instance.ForbidSetChessman ();
		cameraMovement.RefreshCameraPos ();
		MainDirector.Instance.ToPlayerAnswerUI ();
	}
}
