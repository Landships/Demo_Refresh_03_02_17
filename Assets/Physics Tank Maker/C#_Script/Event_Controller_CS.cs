using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class Event_Controller_CS : MonoBehaviour {
	// Trigger settings.
	public int Trigger_Type ; // 0=Timer, 1=Destroy, 2=Trigger_Collider
	public float Trigger_Time ;
	public int Trigger_Num = 1 ;
	public Transform [] Trigger_Tanks ;
	public int Operator_Type ; // 0 = AND, 1 = OR.
	public bool All_Trigger_Flag = true ;
	public int Necessary_Num = 1 ;
	public Trigger_Collider_CS Trigger_Collider_Script ;
	public int Useless_Event_Num ;
	public Event_Controller_CS [] Useless_Events ;
	public int Disabled_Event_Num ;
	public Event_Controller_CS [] Disabled_Events ;
	public int Event_Type ; // 0=Spawn Tank , 1=Show Message , 2=Change AI Settings , 3=Remove Tank , 4=Artillery Fire ,  , 10=None
	
	// Text settings.
	public bool Show_Message_Flag ;
	public Text Event_Text ;
	public string Event_Message ;
	public Color Event_Message_Color = Color.white ;
	public float Fade_In_Time = 3.0f ;
	public float Display_Time = 10.0f ;
	public float Fade_Out_Time = 3.0f ;
	
	// Tank settings.
	public bool Spawn_Tank_Flag ;
	public GameObject Prefab_Object ;	
	public int Tank_ID = 1 ;
	public int Relationship ;
	public int ReSpawn_Times = 0 ;
	public float Attack_Multiplier = 1.0f ;
	public int Input_Type = 4 ;
	public int Turn_Type = 0 ;

	// AI settings.
	public bool OverWrite_Flag = true ;
	// AI patrol settings.
	public GameObject WayPoint_Pack ;
	public int Patrol_Type = 1 ; // 0 = Order, 1 = Random.
	public Transform Follow_Target ;
	// AI combat settings.
	public bool No_Attack = false ;
	public float Visibility_Radius = 512.0f ;
	public float Approach_Distance = 256.0f ;
	public float OpenFire_Distance = 512.0f ;
	public float Lost_Count = 20.0f ;
	public bool Face_Enemy = false ;
	public float Face_Offest_Angle = 0.0f ;
	// AI text settings.
	public Text AI_State_Text ;
	public string Tank_Name ;
	public Color Default_Text_Color ; // reffered by AI.
	// AI respawn settings.
	public float ReSpawn_Interval = 10.0f ;
	public float Remove_Time = 30.0f ;

	// New AI settings & Remove settings.
	public bool Trigger_Itself_Flag = true ;
	public int Target_Num = 1 ;
	public Transform [] Target_Tanks ;
	// New AI settings.
	public GameObject New_WayPoint_Pack ;
	public int New_Patrol_Type = 1 ;
	public Transform New_Follow_Target ;
	public bool New_No_Attack = false ;
	public bool Renew_ReSpawn_Times_Flag = false ;
	public int New_ReSpawn_Times ;

	// Artillery Fire settings.
	public Artillery_Fire_CS Artillery_Script ;
	public Transform Artillery_Target ;
	public int Artillery_Num ;

	bool Flag ;
	float Current_Time ;

	void Awake () {
		// Change the hierarchy.
		transform.parent = null ;
		// Store the defualt text color.
		if ( AI_State_Text ) {
			Default_Text_Color = AI_State_Text.color ;
		}
	}

	void Start () {
		// Check and set the trigger components.
		switch ( Trigger_Type ) {
		case 0 : // Timer
			Flag = true ;
			break ;
		case 1 : // Destroy
			Flag = Set_Transforms ( Trigger_Tanks ) ;
			break ;
		case 2 : // Trigger_Collider
			if ( Trigger_Collider_Script ) {
				Flag = Set_Transforms ( Trigger_Tanks ) ;
			} else {
				Flag = false ;
			}
			break ;
		}
		// Check and set the event components.
		switch ( Event_Type ) {
		case 0 : // Spawn Tank
			if ( Prefab_Object == null ) {
				Flag = false ;
			}
			break ;
		case 1 : // Show Message
			if ( Event_Text == null ) {
				Flag = false ;
			}
			break ;
		case 2 : // Change AI Settings
			if ( Trigger_Itself_Flag == false ) {
				Flag = Set_Transforms ( Target_Tanks ) ;
			}
			break ;
		case 3 : // Remove Tank
			if ( Trigger_Itself_Flag == false ) {
				Flag = Set_Transforms ( Target_Tanks ) ;
			}
			break ;
		case 4 : // Artillery Fire
			if ( Artillery_Script == null || Artillery_Target == null ) {
				Flag = false ;
			}
			break ;
		case 10 : // None
			break ;
		}
	}

	bool Set_Transforms ( Transform [] Temp_Transforms ) {
		int Temp_Count = 0 ;
		for ( int i = 0 ; i < Temp_Transforms.Length ; i++ ) {
			if ( Temp_Transforms [ i ] ) {
				Temp_Count += 1 ;
				Temp_Transforms [ i ] = Temp_Transforms [ i ].root ;
			}
		}
		if ( Temp_Count == 0 ) {
			return false ;
		} else {
			return true ;
		}
	}

	void Update () {
		if ( Flag ) {
			switch ( Trigger_Type ) {
			case 0 : // Timer
				Timer () ;
				break ;
			case 1 : // Destroy
				switch ( Operator_Type ) {
				case 0 : // AND
					Check_Destroy_AND () ;
					break ;
				case 1 : // OR
					Check_Destroy_OR () ;
					break ;
				}
				break ;
			case 2 : // Trigger_Collider
				switch ( Operator_Type ) {
				case 0 : // AND
					Check_Collider_AND () ;
					break ;
				case 1 : // OR
					Check_Collider_OR () ;
					break ;
				}
				break ;
			}
		}
	}

	void Timer () {
		Current_Time += Time.deltaTime ;
		if ( Current_Time >= Trigger_Time ) {
			Current_Time = 0.0f ;
			Flag = false ;
			Start_Event () ;
		}
	}

	void Check_Destroy_AND () {
		// Check Tags.
		if ( All_Trigger_Flag ) {
			for ( int i = 0 ; i < Trigger_Tanks.Length ; i++ ) {
				if ( Trigger_Tanks [ i ] && Trigger_Tanks [ i ].tag != "Finish" ) {
					return ;
				}
			}
		} else {
			int Temp_Num = 0 ;
			for ( int i = 0 ; i < Trigger_Tanks.Length ; i++ ) {
				if ( Trigger_Tanks [ i ] && Trigger_Tanks [ i ].tag == "Finish" ) {
					Temp_Num += 1 ;
				}
			}
			if ( Temp_Num < Necessary_Num ) {
				return ;
			}
		}
		// Check the remaining Auto ReSpawn Times. 
		for ( int i = 0 ; i < Trigger_Tanks.Length ; i++ ) {
			if ( Trigger_Tanks [ i ] && Trigger_Tanks [ i ].tag == "Finish" ) {
				Tank_ID_Control_CS Temp_Script = Trigger_Tanks [ i ].GetComponentInChildren < Tank_ID_Control_CS > () ;
				if ( Temp_Script && Temp_Script.ReSpawn_Times > 0 ) {
					return ;
				}
			}
		}
		// All the Auto ReSpawn Times are zero.
		Flag = false ;
		Start_Event () ;
	}

	void Check_Destroy_OR () {
		// Check Tags and remaining Auto ReSpawn Times. 
		for ( int i = 0 ; i < Trigger_Tanks.Length ; i++ ) {
			if ( Trigger_Tanks [ i ] && Trigger_Tanks [ i ].tag == "Finish" ) {
				Tank_ID_Control_CS Temp_Script = Trigger_Tanks [ i ].GetComponentInChildren < Tank_ID_Control_CS > () ;
				if ( Temp_Script && Temp_Script.ReSpawn_Times == 0 ) {
					Flag = false ;
					Start_Event () ;
					break ;
				}
			}
		}
	}

	void Check_Collider_AND () {
		if ( Trigger_Collider_Script.Collide_Transform != null ) {
			for ( int i = 0 ; i < Trigger_Tanks.Length ; i++ ) {
				if ( Trigger_Tanks [ i ] && Trigger_Collider_Script.Collide_Transform == Trigger_Tanks [ i ] ) {
					Trigger_Tanks [ i ] = null ;
					// Check the remaining trigger.
					if ( All_Trigger_Flag ) {
						for ( int j = 0 ; j < Trigger_Tanks.Length ; j++ ) {
							if ( Trigger_Tanks [ j ] ) {
								return ;
							}
						}
					} else {
						int Temp_Num = 0 ;
						for ( int j = 0 ; j < Trigger_Tanks.Length ; j++ ) {
							if ( Trigger_Tanks [ j ] == null ) {
								Temp_Num += 1 ;
							}
						}
						if ( Temp_Num < Necessary_Num ) {
							return ;
						}
					}
					// All the necessary Trigger_Tanks are Null.
					Flag = false ;
					Start_Event () ;
					break ;
				}
			}
		}
	}

	void Check_Collider_OR () {
		if ( Trigger_Collider_Script.Collide_Transform != null ) {
			for ( int i = 0 ; i < Trigger_Tanks.Length ; i++ ) {
				if ( Trigger_Tanks [ i ] && Trigger_Collider_Script.Collide_Transform == Trigger_Tanks [ i ] ) {
					Flag = false ;
					if ( ( Event_Type == 2 || Event_Type == 3 ) && Trigger_Itself_Flag ) { // "Change AI Settings" or "Remove Tank" && Target is Trigger itself.
						Target_Tanks [ 0 ] = Trigger_Tanks [ i ] ;
						Trigger_Tanks [ i ] = null ;
						// Check the remaining trigger.
						for ( int j = 0 ; j < Trigger_Tanks.Length ; j++ ) {
							if ( Trigger_Tanks [ j ] ) {
								Flag = true ;
								break ;
							}
						}
					}
					Start_Event () ;
					break ;
				}
			}
		}
	}


	void Start_Event () {
		// Control other triggers.
		Destroy_Useless_Events () ;
		Enable_Disabled_Events () ;
		// Start Event.
		switch ( Event_Type ) {
		case 0 : // Spawn Tank
			Spawn_Tank () ;
			break ;
		case 1 : // Show Message
			StartCoroutine ( "Show_Message" , Event_Message ) ;
			break ;
		case 2 : // Change AI Settings
			Change_AI_Settings () ;
			break ;
		case 3 : // Remove Tank
			Remove_Tank () ;
			break ;
		case 4 : // Artillery Fire
			Artillery_Fire () ;
			break ;
		case 10 : // None
			Destroy ( this.gameObject ) ;
			break ;
		}
	}

	void Destroy_Useless_Events () {
		for ( int i = 0 ; i < Useless_Events.Length ; i++ ) {
			if ( Useless_Events [ i ] ) {
				Destroy ( Useless_Events [ i ].gameObject ) ;
				Useless_Events [ i ] = null ;
			}
		}
	}

	void Enable_Disabled_Events () {
		for ( int i = 0 ; i < Disabled_Events.Length ; i++ ) {
			if ( Disabled_Events [ i ] ) {
				Disabled_Events [ i ].enabled = true ;
				Disabled_Events [ i ] = null ;
			}
		}
	}

	void Spawn_Tank () {
		GameObject Temp_Object ;
		Temp_Object = Instantiate ( Prefab_Object , transform.position , transform.rotation ) as GameObject ;
		Temp_Object.transform.parent = this.transform ;
		Destroy ( this ) ;
	}
	
	IEnumerator Show_Message ( string Temp_String ) {
		Color Temp_Color = Event_Message_Color ;
		float Default_Alpha = Event_Message_Color.a ;
		// Fade in.
		if ( Fade_In_Time == 0.0f ) {
			Event_Text.color = Event_Message_Color ;
		} else {
			float Count = 0.0f ;
			while ( Count <= Fade_In_Time ) {
				Event_Text.text = Temp_String ;
				Temp_Color.a = Mathf.Lerp ( 0.0f , Default_Alpha , Count / Fade_In_Time ) ;
				Event_Text.color = Temp_Color ;
				Count += Time.deltaTime ;
				yield return 0 ;
			}
			Temp_Color.a = Default_Alpha ;
			Event_Text.color = Temp_Color ;
		}
		// Keep displaying.
		if ( Display_Time == Mathf.Infinity ) {
			Destroy ( this.gameObject ) ;
		} else {
			yield return new WaitForSeconds ( Display_Time ) ;
		}
		// Fade out.
		if ( Fade_Out_Time == 0.0f ) {
			Event_Text.color = Event_Message_Color ;
		} else {
			float Count = 0.0f ;
			while ( Count <= Fade_Out_Time ) {
				Event_Text.text = Temp_String ;
				Temp_Color.a = Mathf.Lerp ( Default_Alpha , 0.0f , Count / Fade_Out_Time ) ;
				Event_Text.color = Temp_Color ;
				Count += Time.deltaTime ;
				yield return 0 ;
			}
			Temp_Color.a = 0.0f ;
			Event_Text.color = Temp_Color ;
		}
		Destroy ( this.gameObject ) ;
	}

	void Change_AI_Settings () {
		for ( int i = 0 ; i < Target_Tanks.Length ; i++ ) {
			// for waiting AI tank.
			Event_Controller_CS Temp_Event_Script = Target_Tanks [ i ].GetComponentInChildren < Event_Controller_CS > () ;
			if ( Temp_Event_Script ) {
				Temp_Event_Script.WayPoint_Pack = New_WayPoint_Pack ;
				Temp_Event_Script.Patrol_Type = New_Patrol_Type ;
				Temp_Event_Script.Follow_Target = New_Follow_Target ;
				Temp_Event_Script.No_Attack = New_No_Attack ;
			}
			// for living AI tank.
			AI_CS Temp_AI_Script = Target_Tanks [ i ].GetComponentInChildren < AI_CS > () ;
			if ( Temp_AI_Script ) {
				Temp_AI_Script.Change_AI_Settings ( this ) ;
			}
			// for Respawn.
			Tank_ID_Control_CS Temp_Top_Script = Target_Tanks [ i ].GetComponentInChildren < Tank_ID_Control_CS > () ;
			if ( Temp_Top_Script ) {
				Temp_Top_Script.WayPoint_Pack = New_WayPoint_Pack ;
				Temp_Top_Script.Patrol_Type = New_Patrol_Type ;
				Temp_Top_Script.Follow_Target = New_Follow_Target ;
				Temp_Top_Script.No_Attack = New_No_Attack ;
				if ( Renew_ReSpawn_Times_Flag ) {
					Temp_Top_Script.ReSpawn_Times = New_ReSpawn_Times ;
				}
			}
		}
		if ( Flag == false ) {
			Destroy ( this.gameObject ) ;
		}
	}

	void Remove_Tank () {
		for ( int i = 0 ; i < Target_Tanks.Length ; i++ ) {
			// Delete Event_Controller_CS script.
			Event_Controller_CS Temp_Event_Script = Target_Tanks [ i ].GetComponentInChildren < Event_Controller_CS > () ;
			if ( Temp_Event_Script ) {
				Destroy ( Temp_Event_Script ) ;
			}
			// Call "Remove_Tank()" in the Tank_ID_Control_CS script.
			Tank_ID_Control_CS Temp_Top_Script = Target_Tanks [ i ].GetComponentInChildren < Tank_ID_Control_CS > () ;
			if ( Temp_Top_Script ) {
				Temp_Top_Script.StartCoroutine ( "Remove_Tank" , 0.0f ) ;
			}
		}
		if ( Flag == false ) {
			Destroy ( this.gameObject ) ;
		}
	}

	void Artillery_Fire () {
		if ( Artillery_Script ) {
			Artillery_Script.Fire ( Artillery_Target , Artillery_Num ) ;
		}
		Destroy ( this.gameObject ) ;
	}

}
