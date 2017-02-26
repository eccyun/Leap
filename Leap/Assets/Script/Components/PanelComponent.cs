using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PanelComponent : MonoBehaviour {

	public bool   isFade;
	public bool   isFlap;
	public bool   isPageIn;
	public float  red;
	public float  blue;
	public float  green;
	public float  alfa;
	public float  range;
	private bool orderEditFlg;

	public void setAlfa(float value){
		alfa = value;
		GetComponent<Image>().color = new Color(red, green, blue, alfa);
	}

	public void setRange(float value){
		range = value;
	}

	void Start () {
		red   = GetComponent<Image>().color.r;
		green = GetComponent<Image>().color.g;
		blue  = GetComponent<Image>().color.b;
		alfa  = GetComponent<Image>().color.a;
		range = 0.008f;

		ScriptEngine scriptEngine = GameObject.Find("ScriptEngine").GetComponent<ScriptEngine>();
		if(scriptEngine.load_flg && SceneManager.GetActiveScene().name=="MainGame"){
			GameObject loadLogo = GameObject.Find("LoadLogo");
			if(loadLogo!=null){
				isFade           = false;
				Image renderer   = loadLogo.GetComponent<Image>();
				renderer.enabled = true;
			}
		}

		orderEdit(0);
		orderEditFlg = false;
	}

	void Update () {
		if(isFade){
			if(!orderEditFlg){
				orderEdit(1000);
				orderEditFlg = true;
			}

			if(alfa>=1.0f){
				range = range*-1;
			}

			alfa += range;
			GetComponent<Image>().color = new Color(red, green, blue, alfa);

			// 1.0fを超えるもしくは0.f以下になったら終わり
			if((alfa>=1.0f || alfa<=0.0f) && !isFlap){
				GetComponent<SceneComponent>().runDelegateMethod();

				if(!isPageIn){
					isPageIn = true;
					isFade   = false;

					if(alfa<=0.0f){
						orderEdit(0);
					}

					alfa     = 0.0f;
					orderEditFlg = false;
				}else{
					isFlap = true;
					alfa   = 1.0f;
				}
			}else if((alfa>=1.0f || alfa<=0.0f) && isFlap){
				isFade = false;
				isFlap = false;

				if(alfa<=0.0f){
					orderEdit(0);
				}

				alfa   = 0.0f;
				range  = range*-1;
				orderEditFlg = false;
			}
		}
	}

	public void orderEdit(int order=0){
		// Game Sort
		GameObject[] fadeObjects_ = GameObject.FindGameObjectsWithTag("Fade");
		for (int i=0; i < fadeObjects_.Length; i++) {
			fadeObjects_[i].GetComponent<Canvas>().sortingOrder = order;
		}
	}
}
