using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroductionManager : MonoBehaviour {
	SceneComponent sceneComponent;
	GameObject     drawSprite;
	Camera         main;
	int            moveCount;

	// Use this for initialization
	void Start () {
		sceneComponent = GetComponent<SceneComponent> ();
		drawSprite     = GameObject.FindGameObjectWithTag("MainImage");
		moveCount      = 1;
	}

	// Update is called once per frame
	void Update () {
		main                = GameObject.Find("Main Camera").GetComponent<Camera>();
		Vector3    touchPos = main.ScreenToWorldPoint(Input.mousePosition);
		Collider2D col      = Physics2D.OverlapPoint(touchPos);

		if(Input.GetMouseButtonDown(0) && !sceneComponent.panel.GetComponent<PanelComponent>().isFade){
			if(col == drawSprite.GetComponent<Collider2D>()){
				SpriteRenderer renderer  = drawSprite.GetComponent<SpriteRenderer>();
				MainImage      mainImage = drawSprite.GetComponent<MainImage>();

				sceneComponent.fade("out");
				mainImage.GetComponent<SpriteRenderer>().sprite = null;
				
				if(moveCount==1){
					mainImage.GetComponent<SpriteRenderer>().sprite = mainImage.navigation_2;
				}else if(moveCount==2){
					mainImage.GetComponent<SpriteRenderer>().sprite = mainImage.navigation_3;
				}
				moveCount++;
			}
		}
	}
}
