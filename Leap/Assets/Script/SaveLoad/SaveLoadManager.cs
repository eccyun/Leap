﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.EventSystems;

public class SaveLoadManager : MonoBehaviour {

	private GameMenuManager      gameMenuManager;
	private GameDataComponent    gameDataComponent;
	private int                  mode;       // 0 だったらセーブ 1 だったらロード
	private bool                 action_flg;
	private GameObject           backBtn;
	private GameObject[]         dataBoxs;
	private DialogPanelComponent dialogPanelComponent;
	private GameObject           tmpSaveData;
	private SceneComponent       sceneComponent;
	public  GameObject           dialogPanel;

	// Use this for initialization
	void Start () {
		GameObject panelObject = GameObject.Find("Panel");
		backBtn                = GameObject.FindGameObjectWithTag("backBtn");
		gameDataComponent      = GameObject.Find("GameDataComponent").GetComponent<GameDataComponent>();
		dialogPanelComponent   = dialogPanel.GetComponent<DialogPanelComponent>();
		sceneComponent         = panelObject.GetComponent<SceneComponent> ();
		action_flg = false;

		if(GameObject.Find("GameMenuManager")){
			gameMenuManager = GameObject.Find("GameMenuManager").GetComponent<GameMenuManager>();
			mode            = (gameMenuManager.pushBtnName=="menu_save")?0:1;

			// セーブの場合、キャプションを切り替える
			if(mode==0){
				GameObject.Find("caption").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/SaveLoad/save-caption");
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

			if(Input.GetMouseButtonDown(0)){
				Camera     main     = GameObject.Find("Main Camera").GetComponent<Camera>();
				Vector3    touchPos = main.ScreenToWorldPoint(Input.mousePosition);
				Collider2D col      = Physics2D.OverlapPoint(touchPos);

				if(col == backBtn.GetComponent<Collider2D>()){
					SceneManager.UnloadScene("SaveLoad");
				}else{
					foreach(GameObject _object in dataBoxs){
						if(col == _object.GetComponent<Collider2D>()){
							string dialogMessage = (mode==0)?"セーブしますか？":"ロードしますか？";
							tmpSaveData          = _object;
							dialogPanelComponent.show(dialogMessage, yesCallBack, noCallBack);
						}
					}
				}
			}
		}else{
			if(!sceneComponent.panelComponent.isFade){
				sceneComponent.moveScene("LOAD");
			}
		}
	}

	public void yesCallBack(){
		if(mode==0){
			// セーブ
			int identifier = tmpSaveData.GetComponent<DataBox>().identifier;
			gameDataComponent._save(identifier);
		}else{
			action_flg = true;
		}
		dialogPanelComponent.hide();
	}

	public void noCallBack(){
		dialogPanelComponent.hide();
	}
}
