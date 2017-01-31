using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaveLoadManager : MonoBehaviour {

	private GameMenuManager   gameMenuManager;
	private GameDataComponent gameDataComponent;
	private int               mode;       // 0 だったらセーブ 1 だったらロード
	private GameObject        backBtn;
	private GameObject[]      dataBoxs;

	// Use this for initialization
	void Start () {
		backBtn           = GameObject.FindGameObjectWithTag("backBtn");
		gameDataComponent = GameObject.Find("GameDataComponent").GetComponent<GameDataComponent>();

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

		// セーブデータをセットする
		gameDataComponent.setSaveData();
		dataBoxs = GameObject.FindGameObjectsWithTag("DataBox");
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			Camera     main     = GameObject.Find("Main Camera").GetComponent<Camera>();
			Vector3    touchPos = main.ScreenToWorldPoint(Input.mousePosition);
			Collider2D col      = Physics2D.OverlapPoint(touchPos);

			if(col == backBtn.GetComponent<Collider2D>()){
				SceneManager.UnloadScene("SaveLoad");
			}else{
				foreach(GameObject _object in dataBoxs){
					if(col == _object.GetComponent<Collider2D>()){
						if(mode==0){
							int identifier = _object.GetComponent<DataBox>().identifier;
							gameDataComponent._save(identifier);
						}
					}
				}
			}
		}
	}
}
