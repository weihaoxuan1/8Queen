using UnityEngine;
using System.Collections;

public class TwinkleEffect : MonoBehaviour 
{
	private int twinkleTimes;     //棋子闪烁的次数
	private int curTwinkleTimes;
	private bool isStartTwinkle;  //是否开始闪烁棋子
	private float twinkleTime;    //棋子闪烁一次的时间
	private float twinkleTimer;

    Color oldColor;

	void Awake()
	{
		twinkleTimes = 2;
		curTwinkleTimes = 0;
		isStartTwinkle = false;
		twinkleTime = 0.5f;
		twinkleTimer = 0;
	}
	
	void Update()
	{
		if (isStartTwinkle) 
		{
			Twinkle();
		}
	}
	
	/*
	 * 棋子闪烁
	 */ 
	private void Twinkle()
	{
		twinkleTimer += Time.deltaTime;
		
		if (twinkleTimer < twinkleTime / 2) 
		{
			renderer.material.color = Color.Lerp (Color.white, Color.red,
			                                      twinkleTimer / (twinkleTime / 2));
		}
		
		if (twinkleTimer >= twinkleTime / 2) 
		{
			renderer.material.color = Color.Lerp (Color.red, Color.white,
			                                      (twinkleTimer - twinkleTime / 2) / (twinkleTime / 2));
		}
		
		if (twinkleTimer > twinkleTime) 
		{
			twinkleTimer = 0;
			curTwinkleTimes++;
			
			if(curTwinkleTimes >= twinkleTimes)
			{
				curTwinkleTimes = 0;
                BackToOriginColor();
				isStartTwinkle = false;
			}
		}
	}
	
	/*
	 * 开始闪烁棋子
	 */ 
	public void StartTwinkle()
	{
        oldColor = renderer.material.color;
		isStartTwinkle = true;
	}
	
	/*
	 * 返回棋子闪烁的总时间
	 */ 
	public float GetTotalTwinkleTime()
	{
		return twinkleTime * twinkleTimes;
	}

    public void BackToOriginColor()
    {
        renderer.material.color = oldColor;
    }

}
