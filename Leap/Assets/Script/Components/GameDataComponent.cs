using UnityEngine;
using System;
using System.Collections;


public class GameData {
	public int    chapter;       // 現在のチャプター
	public int    scriptCnt;     // 現在のスクリプトカウンタ
	public string abridgeText;   // 省略テキスト
	public string saveDate;      // 保存日
	public string binaryCapture; // バイナリキャプチャ

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
	public GameData   activeData;
	public GameData[] saveDatas;

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
	}

	public void setSaveData(){
		GameObject[] objects_ = GameObject.FindGameObjectsWithTag("DataBox");
		// セーブデータをセットする
		for (int i=0; i<objects_.Length; i++) {
			DataBox dataBox    = objects_[i].GetComponent<DataBox>();
			dataBox.gameData   = new GameData();
		}
	}

	public bool _save(string name){
		activeData.saveDate = DateTime.Now.ToString("yyyy/MM/dd/ HH:mm:ss");
		PlayerPrefs.SetString(name, LitJson.JsonMapper.ToJson(activeData));
		return true;
	}
}
