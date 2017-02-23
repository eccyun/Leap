using UnityEngine;
using System.Collections;

public class SelfAnimation : MonoBehaviour {
	private SpriteRenderer  renderer;
	private float           alfa;
	public  float           range;
	public  bool            run_flg;
	public  MainGameManager mainGameManager;

	private bool sound_flg;

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
				alfa           += range;
				renderer.color  = new Color(1.0f, 1.0f, 1.0f, alfa);

				if(alfa<=0.4f){
					range = range*-1;
				}

				transform.Translate (0, -0.05f, 0);
				if (transform.position.y < -18.5f ) {
					transform.position = new Vector3 (0, 32.0f, 9);
				}
			}else if(this.name=="18-bg-animation"){
				if(!sound_flg){
					GameObject  audio       = GameObject.Find("BGM");
					AudioSource audioSource = audio.GetComponent<AudioSource>();
					audioSource.clip        = Resources.Load<AudioClip>("BGM/leap2-overture");
					audioSource.volume      = 0.3f;
					audioSource.Play();
					sound_flg = true;
				}

				if(mainGameManager.GameUI.active){
					mainGameManager.canvas.SetActive(false);
					mainGameManager.GameUI.SetActive(false);
					mainGameManager.isUpdateStop = true;
				}

				transform.Translate (0, 0.03f, 0);
				if (transform.position.y > 9.0f) {
					run_flg                      = false;
					mainGameManager.isUpdateStop = false;
				}
			}
		}
	}
}
