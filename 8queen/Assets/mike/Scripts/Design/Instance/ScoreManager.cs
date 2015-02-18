using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour 
{
	public static ScoreManager Instance;

	private int finalScore;
	private int duplicateScore;  //解出重复摆法的基本得分
	private int finishScore;     //解出新的摆法的基本得分

	private int starNum;       //星级数量
	private bool hasHalfStar;  //是否需要半星

	private bool isStartTimer;
	private float timer;

	private float[] timeRewardLimits;    //用时奖励时间限制（单位：秒）
	private float[] timeRewardAdditions;  //用时奖励加成
	private float[] chessboardSizeRatios;  //棋盘规模系数


	//棋盘规模数组下标
	private enum ChessboardSizeIndex
	{
		four  = 0,
		five  = 1,
		six   = 2,
		seven = 3,
		eight = 4,
	}

	ScoreManager()
	{
		Instance = this;
	}

	void Awake()
	{
		duplicateScore = 1500;
		finishScore    = 2000;

		starNum = 0;
		hasHalfStar = false;

		isStartTimer = false;
		timer = 0.0f;

		timeRewardLimits   = new float[]{160, 250, 360, 490, 640};
		timeRewardAdditions = new float[]{0.16f, 0.25f, 0.36f, 0.49f, 0.64f};
		chessboardSizeRatios = new float[]{1.6f, 2.5f, 3.6f, 4.9f, 6.4f};
	}

	void Update()
	{
		if (isStartTimer) 
		{
			timer += Time.deltaTime;
		}
	}

	/*
	 * 计算星级
	 */ 
	private void CalStarNum()
	{
		int chessboardSize = ChessRoot.Instance.GetChessboardSize ();

		float curScore = finalScore;  //当前得分
		float theoryScore = 0.0f;     //理论得分
		float scoreRatio = 0.0f;      //当前得分占理论得分的百分比

		int basicScore = 0;  //基本得分
		int errorFreeReward = 0;  //无错奖励
		float chessboardSizeRatio = 0.0f;  //棋盘规模系数

		basicScore = finishScore;
		errorFreeReward = chessboardSize * 100;

		//根据棋盘规模计算棋盘系数规模
		if (chessboardSize == 4) 
		{
			chessboardSizeRatio = chessboardSizeRatios[(int)ChessboardSizeIndex.four];
		}
		
		else if (chessboardSize == 5) 
		{
			chessboardSizeRatio = chessboardSizeRatios[(int)ChessboardSizeIndex.five];
		}
		
		else if (chessboardSize == 6) 
		{
			chessboardSizeRatio = chessboardSizeRatios[(int)ChessboardSizeIndex.six];
		}
		
		else if (chessboardSize == 7) 
		{
			chessboardSizeRatio = chessboardSizeRatios[(int)ChessboardSizeIndex.seven];
		}
		
		else if (chessboardSize == 8) 
		{
			chessboardSizeRatio = chessboardSizeRatios[(int)ChessboardSizeIndex.eight];
		}
		
		else
		{
			Debug.LogError("chessboardSize is illegal");
		}

		//计算理论得分以及当前得分占理论得分的百分比
Debug.Log (basicScore + " " + chessboardSizeRatio + " " + errorFreeReward);
		theoryScore = basicScore * chessboardSizeRatio + errorFreeReward;
		scoreRatio = curScore / theoryScore;

		//根据当前得分占理论得分的百分比计算星级
		if (scoreRatio >= 0.95) 
		{
			starNum = 5;
			hasHalfStar = false;
		}

		else if(scoreRatio >= 0.9)
		{
			starNum = 4;
			hasHalfStar = true;
		}

		else if(scoreRatio >= 0.85)
		{
			starNum = 4;
			hasHalfStar = false;
		}

		else if(scoreRatio >= 0.8)
		{
			starNum = 3;
			hasHalfStar = true;
		}

		else if(scoreRatio >= 0.75)
		{
			starNum = 3;
			hasHalfStar = false;
		}

		else if(scoreRatio >= 0.7)
		{
			starNum = 2;
			hasHalfStar = true;
		}

		else if(scoreRatio >= 0.65)
		{
			starNum = 2;
			hasHalfStar = false;
		}

		else if(scoreRatio >= 0.6)
		{
			starNum = 1;
			hasHalfStar = true;
		}

		else if(scoreRatio >= 0.55)
		{
			starNum = 1;
			hasHalfStar = false;
		}

		else
		{
			starNum = 0;
			hasHalfStar = true;
		}

Debug.Log ("理论得分： " + theoryScore);
Debug.Log ("百分比： " + scoreRatio);
Debug.Log ("星级： " + starNum);
Debug.Log ("是否存在半星： " + hasHalfStar);
	}

	/*
	 * 计算得分
	 * @isDuplicate 是否为重复摆法
	 */ 
	public void CalScore(bool isDuplicate)
	{
		int chessboardSize = ChessRoot.Instance.GetChessboardSize ();
		int replaceNum = UserBehaviorRecorder.Instance.GetReplaceNum ();
		int wrongStepsNum = UserBehaviorRecorder.Instance.GetWrongStepsNum ();
		float timeRewardLimit = 0.0f;
		float timeRewardAddtion = 0.0f;

		int basicScore = 0;  //基本得分
		int errorFreeReward = 0;  //无错奖励
		float timeReward = 0.0f;  //用时奖励
		float chessboardSizeRatio = 0.0f;  //棋盘规模系数

		//计算基本得分
		if (isDuplicate) 
		{
			basicScore = duplicateScore;
		}

		else
		{
			basicScore = finishScore;
		}

		//计算无错奖励
		if(wrongStepsNum == 0)
		{
			errorFreeReward = chessboardSize * 100;
		}

		//根据棋盘规模计算棋盘系数规模和用时奖励
		if (chessboardSize == 4) 
		{
			chessboardSizeRatio = chessboardSizeRatios[(int)ChessboardSizeIndex.four];
			timeRewardLimit = timeRewardLimits[(int)ChessboardSizeIndex.four];
			timeRewardAddtion = timeRewardAdditions[(int)ChessboardSizeIndex.four];
		}

		else if (chessboardSize == 5) 
		{
			chessboardSizeRatio = chessboardSizeRatios[(int)ChessboardSizeIndex.five];
			timeRewardLimit = timeRewardLimits[(int)ChessboardSizeIndex.five];
			timeRewardAddtion = timeRewardAdditions[(int)ChessboardSizeIndex.five];
		}

		else if (chessboardSize == 6) 
		{
			chessboardSizeRatio = chessboardSizeRatios[(int)ChessboardSizeIndex.six];
			timeRewardLimit = timeRewardLimits[(int)ChessboardSizeIndex.six];
			timeRewardAddtion = timeRewardAdditions[(int)ChessboardSizeIndex.six];
		}

		else if (chessboardSize == 7) 
		{
			chessboardSizeRatio = chessboardSizeRatios[(int)ChessboardSizeIndex.seven];
			timeRewardLimit = timeRewardLimits[(int)ChessboardSizeIndex.seven];
			timeRewardAddtion = timeRewardAdditions[(int)ChessboardSizeIndex.seven];
		}

		else if (chessboardSize == 8) 
		{
			chessboardSizeRatio = chessboardSizeRatios[(int)ChessboardSizeIndex.eight];
			timeRewardLimit = timeRewardLimits[(int)ChessboardSizeIndex.eight];
			timeRewardAddtion = timeRewardAdditions[(int)ChessboardSizeIndex.eight];
		}

		else
		{
			Debug.LogError("chessboardSize is illegal");
		}

		if(timer <= timeRewardLimit)
		{
			timeReward = Mathf.Lerp(timeRewardAddtion, 0, timer / timeRewardLimit);
		}

		//计算最终得分
		finalScore = (int)((basicScore * chessboardSizeRatio + errorFreeReward) * (1 + timeReward) * 
						(chessboardSize - replaceNum) / chessboardSize);

		//计算星级
		CalStarNum ();

Debug.Log ("基本得分： " + basicScore);
Debug.Log ("棋盘规模系数： " + chessboardSizeRatio);
Debug.Log ("无错奖励： " + errorFreeReward);
Debug.Log ("用时： " + timer);
Debug.Log ("用时奖励加成系数： " + timeReward);
Debug.Log ("预放棋子系数： " + (chessboardSize - replaceNum) + " / " + chessboardSize);
Debug.Log ("最终得分： " + finalScore);
	}

	public void StartTimer()
	{
		timer = 0.0f;
		isStartTimer = true;
	}
	
	public void StopTimer()
	{
		isStartTimer = false;
	}

	public int GetFinalScore()
	{
		return finalScore;
	}

	public int GetStarNum()
	{
		return starNum;
	}

	public bool GetHasHalfStar()
	{
		return hasHalfStar;
	}

}
