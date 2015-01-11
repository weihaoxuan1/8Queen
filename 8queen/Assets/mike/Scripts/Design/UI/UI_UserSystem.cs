using UnityEngine;
using System.Collections;

public class UI_UserSystem : MonoBehaviour 
{
	private UILabel label_curUser;

	void Enable()
	{
		if (label_curUser != null) 
		{
			UpdateCurUser();
		}
	}

	void Start()
	{
		label_curUser = transform.FindChild ("Label_curUser").GetComponent<UILabel>();

		UpdateCurUser ();
	}

	/*
	 * 更新当前用户名的显示
	 */ 
	private void UpdateCurUser()
	{
		string curUser = UserManager.Instance.GetCurUser();
		label_curUser.text = "Welcome Back, " + curUser + "!";
	}

}
