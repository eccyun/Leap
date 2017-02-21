using UnityEngine;
using System.Collections;

public class LaunchManager : MonoBehaviour {
	private float          max;
	private float          frame;
	private float          range;
	private bool           isMoveFlg;

	private SceneComponent sceneComponent;
	public  GameObject     panel;

	// Use this for initialization
	void Start () {
		max   = 5.0f;
		range = 0.02f;
		frame = 0.0f;

		sceneComponent = panel.GetComponent<SceneComponent>();
	}

	// Update is called once per frame
	void Update () {
		if(isMoveFlg){
			if(!sceneComponent.panelComponent.isFade){
				sceneComponent.moveScene("Title");
			}
			return;
		}

		if(max<=frame){
			if(!sceneComponent.panelComponent.isFade){
				sceneComponent.panelComponent.isFlap = true;
				sceneComponent.fade();
				isMoveFlg = true;
			}
			return;
		}
		frame = frame+range;
	}}
