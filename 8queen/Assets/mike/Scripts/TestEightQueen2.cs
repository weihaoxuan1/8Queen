using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestEightQueen2 : MonoBehaviour {
	private int dimension;  //棋盘尺寸（dimension * dimension）
	
	/*
	 * 棋盘上的点（位于x行y列,x和y均从1开始）
	 */ 
	private class Point {
		public int x;
		public int y;
		
		public Point(int x, int y) {
			this.x = x;
			this.y = y;
		}
	}
	
	void Awake() {
		dimension = 8;
	}
	
	// Use this for initialization
	void Start () {
		FindAnswer();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/*
	 * 检查棋子的位置是否合法
	 */ 
	private bool CheckPoints(List<Point> points) {
		for(int i=0; i<points.Count; i++) {
			for(int j=i+1; j<points.Count; j++) {
				
				//任意两个棋子不能在同一行
				if(points[i].x == points[j].x) {
					return false;
				}
				
				//任意两个棋子不能在同一列
				if(points[i].y == points[j].y) {
					return false;
				}
				
				//任意两个棋子不能再同一斜线上
				if(points[i].x - points[j].x == points[i].y - points[j].y) {
					return false;
				}
				
				if((points[i].x - points[j].x) + (points[i].y - points[j].y) == 0) {
					return false;
				}
				//
			}
		}
		
		return true;
	}
	
	/*
	 * 计算出所有的解
	 */ 
	private void FindAnswer() {
		int answerNum = 0;  //解的个数
		int curLine   = 1;  //当前最新加入的棋子位于棋盘的第几行
		List<Point> curPoints = new List<Point>();
		
		//加入第一个棋子
		curPoints.Add(new Point(1, 0));
		
		while(curLine > 0) {
			
			curPoints[curLine - 1].y ++;
			
			//当前行未搜索完毕
			while(curPoints[curLine - 1].y <= dimension) {
				
				//当前已加入的棋子位置合法
				if(CheckPoints(curPoints)) {
					
					//当前棋子数量达到dimension个,输出一个正解,并进入下一列
					if(curLine == dimension) {
						answerNum++;
						Debug.Log("**********第" + answerNum + "组解**********");
						
						foreach(Point p in curPoints) {
							Debug.Log("(" + p.x + ", " + p.y + ")");
						}
						
						curPoints[curLine - 1].y ++;
					}
					
					//当前棋子数量不足dimension个,进入下一行,并加入新的棋子
					else if(curLine > 0 && curLine < dimension){
						curLine++;
						curPoints.Add(new Point(curLine, 1));
					}
					
					//异常情况
					else {
						Debug.LogError("curLine out of index");
					}	
				}
				
				//当前已加入的棋子位置不合法,进入下一列
				else {
					curPoints[curLine - 1].y ++;
				}
			}
			
			//当前行已搜索完毕,回溯到上一行
			curPoints.RemoveAt(curLine - 1);
			curLine--;
		}
		
		Debug.Log("计算完毕,共得出" + answerNum + "组解");
	}
}
