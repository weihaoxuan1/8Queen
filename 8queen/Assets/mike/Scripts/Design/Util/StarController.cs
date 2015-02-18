using UnityEngine;
using System.Collections;

public class StarController : MonoBehaviour {
	private Transform halfStar;
	private UITexture half_UITexture;
	
	private int starCount;
	private bool isHalf;
	
	public Texture star_light;
	public Texture star_dark;
	public Texture halfStar_light;
	public Texture halfStar_dark;
	
	// Use this for initialization
	void Start () {
		halfStar = transform.FindChild("star_half");
		half_UITexture = halfStar.GetComponent<UITexture>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	IEnumerator SetStarNum(int inStarNum){
		for(int i = 1;i <= 5;i++ ){
			Transform star = transform.FindChild("star"+i);
			StarScale script_scale = star.GetComponent<StarScale>();
			StarFade script_fade = star.GetComponent<StarFade>();
			
			star.gameObject.SetActive(true);
			//script.StartToScale();
			
			UITexture uiTexture = star.GetComponent<UITexture>();
			
			if(i <= inStarNum){
				script_scale.StartToScale();
				uiTexture.color = new Color(1.0f,1.0f,1.0f,1.0f);
				uiTexture.mainTexture = star_light;
			}else{
				script_fade.StartToFade();
				uiTexture.color = new Color(1.0f,1.0f,1.0f,1.0f);
				uiTexture.mainTexture = star_dark;
			}
			
			yield return new WaitForSeconds(0.5f);
		}
		
		if(half_UITexture == null) {
			halfStar = transform.FindChild("star_half");
			half_UITexture = halfStar.GetComponent<UITexture>();
		}
		
		half_UITexture.color = new Color(1.0f,1.0f,1.0f,1.0f);
		half_UITexture.mainTexture = halfStar_dark;
		half_UITexture.depth = 9;
		
		if(isHalf) {
			SetHalfStar(starCount + 1);
		}
	}
	
	/*
	 * 设置半星
	 */ 
	private void SetHalfStar(int halfPos) {
		halfStar.gameObject.SetActive(true);
		
		Transform half = transform.FindChild("star"+halfPos);
		halfStar.position = half.position;
		half_UITexture.color = new Color(1.0f,1.0f,1.0f,1.0f);
		half_UITexture.mainTexture = halfStar_light;
		half_UITexture.depth = 11;
	}
	
	public void ShowStar() {
		StartCoroutine(SetStarNum(starCount));
	}
	
	public void HideStar() {
		for(int i = 1;i <= 5;i++ ){
			Transform star = transform.FindChild("star"+i);
			star.gameObject.SetActive(false);
		}
		
		if(halfStar == null) {
			halfStar = transform.FindChild("star_half");
		}
		
		halfStar.gameObject.SetActive(false);
	}
	
	public void SetStarCount(int starCount) {
		this.starCount = starCount;
	}
	
	public void SetIsHalf(bool half) {
		isHalf = half;
	}

}
