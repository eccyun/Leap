using UnityEngine;
using System.Collections;

public class TitleManager : MonoBehaviour {

	// シーンコンポーネント定義
	SceneComponent sceneComponent;
	bool           action_flg;
	string         moveSceneName;

	GameObject startBtn;
	GameObject quickStartBtn;
	GameObject continueBtn;

	void Start () {
		sceneComponent = GetComponent<SceneComponent> ();
		action_flg     = false;
		moveSceneName  = "";

		startBtn      = GameObject.FindGameObjectWithTag("startBtn");
		quickStartBtn = GameObject.FindGameObjectWithTag("quickStartBtn");
		continueBtn   = GameObject.FindGameObjectWithTag("continueBtn");
	}

	void Update () {
		if(sceneComponent.getSceneCount() == 2){
			return;
		}

		if(!action_flg){
			Camera     main     = GameObject.Find("Main Camera").GetComponent<Camera>();
			Vector3    touchPos = main.ScreenToWorldPoint(Input.mousePosition);
			Collider2D col      = Physics2D.OverlapPoint(touchPos);

			if(Input.GetMouseButtonDown(0)){
				if(col == startBtn.GetComponent<Collider2D>()){
					moveSceneName = "Introduction";
					sceneComponent.fade();
					action_flg = true;
				}else if(col == quickStartBtn.GetComponent<Collider2D>()){
					moveSceneName = "QUICK_START";
					sceneComponent.fade();
					action_flg = true;
				}else if(col == continueBtn.GetComponent<Collider2D>()){
					sceneComponent.pushScene("SaveLoad");
				}
			}
		}else{
			if(!sceneComponent.panel.GetComponent<PanelComponent>().isFade){
				sceneComponent.moveScene(moveSceneName);
			}
		}
	}
}
