using UnityEngine;
using System.Collections;

[ RequireComponent ( typeof ( Camera ) ) ]
[ RequireComponent ( typeof ( AudioListener ) ) ]

public class RC_Camera_CS : MonoBehaviour {
	public int Input_Type = 4 ;
	public float FOV = 20.0f ;
	public Transform Target_Transform ;
	public float Horizontal_Speed = 1.0f ;
	public float Vertical_Speed = 1.0f ;
	public float Zoom_Speed = 0.3f ;
	public float Min_FOV = 1.0f ;
	public float Max_FOV = 50.0f ;
	public Transform Position_Pack ;

	Transform This_Transform ;
	Transform Top_Transform ;

	public bool Flag = true ; // Referred to from Camera_Distance (Main Camera).

	Camera This_Camera ;
	AudioListener This_AudioListener ;
	int GunCamera_Mode = 0 ;
	Vector3 [] Camera_Positions ;
	int Current_Pos ;
	bool Positions_Flag = false ;

	float Temp_Horizontal ;
	float Temp_Vertical ;
	Vector2 Last_Mouse_Pos ;
	bool Turn_Flag ;
	
	Game_Controller_CS Game_Controller_Script ;
	int  Current_ID = 1 ;

	void Awake () {
		this.tag = "Untagged" ;
		This_Camera = GetComponent < Camera > () ;
		This_Camera.enabled = true ;
		This_Camera.depth = 0 ;
		This_Camera.cullingMask = ~ ( 1 << 8 ) ; // Ignore CrossHair.
		This_Camera.nearClipPlane = 0.05f ;
		This_Camera.fieldOfView = FOV ;
		This_AudioListener = GetComponent < AudioListener > () ;
		This_AudioListener.enabled = true ;
		AudioListener.volume = 1.0f ;
	}
	
	void Start () {
		This_Transform = transform ;
		Game_Controller_Script = FindObjectOfType < Game_Controller_CS > () ;
		if ( Game_Controller_Script == null ) {
			Debug.LogError ( "There is no 'Game_Controller' in the scene. (Physics Tank Maker)" ) ;
			Destroy ( this ) ;
		}
		// Set and store the 'Camera_Positions'.
		Set_Camera_Positions () ;
	}

	void Set_Camera_Positions () {
		if ( Position_Pack ) {
			if ( Position_Pack.childCount != 0 ) {
				Camera_Positions = new Vector3 [ Position_Pack.childCount ] ;
				for ( int i = 0 ; i < Position_Pack.childCount ; i++ ) {
					Camera_Positions [ i ] = Position_Pack.GetChild ( i ).position ;
				}
				This_Transform.position = Camera_Positions [ 0 ] ;
				Positions_Flag = true ;
				return ;
			}
		}
		// Position_Pack is not assigned or empty.
		Positions_Flag = false ;
	}

	void Update () {
		// Set the target at the opening and respawning.
		if ( Target_Transform == null ) {
			Receive_Current_ID ( Current_ID ) ;
		}
		// Switch the camera.
		if ( Input.GetKeyDown ( KeyCode.Tab ) ) {
			if ( Flag ) {
				Flag = false ;
			} else {
				Flag = true ;
			}
			// Send message to Camera_Distance (Main Camera).
			if ( Target_Transform ) {
				Target_Transform.BroadcastMessage ( "Switch_Camera" , Flag , SendMessageOptions.DontRequireReceiver ) ;
			}
			Control_Enabled () ;
		}
		// Turn and zoom.
		if ( Flag ) {
			switch ( Input_Type ) {
			case 0 :
				KeyBoard_Input () ;
				break ;
			case 1 :
				Stick_Input () ;
				break ;
			case 2 :
				Trigger_Input () ;
				break ;
			case 3 :
				Stick_Input () ;
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
			if ( Target_Transform && Turn_Flag == false ) {
				This_Transform.LookAt ( Target_Transform ) ;
			}
			// Control camera position.
			if ( Positions_Flag && Target_Transform ) {
				Control_Position () ;
			}
		}
	}

	void Control_Position () {
		Vector3 Temp_Pos = Target_Transform.position ;
		float Min_Distance = Mathf.Infinity ;
		for ( int i = 0 ; i < Camera_Positions.Length ; i++ ) {
			float Temp_Distance = Vector3.Distance ( Temp_Pos , Camera_Positions [ i ] ) ;
			if ( Temp_Distance < Min_Distance ) {
				Min_Distance = Temp_Distance ;
				Current_Pos = i ;
			}
		}
		This_Transform.position = Camera_Positions [ Current_Pos ] ;
	}
	
	void KeyBoard_Input () {
		if ( Input.GetKey ( "e" ) ) {
			Zoom ( -Zoom_Speed ) ;
		} else if ( Input.GetKey ( "r" ) ) {
			Zoom ( Zoom_Speed ) ;
		}
	}
	
	void Stick_Input () {
		if ( Input.GetButton ( "Fire3" ) ) {
			Zoom ( -Zoom_Speed ) ;
		} else if ( Input.GetButton ( "Fire1" ) ) {
			Zoom ( Zoom_Speed ) ;
		}
		if ( Input.GetButton ( "L_Button" ) ) {
			Turn_Flag = true ;
			Temp_Horizontal = Input.GetAxis ( "Horizontal2" ) ;
			Temp_Vertical = Input.GetAxis ( "Vertical2" ) ;
			Stick_Rotate () ;
		} else {
			Turn_Flag = false ;
		}
	}
	
	void Trigger_Input () {
		if ( Input.GetButton ( "Fire1" ) && Input.GetAxis ( "Vertical" ) > 0 ) {
			Zoom ( -Zoom_Speed ) ;
		} else if ( Input.GetButton ( "Fire1" ) && Input.GetAxis ( "Vertical" ) < 0 ) {
			Zoom ( Zoom_Speed ) ;
		}
		Temp_Horizontal = Input.GetAxis ( "Horizontal2" ) ;
		Temp_Vertical = Input.GetAxis ( "Vertical2" ) ;
		if ( Temp_Horizontal == 0.0f && Temp_Vertical == 0.0f ) {
			Turn_Flag = false ;
		} else {
			Turn_Flag = true ;
			Stick_Rotate () ;
		}
	}
	
	void Mouse_Input () {
		if ( Input.GetMouseButtonDown ( 1 ) ) {
			Last_Mouse_Pos = Input.mousePosition ;
		}
		if ( Input.GetMouseButtonUp ( 1 ) ) {
			Turn_Flag = false ;
		}
		if ( Input.GetMouseButton ( 1 ) ) {
			// Zoom
			if ( Input.GetAxis ( "Mouse ScrollWheel" ) > 0 ) {
				Zoom ( -Zoom_Speed * 3.0f ) ;
			} else if ( Input.GetAxis ( "Mouse ScrollWheel" ) < 0 ) {
				Zoom ( Zoom_Speed * 3.0f ) ;
			}
			// Turn
			Vector2 Current_Mouse_Pos =  Input.mousePosition ;
			if ( !Turn_Flag ) {
				if ( Last_Mouse_Pos.x != Current_Mouse_Pos.x || Last_Mouse_Pos.y != Current_Mouse_Pos.y ) {
					Turn_Flag = true ;
				}
			}
			if ( Turn_Flag ) {
				Temp_Horizontal = ( Current_Mouse_Pos.x - Last_Mouse_Pos.x ) * 0.1f ;
				Temp_Vertical = ( Current_Mouse_Pos.y - Last_Mouse_Pos.y ) * 0.1f ;
				Rotate () ;
			}
			Last_Mouse_Pos = Current_Mouse_Pos ;
		}
	}

	void Zoom ( float Temp_Speed ) {
		FOV += Temp_Speed ;
		FOV = Mathf.Clamp ( FOV , Min_FOV , Max_FOV ) ;
		This_Camera.fieldOfView = FOV ;
	}

	void Rotate () {
		if ( Temp_Horizontal != 0.0f ) {
			float Temp_X = This_Transform.localEulerAngles.x ;
			float Temp_Y = This_Transform.localEulerAngles.y + Temp_Horizontal * Horizontal_Speed ;
			float Temp_Z = This_Transform.localEulerAngles.z ;
			This_Transform.localEulerAngles = new Vector3 ( Temp_X , Temp_Y , Temp_Z ) ;
		} 
		if ( Temp_Vertical != 0.0f ) {
			float Temp_X = This_Transform.localEulerAngles.x - Temp_Vertical * Vertical_Speed ;
			float Temp_Y = This_Transform.localEulerAngles.y ;
			float Temp_Z = This_Transform.localEulerAngles.z ;
			This_Transform.localEulerAngles = new Vector3 ( Temp_X , Temp_Y , Temp_Z ) ;
		}
	}

	void Stick_Rotate () {
		This_Transform.LookAt ( Target_Transform ) ;
		This_Transform.eulerAngles = new Vector3 ( This_Transform.eulerAngles.x - Temp_Vertical * 22.5f , This_Transform.eulerAngles.y + Temp_Horizontal * 45.0f , 0.0f ) ;
	}
	
	void Control_Enabled () {
		if ( GunCamera_Mode == 2 || Flag == false ) {
			This_Camera.enabled = false ;
			This_AudioListener.enabled = false ;
			this.tag = "Untagged" ;
		} else {
			This_Camera.enabled = true ;
			This_AudioListener.enabled = true ;
			this.tag = "MainCamera" ;
		}
	}

	public void Receive_Current_ID ( int Temp_ID ) { // Called from this script and Game_Controller.
		Current_ID = Temp_ID ;
		if ( Game_Controller_Script.Operable_Tanks [ Temp_ID ] ) {
			Target_Transform = Game_Controller_Script.Operable_Tanks [ Temp_ID ].GetComponentInChildren < MainBody_Setting_CS > ().transform ;
			Target_Transform.BroadcastMessage ( "Switch_Camera" , Flag , SendMessageOptions.DontRequireReceiver ) ;
			Gun_Camera_CS Temp_Script = Target_Transform.GetComponentInChildren < Gun_Camera_CS > () ;
			if ( Temp_Script ) {
				GunCamera_Mode = Temp_Script.Mode ;
			} else {
				GunCamera_Mode = 0 ;
			}
			Control_Enabled () ;
		}
	}

	public void Change_GunCamera_Mode ( int Temp_Mode ) { // Called from "Gun_Camera". (This function is also called when the turret is broken.)
		GunCamera_Mode = Temp_Mode ;
		Control_Enabled () ;
	}
	
}