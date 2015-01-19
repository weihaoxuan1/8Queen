using UnityEngine;
using System.Collections;

public class UserSelectRecorder : MonoBehaviour 
{
	public static UserSelectRecorder Instance;

	private int replaceNum;  //玩家当前预放棋子的数量

	UserSelectRecorder()
	{
		Instance = this;
	}

	public void SetReplaceNum(int num)
	{
		replaceNum = num;
	}

	public int GetReplaceNum()
	{
		return replaceNum;
	}

}
