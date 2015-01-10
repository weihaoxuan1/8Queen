using UnityEngine;
using System.Collections;

public class CubeInfo : MonoBehaviour 
{
	public int line;
	public int row;

	public void SetInfo(int line, int row)
	{
		this.line = line;
		this.row = row;
	}
}
