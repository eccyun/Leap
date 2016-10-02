using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneComponent : MonoBehaviour {

	public GameObject panel;

	public void Start(){
		panel = GameObject.FindGameObjectWithTag("EffectBoard");
	}

	/*
		fade
		フェードイン
		wait ... フェード時間
	*/
	public void fade(){
		panel.GetComponent<Panel> ().isFade = true;
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
