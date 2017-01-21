using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class BackLogManager : MonoBehaviour {
	private MainGameManager mainGameManager;
	private ScriptEngine    scriptEngine;
	private float           cellHeight;

	public  GameObject      baseCell;
	public  GameObject      content_;

	void Start () {
		// セルの高さを取得する
		RectTransform rect = GameObject.Find("BaseItem").GetComponent<RectTransform>();
		cellHeight         = rect.rect.height;
		GameObject.Destroy(GameObject.Find("BaseItem"));

		mainGameManager = GameObject.Find("MainGameManager").GetComponent<MainGameManager>();
		scriptEngine    = GameObject.Find("ScriptEngine").GetComponent<ScriptEngine>();

		// コンテントの高さを変える
		RectTransform contentTransform = (RectTransform)content_.transform;
		contentTransform.sizeDelta     = new Vector2(contentTransform.sizeDelta.x, (scriptEngine.textLogs.Count * cellHeight)+5.0f); // 高さを変更する.

		// eachでまわしてスクロールビューを作る
		for (int i = 0; i < scriptEngine.textLogs.Count; i++){
			string[] lineTextLog = scriptEngine.textLogs[i];

			GameObject copyCell = GameObject.Instantiate(baseCell) as GameObject;
	        Text[] itemTexts    = copyCell.GetComponentsInChildren<Text>();

			// テキストをセット
	        itemTexts[0].text = lineTextLog[0];
			itemTexts[1].text = lineTextLog[1];

	        RectTransform itemTransform = (RectTransform)copyCell.transform;
	        itemTransform.SetParent(contentTransform, false);
		}

		// スクロール位置を最下部へ
		GameObject.Find("Scroll View").GetComponent<ScrollRect>().verticalNormalizedPosition = 0.0f;
	}

	void Update () {
		if(Input.GetMouseButtonDown(0)){
			Camera     main     = GameObject.Find("Main Camera").GetComponent<Camera>();
			Vector3    touchPos = main.ScreenToWorldPoint(Input.mousePosition);
			Collider2D col      = Physics2D.OverlapPoint(touchPos);

			if(col == GameObject.Find("save_back").GetComponent<Collider2D>()){
				mainGameManager.isUpdateStop = false;
				SceneManager.UnloadScene("BackLog");
			}
		}
	}
}
