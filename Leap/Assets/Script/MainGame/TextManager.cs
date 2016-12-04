using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TextManager : MonoBehaviour {

	[SerializeField] Text uiText;

	private string displayText                 = "夏休みも終盤に差し掛かった8月の日、僕は近所の高台まで絵を描きに来ていた。";
	private float  displayCharacterElapsed     = 0.0f;
	private float  displayCharacterInterval    = 0.05f;
	int            displayCharacterCount       = 0;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		StartCoroutine("textAnimation");
	}

	IEnumerator textAnimation(){
		if(uiText.text.Length == displayText.Length){
			yield break;
		}

		// クリックから経過した時間が想定表示時間の何%か確認し、表示文字数を出す
		displayCharacterElapsed += Time.deltaTime;

		if(displayCharacterElapsed >= displayCharacterInterval){
			displayCharacterCount++;

			// 表示文字数が前回の表示文字数と異なるならテキストを更新する
			uiText.text = displayText.Substring(0, displayCharacterCount);
			displayCharacterElapsed = 0.0f;
		}
	}
}
