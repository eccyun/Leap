using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainGameManager : MonoBehaviour {
	private ScriptEngine      scriptEngine;
	private SceneComponent    sceneComponent;
	private Camera            camera;

	private string[]     script;
	private bool         isLoading;
	private bool         isEnding;
	private bool         isWait;
	private bool         isMoveTitle;
	private bool         isMoveEOF;

	private float        maxWaitTime;
	private float        waitTime;
	private GameObject[] stillPrefab;

	// public
	public  Text        text;
	public  bool        isUpdateStop;
	public  bool        isFull;
	public  GameObject  nameTagObject;

	public GameObject canvas;
	public GameObject viewCanvas;

	public GameObject character_center;
	public GameObject character_right;
	public GameObject character_left;
	public GameObject background_image;
	public GameObject panel;

	public GameObject[] game_ui;
	public GameObject[] character_ui;

	// ゲームメニューを開く処理
	IEnumerator beforeGameMenuOpen(){
	    yield return new WaitForEndOfFrame();

		// スクリーンキャプチャ
		scriptEngine.setActiveScreenShot();

		// 文字セット
		scriptEngine.setAbridgeText(text.GetComponent<TextManager>().displayText);

		// 非表示にする
		spriteDisp(false);
	}

	void Start () {
		isLoading   = false;
		isWait      = false;
		maxWaitTime = 0.0f;
		waitTime    = 0.0f;

		// 変数の初期化
		scriptEngine   = GameObject.Find("ScriptEngine").GetComponent<ScriptEngine> ();
		camera         = GameObject.Find("Main Camera").GetComponent<Camera>();
		sceneComponent = panel.GetComponent<SceneComponent> ();

		game_ui      = GameObject.FindGameObjectsWithTag("GameUI");
		character_ui = GameObject.FindGameObjectsWithTag("Character");

		// シーン情報初期化
		if(!scriptEngine.load_flg){
			scriptEngine.initGameScene(0);
		}

		// スクリプトの読みこみ
		scriptEngine.readScenarioFile();

		// スチルのセット
		if(scriptEngine.stillPrefab!=null){
			scriptEngine.stillPrefab.transform.SetParent(viewCanvas.transform.Find("ViewPanel").gameObject.transform, false);
			scriptEngine.stillPrefab.transform.SetSiblingIndex(4);
		}
		stillPrefab = GameObject.FindGameObjectsWithTag("Still");
	}

	void Update () {
		if(isUpdateStop){
			return;
		}

		// ウェイトの設定
		if(isWait && maxWaitTime<=waitTime){
			isWait      = false;
			maxWaitTime = 0.0f;
			waitTime    = 0.0f;
			return;
		}else if(isWait && maxWaitTime>waitTime){
			waitTime += 0.03f;
			return;
		}

		// ロード判定
		if(isLoading || isEnding || isMoveTitle || isMoveEOF){
			if(sceneComponent.panelComponent != null && !sceneComponent.panelComponent.isFade){
				// 遷移処理
				if(isLoading){
					sceneComponent.moveScene("Load");
				}else if(isEnding){
					sceneComponent.moveScene("Ending");
				}else if(isMoveTitle){
					sceneComponent.moveScene("Title");
				}else if(isMoveEOF){
					isUpdateStop = true;
					Invoke("moveEOF", 3.0f);
				}
			}
			return;
		}

		if(isFull){
			return;
		}

		/* ここまでが停止チェック処理 */

		// UI表示
		spriteDisp(true);
		script = scriptEngine.readScript();

		// データを確認する
		if(script != null){
			if(script[0]=="# MSG"){
				// ゲーム上のUI要素を非表示にする
				for (int i=0; i<game_ui.Length; i++) {
					game_ui[i].SetActive(true);
				}

				string[] tmpTextLog = new string[2];

				// テキストを送る
				text.GetComponent<TextManager>().setText(script[1]);
				if(script.Length >= 3){
					nameTagObject.GetComponent<Text>().text = script[2];
					tmpTextLog[0] = script[2];
				}else{
					nameTagObject.GetComponent<Text>().text = "";
					tmpTextLog[0] = "ーーー";
				}
				tmpTextLog[1] = script[1];
				scriptEngine.textLogs.Add(tmpTextLog);
			}else if(script[0]=="# BGM"){
				if(script[2]=="PLAY"){
					scriptEngine.bgm.preload_(script[1]);
					if(!scriptEngine.load_flg){
						scriptEngine.bgm.play_();
					}
				}else if(script[2]=="STOP"){
					scriptEngine.bgm.stop_();
				}
			}else if(script[0]=="# IMG"){
				GameObject img = null;
				if(script[2]=="center"){
					img = character_center;
				}else if(script[2]=="right"){
					img = character_right;
				}else if(script[2]=="left"){
					img = character_left;
				}

				// 画像セット
				Sprite s_ = Resources.Load<Sprite>("Sprite/character/"+script[1]);

				// サイズの指定
				RectTransform r_ = img.GetComponent<RectTransform>();
				r_.sizeDelta     = new Vector2(s_.rect.width, s_.rect.height);

				// 画像セット
				Image  i_ = img.GetComponent<Image>();
				i_.sprite = s_;
				i_.color  = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			}else if(script[0]=="# BG"){
				Image i_  = background_image.GetComponent<Image>();
				i_.sprite = Resources.Load<Sprite>("Sprite/Background/"+script[1]);
				i_.color  = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			}else if(script[0]=="# STILL-IMG"){
				// 部分絵の表示
				GameObject still  = GameObject.Find(script[1]);
				Image      image_ = still.GetComponent<Image>();

				// Spriteを表示
				if(script[2]=="show"){
					if(script[3] == "false"){
						still.GetComponent<SpriteScript>().alfa = 1.0f;
						image_.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
					}
					image_.enabled = true;
				}else if(script[2]=="hide"){
					image_.enabled = false;
					still.GetComponent<SpriteScript>().alfa = 0.0f;
					image_.color   = new Color(1.0f, 1.0f, 1.0f, 0.0f);
				}
			}else if(script[0]=="# BLACK" && !scriptEngine.load_flg){
				float fadeRange = float.Parse(script[1]);

				dispHideCharacter();
				sceneComponent.fade("normal", fadeRange, "black", inGameFade);
				isUpdateStop = true;
			}else if(script[0]=="# WHITE" && !scriptEngine.load_flg){
				float fadeRange = float.Parse(script[1]);

				dispHideCharacter();
				sceneComponent.fade("normal", fadeRange, "white", inGameFade);
				isUpdateStop = true;
			}else if(script[0]=="LOADING;"){
				sceneComponent.fade("normal", 0.01f, "black", outGameFade);
				isLoading = true;
			}else if(script[0]=="ENDING;"){
				sceneComponent.fade("normal", 0.01f, "white", outGameFade);
				isEnding = true;
			}else if(script[0]=="# REMOVE-IMG"){
				GameObject img;
				if(script[1]=="bg"){
					img = background_image;
					img.GetComponent<Image>().color  = new Color(0.0f, 0.0f, 0.0f, 0.0f);
				}else{
					img = GameObject.Find("character_"+script[1]);
					img.GetComponent<Image>().color  = new Color(1.0f, 1.0f, 1.0f, 0.0f);
				}
				img.GetComponent<Image>().sprite = null;
			}else if(script[0]=="# EFFECT" && !scriptEngine.load_flg){
				GameObject  audio       = GameObject.Find("Effect");
				AudioSource audioSource = audio.GetComponent<AudioSource>();
				audioSource.clip        = Resources.Load<AudioClip>("Effects/"+script[1]);
				audioSource.Play();
			}else if(script[0] == "# WAIT"){
				isWait      = true;
				maxWaitTime = float.Parse(script[1]);
				// ゲーム上のUI要素を非表示にする
				for (int i=0; i<game_ui.Length; i++) {
					game_ui[i].SetActive(false);
				}
			}else if(script[0] == "# ANIMATION"){
				// アニメーションセット
				for (int i=0; i<scriptEngine.animationObjects.Length; i++) {
					if(scriptEngine.animationObjects[i].name==script[1]){
						scriptEngine.animationObjects[i].GetComponent<SelfAnimation>().mainGameManager = this;
						scriptEngine.animationObjects[i].GetComponent<SelfAnimation>().run_flg         = (script[2]=="run")?true:false;
					}
				}
			}else if(script[0] == "EOF;"){
				// ゲーム上のUI要素を非表示にする
				for (int i=0; i<game_ui.Length; i++) {
					game_ui[i].SetActive(false);
				}
				sceneComponent.fade("normal", 0.01f, "white", outGameFade);
				isMoveEOF = true;
			}
		}
	}

	public void spriteDisp(bool flg=false){
		// キャンバス
		canvas.SetActive(flg);
		viewCanvas.SetActive(flg);

		// ゲーム上のUI要素を非表示にする
		for (int i=0; i<game_ui.Length; i++) {
			game_ui[i].SetActive(flg);
		}

		// キャラクターの立ち絵 非表示切り替え
		for (int i=0; i<character_ui.Length; i++) {
			character_ui[i].SetActive(flg);
		}

		// 背景の非表示切り替え
		background_image.SetActive(flg);

		// プレハブ
		for (int i=0; i<stillPrefab.Length; i++) {
			stillPrefab[i].SetActive(flg);
		}
	}

	private void dispHideCharacter(){
		character_center.GetComponent<Image>().sprite = null;
		character_center.GetComponent<Image>().color  = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		character_right.GetComponent<Image>().sprite  = null;
		character_right.GetComponent<Image>().color   = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		character_left.GetComponent<Image>().sprite   = null;
		character_left.GetComponent<Image>().color    = new Color(1.0f, 1.0f, 1.0f, 0.0f);

		// ゲーム上のUI要素を非表示にする
		for (int i=0; i<game_ui.Length; i++) {
			game_ui[i].SetActive(false);
		}
	}

	public void moveEOF(){
		if(!sceneComponent.panelComponent.isFade){
			ScriptEngine scriptEngine        = GameObject.Find("ScriptEngine").GetComponent<ScriptEngine>();
			scriptEngine.moveLoadedSceneName = "Title";
			scriptEngine.bgm.stop_();
			sceneComponent.moveScene("LOAD");
		}
		return;
	}

	public void inGameFade(){
		isUpdateStop = false;
	}

	public void outGameFade(){
		panel.GetComponent<PanelComponent>().isFade = false;
	}

	public void onTapLog(){
		sceneComponent.pushScene("BackLog");
		// 画面を非表示に
		spriteDisp(false);
		isUpdateStop = true;
	}

	public void onTapFull(){
		canvas.SetActive(false);

		// ゲーム上のUI要素を非表示にする
		for (int i=0; i<game_ui.Length; i++) {
			game_ui[i].SetActive(false);
		}
		isFull = true;
	}

	public void onTapMenu(){
		// 現状のスクリーンショットを取る
		StartCoroutine(beforeGameMenuOpen());

		// ゲーム上のUI要素を非表示にする
		for (int i=0; i<game_ui.Length; i++) {
			game_ui[i].SetActive(false);
		}

		sceneComponent.pushScene("GameMenu");
		isUpdateStop = true;
	}

	public void onTapDisplay(){
		if(isFull){
			canvas.SetActive(true);
			viewCanvas.SetActive(true);

			// ゲーム上のUI要素を非表示にする
			for (int i=0; i<game_ui.Length; i++) {
				game_ui[i].SetActive(true);
			}
			isFull = false;
			return;
		}

		if(text.GetComponent<TextManager>().animation){
			text.GetComponent<TextManager>().setFullText();
		}else{
			text.GetComponent<TextManager>().setText("");
			nameTagObject.GetComponent<Text>().text = "";
			scriptEngine.stop_flg = false;
		}
	}
}
