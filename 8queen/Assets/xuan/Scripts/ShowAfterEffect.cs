using UnityEngine;
using System.Collections;

public class ShowAfterEffect : MonoBehaviour {

    public GameObject[] objects;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowObjects()
    {
        for(int i = 0;i<objects.Length;i++)
        {
            objects[i].SetActive(true);
        }
    }
}
