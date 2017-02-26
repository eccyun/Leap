using UnityEngine;
using System.Collections;

public class BGMComponent : SingletonMonoBehaviour<BGMComponent> {

	public void Awake(){
        if(this != Instance){
            Destroy(this);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

	void Start () {

	}

	void Update () {

	}

	public void preload_(string name){
		AudioSource audioSource = GetComponent<AudioSource>();
		audioSource.clip        = Resources.Load<AudioClip>("BGM/"+name);
	}

	public void play_(string name=null, bool loop_flg=true){
		AudioSource audioSource = GetComponent<AudioSource>();
		if(name!=null){
			preload_(name);
		}
		audioSource.loop = loop_flg;
		audioSource.Play();
	}

	public void stop_(){
		AudioSource audioSource = GetComponent<AudioSource>();
		audioSource.clip        = null;
		audioSource.loop        = true;
		audioSource.Stop();
	}
}
