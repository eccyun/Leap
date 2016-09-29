using UnityEngine;
using System.Collections;

public class TitleManager : MonoBehaviour {

	// シーンコンポーネント定義
	SceneComponent sceneComponent;
	bool           action_flg;
	string         moveSceneName;

	void Start () {
		sceneComponent = GetComponent<SceneComponent> ();
		action_flg     = false;
		moveSceneName  = "";
	}

	void Update () {
		if(!action_flg){
			if(Input.touchCount > 0){
				Touch   _touch     = Input.GetTouch(0);
				Vector2 worldPoint = Camera.main.ScreenToWorldPoint(_touch.position);

				// タップ位置にオブジェクトがあるか
				RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
				if(hit){
					Bounds rect = hit.collider.bounds;
					if(rect.Contains(worldPoint)){
						moveSceneName = hit.collider.gameObject.name;
						if(moveSceneName=="START" || moveSceneName=="QUICK_START" || moveSceneName=="LOAD") {
							if(moveSceneName=="START"){
								moveSceneName = "Introduction";
							}
							sceneComponent.fade();
							action_flg = true;
						}
					}
				}
			}
		}else{
			if(!sceneComponent.panel.GetComponent<Panel>().isFade){
				sceneComponent.moveScene(moveSceneName);
			}
		}
	}
}
