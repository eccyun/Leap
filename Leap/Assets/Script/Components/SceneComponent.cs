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
}
