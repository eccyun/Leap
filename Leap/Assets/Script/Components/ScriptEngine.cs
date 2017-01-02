using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;

public class ScriptEngine : SingletonMonoBehaviour<ScriptEngine> {
	private List<string> listScript;  // スクリプトファイルから読み込んだ命令群を格納
	private int          cnt;         // スクリプトのカウンタ
	public  GameObject   stillPrefab; // スチルのプレハブ
	public  bool         stop_flg;    // テキストの読みこみを止めるかを判定 trueなら止める
	public  string       fileName;    // ファイルネーム

	public void Awake(){
        if(this != Instance){
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

	public void readScenarioFile(string fileName, string stillName){
		// 変数の初期化
		listScript = new List<string>();
		cnt        = 0;

		// スチルデータのセット
		stillPrefab = (GameObject)Resources.Load (stillName);
		Instantiate(stillPrefab, Vector3.zero, Quaternion.identity);

		FileInfo f = new FileInfo(Application.streamingAssetsPath+"/Scenario/"+fileName);
		try{
			// 一行毎読み込み
            using (StreamReader sr = new StreamReader(f.OpenRead(), Encoding.UTF8)){
				while(sr.Peek() >= 0){
					listScript.Add(sr.ReadLine());
				}
				sr.Close();
            }
		}catch (Exception e){
			// エラー処理
		}
	}

	public string[] readScript(){
		if(stop_flg) return null;

		string ret = listScript[cnt];
		cnt++;

		if(ret == "STOP;"){
			stop_flg = true;
			return null;
		}

		return ret.Split(':');
	}

	public void fade(string mode, float range){
		GameObject     img      = GameObject.Find("character_center");
		SpriteRenderer renderer = img.GetComponent<SpriteRenderer>();
		renderer.sprite         = null;

		img      = GameObject.Find("character_left");
		renderer = img.GetComponent<SpriteRenderer>();
		renderer.sprite = null;

		img      = GameObject.Find("character_right");
		renderer = img.GetComponent<SpriteRenderer>();
		renderer.sprite = null;

		SceneComponent sceneComponent = GetComponent<SceneComponent> ();
		sceneComponent.fade(mode, range);
	}

	// 改行コード処理
    private string SetDefaultText(){
        return "C#あ\n";
    }
}
