﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndingManager : MonoBehaviour {
	private SceneComponent  sceneComponent;
	private ScriptEngine    scriptEngine;
	private int             panelCount  = 1;
	private float           maxWaitTime = 5.0f;
	private float           waitTime    = 0.0f;
	public  int             maxPanelCount;

	void Start () {
		sceneComponent = GameObject.Find("Panel").GetComponent<SceneComponent> ();
		scriptEngine   = GameObject.Find("ScriptEngine").GetComponent<ScriptEngine> ();

		scriptEngine.bgm.play_("leap-ending", false);
	}

	void Update () {
		if(sceneComponent.getPanelIsFade()){
			return;
		}

		if(maxWaitTime <= waitTime){
			sceneComponent.fade("normal", 0.0065f, "black", callBack);
			waitTime = 0.0f;
			return;
		}

		waitTime += 0.01f;
	}

	public void callBack(){
		GameObject.Find("ending-"+panelCount).GetComponent<Image>().enabled = false;
		panelCount++;

		if(panelCount>maxPanelCount){
			scriptEngine.bgm.stop_();
			sceneComponent.moveScene("Load");
			return;
		}
		GameObject.Find("ending-"+panelCount).GetComponent<Image>().enabled = true;
	}
}
