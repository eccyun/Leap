using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;


public class DataBox : MonoBehaviour {
	public int        identifier;
	public GameData   gameData;


	public void dispDataBox(){
		// 日付
		GameObject saveDate     = transform.FindChild("SaveDate").gameObject;
		Text       saveDateText = saveDate.GetComponent<Text>();
		saveDateText.enabled    = true;
		saveDateText.text       = gameData.saveDate;

		// ゲーム中の本文
		GameObject abridge     = transform.FindChild("Abridge").gameObject;
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

		// テクスチャをセット
		RectTransform rt  = GetComponent<RectTransform>();

		// テクスチャデータのサイズ
		float ratio         = (rt.rect.height+100.0f)/(float)Screen.height;
	    int   textureWidth  = (int)((float)Screen.width*ratio);
		int   textureHeight = (int)rt.rect.height+100;

		// テクスチャをリサイズ
		TextureScale.Bilinear(texture, textureWidth, textureHeight);

		float s_x = (textureWidth/2)  - ((int)rt.rect.width/2);
		float s_y = (textureHeight/2) - ((int)rt.rect.height/2);

		GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(s_x, s_y, rt.rect.width, rt.rect.height), new Vector2(0.0f, 0.0f), 71.0f);
	}
}
