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

	public void play_(string name){
		AudioSource audioSource = GetComponent<AudioSource>();
		audioSource.clip        = Resources.Load<AudioClip>("BGM/"+name);
		audioSource.Play();
	}

	public void stop_(){
		AudioSource audioSource = GetComponent<AudioSource>();
		audioSource.clip        = null;
		audioSource.Stop();
	}
}
