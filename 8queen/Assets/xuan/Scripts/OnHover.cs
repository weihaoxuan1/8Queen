using UnityEngine;
using System.Collections;

public class OnHover : MonoBehaviour
{

    UIButton button;
    public GameObject showWhileHover;

	private bool isPlayedHoverSound = false;  //是否播放过鼠标悬停的音效

	// Use this for initialization
	void Start () {
        button = gameObject.GetComponent<UIButton>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(button.state == UIButtonColor.State.Hover)
        {
			if(showWhileHover != null)
			{
				showWhileHover.SetActive(true);
				//onBackToMainHover.transform.localPosition = Input.mousePosition;
			}

			//防止鼠标悬停时一直播放声音
			if(!isPlayedHoverSound)
			{
				SoundManager.Instance.PlayHoverButtonSound();
				isPlayedHoverSound = true;
			}
        }
        else
        {
			if(showWhileHover != null)
			{
				showWhileHover.SetActive(false);
			}
            
			isPlayedHoverSound = false;
        }
	}
}
