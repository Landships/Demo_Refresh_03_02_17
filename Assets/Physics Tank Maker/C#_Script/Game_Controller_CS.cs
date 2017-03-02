using UnityEngine;
using System.Collections;

public class Game_Controller_CS : MonoBehaviour {

	public float Assign_Frequency = 3.0f ;

	public float Time_Scale = 1.0f ;
	public float Gravity = -9.81f ;
	public float Fixed_TimeStep = 0.02f ;

	int Max_Friendly_Num ;
	int Max_Hostile_Num ;
	public Transform [] Operable_Tanks ; // Referred to from RC_Camera.
	public Transform [] Not_Operable_Tanks ;
	public Transform [] Friendly_Body_Transforms ;
	public Transform [] Hostile_Body_Transforms ;
	public MainBody_Setting_CS [] Friendly_Body_Scripts ;
	public MainBody_Setting_CS [] Hostile_Body_Scripts ;
	public AI_CS [] Friendly_AI_Scripts ;
	public AI_CS [] Hostile_AI_Scripts ;

	public float Assign_Count ; // Referred to from AI.
	public float Initial_TimeScale ;
	public RC_Camera_CS RC_Camera_Script ;

	int Current_ID = 1 ;

	void Awake () {
		this.gameObject.name = "Game_Controller" ; // This name is referred to by "Tank_ID_Control".
		this.tag = "GameController" ;
		// Modify the Physics and Time manager.
		Initial_TimeScale = Time_Scale ;
		Set_TimeScale () ;
		Physics.gravity = new Vector3 ( 0.0f , Gravity , 0.0f ) ;
		Physics.sleepThreshold = 0.5f ;
		Time.fixedDeltaTime = Fixed_TimeStep ;
		// Find RC_Camera.
		RC_Camera_Script = FindObjectOfType < RC_Camera_CS > () ;
		// Count the number of tanks.
		int Operable_Count = 0 ;
		int Not_Operable_Count = 0 ;
		int Friendly_Count = 0 ;
		int Hostile_Count = 0 ;
		Event_Controller_CS [] Temp_Event_Scripts = FindObjectsOfType < Event_Controller_CS > () ;
		for ( int i = 0 ; i < Temp_Event_Scripts.Length ; i ++ ) {
			if ( Temp_Event_Scripts [ i ].Event_Type == 0 ) {
				if ( Temp_Event_Scripts [ i ].Tank_ID != 0 ) {
					Operable_Count += 1 ;
				} else {
					Not_Operable_Count += 1 ;
				}
				if ( Temp_Event_Scripts [ i ].Relationship == 0 ) {
					Friendly_Count += 1 ;
				} else {
					Hostile_Count += 1 ;
				}
			} 
		}
		Tank_ID_Control_CS [] Temp_Top_Scripts = FindObjectsOfType < Tank_ID_Control_CS > () ;
		for ( int i = 0 ; i < Temp_Top_Scripts.Length ; i ++ ) {
			if ( Temp_Top_Scripts [ i ].Tank_ID != 0 ) {
				Operable_Count += 1 ;
			} else {
				Not_Operable_Count += 1 ;
			}
			if ( Temp_Top_Scripts [ i ].Relationship == 0 ) {
				Friendly_Count += 1 ;
			} else {
				Hostile_Count += 1 ;
			}
		}
		if ( Operable_Count > 10 ) {
			Not_Operable_Count += Operable_Count - 10 ;
			Operable_Count = 10 ;
		}
		//Debug.Log ( "Operable" + Operable_Count + "/" + "Not_Operable" + Not_Operable_Count + "::" + "Frinedly" + Friendly_Count + "/" + "Hostile" +  Hostile_Count ) ;
		Operable_Tanks = new Transform [ 11 ] ;
		Not_Operable_Tanks = new Transform [ Not_Operable_Count ] ;
		Max_Friendly_Num = Friendly_Count ;
		Max_Hostile_Num = Hostile_Count ;
		Friendly_Body_Transforms = new Transform [ Max_Friendly_Num ] ;
		Hostile_Body_Transforms = new Transform [ Max_Hostile_Num ] ;
		Friendly_Body_Scripts = new MainBody_Setting_CS [ Max_Friendly_Num ] ;
		Hostile_Body_Scripts = new MainBody_Setting_CS [ Max_Hostile_Num ] ;
		Friendly_AI_Scripts = new AI_CS [ Max_Friendly_Num ] ;
		Hostile_AI_Scripts = new AI_CS [ Max_Hostile_Num ] ;
	}

	public int Receive_ID ( int Temp_ID , Transform Temp_Transform ) { // Called from "Tank_ID_Control" at the opening.
		// In case of operable.
		if ( Temp_ID != 0 ) {
			if ( Operable_Tanks [ Temp_ID ] == null ) { // Operable_Tanks[ # ] is empty.
				Operable_Tanks [ Temp_ID ] = Temp_Transform ;
				Store_Components ( Temp_Transform ) ; // Store MainBodies and AI scripts.
				return Temp_ID ;
			} else { // Operable_Tanks[ # ] is not empty.
				for ( int i = 1 ; i < Operable_Tanks.Length ; i++ ) { // Search empty ID number.
					if ( Operable_Tanks [ i ] == null ) {
						Operable_Tanks [ i ] = Temp_Transform ;
						Store_Components ( Temp_Transform ) ; // Store MainBodies and AI scripts.
						return i ;
					}
				}
			}
		}
		// In case of Not operable, or "Operable_Tanks" is full.
		for ( int i = 0 ; i < Not_Operable_Tanks.Length ; i++ ) {
			if ( Not_Operable_Tanks [ i ] == null ) {
				Not_Operable_Tanks [ i ] = Temp_Transform ;
				Store_Components ( Temp_Transform ) ;
				return 0 ;
			}
		}
		Debug.LogWarning ( "'Game_Controller' cannot find all the tanks in the scene. (Physics Tank Maker)" ) ;
		return 0 ;
	}

	void Store_Components ( Transform Temp_Transform ) { // Store MainBody's transform and sctipt ,and AI scripts.
		MainBody_Setting_CS Temp_MainBodyScript = Temp_Transform.GetComponentInChildren < MainBody_Setting_CS > () ;
		if ( Temp_MainBodyScript ) {
			if ( Temp_Transform.root.tag == "Player" ) { // Friendly tank.
				for ( int i = 0 ; i < Friendly_Body_Transforms.Length ; i++ ) { // Search empty.
					if ( Friendly_Body_Transforms [ i ] == null ) {
						// Store components.
						Friendly_Body_Transforms [ i ] = Temp_MainBodyScript.transform ;
						Friendly_Body_Scripts [ i ] = Temp_MainBodyScript ;
						AI_CS Temp_AI_Script = Friendly_Body_Transforms [ i ].GetComponentInChildren < AI_CS > () ;
						if ( Temp_AI_Script ) {
							Friendly_AI_Scripts [ i ] = Temp_AI_Script ;
						}
						break ;
					}
				}
			} else if ( Temp_Transform.root.tag != "Finish" ) { // Hostile tank.
				for ( int i = 0 ; i < Hostile_Body_Transforms.Length ; i++ ) { // Search empty.
					if ( Hostile_Body_Transforms [ i ] == null ) {
						// Store components.
						Hostile_Body_Transforms [ i ] = Temp_MainBodyScript.transform ;
						Hostile_Body_Scripts [ i ] = Temp_MainBodyScript ;
						AI_CS Temp_AI_Script = Hostile_Body_Transforms [ i ].GetComponentInChildren < AI_CS > () ;
						if ( Temp_AI_Script ) {
							Hostile_AI_Scripts [ i ] = Temp_AI_Script ;
						}
						break ;
					}
				}
			}
		} else {
			Debug.LogWarning ( "'Game_Controller' cannot find the MainBody of '" + Temp_Transform.name + "'. (Physics Tank Maker)" ) ;
		}
	}
	
	void Update () {
		if ( Assign_Count <= 0.0f ) {
				Assign_Count = Assign_Frequency ;
				Assign_Target () ;
			} else {
				Assign_Count -= Time.deltaTime ;
			}
		if ( Input.anyKeyDown ) {
			Key_Check () ;
		}
	}

	void Assign_Target () {
		// Assign the target to friendly AI tanks.
		float Min_Distance = 10000.0f ;
		Transform Temp_Target = null ;
		MainBody_Setting_CS Temp_Target_Script = null ;
		for ( int i = 0 ; i < Friendly_Body_Transforms.Length ; i++ ) {
			if ( Friendly_AI_Scripts [ i ] && !Friendly_AI_Scripts [ i ].Fire_Flag ) {
				for ( int j = 0 ; j < Hostile_Body_Transforms.Length ; j++ ) {
					if ( Hostile_Body_Transforms [ j ] && Hostile_Body_Transforms [ j ].root.tag != "Finish" ) {
						float Temp_Distance = Vector3.Distance ( Friendly_Body_Transforms [ i ].position , Hostile_Body_Transforms [ j ].position ) ;
						if ( Temp_Distance < Friendly_AI_Scripts [ i ].Visibility_Radius && Temp_Distance < Min_Distance ) {
							if ( Friendly_AI_Scripts [ i ].RayCast_Check ( Hostile_Body_Transforms [ j ] , Hostile_Body_Scripts [ j ] ) ) {
								Min_Distance = Temp_Distance ;
								Temp_Target = Hostile_Body_Transforms [ j ] ;
								Temp_Target_Script = Hostile_Body_Scripts [ j ] ;
							}
						}
					}
				}
				if ( Temp_Target ) {
					Friendly_AI_Scripts [ i ].Set_Target ( Temp_Target , Temp_Target_Script ) ;
				}
			}
		}
		// Assign the target to hostile AI tanks.
		Min_Distance = 10000.0f ;
		Temp_Target = null ;
		Temp_Target_Script = null ;
		for ( int i = 0 ; i < Hostile_Body_Transforms.Length ; i++ ) {
			if ( Hostile_AI_Scripts [ i ] && !Hostile_AI_Scripts [ i ].Fire_Flag ) {
				for ( int j = 0 ; j < Friendly_Body_Transforms.Length ; j++ ) {
					if ( Friendly_Body_Transforms [ j ] && Friendly_Body_Transforms [ j ].root.tag != "Finish" ) {
						float Temp_Distance = Vector3.Distance ( Hostile_Body_Transforms [ i ].position , Friendly_Body_Transforms [ j ].position ) ;
						if ( Temp_Distance < Hostile_AI_Scripts [ i ].Visibility_Radius && Temp_Distance < Min_Distance ) {
							if ( Hostile_AI_Scripts [ i ].RayCast_Check ( Friendly_Body_Transforms [ j ] , Friendly_Body_Scripts [ j ] ) ) {
								Min_Distance = Temp_Distance ;
								Temp_Target = Friendly_Body_Transforms [ j ] ;
								Temp_Target_Script = Friendly_Body_Scripts [ j ] ;
							}
						}
					}
				}
				if ( Temp_Target ) {
					// Send message to "AI".
					Hostile_AI_Scripts [ i ].Set_Target ( Temp_Target , Temp_Target_Script ) ;
				}
			}
		}
	}

	void Key_Check () {
		if ( Input.GetKeyDown ( KeyCode.Keypad1 ) || Input.GetKeyDown ( "1" ) ) {
			Check_and_Cast_Current_ID ( 1 ) ;
			return ;
		} else if ( Input.GetKeyDown ( KeyCode.Keypad2 ) || Input.GetKeyDown ( "2" ) ) {
			Check_and_Cast_Current_ID ( 2 ) ;
			return ;
		} else if ( Input.GetKeyDown ( KeyCode.Keypad3 ) || Input.GetKeyDown ( "3" ) ) {
			Check_and_Cast_Current_ID ( 3 ) ;
			return ;
		} else if ( Input.GetKeyDown ( KeyCode.Keypad4 ) || Input.GetKeyDown ( "4" ) ) {
			Check_and_Cast_Current_ID ( 4 ) ;
			return ;
		} else if ( Input.GetKeyDown ( KeyCode.Keypad5 ) || Input.GetKeyDown ( "5" ) ) {
			Check_and_Cast_Current_ID ( 5 ) ;
			return ;
		} else if ( Input.GetKeyDown ( KeyCode.Keypad6 ) || Input.GetKeyDown ( "6" ) ) {
			Check_and_Cast_Current_ID ( 6 ) ;
			return ;
		} else if ( Input.GetKeyDown ( KeyCode.Keypad7 ) || Input.GetKeyDown ( "7" ) ) {
			Check_and_Cast_Current_ID ( 7 ) ;
			return ;
		} else if ( Input.GetKeyDown ( KeyCode.Keypad8 ) || Input.GetKeyDown ( "8" ) ) {
			Check_and_Cast_Current_ID ( 8 ) ;
			return ;
		} else if ( Input.GetKeyDown ( KeyCode.Keypad9 ) || Input.GetKeyDown ( "9" ) ) {
			Check_and_Cast_Current_ID ( 9 ) ;
			return ;
		} else if ( Input.GetKeyDown ( KeyCode.Keypad0 ) || Input.GetKeyDown ( "0" ) ) {
			Check_and_Cast_Current_ID ( 10 ) ;
			return ;
		} else if ( Input.GetKeyDown ( KeyCode.KeypadPlus ) ) {
			Time_Scale += 0.5f ;
			Set_TimeScale () ;
			return ;
		} else if ( Input.GetKeyDown ( KeyCode.KeypadMinus ) ) {
			Time_Scale -= 0.5f ;
			Set_TimeScale () ;
			return ;
		} else if ( Input.GetKeyDown ( KeyCode.KeypadEnter ) ) {
			Time_Scale = Initial_TimeScale ;
			Set_TimeScale () ;
			return ;
		}
	}

	void Check_and_Cast_Current_ID ( int Temp_ID ) {
		if  ( Operable_Tanks.Length > Temp_ID ) { // To avoid overflowing.
			if ( Operable_Tanks [ Temp_ID ] ) {
				Current_ID = Temp_ID ;
				// Broadcast Current_ID to all tanks.
				for ( int i = 0 ; i < Operable_Tanks.Length ; i++ ) {
					if ( Operable_Tanks [ i ] ) {
						Operable_Tanks [ i ].BroadcastMessage ( "Receive_Current_ID" , Current_ID , SendMessageOptions.DontRequireReceiver ) ;
					}
				}
				// Send Current_ID to RC_Camera.
				if ( RC_Camera_Script ) {
					RC_Camera_Script.Receive_Current_ID ( Current_ID ) ;
				}
			}
		}
	}

	void Set_TimeScale () {
		Time_Scale = Mathf.Clamp ( Time_Scale , 0.5f , 10.0f ) ;
		Time.timeScale = Time_Scale ;
	}

	public void ReSpawn_ReSetting () { // Called from "Tank_ID_Control" when ReSpawn.
		// Store MainBodies and AI scripts again.
		Friendly_Body_Transforms = new Transform [ Max_Friendly_Num ] ;
		Hostile_Body_Transforms = new Transform [ Max_Hostile_Num ] ;
		Friendly_Body_Scripts = new MainBody_Setting_CS [ Max_Friendly_Num ] ;
		Hostile_Body_Scripts = new MainBody_Setting_CS [ Max_Hostile_Num ] ;
		Friendly_AI_Scripts = new AI_CS [ Max_Friendly_Num ] ;
		Hostile_AI_Scripts = new AI_CS [ Max_Hostile_Num ] ;
		for ( int i = 0 ; i < Operable_Tanks.Length ; i++ ) {
			if ( Operable_Tanks [ i ] ) {
				Store_Components ( Operable_Tanks [ i ] ) ;
			}
		}
		for ( int i = 0 ; i < Not_Operable_Tanks.Length ; i++ ) {
			if ( Not_Operable_Tanks [ i ] ) {
				Store_Components ( Not_Operable_Tanks [ i ] ) ;
			}
		}
		Assign_Count = 0.0f ;
	}

	public bool Remove_Tank ( int Temp_Tank_ID , Transform Temp_Transform ) { // Called from "Tank_ID_Control".
		if ( Temp_Tank_ID != 0 ) {
			for ( int i = 0 ; i < Operable_Tanks.Length ; i++ ) {
				if ( Operable_Tanks [ i ] == Temp_Transform ) {
					Operable_Tanks [ i ] = null ;
					break ;
				}
			}
		} else {
			for ( int i = 0 ; i < Not_Operable_Tanks.Length ; i++ ) {
				if ( Not_Operable_Tanks [ i ] == Temp_Transform ) {
					Not_Operable_Tanks [ i ] = null ;
					break ;
				}
			}
		}
		// Change the Current_ID when the tank is current.
		if ( Temp_Tank_ID == Current_ID ) {
			for ( int i = 1 ; i < Operable_Tanks.Length ; i++ ) {
				if ( Operable_Tanks [ i ] ) {
					Check_and_Cast_Current_ID ( i ) ;
					return true ;
				}
			}
			return false ;
		}
		//
		return true ;
	}

}
