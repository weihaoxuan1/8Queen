using UnityEngine;
using System.Collections;

public class StarScale : MonoBehaviour {
	private bool isStartScale = false;

	private float startScale = 5.0f;
	private float endScale = 1.0f;
	private float scaleTime = 0.5f;
	private float timer = 0.0f;

	void Update () 
	{
		if (isStartScale) 
		{
			timer += Time.deltaTime;
			transform.localScale = Mathf.Lerp(startScale, endScale, timer / scaleTime) * new Vector3(1.0f, 1.0f, 1.0f);

			if(timer > scaleTime)
			{
				timer = 0;
				isStartScale = false;
			}
		}
	}
	
	public void StartToScale() 
	{
		isStartScale = true;
	}

}