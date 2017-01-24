using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaveLoadManager : MonoBehaviour {

	private GameMenuManager gameMenuManager;
	private int             mode; // 0 だったらセーブ 1 だったらロード
	private GameObject      backBtn;

	// Use this for initialization
	void Start () {
		backBtn = GameObject.FindGameObjectWithTag("backBtn");

		if(GameObject.Find("GameMenuManager")){
			gameMenuManager = GameObject.Find("GameMenuManager").GetComponent<GameMenuManager>();
			mode            = (gameMenuManager.pushBtnName=="menu_save")?0:1;

			// セーブの場合、キャプションを切り替える
			if(mode==0){
				GameObject.Find("caption").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprite/SaveLoad/save-caption");
			}
		}else{
			gameMenuManager = null;
			mode            = 1;
		}
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			Camera     main     = GameObject.Find("Main Camera").GetComponent<Camera>();
			Vector3    touchPos = main.ScreenToWorldPoint(Input.mousePosition);
			Collider2D col      = Physics2D.OverlapPoint(touchPos);

			if(col == backBtn.GetComponent<Collider2D>()){
				SceneManager.UnloadScene("SaveLoad");
			}
		}
	}
}
