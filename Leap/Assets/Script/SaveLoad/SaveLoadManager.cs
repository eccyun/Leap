using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SaveLoadManager : MonoBehaviour {

	GameObject backBtn;

	// Use this for initialization
	void Start () {
		backBtn = GameObject.FindGameObjectWithTag("backBtn");
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			Camera     main     = GameObject.Find("Main Camera").GetComponent<Camera>();
			Vector3    touchPos = main.ScreenToWorldPoint(Input.mousePosition);
			Collider2D col      = Physics2D.OverlapPoint(touchPos);

			if(col == backBtn.GetComponent<Collider2D>()){
				SceneManager.UnloadScene("SaveLoad");
			}
		}
	}
}
