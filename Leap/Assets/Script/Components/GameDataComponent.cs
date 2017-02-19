using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameData {
	public  int          chapter;       // 現在のチャプター
	public  int          scriptCnt;     // 現在のスクリプトカウンタ
	public  string       abridgeText;   // 省略テキスト
	public  string       saveDate;      // 保存日
	public  string       binaryCapture; // バイナリキャプチャ

	public void setActiveData(int chapter, int scriptCnt){
		this.chapter   = chapter;
		this.scriptCnt = scriptCnt;
	}

	public void setActiveScreenShot(string binaryCapture){
		this.binaryCapture = binaryCapture;
	}

	public void setAbridgeText(string abridgeText){
		this.abridgeText = abridgeText;
	}
}

public class GameDataComponent : SingletonMonoBehaviour<GameDataComponent> {
	public  GameData      activeData;
	private List<DataBox> dataBoxes;
	public  GameObject[]  objects_;

	public void Awake(){
        if(this != Instance){
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

	public void Start () {
		// 初期化
		activeData = new GameData();
		dataBoxes  = new List<DataBox>();
	}

	public GameObject[] setSaveData(){
		dataBoxes  = new List<DataBox>();
		objects_   = GameObject.FindGameObjectsWithTag("DataBox");

		// セーブデータをセットする
		for (int i=0; i<objects_.Length; i++) {
			// データのセット
			DataBox dataBox = objects_[i].GetComponent<DataBox>();

			// セーブデータを取ってくる
			string  json = PlayerPrefs.GetString("save_"+dataBox.identifier.ToString());

			if (json.Length > 0){
				dataBox.gameData = LitJson.JsonMapper.ToObject<GameData>(json);
				dataBox.dispDataBox();
			}else{
				dataBox.gameData = null;
			}

			dataBoxes.Add(dataBox);
		}

		return objects_;
	}

	public GameData getQuickStartData(){
		return LitJson.JsonMapper.ToObject<GameData>(PlayerPrefs.GetString("save_999"));
	}

	public bool _save(int identifier){
		activeData.saveDate = DateTime.Now.ToString("yyyy/MM/dd/HH:mm:ss");
		PlayerPrefs.SetString("save_"+identifier.ToString(), LitJson.JsonMapper.ToJson(activeData));

		// セーブデータをセットする
		foreach(DataBox dataBox in dataBoxes){
			if(dataBox.identifier==identifier){
				dataBox.gameData = activeData;
				dataBox.dispDataBox();
				break;
			}
		}

		return true;
	}
}
