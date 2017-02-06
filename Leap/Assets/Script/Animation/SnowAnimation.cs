using UnityEngine;
using System.Collections;

public class SnowAnimation : MonoBehaviour {
	private SpriteRenderer renderer;
	private float          alfa;
	public  float          range;

	// Use this for initialization
	void Start () {
		renderer = GetComponent<SpriteRenderer>();
		alfa     = renderer.color.a;
	}

	void Update () {
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
			transform.position = new Vector3 (0, 34.0f, 0);
		}
	}
}
