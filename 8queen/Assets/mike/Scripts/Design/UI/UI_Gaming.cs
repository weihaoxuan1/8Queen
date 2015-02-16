using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Gaming : MonoBehaviour 
{
	private CameraMovement cameraMovement;

    private GameObject finishUI;
    private GameObject duplicateUI;

    Vector3 mousePos;
    float enableMoveLimit = 2;

	void Start()
	{
		cameraMovement = Camera.main.GetComponent<CameraMovement> ();
        finishUI = transform.FindChild("FinishUI").gameObject;
        duplicateUI = transform.FindChild("DuplicateUI").gameObject;
	}

	void Update()
	{
		RotationControl ();
	}

    void OnDisable()
    {
        transform.FindChild("Note").gameObject.SetActive(true);
    }
	/*
	 * 旋转棋盘和相机
	 */ 
	private void RotationControl()
	{
		if (Input.GetKey (KeyCode.UpArrow)) 
		{
			cameraMovement.CameraMove(true);
		}

		if (Input.GetKey (KeyCode.DownArrow)) 
		{
			cameraMovement.CameraMove(false);
		}

		if (Input.GetKey (KeyCode.LeftArrow)) 
		{
			ChessRoot.Instance.RotateChessboard(false);
		}

		if (Input.GetKey (KeyCode.RightArrow)) 
		{
			ChessRoot.Instance.RotateChessboard(true);
		}
        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            mousePos = Input.mousePosition;
        }
        if(Input.GetKey(KeyCode.Mouse1))
        {
            Vector3 offset = Input.mousePosition;
            float deltaY = offset.y - mousePos.y;
            if (deltaY > enableMoveLimit)
            {
                cameraMovement.CameraMove(true);
            }
            else if (deltaY < -enableMoveLimit)
            {
                cameraMovement.CameraMove(false);
            }
            float deltaX = offset.x - mousePos.x;
            if (deltaX > enableMoveLimit)
            {
                ChessRoot.Instance.RotateChessboard(false);
            }
            else if (deltaX < -enableMoveLimit)
            {
                ChessRoot.Instance.RotateChessboard(true);
            }
            mousePos = offset;
        }
	}

    public void OnBack()
    {
        finishUI.SetActive(false);
        duplicateUI.SetActive(false);
        ChessRoot.Instance.ClearAllChessmans(true);
        ChessRoot.Instance.ClearChessboard();
        MainDirector.Instance.CloseFinishUI();
        MainDirector.Instance.ToSelectChessboardUI();
    }

    public void ClearAllChessmans()
    {
        ChessRoot.Instance.ClearAllChessmans(false);
        MainDirector.Instance.CloseFinishUI();
        ChessRoot.Instance.AllowSetChessman();
    }

    public void OnContinue()
    {
        MainDirector.Instance.CloseDuplicateUI();
        ChessRoot.Instance.AllowSetChessman();
    }

    private void ShowPlayerAnswer(int chessboardSize)
    {
        List<ChessmanInfo[]> playerAnswer = PlayerAnswerRecorder.Instance.GetPlayerAnswer(chessboardSize);

        //玩家尚未解出答案
        if (playerAnswer == null)
        {
            //Debug.Log("尚未解出" + chessboardSize + "*" + chessboardSize + "棋盘规模的答案");
            //noFinishedAnswer.SetActive(true);
            return;
        }
        ChessRoot.Instance.CreateChessboard(chessboardSize);
        ChessRoot.Instance.ForbidSetChessman();
        cameraMovement.RefreshCameraPos();
        
        MainDirector.Instance.ToPlayerAnswerUI();
    }

    public void OnShowPlayerAnswer()
    {
        finishUI.SetActive(false);
        duplicateUI.SetActive(false);
        ClearAllChessmans();
        ShowPlayerAnswer(ChessRoot.Instance.GetChessboardSize());
    }

	/*
	 * 重置为玩家预先放置的棋子
	 */ 
	public void ResetUnremovableChessmans()
	{
		int chessboardSize = ChessRoot.Instance.GetChessboardSize ();
		int replaceNum = UserBehaviorRecorder.Instance.GetReplaceNum ();
		ChessRoot.Instance.ClearAllChessmans (true);
		ChessmanInfo[] info = StandardAnswerRecorder.Instance.GetRandomChessmanInfos(chessboardSize, replaceNum);
		ChessRoot.Instance.SetUnremovableChessman(info);
	}

}
