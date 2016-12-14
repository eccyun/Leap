using UnityEngine;
using System.Collections;

public class SpriteScript : MonoBehaviour {
	private SpriteRenderer renderer;
	private float  range;
	public  float  alfa;

	void Start () {
		renderer       = GetComponent<SpriteRenderer>();
		renderer.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		alfa           = 0.0f;
		range          = 0.08f;
	}

	void Update () {
		if(alfa <= 1.0f && renderer.enabled){
			alfa          += range;
			renderer.color = new Color(1.0f, 1.0f, 1.0f, alfa);
		}else if(!renderer.enabled && alfa != 0.0f){
			alfa = 0.0f;
			renderer.color = new Color(1.0f, 1.0f, 1.0f, alfa);
		}
	}
}
