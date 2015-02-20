using UnityEngine;
using System.Collections;

using System;
using System.IO;

public class UserManager : MonoBehaviour 
{
	public static UserManager Instance;

	private string curUser;        //当前用户名
	private string defaultUser;    //系统默认建立的初始用户名
	private string userDirectory;  //存放所有用户答案信息的文件夹名称

	UserManager()
	{
		Instance = this;
	}

	void Awake()
	{
		defaultUser = "Master";
		userDirectory = "UserInfo";

		CreateUserDirectory ();
		ConfireCurUser ();
	}

	/*
	 * 建立存放所有用户答案信息的文件夹
	 */ 
	private void CreateUserDirectory()
	{
		string path = Application.dataPath + "//" + userDirectory;

		if (!Directory.Exists (path)) 
		{
			Directory.CreateDirectory(path);
		}
	}

	/*
	 * 确认当前用户
	 */ 
	private void ConfireCurUser()
	{
		string path = Application.dataPath + "//" + userDirectory;
		string[] directories = Directory.GetDirectories (path);

		//若不存在任何用户,创建一个初始用户
		if (directories.Length == 0) 
		{
			CreateUser(defaultUser);
		}

		//若存在至少一个用户,选择最后一次登录的用户
		else
		{
			int lastAccessIndex = 0;

			//找出最后一次访问的文件夹
			for(int i=1; i<directories.Length; i++)
			{
				DateTime curTime = Directory.GetLastAccessTime(path + "//" + directories[i]);
				DateTime lastAccessTime = Directory.GetLastAccessTime(path + "//" + directories[lastAccessIndex]);

				if(curTime.CompareTo(lastAccessTime) == 1)
				{
					lastAccessIndex = i;
				}
			}
		
			DirectoryInfo info = new DirectoryInfo(path + "//" + directories[lastAccessIndex]);
			curUser = info.Name;
		}
	}

	/*
	 * 创建新的用户
	 */ 
	public void CreateUser(string userName)
	{
		string path = Application.dataPath + "//" + userDirectory + "//" + userName;
		
		if (!Directory.Exists (path)) 
		{
			Directory.CreateDirectory(path);
		}

		curUser = userName;
	}

	/*
	 * 返回当前用户名
	 */ 
	public string GetCurUser()
	{
		return curUser;
	}

	/*
	 * 返回存放所有用户答案信息的文件夹名称
	 */ 
	public string GetUserDirectory()
	{
		return userDirectory;
	}

}
