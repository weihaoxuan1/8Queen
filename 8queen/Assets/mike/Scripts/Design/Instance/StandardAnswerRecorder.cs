using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StandardAnswerRecorder : MonoBehaviour 
{
	public static StandardAnswerRecorder Instance;

	private int minChessboardSize;  //最小的棋盘规模
	private int maxChessboardSize;  //最大的棋盘规模
	private Dictionary<int, List<ChessmanInfo[]>> answerChessmanInfos;  //不同棋盘规模的棋子答案信息
	
	StandardAnswerRecorder()
	{
		Instance = this;

		minChessboardSize = 4;
		maxChessboardSize = 8;
		answerChessmanInfos = new Dictionary<int, List<ChessmanInfo[]>> ();

		//计算出4*4 ~ 8*8棋盘规模的所有正解
		for (int i=minChessboardSize; i<=maxChessboardSize; i++) 
		{
			CalStandaraAnswer(i);
		}
	}

	/*
	 * 计算出标准答案并存储答案信息
	 */ 
	private void CalStandaraAnswer(int chessboardSize) {
		int curLine   = 1;  //当前最新加入的棋子位于棋盘的第几行
		List<ChessmanInfo> curPoints = new List<ChessmanInfo>();
		List<ChessmanInfo[]> finalAnswer = new List<ChessmanInfo[]>();
		
		//加入第一个棋子
		curPoints.Add(new ChessmanInfo(1, 0));
		
		while(curLine > 0) {
			
			curPoints[curLine - 1].row ++;
			
			//当前行未搜索完毕
			while(curPoints[curLine - 1].row <= chessboardSize) {
				
				//当前已加入的棋子位置合法
				if(CheckChessmans(curPoints)) {
					
					//当前棋子数量达到棋盘规模,存储一个答案,并进入下一列
					if(curLine == chessboardSize) {
						ChessmanInfo[] curPointsArray = curPoints.ToArray();
						ChessmanInfo[] oneAnswer = new ChessmanInfo[curPointsArray.Length];
						for(int i=0; i<curPointsArray.Length; i++) {
							ChessmanInfo info = new ChessmanInfo(curPointsArray[i].line - 1, curPointsArray[i].row - 1);
							oneAnswer[i] = info;
						}
						finalAnswer.Add(oneAnswer);
						
						curPoints[curLine - 1].row ++;
					}
					
					//当前棋子数量不足棋盘规模,进入下一行,并加入新的棋子
					else if(curLine > 0 && curLine < chessboardSize){
						curLine++;
						curPoints.Add(new ChessmanInfo(curLine, 1));
					}
					
					//异常情况
					else {
						Debug.LogError("curLine out of index");
					}	
				}
				
				//当前已加入的棋子位置不合法,进入下一列
				else {
					curPoints[curLine - 1].row ++;
				}
			}
			
			//当前行已搜索完毕,回溯到上一行
			curPoints.RemoveAt(curLine - 1);
			curLine--;
		}
		
		//存储答案信息
		answerChessmanInfos.Add(chessboardSize, finalAnswer);
	}

	/*
	 * 检查棋子的位置是否合法
	 */ 
	private bool CheckChessmans(List<ChessmanInfo> infos) 
	{
		for(int i=0; i<infos.Count; i++) 
		{
			for(int j=i+1; j<infos.Count; j++) 
			{
				
				//任意两个棋子不能在同一行
				if(infos[i].line == infos[j].line) 
				{
					return false;
				}
				
				//任意两个棋子不能在同一列
				if(infos[i].row == infos[j].row) 
				{
					return false;
				}
				
				//任意两个棋子不能再同一斜线上
				if(infos[i].line - infos[j].line == infos[i].row - infos[j].row) 
				{
					return false;
				}
				
				if((infos[i].line - infos[j].line) + (infos[i].row - infos[j].row) == 0) 
				{
					return false;
				}
				//
			}
		}
		
		return true;
	}

	/*
	 * 随机返回标准答案中chessmanNum个棋子的位置信息
	 */ 
	public ChessmanInfo[] GetRandomChessmanInfos(int chessboardSize, int chessmanNum)
	{
        if (chessmanNum == 0)
        {
            return null;
        }

		if (chessboardSize < minChessboardSize || chessboardSize > maxChessboardSize) 
		{
			Debug.LogError("chessboardSize is illegal");
			return null;
		}

		if (chessmanNum >= chessboardSize || chessmanNum < 0) 
		{
			Debug.LogError("chessmanNum is illegal");
			return null;
		}

		//从所有答案中随机取出一个答案
		List<ChessmanInfo[]> answers = answerChessmanInfos [chessboardSize];
		int randomAnswerIndex = Random.Range (0, answers.Count);
		ChessmanInfo[] randomAnswer = (ChessmanInfo[])answers.ToArray ().GetValue (randomAnswerIndex);

		//从一个答案中随机取出chessmanNum个棋子
		ChessmanInfo[] randomChessmaninfos = new ChessmanInfo[chessmanNum];
		List<int> intList = new List<int> ();
		for (int i=0; i<chessboardSize; i++) 
		{
			intList.Add(i);
		}
		for (int i=0; i<chessboardSize - chessmanNum; i++) 
		{
			int randomRemoveIndex = Random.Range(0, intList.Count);
			intList.RemoveAt(randomRemoveIndex);
		}
		for (int i=0; i<chessmanNum; i++) 
		{
			//randomChessmaninfos[i] = (ChessmanInfo)intList.ToArray().GetValue(i);
            int index = (int)intList.ToArray().GetValue(i);
            randomChessmaninfos[i] = randomAnswer[index];
		}
            

		return randomChessmaninfos;
	}

	/*
	 * 返回标准答案的个数
	 */ 
	public int GetStandaraAnswerNum(int chessboardSize)
	{
		if (chessboardSize < minChessboardSize || chessboardSize > maxChessboardSize) 
		{
			Debug.LogError("chessboardSize is illegal");
			return -1;
		}

		return answerChessmanInfos[chessboardSize].Count;
	}

}
