using UnityEngine;
using System.Collections;

public class SelfAnimation : MonoBehaviour {
	private SpriteRenderer  renderer;
	private float           alfa;
	public  float           range;
	public  bool            run_flg;
	public  MainGameManager mainGameManager;
	public  ScriptEngine    scriptEngine;

	private bool sound_flg;

	// Use this for initialization
	void Start () {
		renderer     = GetComponent<SpriteRenderer>();
		alfa         = renderer.color.a;
		scriptEngine = GameObject.Find("ScriptEngine").GetComponent<ScriptEngine>();
	}

	void Update () {
		if(run_flg){
			if(this.name=="18-snow-animation"){
				if(alfa>=1.0f){
					range = range*-1;
				}
				alfa           += range;
				renderer.color  = new Color(1.0f, 1.0f, 1.0f, alfa);

				if(alfa<=0.4f){
					range = range*-1;
				}

				transform.Translate (0, -0.05f, 0);
				if (transform.position.y < -18.5f ) {
					transform.position = new Vector3 (0, 32.0f, 9);
				}
			}else if(this.name=="18-bg-animation"){
				if(!sound_flg){
					scriptEngine.bgm.play_("leap2-overture");
					sound_flg = true;
				}

				if(mainGameManager.GameUI.active){
					mainGameManager.canvas.SetActive(false);

					for (int i=0; i<mainGameManager.game_ui.Length; i++) {
						mainGameManager.game_ui[i].SetActive(false);
					}

					mainGameManager.isUpdateStop = true;
				}

				transform.Translate (0, 0.03f, 0);
				if (transform.position.y > 8.0f) {
					run_flg                      = false;
					mainGameManager.isUpdateStop = false;
				}
			}
		}
	}
}
