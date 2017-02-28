using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpriteScript : MonoBehaviour {
	private Image  image_;
	private float  range;
	public  float  alfa;

	void Start () {
		image_       = GetComponent<Image>();
		image_.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		alfa         = 0.0f;
		range        = 0.08f;
	}

	void Update () {
		if(alfa <= 1.0f && image_.enabled){
			alfa          += range;
			image_.color = new Color(1.0f, 1.0f, 1.0f, alfa);
		}else if(!image_.enabled && alfa != 0.0f){
			alfa = 0.0f;
			image_.color = new Color(1.0f, 1.0f, 1.0f, alfa);
		}
	}
}
