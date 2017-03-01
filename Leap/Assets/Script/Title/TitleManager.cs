using UnityEngine;
using System.Collections;

public class TitleManager : MonoBehaviour {

	// シーンコンポーネント定義
	private SceneComponent    sceneComponent;
	private ScriptEngine      scriptEngine;
	private GameDataComponent gameDataComponent;

	private bool           action_flg;
	private string         moveSceneName;

	void Start () {
		sceneComponent    = GameObject.Find("Panel").GetComponent<SceneComponent> ();
		scriptEngine      = GameObject.Find("ScriptEngine").GetComponent<ScriptEngine>();
		gameDataComponent = GameObject.Find("GameDataComponent").GetComponent<GameDataComponent>();
		action_flg        = false;
		moveSceneName     = "";

		scriptEngine.bgm.play_("time-leap");
	}

	void Update () {
		if(sceneComponent.getSceneCount() == 2){
			return;
		}

		if(action_flg){
			if(!sceneComponent.panelComponent.isFade){
				scriptEngine.bgm.stop_();
				sceneComponent.moveScene(moveSceneName);
			}
		}
	}

	public void onTapGameStart(){
		// 初期化
		scriptEngine.setLoadGameData();

		moveSceneName                        = "Introduction";
		sceneComponent.panelComponent.isFlap = true;
		sceneComponent.fade();
		action_flg = true;
	}

	public void onTapGameQuickStart(){
		// 中断したデータから再開する
		GameData qData = gameDataComponent.getQuickStartData();
		if(qData!=null){
			scriptEngine.setLoadGameData(qData);

			// 画面遷移
			moveSceneName = "LOAD";
			sceneComponent.panelComponent.isFlap = true;
			sceneComponent.fade();
			action_flg = true;
		}else{
			// クイックスタートデータがない場合、最初から
			onTapGameStart();
		}
	}

	public void onTapGameLoad(){
		sceneComponent.pushScene("SaveLoad");
	}
}
