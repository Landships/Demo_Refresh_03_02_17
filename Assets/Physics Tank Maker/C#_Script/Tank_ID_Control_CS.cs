using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class Tank_ID_Control_CS: MonoBehaviour {

	public int Tank_ID = 1 ;
	public int Relationship ;
	public int ReSpawn_Times = 0 ;
	public float Attack_Multiplier = 1.0f ;
	public int Input_Type = 4 ;
	public int Turn_Type = 0 ;

	// 'public' is needed for Editor script.
	public bool ReSpawn_Flag ;
	public string Prefab_Path ;
	
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
	// AI Auto respawn settings.
	public float ReSpawn_Interval = 10.0f ;
	public float Remove_Time = 30.0f ;
	// Stored values set and used by RC_Camera.
	public Vector3 [] Camera_Positions ;
	
	bool Flag = true ; 
	int Current_ID = 1 ;

	Game_Controller_CS Game_Controller_Script ;

	void Awake () {
		// Change the hierarchy.
		if ( GetComponentInParent < Event_Controller_CS > () == null) {
			transform.parent = null ;
		}
		// Store the defualt color.
		if ( AI_State_Text ) {
			Default_Text_Color = AI_State_Text.color ;
		}
	}

	void Start () {
		// Case of spawning in event.
		Event_Controller_CS Event_Script = GetComponentInParent < Event_Controller_CS > () ;
		if ( Event_Script ) { // Overwritten by Event_Controller.
			Tank_ID = Event_Script.Tank_ID ;
			Relationship = Event_Script.Relationship ;
			ReSpawn_Times = Event_Script.ReSpawn_Times ;
			Attack_Multiplier = Event_Script.Attack_Multiplier ;
			Input_Type = Event_Script.Input_Type ;
			Turn_Type = Event_Script.Turn_Type ;
			if ( Event_Script.OverWrite_Flag ) { // Overwrite AI settings.
				WayPoint_Pack = Event_Script.WayPoint_Pack ;
				Patrol_Type = Event_Script.Patrol_Type ;
				Follow_Target = Event_Script.Follow_Target ;
				No_Attack = Event_Script.No_Attack ;
				Visibility_Radius = Event_Script.Visibility_Radius ;
				Approach_Distance = Event_Script.Approach_Distance ;
				OpenFire_Distance = Event_Script.OpenFire_Distance ;
				Lost_Count = Event_Script.Lost_Count ;
				Face_Enemy = Event_Script.Face_Enemy ;
				Face_Offest_Angle = Event_Script.Face_Offest_Angle ;
				AI_State_Text = Event_Script.AI_State_Text ;
				Default_Text_Color = Event_Script.Default_Text_Color ;
				Tank_Name = Event_Script.Tank_Name ;
				ReSpawn_Interval = Event_Script.ReSpawn_Interval ;
				Remove_Time = Event_Script.Remove_Time ;
			}
		}
		//
		Set_Tag () ;
		// Find Game_Controller.
		GameObject Temp_Object = GameObject.Find ( "Game_Controller" ) ;
		// Send this Transform, and Get Tank_ID.
		if ( Temp_Object ) {
			Game_Controller_Script = Temp_Object.GetComponent < Game_Controller_CS > () ;
			if ( Game_Controller_Script ) {
				Tank_ID = Game_Controller_Script.Receive_ID ( Tank_ID , this.transform ) ;
			}
		}
		// Send settings to all parts.
		Send_Settings () ;
	}

	void Set_Tag () { // This function is called also from 'ReSpawn ()'.
		if ( Relationship == 0 ) { // 0 = Friendly.
			transform.root.tag = "Player" ;
		} else { // 1 = Hostile.
			transform.root.tag = "Untagged" ;
		}
	}

	void Send_Settings () { // This function is called also from 'ReSpawn'.
		// Broadcast Input_Type.
		if ( GetComponentInChildren < AI_CS > () ) { //This tank has AI.
			Input_Type = 10 ;
		} else if ( Input_Type == 10 ) { // This tank has no AI, but Input_Type is set to "AI".
			Input_Type = 4 ;
		}
		BroadcastMessage ( "Set_Input_Type" , Input_Type , SendMessageOptions.DontRequireReceiver ) ;
		if ( Input_Type == 0 || Input_Type == 1 || Input_Type == 5 ) {
			BroadcastMessage ( "Set_Turn_Type" , Turn_Type , SendMessageOptions.DontRequireReceiver ) ;
		}
		// Broadcast Tank_ID and Current_ID.
		BroadcastMessage ( "Set_Tank_ID" , Tank_ID , SendMessageOptions.DontRequireReceiver ) ;
		BroadcastMessage ( "Receive_Current_ID" , Current_ID , SendMessageOptions.DontRequireReceiver ) ;
	}

	void Update () {
		if ( Flag ) {
			if ( Input.GetKeyDown ( KeyCode.Backspace ) ) {
				Application.LoadLevel ( Application.loadedLevelName ) ; // ReLoad this scene.
			} else if ( Input.GetKeyDown ( KeyCode.Return ) ) {
				if ( ReSpawn_Flag && ReSpawn_Times > 0 ) {
					ReSpawn () ;
				}
			}
		}
	}

	void ReSpawn () {
		// Make sure that the prefab exists.
		GameObject Check_Object = Resources.Load ( Prefab_Path ) as GameObject ;
		if ( Check_Object == null ) {
			ReSpawn_Flag = false ;
			return ;
		}
		//
		ReSpawn_Times -= 1 ;
		// This object is continuously used even when a new tank is spawned.
		// Destroy child parts.
		int Temp_Num = transform.childCount ;
		for ( int i = 0 ;  i  < Temp_Num ; i++ ) {
			DestroyImmediate ( transform.GetChild ( 0 ).gameObject ) ;
		}
		if ( transform.childCount == 0 ) { // Destroying succeeded.
			// Set this Tag again.
			Set_Tag () ;
			// Instantiate the new tank with reference to the Prefab_Path from 'Resources' folder.
			GameObject Temp_Object = Instantiate ( Resources.Load ( Prefab_Path ) , transform.position , transform.rotation ) as GameObject ;
			// Change the hierarchy of the new tank.
			Temp_Num = Temp_Object.transform.childCount ;
			for ( int i = 0 ;  i  < Temp_Num ; i++ ) {
				Temp_Object.transform.GetChild ( 0 ).parent = this.transform ; // New child objects are moved under this object as its children.
			}
			// Destroy the top object of the new tank.
			DestroyImmediate ( Temp_Object ) ;
			// Broadcast settings to new children.
			Send_Settings () ;
			// Reset the stored components in the "Game_Controller".
			Game_Controller_Script.ReSpawn_ReSetting () ;
		}
	}

	void MainBody_Linkage () { // Called from "Damage_Control" in the MainBody.
		transform.root.tag = "Finish" ;
		if ( Input_Type == 10 ) {
			if ( ReSpawn_Flag && ReSpawn_Times > 0 ) {
				StartCoroutine ( "Auto_ReSpawn" ) ;
			} else {
				if ( Remove_Time != Mathf.Infinity ) {
					StartCoroutine ( "Remove_Tank" , Remove_Time ) ;
				}
			}
		}
	}

	IEnumerator Auto_ReSpawn () {
		yield return new WaitForSeconds ( ReSpawn_Interval ) ;
		//yield return null ;
		ReSpawn () ;
	}

	public IEnumerator Remove_Tank ( float Temp_Time ) { // Also called from Event_Controller.
		yield return new WaitForSeconds ( Temp_Time ) ;
		if ( Game_Controller_Script.Remove_Tank ( Tank_ID , this.transform ) ) {
			gameObject.SetActive ( false ) ;
		}
	}

	void Receive_Current_ID ( int Temp_Current_ID ) {
		Current_ID = Temp_Current_ID ;
		if ( Temp_Current_ID == Tank_ID ) {
			Flag = true ;
		} else {
			Flag = false ;
		}
	}

}
