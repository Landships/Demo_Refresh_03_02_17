using UnityEngine;
using System.Collections;

public class Look_At_Point_CS : MonoBehaviour {

	public float Offset_X = 0.0f ;
	public float Offset_Y = -1.0f ;
	public float Offset_Z = 0.75f ;
	public float Horizontal_Speed = 3.0f ;
	public float Vertical_Speed = 2.0f ;
	public bool Invert_Flag = false ;
	
	Transform This_Transform ;
	Vector3 Default_Position ;
	float Angle_Y ;
	float Angle_Z ;
	bool TPS_View_Flag = true ;
	float Temp_Horizontal ;
	float Temp_Vertical ;
	int Invert_Num = 1 ;
	Vector2 Last_Mouse_Pos ;
	Transform MainBody_Transform ;
	Camera Main_Camera ;

	bool Flag = true ;
	int Tank_ID ;
	int Input_Type = 4 ;
	
	void Start () {
		This_Transform = transform ;
		Default_Position = This_Transform.localPosition ;
		This_Transform.localPosition = Default_Position + new Vector3 ( Offset_X , Offset_Y , Offset_Z ) ;
		Angle_Y = This_Transform.eulerAngles.y ;
		Angle_Z = This_Transform.eulerAngles.z ;
		if ( Invert_Flag ) {
			Invert_Num = -1 ;
		} else {
			Invert_Num = 1 ;
		}
		MainBody_Transform = GetComponentInParent < MainBody_Setting_CS > ().transform ;
		Main_Camera = GetComponentInChildren < Camera > () ;
		//
		this.enabled = false ;
		this.enabled = true ;
	}
	
	void Update () {
		if ( Flag ) {
			/*if ( Main_Camera.enabled ) {
				switch ( Input_Type ) {
				case 0 :
					KeyBoard_Input () ;
					break ;
				case 1 :
					GamePad_Input () ;
					break ;
				case 2 :
					KeyBoard_Input () ;
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
			}*/
		}
	}
	
	void KeyBoard_Input () {
		Temp_Horizontal = Input.GetAxis ( "Horizontal2" ) ;
		Temp_Vertical = Input.GetAxis ( "Vertical2" ) ;
		if ( TPS_View_Flag ) {
			Rotate_TPS () ;
		} else {
			Rotate_FPS () ;
		}
	}
	
	void GamePad_Input (){
		if ( Input.GetButton ( "L_Button" ) == false ) {
			Temp_Horizontal = Input.GetAxis ( "Horizontal2" ) ;
			Temp_Vertical = Input.GetAxis ( "Vertical2" ) ;
		} else {
			Temp_Horizontal = 0.0f ;
			Temp_Vertical = 0.0f ;
		}
		if ( TPS_View_Flag ) {
			Rotate_TPS () ;
		} else {
			Rotate_FPS () ;
		}
	}
	
	void Mouse_Input () {
		if ( Input.GetMouseButtonDown ( 1 ) ) {
			Last_Mouse_Pos = Input.mousePosition ;
		}
		if ( Input.GetMouseButton ( 1 ) ) {
			Temp_Horizontal = ( Input.mousePosition.x - Last_Mouse_Pos.x ) * 0.1f ;
			Temp_Vertical = ( Input.mousePosition.y - Last_Mouse_Pos.y ) * 0.1f ;
			Last_Mouse_Pos = Input.mousePosition ;
		} else {
			Temp_Horizontal = 0.0f ;
			Temp_Vertical = 0.0f ;
		}
		
		if ( TPS_View_Flag ) {
			Rotate_TPS () ;
		} else {
			Rotate_FPS () ;
		}
	}

	void Rotate_TPS () {
		Angle_Y += Temp_Horizontal * Horizontal_Speed ;
		Angle_Z -= Temp_Vertical * Invert_Num * Vertical_Speed ;
		This_Transform.rotation = Quaternion.Euler ( 0.0f , Angle_Y , Angle_Z ) ;
	}

	void Rotate_FPS () {
		if ( Temp_Horizontal != 0.0f ) {
			float Temp_X = This_Transform.localEulerAngles.x ;
			float Temp_Y = This_Transform.localEulerAngles.y + Temp_Horizontal * Horizontal_Speed ;
			float Temp_Z = This_Transform.localEulerAngles.z ;
			This_Transform.localEulerAngles = new Vector3 ( Temp_X , Temp_Y , Temp_Z ) ;
		} 
		if ( Temp_Vertical != 0.0f ) {
			float Temp_X = This_Transform.localEulerAngles.x ;
			float Temp_Y = This_Transform.localEulerAngles.y ;
			float Temp_Z = This_Transform.localEulerAngles.z - Temp_Vertical * Invert_Num * Vertical_Speed ;
			This_Transform.localEulerAngles = new Vector3 ( Temp_X , Temp_Y , Temp_Z ) ;
		}
	}
	
	void Switch_View ( bool Temp_Flag ) {
		TPS_View_Flag = Temp_Flag ;
		if ( TPS_View_Flag ) {
			Angle_Y = This_Transform.eulerAngles.y ;
			Angle_Z = This_Transform.eulerAngles.z ;
			This_Transform.localPosition = Default_Position + new Vector3 ( Offset_X , Offset_Y , Offset_Z ) ;
		} else {
			This_Transform.localEulerAngles = new Vector3 ( 0.0f , 90.0f , 0.0f ) ;
			This_Transform.localPosition = Default_Position ;
		}	
	}

	void Turret_Linkage () {
		This_Transform.parent = MainBody_Transform ;
	}
	
	void Set_Input_Type ( int Temp_Input_Type ) {
		Input_Type = Temp_Input_Type ;
	}
	
	void Set_Tank_ID ( int Temp_Tank_ID ) {
		Tank_ID = Temp_Tank_ID ;
	}
	
	void Receive_Current_ID ( int Temp_Current_ID ) {
		if ( Temp_Current_ID == Tank_ID ) {
			Flag = true ;
		} else {
			Flag = false ;
		}
	}
}