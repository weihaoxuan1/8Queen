using UnityEngine;
using System.Collections;

public class UIInstruction : MonoBehaviour
{

    public GameObject beginUI;
    public GameObject gameBGTitle;
    public GameObject gameBG;
    public GameObject HowToPlayTitle;
    public GameObject HowToPlay;
	// Use this for initialization
	void Start () {
        Invoke("ShowLabel", 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyUp(KeyCode.Mouse0) || Input.GetKeyUp(KeyCode.Mouse1))
        {
            MainDirector.Instance.ToBeginUI();
        }
	}

    void ShowLabel()
    {
        gameBGTitle.SetActive(true);
        gameBG.SetActive(true);
        HowToPlayTitle.SetActive(true);
        HowToPlay.SetActive(true);
    }
}
