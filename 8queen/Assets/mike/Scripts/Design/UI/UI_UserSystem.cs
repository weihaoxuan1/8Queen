using UnityEngine;
using System.Collections;

public class UI_UserSystem : MonoBehaviour 
{
	private UILabel label_userName;
	private GameObject userAchievement;

	void OnEnable()
	{
		if (label_userName == null) 
		{
			label_userName = transform.FindChild ("Text_userName").GetComponent<UILabel>();
		}

		if (userAchievement == null) 
		{
			userAchievement = transform.FindChild("UserAchievement").gameObject;
		}

		Reset ();
	}

	void OnDisable()
	{
		for (int chessboardSize=4; chessboardSize<=8; chessboardSize++)
		{
			Transform achievement = userAchievement.transform.FindChild (chessboardSize + "*" + chessboardSize);
			StarController starController = achievement.FindChild ("StarController").GetComponent<StarController> ();
			starController.HideStar ();
		}
	}

	void Update () 
	{
		if (Input.GetKeyUp(KeyCode.Mouse1))
		{
			MainDirector.Instance.ToBeginUI();
		}
	}

	/*
	 * 各种更新
	 */ 
	private void Reset()
	{
		//更新当前用户名
		string curUser = UserManager.Instance.GetCurUser();
		label_userName.text = curUser;

		for (int chessboardSize=4; chessboardSize<=8; chessboardSize++) 
		{
			ResetUserAchievement(chessboardSize);
		}
	}

	/*
	 * 更新用户的成就信息
	 */ 
	private void ResetUserAchievement(int chessboardSize)
	{
		Transform achievement = userAchievement.transform.FindChild (chessboardSize + "*" + chessboardSize);

		UILabel label_bestScore = achievement.FindChild ("Text_bestScore").GetComponent<UILabel> ();
		StarController starController = achievement.FindChild ("StarController").GetComponent<StarController> ();

		label_bestScore.text = ScoreManager.Instance.GetBestScore (chessboardSize).ToString();
		starController.HideStar ();
		starController.SetStarCount (ScoreManager.Instance.GetBestStarNum(chessboardSize));
		starController.SetIsHalf (ScoreManager.Instance.GetHasHalfStar(chessboardSize));
		starController.ShowStar ();
	}

}
