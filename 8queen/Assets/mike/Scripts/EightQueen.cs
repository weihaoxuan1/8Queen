using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class EightQueen : MonoBehaviour {
	public GameObject chessmanPrefab;
	public GameObject blackCubePrefab;
	public GameObject whiteCubePrefab;
	public GameObject mWinObj;  //胜利标志
	
	private int cubeSize;
	private int chessboardSize;  //棋盘规格
	private int curAnswerIndex;
	private bool isWin;
	private bool isShowAnswer;
	private bool isFindAnswerOver;  //是否已经解出答案
	private float chessmanHeight;
	private float rotateSpeed;  //棋盘的旋转速度（每秒转多少度）
	private float cameraMoveSpeed;  //相机移动的速度
	private float cameraLookAtSpeed;  //相机面向棋盘的速度
	private string sizeInput;    //用户输入的棋盘规格
	private Vector3 cameraAbovePos;  //相机俯视棋盘的位置
	private Vector3 cameraFrontPos;  //相机平视棋盘的位置
	private Vector3 cameraInitPos;  //相机的初始位置
	private GameObject mChessboard;
	private GameObject chessRoot;  //棋盘和棋子的父级物体
	private List<ChessmanInfo> mChessmanInfos;  //棋盘上的棋子坐标信息
	private List<GameObject> answerChessmans;  //用于展示答案的棋子
	private Dictionary<int, List<ChessmanInfo[]>> answerChessmanInfos;  //不同棋盘规模的棋子答案信息
	private Dictionary<int, List<ChessmanInfo[]>> mAnswerChessmanInfos;  //用户解出的棋子答案信息
	private Dictionary<int, int> answerlengthInfos;  //不同棋盘规模的答案总数量
	
	/*
	 * 棋子坐标信息
	 */ 
	private class ChessmanInfo {
		public int line;  //行
		public int row;   //列
		public GameObject chessman;

		public ChessmanInfo(int line, int row) {
			this.line = line;
			this.row  = row;
		}

		public ChessmanInfo(int line, int row, GameObject chessman) {
			this.line = line;
			this.row  = row;
			this.chessman = chessman;
		}
	}

	void Awake() {
		cubeSize = 1;
		chessboardSize = 0;
		curAnswerIndex = 0;
		isWin = false;
		isShowAnswer = false;
		isFindAnswerOver = false;
		chessmanHeight = 0.0f;
		rotateSpeed = 90.0f;
		cameraMoveSpeed = 5.0f;
		cameraLookAtSpeed = 5.0f;
		sizeInput = "请输入数字4-8";
		cameraInitPos = transform.position;
		chessRoot = new GameObject ();
		mChessmanInfos = new List<ChessmanInfo>();
		answerChessmans = new List<GameObject>();
		answerChessmanInfos = new Dictionary<int, List<ChessmanInfo[]>>();
		mAnswerChessmanInfos = new Dictionary<int, List<ChessmanInfo[]>>();
		answerlengthInfos = new Dictionary<int, int>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!isShowAnswer) {
			SetChessman ();
		}

		RotateChessboard ();
		CameraMovement ();
	}

	void OnGUI() {
		if(!isShowAnswer) {
			Button_CreateChessboard ();
			TextField_ChessBoardSize ();
			Button_CheckChessmans ();

			if(isWin) {
				Button_ReStart ();
			}

			if(chessboardSize != 0) {
				Button_ShowAnswer();
			}
		}

		else {
			if(isFindAnswerOver) {
				Button_PreviousAnswer();
				Label_CurAnswerIndex();
				Button_NextAnswer();
			}

			Button_ExitAnswer();
		}

	}

	/*
	 * 生成棋盘的按钮
	 */ 
	private void Button_CreateChessboard() {
		if(GUI.Button (new Rect (200, 10, 100, 50), "生成棋盘")) {
			int size = 0;

			if(int.TryParse(sizeInput, out size)) {
				if(size >= 4 && size <= 8) {
					GameObject.Destroy(mChessboard);
					chessboardSize = size;
					CreateChessboard();

					RefreshCameraPos();
				}

				else {
					Debug.LogError("请输入4-8的数字");
				}
			}

			else {
				Debug.LogError("请输入4-8数字");
			}
		}
	}

	/*
	 * 输入棋盘规模的输入框
	 */ 
	private void TextField_ChessBoardSize() {
		sizeInput = GUI.TextField (new Rect(50, 10, 100, 50), sizeInput);
	}

	/*
	 * 检查棋子位置是否合法的按钮
	 */ 
	private void Button_CheckChessmans() {
		if(GUI.Button (new Rect (350, 10, 100, 50), "检查棋子")) {
			if(CheckChessmans(mChessmanInfos)) {
				if(chessboardSize != 0 && mChessmanInfos.Count == chessboardSize) {
					WinGame();
					WriteAnswer();
				}
			}
		}
	}

	/*
	 * 重新开始的按钮
	 */ 
	private void Button_ReStart() {
		if(GUI.Button(new Rect(550, 500, 100, 50), "重新开始")) {
			ReStart();
		}
	}

	/*
	 * 展示答案的按钮
	 */ 
	private void Button_ShowAnswer() {
		if(GUI.Button (new Rect (500, 10, 100, 50), "展示答案")) {
			ShowAnswer ();
		}
	}

	/*
	 * 上一个答案的按钮
	 */ 
	private void Button_PreviousAnswer() {
		if(GUI.Button (new Rect (250, 500, 100, 50), "上一个")) {
			PreviousAnswer();
		}
	}

	/*
	 * 当前答案序号的标签
	 */ 
	private void Label_CurAnswerIndex() {
		GUI.Label (new Rect (385, 520, 100, 50), curAnswerIndex + " / " + answerlengthInfos[chessboardSize]);
	}

	/*
	 * 下一个答案的按钮
	 */ 
	private void Button_NextAnswer() {
		if(GUI.Button (new Rect (450, 500, 100, 50), "下一个")) {
			NextAnswer();
		}
	}

	/*
	 * 退出答案显示界面的按钮
	 */ 
	private void Button_ExitAnswer() {
		if(GUI.Button (new Rect (350, 10, 100, 50), "返回")) {
			ExitAnswer();
		}
	}

	/*
	 * 生成棋盘
	 */ 
	private void CreateChessboard() {
		bool isBlackCube = true;
		mChessboard = new GameObject ();
		mChessboard.name = "棋盘(" + chessboardSize + "*" + chessboardSize + ")"; 
		mChessboard.transform.parent = chessRoot.transform;

		for(int curLine = 0; curLine < chessboardSize; curLine++) {

			if(chessboardSize % 2 == 0) {
				if(curLine % 2 == 0) {
					isBlackCube = true;
				}

				else {
					isBlackCube = false;
				}
			}

			for(int curRow = 0; curRow < chessboardSize; curRow++) {
				if(isBlackCube) {
					GameObject blackCube = (GameObject)GameObject.Instantiate(blackCubePrefab);
					blackCube.transform.position = new Vector3((curRow + 0.5f) * cubeSize, 0, (curLine + 0.5f) * cubeSize);
					blackCube.transform.parent = mChessboard.transform;
					blackCube.GetComponent<CubeInfo>().SetInfo(curLine, curRow);
					isBlackCube = false;
				}

				else {
					GameObject whiteCube = (GameObject)GameObject.Instantiate(whiteCubePrefab);
					whiteCube.transform.position = new Vector3((curRow + 0.5f) * cubeSize, 0, (curLine + 0.5f) * cubeSize);
					whiteCube.transform.parent = mChessboard.transform;
					whiteCube.GetComponent<CubeInfo>().SetInfo(curLine, curRow);
					isBlackCube = true;
				}
			}
		}
	}

	/*
	 * 点击棋盘上的格子,若格子上没有棋子则放置棋子,反之则拿掉棋子
	 */ 
	private void SetChessman() {

		//点击鼠标左键
		if(Input.GetMouseButtonDown(0)) {
			RaycastHit hitInfo;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			//点击到棋盘
			if(Physics.Raycast(ray, out hitInfo)) {

				//int line = (int)(hitInfo.point.x / 1);
				//int row  = (int)(hitInfo.point.z / 1);
				GameObject cube = hitInfo.collider.gameObject;
				CubeInfo cubeInfo = cube.GetComponent<CubeInfo>();
				int line = cubeInfo.line;
				int row = cubeInfo.row;

				//检查点击的位置是否已经放置了棋子
				foreach(ChessmanInfo info in mChessmanInfos) {

					//若已经放置棋子,则去掉棋子,并删除记录
					if(info.line == line && info.row == row) {
						GameObject.Destroy(info.chessman);
						mChessmanInfos.Remove(info);
						return;
					}
				}

				//若没有放置棋子,则生成棋子,并添加记录
				GameObject mChessman = (GameObject)GameObject.Instantiate(chessmanPrefab);

				//float x = (line + 0.5f) * cubeSize;
				//float y = cubeSize / 2.0f + chessmanHeight / 2;
				//float z = (row + 0.5f) * cubeSize;
				float x = cube.transform.position.x;
				float y = cubeSize / 2.0f + chessmanHeight / 2;
				float z = cube.transform.position.z;

				mChessman.transform.position = new Vector3(x, y, z);
				mChessman.transform.parent = chessRoot.transform;
				mChessmanInfos.Add(new ChessmanInfo(line, row, mChessman));
			}
		}
	}

	/*
	 * 检查棋子的位置是否合法
	 */ 
	private bool CheckChessmans(List<ChessmanInfo> infos) {
		for(int i=0; i<infos.Count; i++) {
			for(int j=i+1; j<infos.Count; j++) {
				
				//任意两个棋子不能在同一行
				if(infos[i].line == infos[j].line) {
					if(infos[i].chessman != null) {
						infos[i].chessman.transform.Rotate(new Vector3(0, 90, 0));
						infos[j].chessman.transform.Rotate(new Vector3(0, 90, 0));
					}

					return false;
				}
				
				//任意两个棋子不能在同一列
				if(infos[i].row == infos[j].row) {
					if(infos[i].chessman != null) {
						infos[i].chessman.transform.Rotate(new Vector3(0, 90, 0));
						infos[j].chessman.transform.Rotate(new Vector3(0, 90, 0));
					}

					return false;
				}
				
				//任意两个棋子不能再同一斜线上
				if(infos[i].line - infos[j].line == infos[i].row - infos[j].row) {
					if(infos[i].chessman != null) {
						infos[i].chessman.transform.Rotate(new Vector3(0, 90, 0));
						infos[j].chessman.transform.Rotate(new Vector3(0, 90, 0));
					}

					return false;
				}
				
				if((infos[i].line - infos[j].line) + (infos[i].row - infos[j].row) == 0) {
					if(infos[i].chessman != null) {
						infos[i].chessman.transform.Rotate(new Vector3(0, 90, 0));
						infos[j].chessman.transform.Rotate(new Vector3(0, 90, 0));
					}

					return false;
				}
				//
			}
		}
		
		return true;
	}

	/*
	 * 玩家获得胜利
	 */ 
	private void WinGame() {
		isWin = true;
		mWinObj.SetActive(true);
	}

	/*
	 * 重新开始
	 */ 
	private void ReStart() {
		isWin = false;
		mWinObj.SetActive(false);

		//去除棋盘上的所有棋子并删除记录
		ChessmanInfo[] chessmanArray = mChessmanInfos.ToArray();
		for(int i=0; i<chessmanArray.Length; i++) {
			GameObject.Destroy(chessmanArray[i].chessman);
			mChessmanInfos.Remove(chessmanArray[i]);
		}
	}

	/*
	 * 展示答案
	 */ 
	private void ShowAnswer() {
		isShowAnswer = true;

		//隐藏玩家放置的所有棋子
		foreach(ChessmanInfo info in mChessmanInfos) {
			info.chessman.SetActive(false);
		}

		//准备好用于展示答案的棋子
		for(int i=0; i<chessboardSize; i++) {
			GameObject answerChessman = (GameObject)GameObject.Instantiate(chessmanPrefab);
			answerChessman.SetActive(false);
			answerChessmans.Add(answerChessman);
		}

		//若当前棋盘规模没有存储答案信息,则解出答案并存储答案信息
		if(!answerChessmanInfos.ContainsKey (chessboardSize)) {
			FindAnswer();
		}

		//展示第一种答案
		curAnswerIndex = 1;
		ShowOneAnswer(curAnswerIndex);
		isFindAnswerOver = true;

		ReadAnswer (chessboardSize);
	}

	/*
	 * 查看上一个答案
	 */ 
	private void PreviousAnswer() {
		if(curAnswerIndex == 1) {
			curAnswerIndex = answerlengthInfos[chessboardSize];
		}
	
		else {
			curAnswerIndex--;
		}

		ShowOneAnswer (curAnswerIndex);
	}

	/*
	 * 查看下一个答案
	 */ 
	private void NextAnswer() {
		if(curAnswerIndex == answerlengthInfos[chessboardSize]) {
			curAnswerIndex = 1;
		}
		
		else {
			curAnswerIndex++;
		}
		
		ShowOneAnswer (curAnswerIndex);
	}

	/*
	 * 退出答案展示界面
	 */ 
	private void ExitAnswer() {
		isShowAnswer = false;
		isFindAnswerOver = false;

		//去除用于展示答案的所有棋子
		GameObject[] answerChessmansArray = answerChessmans.ToArray();
		for(int i=0; i<answerChessmansArray.Length; i++) {
			GameObject.Destroy(answerChessmansArray[i]);
		}
		answerChessmans.Clear();

		//显示玩家放置的所有棋子
		foreach(ChessmanInfo info in mChessmanInfos) {
			info.chessman.SetActive(true);
		}
	}

	/*
	 * 解出答案并存储答案信息
	 */ 
	private void FindAnswer() {
		int answerNum = 0;  //解的个数
		int curLine   = 1;  //当前最新加入的棋子位于棋盘的第几行
		List<ChessmanInfo> curPoints = new List<ChessmanInfo>();
		List<ChessmanInfo[]> finalAnswer = new List<ChessmanInfo[]>();
		
		//加入第一个棋子
		curPoints.Add(new ChessmanInfo(1, 0));
		
		while(curLine > 0) {
			
			curPoints[curLine - 1].row ++;
			
			//当前行未搜索完毕
			while(curPoints[curLine - 1].row <= chessboardSize) {
				
				//当前已加入的棋子位置合法
				if(CheckChessmans(curPoints)) {
					
					//当前棋子数量达到棋盘规模,存储一个答案,并进入下一列
					if(curLine == chessboardSize) {
						answerNum++;

						ChessmanInfo[] curPointsArray = curPoints.ToArray();
						ChessmanInfo[] oneAnswer = new ChessmanInfo[curPointsArray.Length];
						for(int i=0; i<curPointsArray.Length; i++) {
							ChessmanInfo info = new ChessmanInfo(curPointsArray[i].line, curPointsArray[i].row);
							oneAnswer[i] = info;
						}
						finalAnswer.Add(oneAnswer);
						
						curPoints[curLine - 1].row ++;
					}
					
					//当前棋子数量不足棋盘规模,进入下一行,并加入新的棋子
					else if(curLine > 0 && curLine < chessboardSize){
						curLine++;
						curPoints.Add(new ChessmanInfo(curLine, 1));
					}
					
					//异常情况
					else {
						Debug.LogError("curLine out of index");
					}	
				}
				
				//当前已加入的棋子位置不合法,进入下一列
				else {
					curPoints[curLine - 1].row ++;
				}
			}
			
			//当前行已搜索完毕,回溯到上一行
			curPoints.RemoveAt(curLine - 1);
			curLine--;
		}

		//存储答案信息
		answerlengthInfos.Add(chessboardSize, answerNum);
		answerChessmanInfos.Add(chessboardSize, finalAnswer);
	}

	/*
	 * 展示第index种答案
	 */ 
	private void ShowOneAnswer(int index) {
		int curIndex = 0;
		ChessmanInfo[] oneAnswer = (ChessmanInfo[])answerChessmanInfos [chessboardSize].ToArray ().GetValue (index - 1);

		foreach(GameObject chessman in answerChessmans) {
			int line = oneAnswer[curIndex].line - 1;
			int row = oneAnswer[curIndex].row - 1;

			float x = (line + 0.5f) * cubeSize;
			float y = cubeSize / 2.0f + chessmanHeight / 2;
			float z = (row + 0.5f) * cubeSize;
			chessman.transform.position = new Vector3(x, y, z);
			chessman.SetActive(true);

			curIndex++;
		}
	}

	/*
	 * 绕Y轴旋转棋盘
	 */ 
	private void RotateChessboard()
	{
		if (Input.GetKey (KeyCode.RightArrow)) 
		{
			float size = chessboardSize;
			float center = size / 2;
			chessRoot.transform.RotateAround(new Vector3(center, 0, center), Vector3.up, rotateSpeed * Time.deltaTime);
		}

		if (Input.GetKey (KeyCode.LeftArrow)) 
		{
			float size = chessboardSize;
			float center = size / 2;
			chessRoot.transform.RotateAround(new Vector3(center, 0, center), Vector3.up, (-rotateSpeed) * Time.deltaTime);
		}
	}

	/*
	 * 更新相机平视和俯视棋盘的位置
	 */ 
	private void RefreshCameraPos()
	{
		float size = chessboardSize;
		float center = size / 2;

		cameraAbovePos = new Vector3 (center, 10, center);
		cameraFrontPos = new Vector3 (center, 0, -10);
	}

	/*
	 * 移动相机的位置
	 */ 
	private void CameraMovement()
	{
		if (Input.GetKey (KeyCode.UpArrow)) 
		{
			transform.position = Vector3.Lerp(transform.position, cameraAbovePos, cameraMoveSpeed * Time.deltaTime);
			CameraLookAt();
		}

		if (Input.GetKey (KeyCode.DownArrow)) 
		{
			transform.position = Vector3.Lerp(transform.position, cameraFrontPos, cameraMoveSpeed * Time.deltaTime);
			CameraLookAt();
		}
	}

	/*
	 * 让相机面向棋盘
	 */ 
	private void CameraLookAt()
	{
		float size = chessboardSize;
		float center = size / 2;

		Vector3 lookAtpoint = new Vector3 (center, 0, center) - transform.position;
		Quaternion lookAtRatation = Quaternion.LookRotation (lookAtpoint, Vector3.up);
		transform.rotation = Quaternion.Lerp (transform.rotation, lookAtRatation, cameraLookAtSpeed * Time.deltaTime);
	}

	/*
	 * 将用户解出的答案保存在文件中
	 */ 
	private void WriteAnswer()
	{
		string message = "";

		foreach (ChessmanInfo info in mChessmanInfos) 
		{
			message += info.line + "," + info.row + " ";
		}

		if(CheckAnswerSaved(message))
		{
			return;
		}

		StreamWriter sw;
		FileInfo fileInfo = new FileInfo (Application.dataPath + "//" + "MyAnswer_" + chessboardSize);

		if (!fileInfo.Exists) 
		{
			sw = fileInfo.CreateText ();
		}

		else 
		{
			sw = fileInfo.AppendText();
		}

		sw.WriteLine (message);
		sw.Close ();
		sw.Dispose ();
	}

	/*
	 * 检查用户解出的答案是否已经保存在文件中
	 */ 
	private bool CheckAnswerSaved(string answer)
	{
		string line;
		StreamReader sr = null;
		
		try {
			sr = File.OpenText(Application.dataPath + "//" + "MyAnswer_" + chessboardSize);
		} catch(FileNotFoundException e)
		{
			sr.Close();
			sr.Dispose();
			return false;
		}
		
		while ((line = sr.ReadLine()) != null) 
		{
			if(line.Equals(answer))
			{
				sr.Close();
				sr.Dispose();
				return true;
			}
		}

		sr.Close();
		sr.Dispose ();
		return false;
	}

	/*
	 * 从文件中读取用户解出的答案,并保存到程序中
	 */ 
	private void ReadAnswer(int size)
	{
		if (mAnswerChessmanInfos.ContainsKey (size)) 
		{
			mAnswerChessmanInfos.Remove(size);
		}

		List<ChessmanInfo[]> myAnswers = new List<ChessmanInfo[]> ();

		string line;
		StreamReader sr = null;

		try {
			sr = File.OpenText(Application.dataPath + "//" + "MyAnswer_" + size);
		} catch(FileNotFoundException e)
		{
			Debug.LogError("File not found");
		}

		while ((line = sr.ReadLine()) != null) 
		{
			ChessmanInfo[] oneAnswer = SaveOneAnswer(line, size);
			myAnswers.Add(oneAnswer);
		}

		mAnswerChessmanInfos.Add (size, myAnswers);
	}

	/*
	 * 从文件中保存一个用户解出的答案到程序中
	 */ 
	private ChessmanInfo[] SaveOneAnswer(string answer, int size)
	{
		int infoIndex = 0;
		ChessmanInfo[] infos = new ChessmanInfo[size];
		string[] coordinates = answer.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);

		foreach(string coordinate in coordinates)
		{
			string[] elements = coordinate.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);
			int line = -1;
			int row = -1;
			if(!int.TryParse(elements[0], out line))
			{
				Debug.LogError("error when parse to int");
			}
			if(!int.TryParse(elements[1], out row))
			{
				Debug.LogError("error when parse to int");
			}

			ChessmanInfo info = new ChessmanInfo(line, row);
			infos[infoIndex] = info;
			infoIndex++;
		}

		return infos;
	}

}
