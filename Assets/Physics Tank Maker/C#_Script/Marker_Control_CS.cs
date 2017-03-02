using UnityEngine;
using System.Collections;

public class Marker_Control_CS : MonoBehaviour {

	// Set from "Turret_Horizontal".
	public Turret_Horizontal_CS Turret_Horizontal_Script ;
	public bool Main_Flag ;

	public Color Normal_Color = new Vector4 ( 1.0f , 1.0f , 1.0f , 0.5f ) ;
	public Color LockOn_Color = new Vector4 ( 1.0f , 0.0f , 0.0f , 0.5f ) ;

	Vector2 Texture_Size ;
	Vector2 Center_Offset ;
	GUITexture This_GUITexture ;
	GameObject This_GameObject ;

	void Awake () {
		transform.position = Vector3.zero ;
		transform.localScale = new Vector3 ( 0.0f , 0.0f , 1.0f ) ;
		This_GUITexture = GetComponent < GUITexture > () ;
		Texture_Size = new Vector2 ( This_GUITexture.texture.width , This_GUITexture.texture.height ) ;
		Center_Offset = Texture_Size * 0.5f ;
		This_GameObject = gameObject ;
	}

	void Start () {
		if ( Main_Flag ) { // Main Marker.
			This_GameObject.layer = 5 ; // Layer for UI. ( ignored by Gun Camera )
		} else { // Sub Marker.
			This_GameObject.layer = 8 ; // Layer for CrossHair. ( ignored by Main Camera )
		}
	}

	void LateUpdate () {
		if ( Turret_Horizontal_Script.Marker_Flag ) {
			Camera[] Temp_Cameras = Camera.allCameras ;
			if ( Turret_Horizontal_Script.Free_Aim_Flag ) { // Free Aiming.
				This_GUITexture.color = Normal_Color ;
				if ( Temp_Cameras.Length == 1 ) { // There is only one camera window.
					if ( Main_Flag ) { // Main marker
						This_GameObject.layer = 0 ;
						Track_Cursor ( Temp_Cameras [ 0 ] ) ;
					} else { // Sub Marker
						This_GUITexture.enabled = false ;
					}
				} else { // There are two camera windows.
					if ( Main_Flag ) { // Main marker
						This_GameObject.layer = 5 ;
						Track_Cursor ( Temp_Cameras [ 0 ] ) ;
					} else { // Sub Marker
						Track_Cursor ( Temp_Cameras [ 1 ] ) ;
					}
				}
			} else { // Lock On.
				This_GUITexture.color = LockOn_Color ;
				if ( Temp_Cameras.Length == 1 ) { // There is only one camera window.
					if ( Main_Flag ) { // Main marker
						This_GameObject.layer = 0 ;
						Move_Marker ( Temp_Cameras [ 0 ] ) ;
					} else { // Sub Marker
						This_GUITexture.enabled = false ;
					}
				} else { // There are two camera windows.
					if ( Main_Flag ) { // Main marker
						This_GameObject.layer = 5 ;
						Move_Marker ( Temp_Cameras [ 0 ] ) ;
					} else { // Sub Marker
						Move_Marker ( Temp_Cameras [ 1 ] ) ;
					}
				}
			}
		} else {
			This_GUITexture.enabled = false ;
		}
	}

	void Move_Marker ( Camera Temp_Camera ) {
		Vector3 Temp_Pos = Temp_Camera.WorldToScreenPoint ( Turret_Horizontal_Script.Target_Pos ) ;
		if ( Temp_Pos.z < 0.0f ) {
			This_GUITexture.enabled = false ;
		} else {
			if ( Main_Flag ) { // Main marker
				This_GUITexture.enabled = true ;
				This_GUITexture.pixelInset = new Rect ( ( Temp_Pos.x - Center_Offset.x ) - ( Screen.width * Temp_Camera.rect.x ) , Temp_Pos.y - Center_Offset.y , Texture_Size.x , Texture_Size.y ) ;
			} else { // Sub Marker
				if ( new Rect ( Screen.width * Temp_Camera.rect.x , Screen.height * Temp_Camera.rect.y , Screen.width * Temp_Camera.rect.width , Screen.height * Temp_Camera.rect.height ).Contains ( new Vector2 ( Temp_Pos.x , Temp_Pos.y ) ) ) {
					This_GUITexture.enabled = true ;
					This_GUITexture.pixelInset = new Rect ( ( Temp_Pos.x - Center_Offset.x ) - ( Screen.width * Temp_Camera.rect.x ) , ( Temp_Pos.y - Center_Offset.y ) - ( Screen.height * Temp_Camera.rect.y ) , Texture_Size.x , Texture_Size.y ) ;
				} else {
					This_GUITexture.enabled = false ;
				}
			}
		}
	}

	void Track_Cursor ( Camera Temp_Camera ) {
		Vector2 Temp_Pos = Input.mousePosition ;
		if ( Main_Flag ) { // Main marker
			This_GUITexture.enabled = true ;
			This_GUITexture.pixelInset = new Rect ( ( Temp_Pos.x - Center_Offset.x ) - ( Screen.width * Temp_Camera.rect.x ) , Temp_Pos.y - Center_Offset.y , Texture_Size.x , Texture_Size.y ) ;
		} else { // Sub Marker
			if ( new Rect ( Screen.width * Temp_Camera.rect.x , Screen.height * Temp_Camera.rect.y , Screen.width * Temp_Camera.rect.width , Screen.height * Temp_Camera.rect.height ).Contains ( Temp_Pos ) ) {
				This_GUITexture.enabled = true ;
				This_GUITexture.pixelInset = new Rect ( ( Temp_Pos.x - Center_Offset.x ) - ( Screen.width * Temp_Camera.rect.x ) , ( Temp_Pos.y - Center_Offset.y ) - ( Screen.height * Temp_Camera.rect.y ) , Texture_Size.x , Texture_Size.y ) ;
			} else {
				This_GUITexture.enabled = false ;
			}
		}
	}
}
