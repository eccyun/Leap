using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneComponent : MonoBehaviour {

	public PanelComponent panelComponent;

	public void Start(){
		if(GameObject.Find("Panel")){
			panelComponent = GameObject.Find("Panel").GetComponent<PanelComponent> ();
		}
	}

	/*
		fade
		フェードイン
		wait ... フェード時間
	*/
	public void fade(string mode="normal", float range = 0.01f, string color="black"){
		if (color=="black"){
			panelComponent.red   = 0.0f;
			panelComponent.green = 0.0f;
			panelComponent.blue  = 0.0f;
		}else if(color=="white"){
			panelComponent.red   = 1.0f;
			panelComponent.green = 1.0f;
			panelComponent.blue  = 1.0f;
		}

		if(mode=="out"){
			panelComponent.setAlfa(1.0f);
		}
		panelComponent.isFade = true;
		panelComponent.setRange(range);
	}

	/*
		moveScene
		シーン遷移する
		name ... シーン名
	*/
	public void moveScene(string name){
		SceneManager.LoadScene(name);
	}

	/*
		pushScene
		シーン遷移する(一時遷移)
		name ... シーン名
	*/
	public void pushScene(string name){
		SceneManager.LoadScene(name, LoadSceneMode.Additive);
	}

	/*
		getSceneCount
		現在表示されているシーン数を返す
		name ... シーン名
	*/
	public int getSceneCount(){
		return SceneManager.sceneCount;
	}
}
