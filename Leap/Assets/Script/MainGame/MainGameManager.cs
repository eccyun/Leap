using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainGameManager : MonoBehaviour {
	private ScriptEngine   scriptEngine;
	private SceneComponent sceneComponent;

	private string[]     script;
	private bool         isLoading;
	private bool         isWait;
	private float        maxWaitTime;
	private float        waitTime;
	private Camera       camera;

	// public
	public  Text         text;
	public  bool         isMoveScene;
	public  bool         isFull;

	public GameObject canvas;
	public GameObject MenuBtn;
	public GameObject message_window;

	void Start () {
		isLoading   = false;
		isWait      = false;
		maxWaitTime = 0.0f;
		waitTime    = 0.0f;

		// カメラ取得
		camera         = GameObject.Find("Main Camera").GetComponent<Camera>();

		// スクリプトエンジン取得
		scriptEngine   = GameObject.Find("ScriptEngine").GetComponent<ScriptEngine> ();
		sceneComponent = GetComponent<SceneComponent> ();

		// スクリプトの読みこみ
		scriptEngine.readScenarioFile();
	}

	void Update () {
		if(isMoveScene){
			return;
		}else if(!isFull){
			canvas.SetActive(true);
			MenuBtn.SetActive(true);
			message_window.SetActive(true);
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
		if(isLoading){
			if(sceneComponent.panelComponent != null && !sceneComponent.panelComponent.isFade){
				sceneComponent.moveScene("Load");
			}
			return;
		}

		// タッチイベント
		if(Input.GetMouseButtonDown(0)){
			if(isFull){
				canvas.SetActive(true);
				MenuBtn.SetActive(true);
				message_window.SetActive(true);
				isFull = false;
				return;
			}

			Vector3    touchPos = camera.ScreenToWorldPoint(Input.mousePosition);
			Collider2D col      = Physics2D.OverlapPoint(touchPos);

			if(col==GameObject.Find("log").GetComponent<Collider2D>()){
				sceneComponent.pushScene("BackLog");

				// テキストを非表示にする
				canvas.SetActive(false);
				MenuBtn.SetActive(false);
				message_window.SetActive(false);

				isMoveScene = true;
				return;
			}else if(col==GameObject.Find("full").GetComponent<Collider2D>()){
				canvas.SetActive(false);
				MenuBtn.SetActive(false);
				message_window.SetActive(false);
				isFull = true;
				return;
			}else if(col==GameObject.Find("menu").GetComponent<Collider2D>()){
				sceneComponent.pushScene("GameMenu");

				canvas.SetActive(false);
				MenuBtn.SetActive(false);
				message_window.SetActive(false);

				isMoveScene = true;
				return;
			}else{
				if(text.GetComponent<TextManager>().animation){
					text.GetComponent<TextManager>().setFullText();
				}else{
					scriptEngine.stop_flg = false;
				}
			}
		}

		if(isFull){
			return;
		}

		script = scriptEngine.readScript();

		// データを確認する
		if(script != null){
			if(script[0]=="# MSG"){
				string[] tmpTextLog = new string[2];

				// テキストを送る
				text.GetComponent<TextManager>().setText(script[1]);
				if(script.Length >= 3){
					GameObject nameTagObject = GameObject.Find("NameTag");
					nameTagObject.GetComponent<Text>().text = script[2];
					tmpTextLog[0] = script[2];
				}else{
					tmpTextLog[0] = "ーーー";
				}
				tmpTextLog[1] = script[1];
				scriptEngine.textLogs.Add(tmpTextLog);
			}else if(script[0]=="# BGM"){
				GameObject  audio       = GameObject.Find("BGM");
				AudioSource audioSource = audio.GetComponent<AudioSource>();
				if(script[2]=="PLAY"){
					audioSource.clip        = Resources.Load<AudioClip>("BGM/"+script[1]);
					audioSource.Play();
				}else if(script[2]=="STOP"){
					audioSource.Stop();
				}
			}else if(script[0]=="# IMG"){
				GameObject     img      = GameObject.Find("character_"+script[2]);
				SpriteRenderer renderer = img.GetComponent<SpriteRenderer>();
				renderer.sprite         = Resources.Load<Sprite>("Sprite/character/"+script[1]);
			}else if(script[0]=="# BG"){
				GameObject     img      = GameObject.Find("background_image");
				SpriteRenderer renderer = img.GetComponent<SpriteRenderer>();
				renderer.sprite         = Resources.Load<Sprite>("Sprite/Background/"+script[1]);
			}else if(script[0]=="# STILL-IMG"){
				// 部分絵の表示
				GameObject     still    = GameObject.Find(script[1]);
				SpriteRenderer renderer = still.GetComponent<SpriteRenderer>();

				// Spriteを表示
				if(script[2]=="show"){
					if(script[3] == "false"){
						still.GetComponent<SpriteScript>().alfa = 1.0f;
						renderer.color                          = new Color(1.0f, 1.0f, 1.0f, 1.0f);
					}
					renderer.enabled = true;
				}else if(script[2]=="hide"){
					renderer.enabled                        = false;
					still.GetComponent<SpriteScript>().alfa = 0.0f;
					renderer.color                          = new Color(1.0f, 1.0f, 1.0f, 0.0f);
				}
			}else if(script[0]=="# BLACK;"){
				scriptEngine.fade("out", 0.03f, "black");
			}else if(script[0]=="# WHITE;"){
				scriptEngine.fade("out", 0.03f, "white");
			}else if(script[0]=="LOADING;"){
				scriptEngine.fade("normal", 0.01f);
				isLoading = true;
			}else if(script[0]=="# REMOVE-IMG"){
				GameObject img;
				if(script[1]=="bg"){
					img = GameObject.Find("background_image");
				}else{
					img = GameObject.Find("character_"+script[1]);
				}
				SpriteRenderer renderer = img.GetComponent<SpriteRenderer>();
				renderer.sprite         = null;
			}else if(script[0]=="# EFFECT"){
				GameObject  audio       = GameObject.Find("Effect");
				AudioSource audioSource = audio.GetComponent<AudioSource>();
				audioSource.clip        = Resources.Load<AudioClip>("Effects/"+script[1]);
				audioSource.Play();
			}else if(script[0] == "# WAIT"){
				isWait = true;
				maxWaitTime = float.Parse(script[1]);
			}
		}
	}
}
