using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour 
{
	private float cameraMoveSpeed;      //相机移动的速度
	private float cameraLookAtSpeed;    //相机面向棋盘的速度	
	private float cameraAboveDistance;  //相机俯视棋盘时与棋盘的距离
	private float cameraFrontDistance;  //相机平视棋盘时与棋盘的距离
	private Vector3 cameraAbovePos;     //相机俯视棋盘的位置
	private Vector3 cameraFrontPos;     //相机平视棋盘的位置
	private Vector3 cameraInitPos;      //相机的初始位置

	CameraMovement()
	{
		cameraMoveSpeed = 2.0f;
		cameraLookAtSpeed = 15.0f;
		cameraAboveDistance = 10.0f;
		cameraFrontDistance = 15.0f;
	}

	void Awake()
	{
		cameraInitPos = transform.position;
	}

	/*
	 * 让相机面向棋盘
	 */ 
	private void CameraLookAt()
	{	
		Vector3 lookAtpoint = ChessRoot.Instance.GetCenterPos() - transform.position;
		Quaternion lookAtRatation = Quaternion.LookRotation (lookAtpoint, Vector3.up);
		transform.rotation = Quaternion.Lerp (transform.rotation, lookAtRatation, cameraLookAtSpeed * Time.deltaTime);
	}

	/*
	 * 移动相机的位置
	 */ 
	public void CameraMove(bool isUp)
	{
		//相机向上移动
		if (isUp) 
		{
			transform.position = Vector3.Lerp(transform.position, cameraAbovePos, cameraMoveSpeed * Time.deltaTime);

		}

		//相机向下移动
		else 
		{
			transform.position = Vector3.Lerp(transform.position, cameraFrontPos, cameraMoveSpeed * Time.deltaTime);
		}

		CameraLookAt();
	}
	
	
	/*
	 * 更新相机平视和俯视棋盘的位置
	 */ 
	public void RefreshCameraPos()
	{
		Vector3 centerPos = ChessRoot.Instance.GetCenterPos ();
		
		cameraAbovePos = centerPos + Vector3.up * cameraAboveDistance;
		cameraFrontPos = centerPos + Vector3.forward * (-cameraFrontDistance);
	}

}
