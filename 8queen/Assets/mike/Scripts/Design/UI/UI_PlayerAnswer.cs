using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_PlayerAnswer : MonoBehaviour 
{
	private int curAnswerIndex;
	private int chessboardSize;
    private UILabel curTotal;
	private List<ChessmanInfo[]> playerAnswer;
	private CameraMovement cameraMovement;
    Vector3 mousePos;
    float enableMoveLimit = 2;
    
	void Start()
	{
		cameraMovement = Camera.main.GetComponent<CameraMovement> ();
        curTotal = transform.FindChild("LabelCurTotal").gameObject.GetComponent<UILabel>();
        OnEnable();
	}

	void Update()
	{
		RotationControl ();
	}

	void OnEnable()
	{
		playerAnswer = PlayerAnswerRecorder.Instance.GetPlayerAnswer (ChessRoot.Instance.GetChessboardSize());

		if (playerAnswer != null) 
		{
			ChessRoot.Instance.SetUnremovableChessman (playerAnswer.ToArray()[0]);
			curAnswerIndex = 1;
		}
        FlashLabel();
	}

	
	/*
	 * 查看上一个答案
	 */ 
	public void PreviousAnswer() {
		if(curAnswerIndex == 1) {
			curAnswerIndex = playerAnswer.Count;
		}
		
		else {
			curAnswerIndex--;
		}
		
		ShowOneAnswer (curAnswerIndex);
	}
	
	/*
	 * 查看下一个答案
	 */
    public void NextAnswer()
    {
		if(curAnswerIndex == playerAnswer.Count) {
			curAnswerIndex = 1;
		}
		
		else {
			curAnswerIndex++;
		}
		
		ShowOneAnswer (curAnswerIndex);
	}

	/*
	 * 展示一种答案
	 */ 
	private void ShowOneAnswer(int index)
	{
        FlashLabel();
		ChessRoot.Instance.ClearAllChessmans (true);
		ChessRoot.Instance.SetUnremovableChessman (playerAnswer.ToArray()[index - 1]);
	}

	/*
	 * 退出展示玩家答案界面
	 */
    public void BackToMain()
	{
		ChessRoot.Instance.ClearAllChessmans (true);
		ChessRoot.Instance.ClearChessboard ();

		MainDirector.Instance.ToSelectChessboardUI ();
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

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            mousePos = Input.mousePosition;
        }

        if (Input.GetKey(KeyCode.Mouse1))
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

    private void FlashLabel()
    {
        if(curTotal) curTotal.text = curAnswerIndex.ToString() + "/" + playerAnswer.Count;
    }
}
