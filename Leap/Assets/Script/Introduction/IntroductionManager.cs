using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroductionManager : MonoBehaviour {
	SceneComponent sceneComponent;
	Camera         main;
	int            moveCount;

	public GameObject drawSprite;
	public GameObject bg;

	private float      maxWaitTime;
	private float      waitTime;

	// Use this for initialization
	void Start () {
		maxWaitTime    = 10.0f;
		waitTime       = 0.0f;
		sceneComponent = GameObject.Find("Panel").GetComponent<SceneComponent> ();
		moveCount      = 1;
	}

	// Update is called once per frame
	void Update () {
		if(moveCount==4){
			return;
		}
		if(moveCount==3 && maxWaitTime<=waitTime){
			sceneComponent.panelComponent.isFlap = true;
			sceneComponent.fade();
			StartCoroutine(moveScene());
			moveCount++;
			return;
		}else if(moveCount==3 && maxWaitTime>waitTime){
			waitTime += 0.03f;
			return;
		}

		main                = GameObject.Find("Main Camera").GetComponent<Camera>();
		Vector3    touchPos = main.ScreenToWorldPoint(Input.mousePosition);
		Collider2D col      = Physics2D.OverlapPoint(touchPos);

		if(Input.GetMouseButtonDown(0) && !sceneComponent.panelComponent.isFade){
			if(col == drawSprite.GetComponent<Collider2D>()){
				if(moveCount < 3){
					sceneComponent.panelComponent.isFlap = true;
					sceneComponent.fade("out");
					bg.GetComponent<Image>().sprite = null;
				}

				if(moveCount==1){
					bg.GetComponent<Image>().sprite = bg.GetComponent<MainImage>().navigation_2;
				}else if(moveCount==2){
					bg.GetComponent<Image>().sprite = bg.GetComponent<MainImage>().navigation_3;
				}
				moveCount++;
			}
		}
	}

	IEnumerator moveScene(){
		yield return new WaitForSeconds(4);
		sceneComponent.moveScene("MainGame");
	}
}
