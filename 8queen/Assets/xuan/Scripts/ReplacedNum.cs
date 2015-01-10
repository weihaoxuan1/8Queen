using UnityEngine;
using System.Collections;

public class ReplacedNum : MonoBehaviour {

    GameObject replacedNum;
    UISlider control;

    float num = 0;
	// Use this for initialization
	void Start () {
        
        //if(replacedNum)Debug.Log("num find")
        
        
        //if (control) Debug.Log("control find");
	}

    void OnEnable()
    {
        replacedNum = transform.FindChild("LabelReplaced/ReplacedNum").gameObject;
        control = transform.FindChild("Control").GetComponent<UISlider>();
        //Debug.Log("enable");
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnChangeReplacedNum()
    {
        
        int steps = control.numberOfSteps;
        float value = control.value;
        num = (steps-1) * (value/1);
        replacedNum.GetComponent<UILabel>().text = num.ToString();
        //Debug.Log((steps-1) * (value/1));
    }

    public int GetReplacedNum()
    {
        return (int)num;
    }
}
