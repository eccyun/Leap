using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameMenuManager : MonoBehaviour {
	private Camera          camera;
	private MainGameManager mainGameManager;
	private SceneComponent  sceneComponent;
	public  string          pushBtnName = "";

	// Use this for initialization
	void Start () {
		camera          = GameObject.Find("Main Camera").GetComponent<Camera>();
		mainGameManager = GameObject.Find("MainGameManager").GetComponent<MainGameManager>();
		sceneComponent  = GetComponent<SceneComponent> ();
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			Vector3    touchPos = camera.ScreenToWorldPoint(Input.mousePosition);
			Collider2D col      = Physics2D.OverlapPoint(touchPos);

			if(col==GameObject.Find("back_game").GetComponent<Collider2D>()){
				mainGameManager.isMoveScene = false;
				SceneManager.UnloadScene("GameMenu");
			}else if(col==GameObject.Find("menu_save").GetComponent<Collider2D>()||col==GameObject.Find("menu_load").GetComponent<Collider2D>()){
				if(col==GameObject.Find("menu_save").GetComponent<Collider2D>()){
					pushBtnName = "menu_save";
				}else if(col==GameObject.Find("menu_load").GetComponent<Collider2D>()){
					pushBtnName = "menu_load";
				}
				sceneComponent.pushScene("SaveLoad");
			}
		}
	}
}
