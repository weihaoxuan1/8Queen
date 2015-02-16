using UnityEngine;
using System.Collections;

public class UserBehaviorRecorder : MonoBehaviour 
{
	public static UserBehaviorRecorder Instance;

	private int replaceNum;     //玩家当前预放棋子的数量
	private int wrongStepsNum;  //玩家一局游戏中棋子放置错误的次数

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

}
