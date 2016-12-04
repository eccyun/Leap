using UnityEngine;
using System.Collections;

public class ScriptManager : SingletonMonoBehaviour<AudioManager> {

    public void Awake(){
        if(this != Instance){
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
