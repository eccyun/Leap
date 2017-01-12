using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class BackLogManager : MonoBehaviour {

	private MainGameManager mainGameManger;

	// Use this for initialization
	void Start () {
		mainGameManger = GameObject.Find("MainGameManager").GetComponent<MainGameManager>();
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			Camera     main     = GameObject.Find("Main Camera").GetComponent<Camera>();
			Vector3    touchPos = main.ScreenToWorldPoint(Input.mousePosition);
			Collider2D col      = Physics2D.OverlapPoint(touchPos);

			if(col == GameObject.Find("save_back").GetComponent<Collider2D>()){
				// テキストを非表示にする
				GameObject.Find("NameTag").GetComponent<Text>().enabled = true;
				GameObject.Find("Text").GetComponent<Text>().enabled    = true;
				mainGameManger.isMoveScene                              = false;
				SceneManager.UnloadScene("BackLog");
			}
		}
	}
}
