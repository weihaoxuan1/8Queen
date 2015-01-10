using UnityEngine;
using System.Collections;

public class ChessmanInfo
{
	public int line;  //行
	public int row;   //列
	public bool canBeRemove;  //是否能被玩家从棋盘上拿掉
	public GameObject chessman;
	
	public ChessmanInfo(int line, int row) 
	{
		this.line = line;
		this.row  = row;
	}
	
	public ChessmanInfo(int line, int row, bool canBeRemove, GameObject chessman)
	{
		this.line = line;
		this.row  = row;
		this.canBeRemove = canBeRemove;
		this.chessman = chessman;
	}
	
}
