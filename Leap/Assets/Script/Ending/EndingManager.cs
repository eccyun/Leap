using UnityEngine;
using System.Collections;

public class EndingManager : MonoBehaviour {
	private SceneComponent  sceneComponent;
	private int    panelCount  = 1;
	private float  maxWaitTime = 5.0f;
	private float  waitTime    = 0.0f;

	void Start () {
		sceneComponent = GameObject.Find("Panel").GetComponent<SceneComponent> ();
	}

	void Update () {
		if(maxWaitTime <= waitTime){
			sceneComponent.fade("normal", 0.01f, "black", callBack);
			waitTime = 0.0f;
			return;
		}
		waitTime += 0.03f;
	}

	public void callBack(){
		Debug.Log("処理終わり");
	}
}
