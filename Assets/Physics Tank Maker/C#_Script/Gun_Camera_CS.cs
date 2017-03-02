using UnityEngine;
using System.Collections;

[ RequireComponent ( typeof ( Camera ) ) ]
[ RequireComponent ( typeof ( AudioListener ) ) ]

public class Gun_Camera_CS : MonoBehaviour {

	public bool Enable_Small_Camera = true ;
	public float Small_Width = 0.4f ;
	public float Small_Height = 0.4f ;

	public int Mode = 0 ; // Referred to from RC_Camera.
	Camera This_Camera ;
	AudioListener This_AudioListener ;
	bool Camera_Flag = false ;
	bool AudioListener_Flag = false ;

	float Rect_Left ;
	float Rect_Top ;
	float Rect_Width ;
	float Rect_Height ;

	float Angle ;
	float Temp_Horizontal ;
	float Temp_Vertical ;

	Transform Top_Transform ;

	RC_Camera_CS RC_Camera_Script ;
	
	bool Flag = true ;
	int Tank_ID ;
	int Input_Type = 4 ;
	
	void Awake () {
		this.tag = "Untagged" ;
		This_Camera = GetComponent < Camera > () ;
		This_Camera.enabled = false ;
		This_Camera.depth = 1 ;
		This_Camera.cullingMask = ~ ( 1 << 5 ) ; // Ignore "UI" Layer.
		This_AudioListener = GetComponent < AudioListener > () ;
		if ( This_AudioListener == null ) {
			This_AudioListener = gameObject.AddComponent < AudioListener > () ;
		}
		This_AudioListener.enabled = false ;
		AudioListener.volume = 1.0f ;
	}
	
	void Start () {
		Top_Transform = GetComponentInParent < Tank_ID_Control_CS > ().transform ;
		RC_Camera_Script = FindObjectOfType < RC_Camera_CS > () ;
		// Send message to "Turret_Horizontal" and "CrossHair".
		Top_Transform.BroadcastMessage ( "Get_Gun_Camera" , This_Camera , SendMessageOptions.DontRequireReceiver ) ;
	}
	
	void Update () {
		if ( Flag ) {
			switch ( Input_Type ) {
			case 0 :
				KeyBoard_Input () ;
				break ;
			case 1 :
				GamePad_Input () ;
				break ;
			case 2 :
				GamePad_Input () ;
				break ;
			case 3 :
				GamePad_Input () ;
				break ;
			case 4 :
				Mouse_Input () ;
				break ;
			case 5 :
				Mouse_Input () ;
				break ;
			case 10 :
				Mouse_Input () ;
				break ;
			}
		}
	}
	
	void KeyBoard_Input () {
		if ( Input.GetKeyDown ( "q" ) ) { 
			Switch () ;
		}
		if ( Mode != 0 ) {
			if ( Input.GetKey ( "c" ) ) { 
				Temp_Horizontal = Input.GetAxisRaw ( "Horizontal" ) ;
				Temp_Vertical = Input.GetAxisRaw ( "Vertical" ) * 0.05f ;
				Zoom () ;
				Rotate () ;
			}
		}
	}
	
	void GamePad_Input () {
		if ( Input.GetButtonDown ( "Fire2" ) ) {
			Switch () ;
		}
		if ( Mode != 0 ) {
			if ( Input.GetButton ( "Jump" ) ) {
				Temp_Horizontal = Input.GetAxis ( "Horizontal" ) ;
				Temp_Vertical = Input.GetAxis ( "Vertical" ) * 0.05f ;
				Zoom () ;
				Rotate () ;
			}
		}
	}
	
	void Mouse_Input () {
		if ( Input.GetKeyDown ( "r" ) ) {
			Switch () ;
		}
		if ( Mode != 0 ) {
			if ( !Input.GetMouseButton ( 1 ) ) {
				float Temp_Axis = Input.GetAxis ( "Mouse ScrollWheel" ) ;
				if ( Temp_Axis != 0.0f ) {
					if ( Input.GetKey ( "f" ) ) {
						Temp_Vertical = Temp_Axis ;
						Rotate () ;
					} else {
						Temp_Horizontal = Temp_Axis ;
						Zoom () ;
					}
				}
			}
		}
	}

	void Switch () {
		if ( Enable_Small_Camera ) {
			if ( Mode < 2 ) {
				Mode += 1 ;
			} else {
				Mode = 0 ;
			}
		} else {
			if ( Mode == 0 ) {
				Mode = 2 ;
			} else {
				Mode = 0 ;
			}
		}
		switch ( Mode ) {
		case 0 : // Off
			Camera_Flag = false ;
			AudioListener_Flag = false ;
			this.tag = "Untagged" ;
			break ;
		case 1 : // Small window.
			Rect_Left = 0.0f ;
			Rect_Top = 0.0f ;
			Rect_Width = Small_Width ;
			Rect_Height = Small_Height ;
			Camera_Flag = true ;
			AudioListener_Flag = false ;
			this.tag = "Untagged" ;
			break ;
		case 2 : // Full screen.
			Rect_Left = 0.0f ;
			Rect_Top = 0.0f ;
			Rect_Width = 1.0f ;
			Rect_Height = 1.0f ;
			Camera_Flag = true ;
			AudioListener_Flag = true ;
			this.tag = "MainCamera" ;
			break ;
		}
		This_Camera.rect = new Rect ( Rect_Left , Rect_Top , Rect_Width , Rect_Height) ;
		This_Camera.enabled = Camera_Flag ;
		This_AudioListener.enabled = AudioListener_Flag ;
		// Send message to "CrossHair", "Main Camera"(Camera_Distance_CS).
		Top_Transform.BroadcastMessage ( "Change_GunCamera_Mode" , Mode , SendMessageOptions.DontRequireReceiver ) ;
		// Send message to RC_Camera.
		if ( RC_Camera_Script ) {
			RC_Camera_Script.Change_GunCamera_Mode ( Mode ) ;
		}
	}

	void _Switch () {
		switch ( Mode ) {
		case 0 :
			Mode = 1 ; // Small window.
			Rect_Left = 0.0f ;
			Rect_Top = 0.0f ;
			Rect_Width = Small_Width ;
			Rect_Height = Small_Height ;
			Camera_Flag = true ;
			AudioListener_Flag = false ;
			this.tag = "Untagged" ;
			break ;
		case 1 :
			Mode = 2 ; // Full screen.
			Rect_Left = 0.0f ;
			Rect_Top = 0.0f ;
			Rect_Width = 1.0f ;
			Rect_Height = 1.0f ;
			Camera_Flag = true ;
			AudioListener_Flag = true ;
			this.tag = "MainCamera" ;
			break ;
		case 2 :
			Mode = 0 ; // Off
			Camera_Flag = false ;
			AudioListener_Flag = false ;
			this.tag = "Untagged" ;
			break ;
		}
		This_Camera.rect = new Rect ( Rect_Left , Rect_Top , Rect_Width , Rect_Height) ;
		This_Camera.enabled = Camera_Flag ;
		This_AudioListener.enabled = AudioListener_Flag ;
		// Send message to "CrossHair", "Main Camera"(Camera_Distance_CS).
		Top_Transform.BroadcastMessage ( "Change_GunCamera_Mode" , Mode , SendMessageOptions.DontRequireReceiver ) ;
		// Send message to RC_Camera.
		if ( RC_Camera_Script ) {
			RC_Camera_Script.Change_GunCamera_Mode ( Mode ) ;
		}
	}

	void Zoom () {
		//This_Camera.fieldOfView -= Temp_Horizontal ;
		if ( Temp_Horizontal > 0.0f ) {
			This_Camera.fieldOfView *= 0.9f ;
		} else if ( Temp_Horizontal < 0.0f ) {
			This_Camera.fieldOfView *= 1.1f ;
		}
		This_Camera.fieldOfView = Mathf.Clamp ( This_Camera.fieldOfView , 0.1f , 50.0f ) ;
	}

	void Rotate () {
		Angle -= Temp_Vertical ;
		Angle = Mathf.Clamp ( Angle , 0.0f , 90.0f ) ;
		transform.localRotation = Quaternion.Euler ( new Vector3 ( Angle , 0.0f , 0.0f ) ) ;
	}

	void Turret_Linkage () { // Called from "Damage_Control" in the turret.
		// Send message to "CrossHair", "Main Camera"(Camera_Distance_CS).
		Top_Transform.BroadcastMessage ( "Change_GunCamera_Mode" , 0 , SendMessageOptions.DontRequireReceiver ) ;
		// Send message to RC_Camera.
		if ( Flag && RC_Camera_Script ) {
			RC_Camera_Script.Change_GunCamera_Mode ( 0 ) ;
		}
		Destroy ( this.gameObject ) ;
	}

	void Set_Input_Type (  int Temp_Input_Type   ){
		Input_Type = Temp_Input_Type ;
	}
	
	void Set_Tank_ID (  int Temp_Tank_ID   ){
		Tank_ID = Temp_Tank_ID ;
	}
	
	void Receive_Current_ID (  int Temp_Current_ID   ){
		if ( Temp_Current_ID == Tank_ID ) {
			Flag = true ;
			This_Camera.enabled = Camera_Flag ;
			This_AudioListener.enabled = AudioListener_Flag ;
		} else {
			Flag = false ;
			This_Camera.enabled = false ;
			This_AudioListener.enabled = false ;
		}
	}
}