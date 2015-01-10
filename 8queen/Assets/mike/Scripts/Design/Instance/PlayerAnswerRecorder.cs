using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class PlayerAnswerRecorder : MonoBehaviour 
{
	public static PlayerAnswerRecorder Instance;

	private Dictionary<int, List<ChessmanInfo[]>> mAnswerChessmanInfos;  //用户解出的棋子答案信息

	PlayerAnswerRecorder()
	{
		Instance = this;
		mAnswerChessmanInfos = new Dictionary<int, List<ChessmanInfo[]>> ();
	}

	/*
	 * 从文件中读取用户解出的答案,并保存到程序中
	 */ 
	private void ReadAnswer(int size)
	{
		//将旧的信息去除,以便添加新的信息
		if (mAnswerChessmanInfos.ContainsKey (size)) 
		{
			mAnswerChessmanInfos.Remove(size);
		}
		
		List<ChessmanInfo[]> myAnswers = new List<ChessmanInfo[]> ();
		
		string line;
		StreamReader sr = null;

        if (File.Exists(Application.dataPath + "//" + "MyAnswer_" + size))
            sr = File.OpenText(Application.dataPath + "//" + "MyAnswer_" + size);
        else
            return;
		
		while ((line = sr.ReadLine()) != null) 
		{
			ChessmanInfo[] oneAnswer = SaveOneAnswer(line, size);
			myAnswers.Add(oneAnswer);
		}
		
		mAnswerChessmanInfos.Add (size, myAnswers);

        sr.Close();
        sr.Dispose();
	}
	
	/*
	 * 将从文件中读取的一行字符串转换为棋子信息
	 */ 
	private ChessmanInfo[] SaveOneAnswer(string answer, int size)
	{
		int infoIndex = 0;
		ChessmanInfo[] infos = new ChessmanInfo[size];

		//通过空格字符区分开每个棋子
		string[] coordinates = answer.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
		
		foreach(string coordinate in coordinates)
		{
			//通过逗号字符区分开行列信息
			string[] elements = coordinate.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);

			int line = -1;
			int row = -1;
			if(!int.TryParse(elements[0], out line))
			{
				Debug.LogError("error when parse to int");
				return null;
			}
			if(!int.TryParse(elements[1], out row))
			{
				Debug.LogError("error when parse to int");
				return null;
			}
			
			ChessmanInfo info = new ChessmanInfo(line, row);
			infos[infoIndex] = info;
			infoIndex++;
		}
		
		return infos;
	}

	/*
	 * 检查用户解出的答案是否已经保存在文件中
	 */ 
    /*
	private bool CheckAnswerSaved(string answer, int chessboardSize)
	{
		string line;
		StreamReader sr = null;
		
		try 
		{
			sr = File.OpenText(Application.dataPath + "//" + "MyAnswer_" + chessboardSize);
		} catch(FileNotFoundException e)
		{
			return false;
		}
		
		while ((line = sr.ReadLine()) != null) 
		{
			if(line.Equals(answer))
			{
				sr.Close();
				sr.Dispose();
				return true;
			}
		}
		
		sr.Close();
		sr.Dispose ();
		return false;
	}
    */

	/*
	 * 将用户解出的答案保存在文件中
	 */ 
	public void WriteAnswer(List<ChessmanInfo> chessmanInfos)
	{
		//准备要保存的信息
		string message = "";
		foreach (ChessmanInfo info in chessmanInfos) 
		{
			message += info.line + "," + info.row + " ";
		}

		//避免重复保存相同的答案
        /*
		if(CheckAnswerSaved(message, chessmanInfos.Count))
		{
			return;
		}
        */
		
		StreamWriter sw;
		FileInfo fileInfo = new FileInfo (Application.dataPath + "//" + "MyAnswer_" + chessmanInfos.Count);
		
		if (!fileInfo.Exists) 
		{
			sw = fileInfo.CreateText ();
		}
		
		else 
		{
			sw = fileInfo.AppendText();
		}
		
		sw.WriteLine (message);
		sw.Close ();
		sw.Dispose ();
	}

	/*
	 * 返回玩家的答案信息
	 */ 
	public List<ChessmanInfo[]> GetPlayerAnswer(int chessboardSize)
	{
		//返回答案信息前要从文件中更新
		ReadAnswer (chessboardSize);

		if(mAnswerChessmanInfos.ContainsKey(chessboardSize))
		{
			return mAnswerChessmanInfos[chessboardSize];
		}

		else 
		{
			return null;
		}
	}
	
}
