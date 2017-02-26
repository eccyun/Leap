using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour {
	private Camera               camera;
	private MainGameManager      mainGameManager;
	private SceneComponent       sceneComponent;
	private DialogPanelComponent dialogPanelComponent;
	public  string               pushBtnName = "";
	public  GameObject           dialogPanel;
	public  bool                 action_flg;

	// Use this for initialization
	void Start () {
		camera               = GameObject.Find("Main Camera").GetComponent<Camera>();
		mainGameManager      = GameObject.Find("MainGameManager").GetComponent<MainGameManager>();
		sceneComponent       = GameObject.Find("Panel").GetComponent<SceneComponent> ();
		dialogPanelComponent = dialogPanel.GetComponent<DialogPanelComponent>();
	}

	// Update is called once per frame
	void Update () {
		if(!action_flg){
			if(dialogPanelComponent.gameObject.activeSelf){
				return;
			}
		}else{
			if(!sceneComponent.panelComponent.isFade){
				ScriptEngine scriptEngine        = GameObject.Find("ScriptEngine").GetComponent<ScriptEngine>();
				scriptEngine.moveLoadedSceneName = "Title";
				scriptEngine.bgm.stop_();
				sceneComponent.moveScene("LOAD");
			}
		}
	}

	public void yesCallBack(){
		sceneComponent.fade();
		action_flg = true;
		dialogPanelComponent.hide();
	}

	public void noCallBack(){
		dialogPanelComponent.hide();
	}

	public void onTapGameBack(){
		mainGameManager.isUpdateStop = false;
 		SceneManager.UnloadScene("GameMenu");
	}

	public void onTapGameSave(){
		pushBtnName = "menu_save";
		sceneComponent.pushScene("SaveLoad");
	}

	public void onTapGameLoad(){
		pushBtnName = "menu_load";
		sceneComponent.pushScene("SaveLoad");
	}

	public void onTapBackTitle(){
		dialogPanelComponent.show("ゲームを終了しますか？", yesCallBack, noCallBack);
	}
}
