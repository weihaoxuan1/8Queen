using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChessRoot : MonoBehaviour 
{
	public static ChessRoot Instance;

    public GameObject scene;

	public GameObject chessmanPrefab;
	public GameObject basePrefab;  //棋盘底座预制
	public GameObject blackCubePrefab;
	public GameObject whiteCubePrefab;
    public Material staticChessMaterial;
	
	private int chessboardSize;   //棋盘规格
	private float cubeSize;       //组成棋盘表面的每个方格的边长
	private float curBaseHeight;  //当前棋盘底座的高度
	private float maxBaseHeight;  //棋盘底座的最大高度
	private float rotateSpeed;    //棋盘的旋转速度（每秒转多少度）
	private bool canSetChessman;  //是否允许在棋盘上放置或拿掉棋子
	private Vector3 centerPos;    //棋盘的中心位置
	private GameObject mChessboard;
	private GameObject mSurface;  //棋盘表面
	private GameObject mBase;     //棋盘底座
	private GameObject mChessmans;
	private List<ChessmanInfo> mChessmanInfos;  //棋盘上的棋子坐标信息

	ChessRoot()
	{
		Instance = this;

		cubeSize = 1.2f;
		curBaseHeight = 0.0f;
		maxBaseHeight = 3.3f;
		rotateSpeed = 120.0f;
		canSetChessman = false;
		mChessmanInfos = new List<ChessmanInfo>();
	}

	void Awake()
	{
		mChessboard = new GameObject ();
		mChessboard.name = "ChessBoard"; 
		mChessboard.transform.parent = transform;

		mSurface = new GameObject ();
		mSurface.name = "Surface";
		mSurface.transform.parent = mChessboard.transform;

		mChessmans = new GameObject ();
		mChessmans.name = "Chessmans";
		mChessmans.transform.parent = transform;
	}

	void Update()
	{
		if (canSetChessman) 
		{
			SetChessman();
		}
	}
	

	/*
	 * 点击棋盘上的格子,若格子上没有棋子则放置棋子,反之则拿掉棋子
	 */ 
	private void SetChessman() 
	{
		
		//点击鼠标左键
		if(Input.GetMouseButtonDown(0)) 
		{
			RaycastHit hitInfo;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			//点击到棋盘
			if(Physics.Raycast(ray, out hitInfo)) 
			{
				GameObject cube = hitInfo.collider.gameObject;
				CubeInfo cubeInfo = cube.GetComponent<CubeInfo>();
				
				//若碰撞物体无CubeInfo.cs脚本,说明该碰撞物体不是组成棋盘的方格
				if(cubeInfo == null)
				{
					return;
				}
				
				//获取方格位于棋盘的坐标信息
				int line = cubeInfo.line;
				int row = cubeInfo.row;
				
				//检查点击的位置是否已经放置了棋子
				for(int i=0; i<mChessmanInfos.Count; i++)
				{
					ChessmanInfo info = (ChessmanInfo)mChessmanInfos.ToArray().GetValue(i);

					//若已经放置棋子
					if(info.line == line && info.row == row) 
					{
						//若该棋子能被玩家拿掉,则去掉棋子,并删除记录
						if(info.canBeRemove)
						{
							GameObject.Destroy(info.chessman);
							mChessmanInfos.RemoveAt(i);
							SoundManager.Instance.PlayDropChessmanSound();
						}

						return;
					}
				}
				
				//若没有放置棋子,则生成棋子,并添加记录
				GameObject mChessman = (GameObject)GameObject.Instantiate(chessmanPrefab);
				float x = cube.transform.position.x;
				float y = cubeSize / 2.0f + curBaseHeight;
				float z = cube.transform.position.z;
				mChessman.transform.position = new Vector3(x, y, z);
				mChessman.transform.parent = mChessmans.transform;
				ChessmanInfo newInfo = new ChessmanInfo(line, row, true, mChessman);
				mChessmanInfos.Add(newInfo);
				SoundManager.Instance.PlaySetChessmanSound();

				//若放置的棋子不合法,在提示完毕后去除该棋子
				if(!CheckChessmans(mChessmanInfos))
				{
					ForbidSetChessman();
					Invoke("RemoveLastChessman", mChessman.GetComponent<TwinkleEffect>().GetTotalTwinkleTime());
					SoundManager.Instance.PlayMakeMistakeSound();
					UserBehaviorRecorder.Instance.OneWrongStep();
				}

				//若放置的棋子合法,并且达到所需棋子数量,游戏胜利
				else
				{
					if(mChessmanInfos.Count == chessboardSize)
					{
						ScoreManager.Instance.StopTimer();

                        //当前解之前没有解出过
                        if (CheckAnswerRepeat(mChessmanInfos.ToArray()))
                        {
                            PlayerAnswerRecorder.Instance.WriteAnswer(mChessmanInfos);
							MainDirector.Instance.ShowFinishUI_delay(0.1f);
							SoundManager.Instance.PlayNewAnswerSound();
							ScoreManager.Instance.CalScore(false);
							MainDirector.Instance.ShowBalanceUI();
                            ForbidSetChessman();
                        }

                        //当前解之前解出过
                        else
                        {
							MainDirector.Instance.ShowDuplicateUI_delay(0.1f);
							SoundManager.Instance.PlayDuplicateAnswerSound();
							ScoreManager.Instance.CalScore(true);
							MainDirector.Instance.ShowBalanceUI();
                            ForbidSetChessman();
                        }
						
					}
				}
			}
		}
	}

    /*
     * 检查当前解是否是之前没有解出过的
     */
    private bool CheckAnswerRepeat(ChessmanInfo[] infos)
    {
        List<ChessmanInfo[]> playerAnswer = PlayerAnswerRecorder.Instance.GetPlayerAnswer(chessboardSize);

        if (playerAnswer == null)
        {
            return true;
        }

        foreach (ChessmanInfo[] answer in playerAnswer)
        {
            for (int i = 0; i < infos.Length; i++)
            {
                for (int j = 0; j < answer.Length; j++)
                {
                    if (infos[i].line == answer[j].line && infos[i].row == answer[j].row)
                    {
                        //找到解集中的一组解与当前解相同
                        if (i == chessboardSize - 1)
                        {
                            return false;
                        }

                        //检查当前解的下一个棋子信息
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }

        return true;
    }

	/*
	 * 检查棋子的位置是否合法
	 */ 
	private bool CheckChessmans(List<ChessmanInfo> infos) 
	{
		for(int i=0; i<infos.Count; i++) 
		{
			for(int j=i+1; j<infos.Count; j++) 
			{
				
				//任意两个棋子不能在同一行
				if(infos[i].line == infos[j].line) 
				{
					infos[i].chessman.SendMessage("StartTwinkle");
					infos[j].chessman.SendMessage("StartTwinkle");
					return false;
				}
				
				//任意两个棋子不能在同一列
				if(infos[i].row == infos[j].row) 
				{
					infos[i].chessman.SendMessage("StartTwinkle");
					infos[j].chessman.SendMessage("StartTwinkle");
					return false;
				}
				
				//任意两个棋子不能再同一斜线上
				if(infos[i].line - infos[j].line == infos[i].row - infos[j].row) 
				{
					infos[i].chessman.SendMessage("StartTwinkle");
					infos[j].chessman.SendMessage("StartTwinkle");
					return false;
				}
				
				if((infos[i].line - infos[j].line) + (infos[i].row - infos[j].row) == 0) 
				{
					infos[i].chessman.SendMessage("StartTwinkle");
					infos[j].chessman.SendMessage("StartTwinkle");
					return false;
				}
				//
			}
		}
		
		return true;
	}

	/*
	 * 去除最新加入的棋子
	 */ 
	private void RemoveLastChessman()
	{
		Destroy(mChessmanInfos.ToArray()[mChessmanInfos.Count - 1].chessman);
		mChessmanInfos.RemoveAt (mChessmanInfos.Count - 1);
		AllowSetChessman ();
	}

	/*
	 * 根据行列信息在棋盘表面中查找方格
	 */ 
	private GameObject FindCubeInChessboard(int line, int row)
	{
		foreach (Transform cube in mSurface.transform) 
		{
			CubeInfo info = cube.GetComponent<CubeInfo>();

			if(info.line == line && info.row == row)
			{
				return cube.gameObject;
			}
		}

		Debug.LogError("Cube not found");
		return null;
	}

	/*
	 * 生成一个组成棋盘的方格
	 */ 
	private void CreateCube(bool isBlackCube, int line, int row)
	{
		GameObject cube;
		
		//生成一个黑色方格
		if (isBlackCube) 
		{
			cube = (GameObject)GameObject.Instantiate(blackCubePrefab);
		}
		
		//生成一个白色方格
		else 
		{
			cube = (GameObject)GameObject.Instantiate(whiteCubePrefab);
		}
		
		//根据行列信息确定方格的位置,并记录方格位于棋盘的坐标信息
		cube.transform.position = new Vector3((row + 0.5f) * cubeSize, 0, (line + 0.5f) * cubeSize);
		cube.transform.parent = mSurface.transform;
		cube.GetComponent<CubeInfo>().SetInfo(line, row);
	}

	/*
	 * 计算棋盘的中心位置
	 */ 
	private void CalCenterPos()
	{
		Vector3 lowerLeftPos = Vector3.zero;  //棋盘左下角方格的位置
		Vector3 upperRightPos = Vector3.zero;  //棋盘右上角方格的位置
		
		//获取棋盘左下角和右上角方格的位置
		foreach (Transform cube in mSurface.transform) 
		{
			CubeInfo info = cube.GetComponent<CubeInfo>();
			
			if(info.line == 0 && info.row == 0)
			{
				lowerLeftPos = cube.transform.position;
			}
			
			if(info.line == chessboardSize-1 && info.row == chessboardSize-1)
			{
				upperRightPos = cube.transform.position;
			}
		}
		
		centerPos = (lowerLeftPos + upperRightPos) / 2;
	}

	/*
	 * 调整棋盘表面和底座的相对位置和大小
	 */ 
	private void AdjustChessboard()
	{
		//生成棋盘底座
		if (mBase == null) 
		{
			mBase = (GameObject)GameObject.Instantiate (basePrefab);
			mBase.transform.parent = mChessboard.transform;
		}
		else 
		{
			mBase.SetActive(true);
		}

		//根据棋盘规模调节棋盘底座大小
		float size = 1 - (8 - chessboardSize) * 0.12f;
		curBaseHeight = size * maxBaseHeight;
		mBase.transform.localScale = new Vector3 (size, size , size);

		//让棋盘表面位于棋盘底座中央
		float _chessboardSize = chessboardSize;
		float surfacePoint = -(cubeSize * _chessboardSize / 2);
		mSurface.transform.position = new Vector3 (surfacePoint, curBaseHeight, surfacePoint);
	}

	/*
	 * 生成棋盘
	 */ 
	public void CreateChessboard(int size) {
		bool isBlackCube = true;  //用于控制棋盘的相邻方格之间颜色不同
		chessboardSize = size;
		
		for(int curLine = 0; curLine < chessboardSize; curLine++) {
			
			//当棋盘规模为偶数时,需要通过当前行数的奇偶性来确定该行首个方格的颜色
			if(chessboardSize % 2 == 0) {
				if(curLine % 2 == 0) {
					isBlackCube = true;
				}
				
				else {
					isBlackCube = false;
				}
			}
			
			for(int curRow = 0; curRow < chessboardSize; curRow++) {
				CreateCube(isBlackCube, curLine, curRow);
				isBlackCube = !isBlackCube;  //下一列的方格颜色与上一列相反
			}
		}

		AdjustChessboard ();
		CalCenterPos ();
	}

	/*
	 * 在棋盘中放置玩家不能拿掉的棋子
	 */ 
	public void SetUnremovableChessman(ChessmanInfo[] chessmans)
	{
        if(chessmans == null)
        {
            return;
        }

		foreach (ChessmanInfo info in chessmans) 
		{
			GameObject chessman = (GameObject)GameObject.Instantiate(chessmanPrefab);
			GameObject cube = FindCubeInChessboard(info.line, info.row);

			float x = cube.transform.position.x;
			float y = cubeSize / 2.0f + curBaseHeight;
			float z = cube.transform.position.z;

			chessman.transform.position = new Vector3(x, y, z);
			chessman.transform.parent = mChessmans.transform;
            chessman.renderer.material = staticChessMaterial;
			mChessmanInfos.Add(new ChessmanInfo(info.line, info.row, false, chessman));
		}
	}

	/*
	 * 清空棋盘上的所有棋子
	 * @isRemoveAll 是否连同系统事先放置的棋子一起清空
	 */ 
	public void ClearAllChessmans(bool isRemoveAll)
	{
		ChessmanInfo[] array = mChessmanInfos.ToArray ();

		for (int i=0; i<array.Length; i++) 
		{
			ChessmanInfo info = array[i];

			if(info.canBeRemove || (!info.canBeRemove && isRemoveAll))
			{
				Destroy(info.chessman);
				mChessmanInfos.Remove(info);
			}
		}
	}

	/*
	 * 清空棋盘
	 */ 
	public void ClearChessboard()
	{
		//清空棋盘表面
		foreach (Transform cube in mSurface.transform) 
		{
			Destroy(cube.gameObject);
		}
		mSurface.transform.position = new Vector3 (0, 0, 0);

		//清空棋盘底座
		mBase.transform.rotation = Quaternion.Euler (new Vector3 (0, 0, 0));
		mBase.SetActive (false);
	}

	/*
	 * 绕棋盘正中心旋转棋盘
	 */ 
	public void RotateChessboard(bool isClockWise)
	{
		//顺时针旋转
		if (isClockWise) 
		{
			transform.RotateAround(centerPos, Vector3.up, rotateSpeed * Time.deltaTime);
			scene.transform.RotateAround(centerPos, Vector3.up, rotateSpeed * Time.deltaTime);
		}

		//逆时针旋转
		else
		{
			transform.RotateAround(centerPos, Vector3.up, (-rotateSpeed) * Time.deltaTime);
            scene.transform.RotateAround(centerPos, Vector3.up, (-rotateSpeed) * Time.deltaTime);
		}
	}

	/*
	 * 允许在棋盘上放置或拿掉棋子
	 */ 
	public void AllowSetChessman()
	{
		canSetChessman = true;
	}

	/*
	 * 禁止在棋盘上放置或拿掉棋子
	 */ 
	public void ForbidSetChessman()
	{
		canSetChessman = false;
	}

	/*
	 * 返回棋盘的中心位置
	 */ 
	public Vector3 GetCenterPos()
	{
		return centerPos;
	}

	/*
	 * 返回棋盘规模
	 */ 
	public int GetChessboardSize()
	{
		return chessboardSize;
	}

    public void HideAllChessmans()
    {
        foreach(ChessmanInfo info in mChessmanInfos)
        {
            info.chessman.SetActive(false);
        }
    }

    public void ShowAllChessmans()
    {
        foreach(ChessmanInfo info in mChessmanInfos)
        {
            info.chessman.SetActive(true);
        }
    }

}
