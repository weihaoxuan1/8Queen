using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System;
using System.IO;

public class UI_UserSetting : MonoBehaviour 
{
	private int maxUserNum = 4;  //最大用户数量

	private GameObject userList = null;
	private GameObject createUserButton = null;

	private GameObject userChoice = null;
	private GameObject rename = null;
	private GameObject createUser = null;
	private GameObject deleteUser = null;

	private GameObject[] userCheckboxs = null;
	
	void OnEnable()
	{
		Init ();
		Reset ();
	}

	void Start()
	{
		userChoice = transform.FindChild ("UserChoice").gameObject;
		rename = transform.FindChild ("Rename").gameObject;
		createUser = transform.FindChild ("CreateUser").gameObject;
		deleteUser = transform.FindChild ("DeleteUser").gameObject;
	}

	private void Init()
	{
		if (userList == null) 
		{
			userList = transform.FindChild("UserChoice/UserList").gameObject;
		}
		
		if (createUserButton == null) 
		{
			createUserButton = userList.transform.FindChild("Button_createUser").gameObject;
		}

		if (userCheckboxs == null) 
		{
			userCheckboxs = new GameObject[maxUserNum];

			for(int i=0; i<maxUserNum; i++)
			{
				GameObject user = userList.transform.FindChild("Checkbox_user" + i).gameObject;
				userCheckboxs[i] = user;
			}
		}
	}

	/*
	 * 各种更新
	 */ 
	private void Reset()
	{
		string path = Application.dataPath + "//" + UserManager.Instance.GetUserDirectory();
		string[] directories = Directory.GetDirectories (path);

		//更新用户列表
		for (int i=0; i<maxUserNum; i++) 
		{
			GameObject user = userCheckboxs[i];
			UIToggle toggle = user.GetComponent<UIToggle>();

			//UILabel label = user.GetComponentInChildren<UILabel>();
			UILabel label = user.transform.FindChild("Label").GetComponent<UILabel>();
			
			if(i < directories.Length)
			{
				DirectoryInfo info = new DirectoryInfo(directories[i]);

				if(info.Name == UserManager.Instance.GetCurUser())
				{
					toggle.value = true;
				}

				else
				{
					toggle.value = false;
				}

				label.text = info.Name;
				user.SetActive(true);
			}
			
			else
			{
				toggle.value = false;
				user.SetActive(false);
			}
		}
		
		//更新创建用户按钮的显示
		if (directories.Length < maxUserNum) 
		{
			createUserButton.transform.position = userCheckboxs[directories.Length].transform.position + 
				new Vector3(0.2f, 0, 0);
			createUserButton.SetActive(true);
		}
		
		else
		{
			createUserButton.SetActive(false);
		}
	}

	private void CloseAll()
	{
		userChoice.SetActive (false);
		rename.SetActive (false);
		createUser.SetActive (false);
		deleteUser.SetActive (false);
	}

	public void ToUserChoice()
	{
		CloseAll ();
		userChoice.SetActive (true);
		Reset ();
	}

	public void ToRename()
	{
		CloseAll ();
		rename.SetActive (true);

		GameObject warning_usernameNull = rename.transform.FindChild("Warning_usernameNull").gameObject;
		GameObject warning_usernameRepeat = rename.transform.FindChild("Warning_usernameRepeat").gameObject;
		warning_usernameNull.SetActive(false);
		warning_usernameRepeat.SetActive(false);

		UILabel label = rename.transform.FindChild ("Input_rename").GetComponentInChildren<UILabel> ();
		label.text = UserBehaviorRecorder.Instance.GetUserChoice ();
	}

	public void ToCreateUser()
	{
		CloseAll ();
		createUser.SetActive (true);

		GameObject warning_usernameNull = createUser.transform.FindChild("Warning_usernameNull").gameObject;
		GameObject warning_usernameRepeat = createUser.transform.FindChild("Warning_usernameRepeat").gameObject;
		warning_usernameNull.SetActive(false);
		warning_usernameRepeat.SetActive(false);
	}

	public void ToDeleteUser()
	{
		CloseAll ();
		deleteUser.SetActive (true);
	}

	/*
	 * 记录玩家在用户设置中选择的用户
	 */ 
	public void RecordUserChoice()
	{
		for (int i=0; i<userCheckboxs.Length; i++) 
		{
			UIToggle toggle = userCheckboxs[i].GetComponent<UIToggle>();

			if(toggle.value)
			{
				//UILabel label = userCheckboxs[i].GetComponentInChildren<UILabel>();
				UILabel label = userCheckboxs[i].transform.FindChild("Label").GetComponent<UILabel>();

				UserBehaviorRecorder.Instance.SetUserChoice(label.text);
			}
		}
	}

	/*
	 * 切换当前用户
	 */ 
	public void SwitchCurUser()
	{
		for (int i=0; i<userCheckboxs.Length; i++) 
		{
			UIToggle toggle = userCheckboxs[i].GetComponent<UIToggle>();
			
			if(toggle.value)
			{
				UILabel label = userCheckboxs[i].transform.FindChild("Label").GetComponent<UILabel>();
				UserManager.Instance.SetCurUser(label.text);
				return;
			}
		}
	}

	/*
	 * 确认创建新用户
	 */ 
	public void OnConfirm_createUser()
	{
		string username = createUser.transform.FindChild ("Input_createUser").GetComponentInChildren<UILabel> ().text;

		//检查输入的用户名是否为空
		if (username == "") 
		{
			GameObject warning_usernameNull = createUser.transform.FindChild("Warning_usernameNull").gameObject;
			GameObject warning_usernameRepeat = createUser.transform.FindChild("Warning_usernameRepeat").gameObject;
			warning_usernameNull.SetActive(false);
			warning_usernameRepeat.SetActive(false);
			warning_usernameNull.SetActive(true);
			return;
		}

		//检查输入的用户名是否已经存在
		if (UserManager.Instance.CheckUserNameExist (username)) 
		{
			GameObject warning_usernameRepeat = createUser.transform.FindChild("Warning_usernameRepeat").gameObject;
			GameObject warning_usernameNull = createUser.transform.FindChild("Warning_usernameNull").gameObject;
			warning_usernameNull.SetActive(false);
			warning_usernameRepeat.SetActive(false);
			warning_usernameRepeat.SetActive(true);
			return;
		}

		UserManager.Instance.CreateUser (username);
		ToUserChoice ();
	}

	/*
	 * 确认重命名用户
	 */ 
	public void OnConfirm_rename()
	{
		string username = rename.transform.FindChild ("Input_rename").GetComponentInChildren<UILabel> ().text;
		
		//检查输入的用户名是否为空
		if (username == "") 
		{
			GameObject warning_usernameNull = rename.transform.FindChild("Warning_usernameNull").gameObject;
			GameObject warning_usernameRepeat = rename.transform.FindChild("Warning_usernameRepeat").gameObject;
			warning_usernameNull.SetActive(false);
			warning_usernameRepeat.SetActive(false);
			warning_usernameNull.SetActive(true);
			return;
		}
		
		//检查输入的用户名是否已经存在
		if (UserManager.Instance.CheckUserNameExist (username)) 
		{
			GameObject warning_usernameRepeat = rename.transform.FindChild("Warning_usernameRepeat").gameObject;
			GameObject warning_usernameNull = rename.transform.FindChild("Warning_usernameNull").gameObject;
			warning_usernameNull.SetActive(false);
			warning_usernameRepeat.SetActive(false);
			warning_usernameRepeat.SetActive(true);
			return;
		}
		
		UserManager.Instance.RenameUser (UserBehaviorRecorder.Instance.GetUserChoice(), username);
		ToUserChoice ();
	}

	/*
	 * 确认删除用户
	 */ 
	public void OnConfirm_deleteUser()
	{
		UserManager.Instance.DeleteUser (UserBehaviorRecorder.Instance.GetUserChoice());
		ToUserChoice ();
	}

}

