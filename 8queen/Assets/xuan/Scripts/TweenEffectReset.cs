using UnityEngine;
using System.Collections;

public class TweenEffectReset : MonoBehaviour {

    public bool ifSetAlphaZero = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnDisable()
    {
        if (ifSetAlphaZero) gameObject.GetComponent<UISprite>().alpha = 0;
        TweenAlpha[] alpha = gameObject.GetComponents<TweenAlpha>();
        TweenPosition[] position = gameObject.GetComponents<TweenPosition>();
        TweenRotation[] rotation = gameObject.GetComponents<TweenRotation>();
        TweenScale[] scale = gameObject.GetComponents<TweenScale>();
        foreach(TweenAlpha a in alpha)
        {
            a.enabled = true;
            a.ResetToBeginning();
        }
        foreach (TweenPosition a in position)
        {
            a.enabled = true;
            a.ResetToBeginning();
        }
        foreach (TweenRotation a in rotation)
        {
            a.enabled = true;
            a.ResetToBeginning();
        }
        foreach (TweenScale a in scale)
        {
            a.enabled = true;
            a.ResetToBeginning();
        }
    }

    public void EffectEnd()
    {
        gameObject.SetActive(false);
        gameObject.GetComponent<UISprite>().alpha = 0;
    }
}
