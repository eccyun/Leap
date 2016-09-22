using UnityEngine;
using System.Collections;

public class MenuParts : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if(Input.touchCount > 0){
			Touch   _touch     = Input.GetTouch(0);
			Vector2 worldPoint = Camera.main.ScreenToWorldPoint(_touch.position);

			// タップ位置にオブジェクトがあるか
			RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
			if(hit){
				Bounds rect = hit.collider.bounds;
				if(rect.Contains(worldPoint)){
					Debug.Log(hit.collider.gameObject.name);
				}
			}

		}
	}
}
