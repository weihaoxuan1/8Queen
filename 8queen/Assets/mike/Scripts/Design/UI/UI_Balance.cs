using UnityEngine;
using System.Collections;

public class UI_Balance : MonoBehaviour 
{
	private int curScore;
	private int finalScore;
	private bool isStartFresh = false;

	private UILabel label_score;
	private StarController starController;

	void OnEnable()
	{
		if (label_score == null) 
		{
			label_score = transform.FindChild ("Text_score").GetComponent<UILabel> ();
		}

		if (starController == null) 
		{
			starController = transform.FindChild("StarController").GetComponent<StarController>();
		}

		Refresh ();
	}

	void OnDisable()
	{
		starController.HideStar ();
	}

	void Update()
	{
		if (isStartFresh) 
		{
			label_score.text = curScore.ToString ();
			curScore += 30;

			if(curScore + 30 >= finalScore)
			{
				curScore = finalScore;
				label_score.text = curScore.ToString ();
				isStartFresh = false;
			}
		}
	}

	//各种刷新
	private void Refresh()
	{
		//刷新得分
		curScore = 0;
		finalScore = ScoreManager.Instance.GetFinalScore ();
		isStartFresh = true;

		//刷新星级评定
		starController.SetStarCount (ScoreManager.Instance.GetStarNum ());
		starController.SetIsHalf (ScoreManager.Instance.GetHasHalfStar ());
		starController.ShowStar ();
	}

}
