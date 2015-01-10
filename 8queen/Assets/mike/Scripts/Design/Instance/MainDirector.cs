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
        finishUI = gamingUI.transform.FindChild("FinishUI").gameObject;
        duplicateUI = gamingUI.transform.FindChild("DuplicateUI").gameObject;
        //ToBeginUI();
	}

	private void CloseAllUI()
	{
        beginUI.SetActive(false);
        instructionUI.SetActive(false);
		selectChessboardUI.SetActive (false);
		gamingUI.SetActive (false);
		playerAnswerUI.SetActive (false);
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
}
