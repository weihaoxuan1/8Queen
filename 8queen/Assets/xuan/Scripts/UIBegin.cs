using UnityEngine;
using System.Collections;

public class UIBegin : MonoBehaviour
{


    public GameObject onAchievement;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDisable()
    {
        onAchievement.SetActive(false);
    }

    public void OnStart()
    {
        MainDirector.Instance.ToSelectChessboardUI();
    }

    public void OnInstruction()
    {
        MainDirector.Instance.ToInstuctionUI();
    }

    public void OnAchievement()
    {
        onAchievement.SetActive(false);
        onAchievement.SetActive(true);
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
