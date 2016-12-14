using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainGameManager : MonoBehaviour {
	private ScriptEngine scriptEngine;
	private string[]     script;
	public  Text         text;

	void Start () {
		// スクリプトエンジン取得
		scriptEngine = GetComponent<ScriptEngine> ();

		// スクリプトの読みこみ
		scriptEngine.readScenarioFile("script01.txt", "Prefab/Still-01");
	}

	void Update () {
		script = scriptEngine.readScript();

		// データを確認する
		if(script != null){
			if(script[0]=="# MSG"){
				// テキストを送る
				text.GetComponent<TextManager>().setText(script[1]);
			}else if(script[0]=="# STILL-IMG"){
				GameObject     still    = GameObject.Find(script[1]);

				// Spriteを表示
				SpriteRenderer renderer = still.GetComponent<SpriteRenderer>();
				renderer.enabled        = true;
			}
		}

		if(Input.GetMouseButtonDown(0)){
			if(text.GetComponent<TextManager>().animation){
				text.GetComponent<TextManager>().setFullText();
			}else{
				scriptEngine.stop_flg = false;
			}
		}
	}
}
