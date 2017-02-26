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
}
