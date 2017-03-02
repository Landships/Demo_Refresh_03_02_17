using UnityEngine;
using System.Collections;

[ RequireComponent ( typeof ( GUITexture ) ) ]

public class CrossHair_CS : MonoBehaviour {

	public Texture Image_Small ;
	public Texture Image_Full ;

	Transform This_Transform ;
	GUITexture This_GUITexture ;
	Camera Gun_Camera ;

	int Tank_ID ;

	void Awake () {
		This_Transform = transform ;
		This_Transform.position = new Vector3 ( 0.5f , 0.5f , 0.0f ) ;
		This_Transform.localScale = new Vector3 ( 0.0f , 0.0f , 1.0f ) ;
		This_GUITexture = GetComponent < GUITexture > () ;
		This_GUITexture.pixelInset = new Rect ( -Screen.width * 0.5f , -Screen.height * 0.5f , Screen.width , Screen.height ) ;
		gameObject.layer = 8 ; // Layer for CrossHair. ( ignored by Main Camera)
	}

	void Change_GunCamera_Mode ( int Temp_Mode ) { // Called from "Gun_Camera".
		// Switch the texture.
		if ( Temp_Mode == 2 ) { // Gun Camera is full size.
			if ( Image_Full ) {
				This_GUITexture.texture = Image_Full ;
			}
		} else {
			if ( Image_Small ) {
				This_GUITexture.texture = Image_Small ;
			}
		}
		// Reset the picture size.
		Vector2 View_Size = new Vector2 ( Screen.width * Gun_Camera.rect.width , Screen.height * Gun_Camera.rect.height ) ;
		This_GUITexture.pixelInset = new Rect ( -View_Size.x * 0.5f , -View_Size.y * 0.5f , View_Size.x , View_Size.y ) ;
	}

	void Get_Gun_Camera ( Camera Temp_Camera ) { // Called from Gun_Camera.
		Gun_Camera = Temp_Camera ;
	}

	void Set_Tank_ID ( int Temp_Tank_ID ) {
		Tank_ID = Temp_Tank_ID ;
	}
	
	void Receive_Current_ID ( int Temp_Current_ID ) {
		if ( Temp_Current_ID == Tank_ID ) {
			This_GUITexture.enabled = true ;
			This_Transform.position = new Vector3 ( 0.5f , 0.5f , 0.0f ) ;
		} else {
			This_GUITexture.enabled = false ;
		}
	}
}