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
				// 部分絵の表示
				GameObject     still    = GameObject.Find(script[1]);
				SpriteRenderer renderer = still.GetComponent<SpriteRenderer>();

				// Spriteを表示
				if(script[2]=="show"){
					if(script[3] == "false"){
						still.GetComponent<SpriteScript>().alfa = 1.0f;
						renderer.color                          = new Color(1.0f, 1.0f, 1.0f, 1.0f);
					}
					renderer.enabled = true;
				}else if(script[2]=="hide"){
					renderer.enabled                        = false;
					still.GetComponent<SpriteScript>().alfa = 0.0f;
					renderer.color                          = new Color(1.0f, 1.0f, 1.0f, 0.0f);
				}
			}else if(script[0]=="# BLACK;"){
				scriptEngine.fade(1.0f);
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
