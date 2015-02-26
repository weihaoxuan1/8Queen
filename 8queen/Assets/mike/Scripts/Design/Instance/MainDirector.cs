using UnityEngine;
using System.Collections;

public class MainDirector : MonoBehaviour 
{
	public static MainDirector Instance;

    private GameObject beginUI;
    private GameObject instructionUI;
	private GameObject selectChessboardUI;
	private GameObject gamingUI;
    private GameObject finishUI;
    private GameObject duplicateUI;
    private GameObject playerAnswerUI;
	private GameObject balanceUI;
	private GameObject userSystemUI;
	private GameObject userSettingUI;

	MainDirector()
	{
		Instance = this;
	}

	void Start()
	{
        GameObject manage = GameObject.Find("UIManage");

        beginUI = manage.transform.FindChild("BeginUI").gameObject;
        instructionUI = manage.transform.FindChild("InstructionUI").gameObject;
		selectChessboardUI = manage.transform.FindChild ("SelectUI").gameObject;
        gamingUI = manage.transform.FindChild("GamingUI").gameObject;
		playerAnswerUI = manage.transform.FindChild ("PlayerAnswerUI").gameObject;
		balanceUI = manage.transform.FindChild ("BalanceUI").gameObject;
		userSystemUI = manage.transform.FindChild ("UserSystemUI").gameObject;
		userSettingUI = manage.transform.FindChild ("UserSettingUI").gameObject;
        finishUI = gamingUI.transform.FindChild("FinishUI").gameObject;
        duplicateUI = gamingUI.transform.FindChild("DuplicateUI").gameObject;

        ToBeginUI();
	}

	private void CloseAllUI()
	{
        beginUI.SetActive(false);
        instructionUI.SetActive(false);
		selectChessboardUI.SetActive (false);
		gamingUI.SetActive (false);
		playerAnswerUI.SetActive (false);
		balanceUI.SetActive (false);
		userSystemUI.SetActive (false);
		userSettingUI.SetActive (false);
	}

    public void ToBeginUI()
    {
        CloseAllUI();
        beginUI.SetActive(true);
    }

    public void ToInstuctionUI()
    {
        CloseAllUI();
        instructionUI.SetActive(true);
    }

	public void ToSelectChessboardUI()
	{
		CloseAllUI ();
		selectChessboardUI.SetActive (true);
	}

	public void ToGamingUI()
	{
		CloseAllUI ();
		gamingUI.SetActive (true);
	}

	public void ToPlayerAnswerUI()
	{
		CloseAllUI ();
		playerAnswerUI.SetActive (true);
	}

	public void ToUserSystemUI()
	{
		CloseAllUI ();
		userSystemUI.SetActive (true);
	}

	public void ToUserSettingUI()
	{
		CloseAllUI ();
		userSettingUI.SetActive (true);
	}

	public void ShowBalanceUI()
	{
		balanceUI.SetActive (true);
	}

    public void ShowFinishUI()
    {
        finishUI.SetActive(true);
    }

    public void ShowDuplicateUI()
    {
        duplicateUI.SetActive(true);
    }

    public void CloseFinishUI()
    {
        finishUI.SetActive(false);
    }

    public void CloseDuplicateUI()
    {
        duplicateUI.SetActive(false);
    }

	//延迟调用ShowFinishUI()方法,是为了解决放置最后一个棋子的同时按钮被点击的问题
	public void ShowFinishUI_delay(float delay)
	{
		Invoke ("ShowFinishUI", delay);
	}

	//延迟调用ShowDuplicateUI()方法，目的同上
	public void ShowDuplicateUI_delay(float delay)
	{
		Invoke ("ShowDuplicateUI", delay);
	}
	
}
