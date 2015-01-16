using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour 
{
	public static SoundManager Instance;

	public AudioClip clickButtonSound;      //鼠标点击按钮的音效
	public AudioClip hoverButtonSound;      //鼠标悬停在按钮上的音效

	public AudioClip setChessmanSound;      //放置棋子的音效
	public AudioClip dropChessmanSound;     //拿掉棋子的音效
	public AudioClip makeMistakeSound;      //放置错误棋子的音效

	public AudioClip newAnswerSound;        //解出新的摆法的音效
	public AudioClip duplicateAnswerSound;  //解出重复摆法的音效

	SoundManager()
	{
		Instance = this;
	}

	public void PlayClickButtonSound()
	{
		audio.PlayOneShot (clickButtonSound);
	}

	public void PlayHoverButtonSound()
	{
		audio.PlayOneShot (hoverButtonSound);
	}

	public void PlaySetChessmanSound()
	{
		audio.PlayOneShot (setChessmanSound);
	}

	public void PlayDropChessmanSound()
	{
		audio.PlayOneShot (dropChessmanSound);
	}

	public void PlayMakeMistakeSound()
	{
		audio.PlayOneShot (makeMistakeSound);
	}

	public void PlayNewAnswerSound()
	{
		audio.PlayOneShot (newAnswerSound);
	}

	public void PlayDuplicateAnswerSound()
	{
		audio.PlayOneShot (duplicateAnswerSound);
	}

}
