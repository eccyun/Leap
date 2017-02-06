using UnityEngine;
using System.Collections;

public class EndingManager : MonoBehaviour {
	private SceneComponent  sceneComponent;
	private int    panelCount  = 1;
	private float  maxWaitTime = 5.0f;
	private float  waitTime    = 0.0f;
	public  int    maxPanelCount;

	void Start () {
		sceneComponent = GameObject.Find("Panel").GetComponent<SceneComponent> ();
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
		GameObject.Find("ending-"+panelCount).GetComponent<SpriteRenderer>().enabled = false;
		panelCount++;

		if(panelCount>maxPanelCount){
			sceneComponent.moveScene("Load");
			return;
		}
		GameObject.Find("ending-"+panelCount).GetComponent<SpriteRenderer>().enabled = true;
	}
}
