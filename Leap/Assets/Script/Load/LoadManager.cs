using UnityEngine;
using System.Collections;

public class LoadManager : MonoBehaviour {
	private float max;
	private float frame;
	private float range;
	private SceneComponent sceneComponent;

	// Use this for initialization
	void Start () {
		max   = 5.0f;
		range = 0.02f;
		frame = 0.0f;
		sceneComponent = GetComponent<SceneComponent>();
	}

	// Update is called once per frame
	void Update () {
		if(max<=frame){
			sceneComponent.moveScene("MainGame");
			return;
		}
		frame = frame+range;
	}
}
