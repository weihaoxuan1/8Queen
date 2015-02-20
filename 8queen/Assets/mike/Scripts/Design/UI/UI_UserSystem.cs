using UnityEngine;
using System.Collections;

public class UI_UserSystem : MonoBehaviour 
{
	private UILabel label_userName;

	void OnEnable()
	{
		if (label_userName == null) 
		{
			label_userName = transform.FindChild ("Text_userName").GetComponent<UILabel>();
		}

		Reset ();
	}

	private void Reset()
	{
		//更新当前用户名
		string curUser = UserManager.Instance.GetCurUser();
		label_userName.text = curUser;
	}


}
