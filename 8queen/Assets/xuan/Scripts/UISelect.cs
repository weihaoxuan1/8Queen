using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UISelect : MonoBehaviour {

    bool[] ifUnlock;
    int[] TotalAnswers;
    public GameObject[] rank;

	private CameraMovement cameraMovement;

    private GameObject rank4;
    private GameObject rank5;
    private GameObject rank6;
    private GameObject rank7;
    private GameObject rank8;

    private GameObject lock4;
    private GameObject lock5;
    private GameObject lock6;
    private GameObject lock7;
    private GameObject lock8;

    private GameObject noFinishedAnswer;

	// Use this for initialization
	void Start () {
        ifUnlock = new bool[5];
        for(int i=0;i<5;i++)
        {
            ifUnlock[i] = false;
        }
        ifUnlock[0] = true;

        TotalAnswers = new int[5];
        TotalAnswers[0] = 2;
        TotalAnswers[1] = 10;
        TotalAnswers[2] = 4;
        TotalAnswers[3] = 40;
        TotalAnswers[4] = 92;

		cameraMovement = Camera.main.GetComponent<CameraMovement> ();

        rank4 = transform.FindChild("bg/4rank").gameObject;
        rank5 = transform.FindChild("bg/5rank").gameObject;
        rank6 = transform.FindChild("bg/6rank").gameObject;
        rank7 = transform.FindChild("bg/7rank").gameObject;
        rank8 = transform.FindChild("bg/8rank").gameObject;

        lock4 = rank4.transform.FindChild("Lock").gameObject;
        lock5 = rank5.transform.FindChild("Lock").gameObject;
        lock6 = rank6.transform.FindChild("Lock").gameObject;
        lock7 = rank7.transform.FindChild("Lock").gameObject;
        lock8 = rank8.transform.FindChild("Lock").gameObject;

        noFinishedAnswer = transform.FindChild("NoFinishedAnswer").gameObject;

        CheckPlayerAnswer();
        CheckLock();
        
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKeyUp(KeyCode.Mouse1))
        {
            MainDirector.Instance.ToBeginUI();
        }
	}

    void OnEnable()
    {
        if(ifUnlock == null)
        {
            return;
        }

        CheckPlayerAnswer();
        CheckLock();
    }

    void CheckLock()
    {
        for(int i =0;i<5;i++)
        {
            if (ifUnlock[i])
            {
                rank[i].transform.FindChild("Lock").gameObject.SetActive(false);
                rank[i].transform.FindChild("Picture").GetComponent<BoxCollider>().enabled = true;
            }
            else
                rank[i].transform.FindChild("Picture").GetComponent<BoxCollider>().enabled = false;
        }
    }

    private void CheckPlayerAnswer()
    {
        for(int i=0; i<5; i++)
        {
            List<ChessmanInfo[]> info = PlayerAnswerRecorder.Instance.GetPlayerAnswer(i+4);

            if(info != null)
            {
                if (i != 4) ifUnlock[i+1] = true;
                rank[i].transform.FindChild("LabelFinishedNum/FinishedNum").GetComponent<UILabel>().text = info.Count.ToString();
                if(info.Count == TotalAnswers[i])
                {
                    rank[i].transform.FindChild("Star").gameObject.SetActive(true);
                }
            }

            else
            {
                if (i != 4) ifUnlock[i + 1] = false;
            }
        }
    }

	public void OnStartChess_4()
	{
		StartChess (4);
        ReplacedNum num = rank4.GetComponent<ReplacedNum>();
		UserSelectRecorder.Instance.SetReplaceNum (num.GetReplacedNum());
        ChessmanInfo[] info = StandardAnswerRecorder.Instance.GetRandomChessmanInfos(4, num.GetReplacedNum());
        ChessRoot.Instance.SetUnremovableChessman(info);
	}

	public void OnStartChess_5()
	{
		StartChess (5);
        ReplacedNum num = rank5.GetComponent<ReplacedNum>();
		UserSelectRecorder.Instance.SetReplaceNum (num.GetReplacedNum());
        ChessmanInfo[] info = StandardAnswerRecorder.Instance.GetRandomChessmanInfos(5, num.GetReplacedNum());
        ChessRoot.Instance.SetUnremovableChessman(info);
	}

	public void OnStartChess_6()
	{
		StartChess (6);
        ReplacedNum num = rank6.GetComponent<ReplacedNum>();
		UserSelectRecorder.Instance.SetReplaceNum (num.GetReplacedNum());
        ChessmanInfo[] info = StandardAnswerRecorder.Instance.GetRandomChessmanInfos(6, num.GetReplacedNum());
        ChessRoot.Instance.SetUnremovableChessman(info);
	}

	public void OnStartChess_7()
	{
		StartChess (7);
        ReplacedNum num = rank7.GetComponent<ReplacedNum>();
		UserSelectRecorder.Instance.SetReplaceNum (num.GetReplacedNum());
        ChessmanInfo[] info = StandardAnswerRecorder.Instance.GetRandomChessmanInfos(7, num.GetReplacedNum());
        ChessRoot.Instance.SetUnremovableChessman(info);
	}

	public void OnStartChess_8()
	{
		StartChess (8);
        ReplacedNum num = rank8.GetComponent<ReplacedNum>();
		UserSelectRecorder.Instance.SetReplaceNum (num.GetReplacedNum());
        ChessmanInfo[] info = StandardAnswerRecorder.Instance.GetRandomChessmanInfos(8, num.GetReplacedNum());
        ChessRoot.Instance.SetUnremovableChessman(info);
	}

    public void OnShowPlayerAnswer_4()
    {
        ShowPlayerAnswer(4);
    }

    public void OnShowPlayerAnswer_5()
    {
        ShowPlayerAnswer(5);
    }

    public void OnShowPlayerAnswer_6()
    {
        ShowPlayerAnswer(6);
    }

    public void OnShowPlayerAnswer_7()
    {
        ShowPlayerAnswer(7);
    }

    public void OnShowPlayerAnswer_8()
    {
        ShowPlayerAnswer(8);
    }

    public void OnCurSizeAnswer()
    {
        int size = ChessRoot.Instance.GetChessboardSize();
        ShowPlayerAnswer(4);
    }

	/*
	 * 开始游戏
	 */ 
	private void StartChess(int chessboardSize)
	{
		ChessRoot.Instance.CreateChessboard(chessboardSize);
		ChessRoot.Instance.AllowSetChessman ();
		cameraMovement.RefreshCameraPos ();
        MainDirector.Instance.ToGamingUI();
	}

    /*
     * 展示玩家解出的答案
     */
    private void ShowPlayerAnswer(int chessboardSize)
    {
        List<ChessmanInfo[]> playerAnswer = PlayerAnswerRecorder.Instance.GetPlayerAnswer(chessboardSize);

        //玩家尚未解出答案
        if (playerAnswer == null)
        {
            Debug.Log("尚未解出" + chessboardSize + "*" + chessboardSize + "棋盘规模的答案");
            noFinishedAnswer.SetActive(false);
            noFinishedAnswer.SetActive(true);
            return;
        }

        ChessRoot.Instance.CreateChessboard(chessboardSize);
        ChessRoot.Instance.ForbidSetChessman();
        cameraMovement.RefreshCameraPos();
        MainDirector.Instance.ToPlayerAnswerUI();
    }

}
