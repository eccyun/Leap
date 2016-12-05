using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;

public class ScriptEngine : SingletonMonoBehaviour<ScriptEngine> {
	private List<string> listScript; // スクリプトファイルから読み込んだ命令群を格納
	private int          cnt;         // スクリプトのカウンタ

	public void Awake(){
        if(this != Instance){
            Destroy(this);
            return;
        }

		//変数の初期化
		listScript = new List<string>();
		cnt         = 0;

		// スクリプトファイルを配列にセットする
		readScenarioFile("script01.txt");

        DontDestroyOnLoad(this.gameObject);
    }

	void Update () {
		if(Input.GetMouseButtonDown(0)){
			Debug.Log(listScript[cnt]);
			cnt++;
		}
	}

	void readScenarioFile(string fileName){
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

	// 改行コード処理
    string SetDefaultText(){
        return "C#あ\n";
    }
}
