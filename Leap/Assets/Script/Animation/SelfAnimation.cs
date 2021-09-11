using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SelfAnimation : MonoBehaviour {
	private Image           image_;
	private float           alfa;
	public  float           range;
	public  bool            run_flg;
	public  MainGameManager mainGameManager;
	public  ScriptEngine    scriptEngine;

	// Use this for initialization
	void Start () {
		image_       = GetComponent<Image>();
		alfa         = image_.color.a;
		scriptEngine = GameObject.Find("ScriptEngine").GetComponent<ScriptEngine>();
	}

	void Update () {
		if(run_flg){
			if(this.name=="18-snow-animation"){
				if(alfa>=1.0f){
					range = range*-1;
				}
				alfa        += range;
				image_.color = new Color(1.0f, 1.0f, 1.0f, alfa);

				if(alfa<=0.4f){
					range = range*-1;
				}

				Vector3 vec = image_.GetComponent<RectTransform>().localPosition;
				image_.GetComponent<RectTransform>().localPosition = new Vector3(0, vec.y-3, 9);

				if (image_.GetComponent<RectTransform>().localPosition.y < -1320 ) {
					image_.GetComponent<RectTransform>().localPosition = new Vector3 (0, 2380, 9);
				}
			}else if(this.name=="18-bg-animation"){
				if(mainGameManager.canvas.active){
					mainGameManager.canvas.SetActive(false);

					for (int i=0; i<mainGameManager.game_ui.Length; i++) {
						mainGameManager.game_ui[i].SetActive(false);
					}

					mainGameManager.isUpdateStop = true;
				}

				Vector3 vec = image_.GetComponent<RectTransform>().localPosition;
				image_.GetComponent<RectTransform>().localPosition = new Vector3(0, vec.y+3, 9);
				if (image_.GetComponent<RectTransform>().localPosition.y > 1553) {
					run_flg                      = false;
					mainGameManager.isUpdateStop = false;
				}
			}
		}
	}
}
