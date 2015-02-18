using UnityEngine;
using System.Collections;

public class StarFade : MonoBehaviour {
	private UIWidget myWidget = null;

	private bool isStartFade = false;
	private bool isFadeOut = false;

	private float startAlpha = 1.0f;
	private float endAlpha = 0.0f;
	private float alphaTime = 0.5f;
	private float timer = 0.0f;
	
	// Use this for initialization
	void Start () 
	{
		myWidget = GetComponent<UIWidget>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (isStartFade) 
		{
			timer += Time.deltaTime;

			if(isFadeOut)
			{
				myWidget.alpha = Mathf.Lerp(startAlpha, endAlpha, timer / alphaTime);

				if(timer > alphaTime)
				{
					timer = 0;
					isFadeOut = false;
				}
			}

			else
			{
				myWidget.alpha = Mathf.Lerp(endAlpha, startAlpha, timer / alphaTime);

				if(timer > alphaTime)
				{
					timer = 0;
					isStartFade = false;
				}
			}
		}
	}
	
	public void StartToFade() 
	{
		isStartFade = true;
		isFadeOut = true;
	}

}
