using UnityEngine;
using System.Collections;

public class OnHover : MonoBehaviour
{

    UIButton button;
    public GameObject showWhileHover;
	// Use this for initialization
	void Start () {
        button = gameObject.GetComponent<UIButton>();
	}
	
	// Update is called once per frame
	void Update () {
	    if(button.state == UIButtonColor.State.Hover)
        {
            showWhileHover.SetActive(true);
            //onBackToMainHover.transform.localPosition = Input.mousePosition;
        }
        else
        {
            showWhileHover.SetActive(false);
        }
	}
}
