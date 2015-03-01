using UnityEngine;
using System.Collections;

public class ScoreRollEffect : MonoBehaviour 
{
	public int chessboardSize;

	private int curScore = 0;
	private int bestScore = 0;
	private int rollSpeed = 50;
	private bool isStartRoll = false;
	private UILabel label;

	void OnEnable()
	{
		if (label == null) 
		{
			label = GetComponent<UILabel>();
		}

		curScore = 0;
		bestScore = ScoreManager.Instance.GetBestScore (chessboardSize);
		isStartRoll = true;
	}

	void Update()
	{
		if (isStartRoll) 
		{
			ScoreRoll ();
		}
	}

	private void ScoreRoll()
	{
		label.text = curScore.ToString ();
		curScore += rollSpeed;
		
		if(curScore >= bestScore)
		{
			curScore = bestScore;
			label.text = curScore.ToString ();
			isStartRoll = false;
		}
	}

}
