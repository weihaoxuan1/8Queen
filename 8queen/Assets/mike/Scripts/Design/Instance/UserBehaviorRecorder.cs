using UnityEngine;
using System.Collections;

public class UserBehaviorRecorder : MonoBehaviour 
{
	public static UserBehaviorRecorder Instance;

	private int replaceNum;     //玩家当前预放棋子的数量
	private int wrongStepsNum;  //玩家一局游戏中棋子放置错误的次数
	private string userChoice;  //玩家在用户设置中选择的用户名

	UserBehaviorRecorder()
	{
		Instance = this;
	}

	public void Init()
	{
		wrongStepsNum = 0;
	}

	public void SetReplaceNum(int num)
	{
		replaceNum = num;
	}

	public int GetReplaceNum()
	{
		return replaceNum;
	}

	public void OneWrongStep()
	{
		wrongStepsNum++;
	}

	public int GetWrongStepsNum()
	{
		return wrongStepsNum;
	}

	public void SetUserChoice(string username)
	{
		userChoice = username;
	}

	public string GetUserChoice()
	{
		return userChoice;
	}

}
