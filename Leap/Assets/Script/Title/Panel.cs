using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Panel : MonoBehaviour {

	public bool   isFade   = false;

	float       red;
	float       blue;
	float       green;
	float       alfa;

	void Start () {
		red   = GetComponent<Image>().color.r;
		green = GetComponent<Image>().color.g;
		blue  = GetComponent<Image>().color.b;
		alfa  = GetComponent<Image>().color.a;
	}

	void Update () {
		if(isFade){
			GetComponent<Image>().color = new Color(red, green, blue, alfa);
			alfa += (alfa>=1.0f)? -0.05f:0.05f;

			if(alfa>=1.0f||alfa<=0.0f){
				isFade = false;
			}
		}
	}
}
