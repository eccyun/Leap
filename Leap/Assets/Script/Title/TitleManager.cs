using UnityEngine;
using System.Collections;

public class TitleManager : MonoBehaviour {

	// シーンコンポーネント定義
	private SceneComponent    sceneComponent;
	private ScriptEngine      scriptEngine;
	private GameDataComponent gameDataComponent;

	private bool           action_flg;
	private string         moveSceneName;
	public  GameObject     startBtn;
	public  GameObject     quickStartBtn;
	public  GameObject     continueBtn;

	void Start () {
		sceneComponent    = GameObject.Find("Panel").GetComponent<SceneComponent> ();
		scriptEngine      = GameObject.Find("ScriptEngine").GetComponent<ScriptEngine>();
		gameDataComponent = GameObject.Find("GameDataComponent").GetComponent<GameDataComponent>();
		action_flg     = false;
		moveSceneName  = "";
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
					// 初期化
					scriptEngine.setLoadGameData();

					moveSceneName                        = "Introduction";
					sceneComponent.panelComponent.isFlap = true;
					sceneComponent.fade();
					action_flg = true;
				}else if(col == quickStartBtn.GetComponent<Collider2D>()){
					// 中断したデータから再開する
					GameData qData = gameDataComponent.getQuickStartData();
					scriptEngine.setLoadGameData(qData);

					// 画面遷移
					moveSceneName = "LOAD";
					sceneComponent.panelComponent.isFlap = true;
					sceneComponent.fade();
					action_flg = true;
				}else if(col == continueBtn.GetComponent<Collider2D>()){
					sceneComponent.pushScene("SaveLoad");
				}
			}
		}else{
			if(!sceneComponent.panelComponent.isFade){
				sceneComponent.moveScene(moveSceneName);
			}
		}
	}
}
