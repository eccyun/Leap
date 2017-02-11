using UnityEngine;
using System.Collections;

public class SelfAnimation : MonoBehaviour {
	private SpriteRenderer renderer;
	private float          alfa;
	public  float          range;
	public  bool           run_flg;

	// Use this for initialization
	void Start () {
		renderer = GetComponent<SpriteRenderer>();
		alfa     = renderer.color.a;
	}

	void Update () {
		if(run_flg){
			if(this.name=="18-snow-animation"){
				if(alfa>=1.0f){
					range = range*-1;
				}
				alfa            += range;
				renderer.color  = new Color(1.0f, 1.0f, 1.0f, alfa);

				if(alfa<=0.4f){
					range = range*-1;
				}

				transform.Translate (0, -0.05f, 0);
				if (transform.position.y < -18.5f ) {
					transform.position = new Vector3 (0, 32.0f, 0);
				}
			}
		}
	}
}
