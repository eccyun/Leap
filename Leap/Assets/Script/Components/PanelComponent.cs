using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelComponent : MonoBehaviour {

	public bool   isFade;
	public float  red;
	public float  blue;
	public float  green;
	public float  alfa;
	public float  range;

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
		range = 0.02f;
	}

	void Update () {
		if(isFade){
			if(alfa>=1.0f){
				range = range*-1;
			}

			alfa += range;
			GetComponent<Image>().color = new Color(red, green, blue, alfa);

			// 1.0fを超えるもしくは0.f以下になったら終わり
			if(alfa>=1.0f || alfa<=0.0f){
				isFade = false;
				alfa   = 0.0f;
				range  = range*-1;
			}
		}
	}
}
