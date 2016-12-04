using UnityEngine;
using System.Collections;

public class ScriptEngine : SingletonMonoBehaviour<ScriptEngine> {
	public void Awake(){
        if(this != Instance){
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }

	void Update () {

	}
}
