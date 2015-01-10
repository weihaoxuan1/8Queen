using UnityEngine;
using System.Collections;

public class Lock : MonoBehaviour {

    public GameObject lockInstruction;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        lockInstruction.SetActive(false);
        lockInstruction.SetActive(true);
    }
}
