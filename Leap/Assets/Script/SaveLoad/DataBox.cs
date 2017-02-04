using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;


public class DataBox : MonoBehaviour {
	public GameData   gameData;
	public GameObject dataBox;
	public int        identifier;

	public void dispDataBox(){
		// 日付
		GameObject saveDate     = dataBox.transform.FindChild("SaveDate").gameObject;
		Text       saveDateText = saveDate.GetComponent<Text>();
		saveDateText.enabled    = true;
//		saveDateText.text       = gameData.saveDate;
		saveDateText.text = identifier.ToString();

		// ゲーム中の本文
		GameObject abridge     = dataBox.transform.FindChild("Abridge").gameObject;
		Text       abridgeText = abridge.GetComponent<Text>();
		abridgeText.enabled    = true;

		if(gameData.abridgeText.Length >= 21){
			abridgeText.text = gameData.abridgeText.Substring(0, 20)+"...";
		}else{
			abridgeText.text = gameData.abridgeText;
		}

		// スクリーンキャプチャ
		Texture2D texture = new Texture2D(Screen.width, Screen.height);
		texture.LoadImage(Convert.FromBase64String (gameData.binaryCapture));

		// テクスチャデータのサイズ
		int textureWidth  = Screen.width/3;
		int textureHeight = Screen.height/3;

		// テクスチャをリサイズ
		TextureScale.Bilinear (texture, textureWidth, textureHeight);

		// テクスチャをセット
		RectTransform rt = dataBox.GetComponent<RectTransform>();
		dataBox.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect((textureWidth/2)  - ((int)rt.rect.width/2),(textureHeight/2) - ((int)rt.rect.height/2),rt.rect.width, rt.rect.height), new Vector2(0.0f, 0.0f), 71.0f);
	}
}
