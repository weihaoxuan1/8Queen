using UnityEngine;
using System.Collections;

public class UIBegin : MonoBehaviour
{
	private GameObject welcome;
	private UILabel label_welcome;

	void OnEnable()
	{
		if (welcome == null) 
		{
			welcome = transform.FindChild("Welcome").gameObject;
		}

		if (label_welcome == null) 
		{
			label_welcome = welcome.GetComponentInChildren<UILabel> ();
		}

		ShowWelcome ();
	}

	/*
	 * 显示欢迎信息
	 */ 
	private void ShowWelcome()
	{
		string curUser = UserManager.Instance.GetCurUser ();
		welcome.SetActive (false);
		label_welcome.text = "欢迎游戏, " + curUser + " !";
		welcome.SetActive (true);
	}

    public void OnStart()
    {
        MainDirector.Instance.ToSelectChessboardUI();
    }

    public void OnInstruction()
    {
        MainDirector.Instance.ToInstuctionUI();
    }

    public void OnUser()
    {
		MainDirector.Instance.ToUserSystemUI ();
    }

    public void OnQuit()
    {
        Application.Quit();
    }

}
