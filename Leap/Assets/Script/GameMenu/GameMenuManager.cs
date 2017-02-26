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

			if(Input.GetMouseButtonDown(0)){
				Vector3    touchPos = camera.ScreenToWorldPoint(Input.mousePosition);
				Collider2D col      = Physics2D.OverlapPoint(touchPos);

				if(col==GameObject.Find("back_game").GetComponent<Collider2D>()){
					mainGameManager.isUpdateStop = false;
					SceneManager.UnloadScene("GameMenu");
				}else if(col==GameObject.Find("menu_save").GetComponent<Collider2D>()||col==GameObject.Find("menu_load").GetComponent<Collider2D>()){
					if(col==GameObject.Find("menu_save").GetComponent<Collider2D>()){
						pushBtnName = "menu_save";
					}else if(col==GameObject.Find("menu_load").GetComponent<Collider2D>()){
						pushBtnName = "menu_load";
					}
					sceneComponent.pushScene("SaveLoad");
				}else if(col==GameObject.Find("back_title").GetComponent<Collider2D>()){
					dialogPanelComponent.show("ゲームを終了しますか？", yesCallBack, noCallBack);
				}
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
}
