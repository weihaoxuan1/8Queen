using UnityEngine;
using System.Collections;

using System;
using System.IO;

public class UserManager : MonoBehaviour 
{
	public static UserManager Instance;
	
	private string defaultUser;         //系统默认建立的初始用户名
	private string defaultUser_delete;  //玩家删除所有用户后系统默认建立的用户名
	private string userDirectory;       //存放所有用户答案信息的文件夹名称
	private string curUserFile;         //存放当前用户名的文件名称

	UserManager()
	{
		Instance = this;
	}

	void Awake()
	{
		defaultUser = "Master";
		defaultUser_delete = "Defalut";
		userDirectory = "UserInfo";
		curUserFile = "CurUser";

		CreateUserDirectory ();
		ConfireCurUser (false);
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
	private void ConfireCurUser(bool isDelete)
	{
		string path = Application.dataPath + "//" + userDirectory;
		string[] directories = Directory.GetDirectories (path);

		//若不存在任何用户,创建一个初始用户
		if (directories.Length == 0) 
		{
			if(isDelete)
			{
				CreateUser(defaultUser_delete);
			}

			else
			{
				CreateUser(defaultUser);
			}
		}

		//若存在至少一个用户
		else
		{
			string curUser = GetCurUser();

			//文件中没有存放当前用户名,设置当前用户为用户列表中的第一位用户
			if(curUser == null)
			{
				SetCurUser(new DirectoryInfo(path + "//" + directories[0]).Name);
			}

			else
			{
				//检查用户列表中是否含有文件中储放的当前用户名
				for(int i=0; i<directories.Length; i++)
				{
					DirectoryInfo info = new DirectoryInfo(path + "//" + directories[i]);
					
					if(curUser == info.Name)
					{
						return;
					}
				}
				
				//若用户列表中没有文件中储放的当前用户名,设置当前用户为用户列表中的第一位用户
				SetCurUser(new DirectoryInfo(path + "//" + directories[0]).Name);
			}

		}
	}

	/*
	 * 创建新的用户
	 */ 
	public void CreateUser(string userName)
	{
		string path = Application.dataPath + "//" + userDirectory + "//" + userName;
		Directory.CreateDirectory(path);
		SetCurUser (userName);
	}

	/*
	 * 删除一个用户
	 */ 
	public void DeleteUser(string userName)
	{
		string path = Application.dataPath + "//" + userDirectory + "//" + userName;

		if (Directory.Exists (path)) 
		{
			Directory.Delete(path, true);
		}

		ConfireCurUser (true);
	}

	/*
	 * 更改用户名
	 */ 
	public void RenameUser(string userName, string newName)
	{
		string path = Application.dataPath + "//" + userDirectory + "//" + userName;
		string newPath = Application.dataPath + "//" + userDirectory + "//" + newName;

		if (Directory.Exists (path) && !Directory.Exists (newPath)) 
		{
			Directory.Move(path, newPath);
			SetCurUser (newName);
		}
	}

	/*
	 * 检查用户名是否存在
	 */ 
	public bool CheckUserNameExist(string userName)
	{
		string path = Application.dataPath + "//" + userDirectory + "//" + userName;
		
		if (Directory.Exists (path)) 
		{
			return true;
		}

		else
		{
			return false;
		}
	}
	
	/*
	 * 设置当前用户名
	 */ 
	public void SetCurUser(string username)
	{
		StreamWriter sw;
		FileInfo fileInfo = new FileInfo (Application.dataPath + "//" + UserManager.Instance.GetUserDirectory() + "//" +
		                                  curUserFile);
		
		//若存在记录文件,将记录文件删除后再重新创建
		if (fileInfo.Exists) 
		{
			fileInfo.Delete();
		}
		
		sw = fileInfo.CreateText ();
		
		sw.WriteLine (username);
		sw.Close ();
		sw.Dispose ();
	}

	/*
	 * 返回当前用户名
	 */ 
	public string GetCurUser()
	{
		string line;
		string path = Application.dataPath + "//" + UserManager.Instance.GetUserDirectory () + "//" + curUserFile;

		StreamReader sr = null;
		
		if (File.Exists (path)) 
		{
			sr = File.OpenText (path);
		}
		
		else
		{
			return null;
		}
		
		line = sr.ReadLine ();
		
		sr.Close();
		sr.Dispose();

		return line;
	}

	/*
	 * 返回存放所有用户答案信息的文件夹名称
	 */ 
	public string GetUserDirectory()
	{
		return userDirectory;
	}

}
