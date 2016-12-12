using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextManager : MonoBehaviour {

	[SerializeField] Text uiText;

	private float  displayCharacterElapsed     = 0.0f;
	private float  displayCharacterInterval    = 0.05f;
	public  bool   animation                   = false;
	int            displayCharacterCount       = 0;
	public  string displayText                 = "";

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		StartCoroutine("textAnimation");
	}

	public void setText(string text){
		uiText.text             = "";
		displayText             = text;
		displayCharacterCount   = 0;
		displayCharacterElapsed = 0.0f;
	}

	public void setFullText(){
		uiText.text = displayText;
	}

	IEnumerator textAnimation(){
		if(uiText.text.Length == displayText.Length){
			animation = false;
			yield break;
		}

		// animation フラグをたてる
		animation = true;

		// クリックから経過した時間が想定表示時間の何%か確認し、表示文字数を出す
		displayCharacterElapsed += Time.deltaTime;

		if(displayCharacterElapsed >= displayCharacterInterval){
			displayCharacterCount++;

			// 表示文字数が前回の表示文字数と異なるならテキストを更新する
			uiText.text             = displayText.Substring(0, displayCharacterCount);
			displayCharacterElapsed = 0.0f;
		}
	}
}
