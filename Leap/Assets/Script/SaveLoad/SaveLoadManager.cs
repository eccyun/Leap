using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveLoadManager : MonoBehaviour {

	private GameMenuManager      gameMenuManager;
	private GameDataComponent    gameDataComponent;
	private int                  mode;       // 0 だったらセーブ 1 だったらロード
	private bool                 action_flg;
	private GameObject[]         dataBoxs;
	private DialogPanelComponent dialogPanelComponent;
	private GameObject           tmpSaveData;
	private SceneComponent       sceneComponent;

	public  GameObject           dialogPanel;
	public  GameObject           dataBoxes;
	public  GameObject			 caption_;
	public  ScriptEngine         scriptEngine;

	// Use this for initialization
	void Start () {
		GameObject panelObject = GameObject.Find("Panel");
		gameDataComponent      = GameObject.Find("GameDataComponent").GetComponent<GameDataComponent>();
		dialogPanelComponent   = dialogPanel.GetComponent<DialogPanelComponent>();
		sceneComponent         = panelObject.GetComponent<SceneComponent> ();
		scriptEngine           = GameObject.Find("ScriptEngine").GetComponent<ScriptEngine>();
		action_flg             = false;

		if(GameObject.Find("GameMenuManager")){
			gameMenuManager = GameObject.Find("GameMenuManager").GetComponent<GameMenuManager>();
			mode            = (gameMenuManager.pushBtnName=="menu_save")?0:1;

			// セーブの場合、キャプションを切り替える
			if(mode==0){
				caption_.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprite/SaveLoad/save-caption");
			}
		}else{
			gameMenuManager = null;
			mode            = 1;
		}

		// セーブデータをセットする
		dataBoxs = gameDataComponent.setSaveData();
	}

	// Update is called once per frame
	void Update () {
		if(!action_flg){
			if(dialogPanelComponent.gameObject.activeSelf){
				return;
			}
		}else{
			if(!sceneComponent.panelComponent.isFade){
				scriptEngine.bgm.stop_();
				sceneComponent.moveScene("LOAD");
			}
		}
	}

	public void yesCallBack(){
		int identifier = tmpSaveData.GetComponent<DataBox>().identifier;
		if(mode==0){
			// セーブ
			gameDataComponent._save(identifier);
		}else{
			GameData     data_ = tmpSaveData.GetComponent<DataBox>().gameData;
			scriptEngine.setLoadGameData(data_);

			sceneComponent.panelComponent.isFlap = true;
			sceneComponent.fade();
			action_flg = true;
		}
		dialogPanelComponent.hide();
	}

	public void noCallBack(){
		dialogPanelComponent.hide();
	}

	public void onTapBackBtn(){
		SceneManager.UnloadScene("SaveLoad");
	}

	public void onTap_(GameObject object_){
		if(object_.GetComponent<DataBox>().gameData==null&&mode!=0){
			return;
		}

		string dialogMessage = (mode==0)?"セーブしますか？":"ロードしますか？";
		tmpSaveData          = object_;
		dialogPanelComponent.show(dialogMessage, yesCallBack, noCallBack);
	}
}
