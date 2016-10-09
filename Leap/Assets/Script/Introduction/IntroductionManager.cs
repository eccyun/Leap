using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroductionManager : MonoBehaviour {
	GameObject drawSprite;
	int        moveCount;
	Camera     main;

	// Use this for initialization
	void Start () {
		drawSprite = GameObject.FindGameObjectWithTag("MainImage");
		moveCount  = 1;
	}

	// Update is called once per frame
	void Update () {
		main                = GameObject.Find("Main Camera").GetComponent<Camera>();
		Vector3    touchPos = main.ScreenToWorldPoint(Input.mousePosition);
		Collider2D col      = Physics2D.OverlapPoint(touchPos);

		if(Input.GetMouseButtonDown(0)){
			if(col == drawSprite.GetComponent<Collider2D>()){
				SpriteRenderer renderer  = drawSprite.GetComponent<SpriteRenderer>();
				MainImage      mainImage = drawSprite.GetComponent<MainImage>();

				if(moveCount==1){
					renderer.sprite = mainImage.navigation_2;
				}else if(moveCount==2){
					renderer.sprite = mainImage.navigation_3;
				}
				moveCount++;
			}
		}
	}
}
