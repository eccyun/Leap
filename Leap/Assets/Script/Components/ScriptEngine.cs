using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
using System.Collections.Generic;

public class ScriptEngine : SingletonMonoBehaviour<ScriptEngine> {
	private List<string> listScript; // スクリプトファイルから読み込んだ命令群を格納
	private int          cnt;        // スクリプトのカウンタ
	public  bool         stop_flg;   // テキストの読みこみを止めるかを判定 trueなら止める
	public  string       fileName;   // ファイルネーム

	public void Awake(){
        if(this != Instance){
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

	public void readScenarioFile(string fileName){
		// 変数の初期化
		listScript = new List<string>();
		cnt        = 0;

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

	// 改行コード処理
    private string SetDefaultText(){
        return "C#あ\n";
    }
}
