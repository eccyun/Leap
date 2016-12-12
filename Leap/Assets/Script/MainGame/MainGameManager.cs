using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainGameManager : MonoBehaviour {
	private ScriptEngine scriptEngine;
	private string[]     script;
	public  Text         text;

	void Start () {
		scriptEngine = GetComponent<ScriptEngine> ();

		// スクリプトの読みこみ
		scriptEngine.readScenarioFile("script01.txt");
	}

	void Update () {
		script = scriptEngine.readScript();

		// データを確認する
		if(script != null){
			if(script[0]=="# MSG"){
				text.GetComponent<TextManager>().setText(script[1]);
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
