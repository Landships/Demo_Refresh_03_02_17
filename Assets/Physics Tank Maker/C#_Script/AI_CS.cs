using UnityEngine;
using System.Collections;

using UnityEngine.UI;

[ RequireComponent ( typeof ( UnityEngine.AI.NavMeshAgent ) ) ]

public class AI_CS : MonoBehaviour {

	// Set from Tank_ID_Control.
	GameObject WayPoint_Pack ;
	int Patrol_Type ; // 0 = Order, 1 = Random.
	Transform Follow_Target ;
	bool No_Attack ;
	float Approach_Distance ;
	float Lost_Count ;
	bool Face_Enemy = false ;
	float Face_Offest_Angle = 0.0f ;
	Text AI_State_Text ;
	string Tank_Name ;

	// Set by user.
	public Transform Eye_Transform ;

	public float WayPoint_Radius = 5.0f ;
	public float Min_Turn_Angle = 1.0f ;
	public float Pivot_Turn_Angle = 60.0f ;
	public float Slow_Turn_Angle = 15.0f ;
	public float Slow_Turn_Rate = 0.4f ;
	public float Min_Turn_Rate = 0.1f ;
	public float Max_Turn_Rate = 1.0f ;
	public float Min_Speed_Rate = 0.3f ;
	public float Max_Speed_Rate = 1.0f ;
	public float Stuck_Count = 3.0f ;
	
	public bool Direct_Fire = true ;
	public float Fire_Angle = 2.0f ;
	public float Fire_Count = 0.5f ;
	public int Bullet_Type = 0 ; // 0 = AP.. 1 = HE.
	
	// Referred to from "Drive_Control".
	public float Speed_Order ;
	public float Turn_Order ;
	// Referred to from "Cannon_Vertical"
	public float Visibility_Radius ;
	public float OpenFire_Distance ;
	public float Target_Distance ;
	public Transform Target_Root_Transform ;
	public bool Detect_Flag = false ;
	public bool Near_Flag = false ;
	public float AI_Upper_Offset ;
	public float AI_Lower_Offset ;
	// Controled by "Cannon_Vertical"
	public bool Fire_Flag = false ;
	// Referred from "Steer_Wheel".
	public bool Slow_Turn_Flag ;

	Transform This_Transform ;
	Transform Parent_Transform ;
	UnityEngine.AI.NavMeshAgent This_Agent ;
	Vector3 Initial_Position ;
	Color Default_Text_Color ;
	Vector3 [] WayPoints ;
	Vector3 Toward_Pos ;
	Transform Follow_MainBody ;

	Transform Target_Transform ;
	int Action_Type ;
	float Lost_Time ;
	int Next_WayPoint_Num = -1 ;
	int Step = 1 ;
	float Search_Count ;
	float SetDestination_Count ;
	bool Toward_Flag = false ;
	int Ray_LayerMask = ~ ( ( 1 << 10 ) + ( 1 << 2 ) ) ; // Layer 2 = Ignore Ray, Layer 10 = Ignore All.

	Game_Controller_CS Game_Controller_Script ;
	AI_Hand_CS Hand_Script ;
	
	void Start () {
		This_Transform = transform ;
		Parent_Transform = This_Transform.parent ;
		// Variables settings.
		Variables_Settings () ;
		// Find "AI_Eye"
		if ( Eye_Transform == null ) {
			Eye_Transform = This_Transform.Find ( "AI_Eye" ) ; // Do not rename "AI_Eye".
			if ( Eye_Transform == null ) {
				Debug.LogError ( "'AI_Eye' can not be found. (Physics Tank Maker" ) ; 
			}
		}
		// Find AI_Hand_CS script.
		Hand_Script = This_Transform.GetComponentInChildren < AI_Hand_CS > () ;
		if ( Hand_Script == null ) {
			Debug.LogError ( "'AI_Hand' can not be found. (Physics Tank Maker" ) ; 
		}
		// Find Game_Controller.
		Game_Controller_Script = FindObjectOfType < Game_Controller_CS > () ;
		if ( Game_Controller_Script == null ) {
			Debug.LogError ( "'Game_Controller' can not be found. (Physics Tank Maker" ) ; 
		}
		// NavMeshAgent settings.
		Initial_Position = This_Transform.localPosition ; // for fixing this object on its default position.
		This_Agent = GetComponent < UnityEngine.AI.NavMeshAgent > () ;
		// Text settings.
		if ( AI_State_Text ) {
			if ( string.IsNullOrEmpty ( Tank_Name ) ) {
				Tank_Name = This_Transform.root.name ;
			}
			Text_Update ( "Search" , Default_Text_Color ) ;
		}
		// WayPoints settings.
		WayPoints_Settings () ;
		// Follow_Target settings.
		if ( Follow_Target ) {
			Follow_Target_Settings () ;
		} else {
			Update_Next_WayPoint () ; // Set the first waypoint.
		}
		// Send this script to "Turret_Horizontal", "Cannon_Vertical", "Drive_Control", "Steer_Wheel".
		Parent_Transform.BroadcastMessage ( "Get_AI" , this , SendMessageOptions.DontRequireReceiver ) ;
	}

	void Variables_Settings () {
		Event_Controller_CS Event_Script = GetComponentInParent < Event_Controller_CS > () ;
		if ( Event_Script && Event_Script.OverWrite_Flag ) { // Use settings in Event_Controller.
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
			Tank_Name = Event_Script.Tank_Name ;
			Default_Text_Color = Event_Script.Default_Text_Color ;
		} else { // Use settings in Tank_ID_Control.
			Tank_ID_Control_CS Top_Script = GetComponentInParent < Tank_ID_Control_CS > () ;
			WayPoint_Pack = Top_Script.WayPoint_Pack ;
			Patrol_Type = Top_Script.Patrol_Type ;
			Follow_Target = Top_Script.Follow_Target ;
			No_Attack = Top_Script.No_Attack ;
			Visibility_Radius = Top_Script.Visibility_Radius ;
			Approach_Distance = Top_Script.Approach_Distance ;
			OpenFire_Distance = Top_Script.OpenFire_Distance ;
			Lost_Count = Top_Script.Lost_Count ;
			Face_Enemy = Top_Script.Face_Enemy ;
			Face_Offest_Angle = Top_Script.Face_Offest_Angle ;
			AI_State_Text = Top_Script.AI_State_Text ;
			Tank_Name = Top_Script.Tank_Name ;
			Default_Text_Color = Top_Script.Default_Text_Color ;
		}
	}

	void WayPoints_Settings () {
		if ( WayPoint_Pack && WayPoint_Pack.transform.childCount > 1 ) { // WayPoint_Pack has more than two points.
			WayPoints = new Vector3 [ WayPoint_Pack.transform.childCount ] ;
			for ( int i = 0 ; i < WayPoint_Pack.transform.childCount ; i++ ) {
				WayPoints [ i ] = WayPoint_Pack.transform.GetChild ( i ).position ;
			}
		} else if ( WayPoint_Pack && WayPoint_Pack.transform.childCount == 1 ) { // WayPoint_Pack has only one point.
			WayPoints = new Vector3 [ 1 ] ;
			WayPoints [ 0 ] = WayPoint_Pack.transform.GetChild ( 0 ).position ;
			Toward_Pos = WayPoints [ 0 ] + ( This_Transform.forward * 100.0f ) ; // Set the initial direction.
		} else { // WayPoint_Pack is not assigined, or has no point.
			WayPoints = new Vector3 [ 1 ] ;
			WayPoints [ 0 ] = This_Transform.position ; // Set its initial position.
			Toward_Pos = WayPoints [ 0 ] + ( This_Transform.forward * 100.0f ) ; // Set the initial direction.
		}
	}

	void Follow_Target_Settings () {
		// Find the "MainBody" of Follow_Target.
		MainBody_Setting_CS MainBody_Script = Follow_Target.GetComponentInChildren < MainBody_Setting_CS > () ;
		if ( MainBody_Script ) {
			Follow_MainBody = MainBody_Script.transform ;
		}
	}

	public void Set_Target ( Transform Temp_Transform , MainBody_Setting_CS Temp_Script ) { // Called from "Game_Controller".
		if ( No_Attack ) {
			return ;
		}
		if ( Target_Transform != Temp_Transform ) {
			Lost_Target () ;
			Target_Transform = Temp_Transform ;
			Target_Root_Transform = Target_Transform.root ;
			if ( Temp_Script ) {
				AI_Upper_Offset = Temp_Script.AI_Upper_Offset ;
				AI_Lower_Offset = Temp_Script.AI_Lower_Offset ;
			}
		}
	}

	void Update () {
		// Fix this position and rotation.
		This_Transform.localPosition = Initial_Position ;
		This_Transform.localRotation = Quaternion.identity ;
		// Search the target.
		if ( Target_Transform ) {
			Search_Count -= Time.deltaTime ;
			if ( Search_Count < 0.0f ) {
				Search_Count = 1.0f ;
				Search () ;
			}
		}
		// Action
		switch ( Action_Type ) {
		case 0 : // Patrol mode.
			if ( Follow_Target ) { // Follow mode.
				Follow_Mode () ;
				break ;
			} else {
				WayPoint_Mode () ; // WayPoint mode.
				break ;
			}
		case 1 : // Chase mode.
			if ( Target_Transform ) {
				Chase_Mode () ;
			} else {
				Lost_Target () ;
			}
			break ;
		}
		// "AI_Hand" touches an obstacle.
		if ( Hand_Script.Work_Flag ) {
			Speed_Order = 0.0f ; // The tank can only turn.
		}
	}

	void Search () {
		Vector3 Target_Pos = Target_Transform.position + ( Target_Transform.up * AI_Upper_Offset ) ;
		Target_Distance = Vector3.Distance ( Eye_Transform.position , Target_Pos ) ;
		if ( Target_Root_Transform.tag != "Finish" ) {
			if ( Target_Distance < Visibility_Radius ) {
				// Cast Ray from "AI_Eye" to the target.
				Ray Temp_Ray = new Ray ( Eye_Transform.position , Target_Pos - Eye_Transform.position ) ;
				RaycastHit Temp_RaycastHit ;
				if ( Physics.Raycast ( Temp_Ray , out Temp_RaycastHit , Visibility_Radius , Ray_LayerMask ) ) { // Ray hits anything.
					if ( Temp_RaycastHit.transform.root == Target_Root_Transform ) { // Ray hits the target.
						Detect_Flag = true ; // Referred to from "Cannon_Vertical".
						switch ( Action_Type ) {
						case 0 : // Patrol mode.
							Action_Type = 1 ;
							SetDestination_Count = 0.0f ;
							Toward_Flag = false ;
							// Send Target_Transform to "Turret_Horizontal".
							Parent_Transform.BroadcastMessage ( "Set_Lock_On" , Target_Transform , SendMessageOptions.DontRequireReceiver ) ;
							// Update Text.
							Text_Update ( "Attack" , Color.red ) ;
							break ;
						case 1 : // Chase mode.
							Lost_Time = 0.0f ;
							// Update Text.
							Text_Update ( "Attack" , Color.red ) ;
							break ;
						}
						return ; // Ignore the following code.
					}
				}
			}
		} else {
			Lost_Target () ; // Target is already dead.
			Game_Controller_Script.Assign_Count = 0.0f ;
			return ;
		}
		// Target is out of range, or Ray does not hit anything, or Ray hits other object.
		Detect_Flag = false ; // Referred to from "Cannon_Vertical".
		if ( Action_Type == 1 ) { // Chase mode.
			Lost_Time += Time.deltaTime + 1.0f ;
			// Update Text.
			Text_Update ( "Lost : " + Mathf.CeilToInt ( Lost_Count - Lost_Time ) , Color.magenta ) ;
			// AI has missed the target.
			if ( Lost_Time > Lost_Count ) {
				Lost_Target () ;
			}
		}
	}
	
	void Lost_Target () {
		Action_Type = 0 ;
		SetDestination_Count = 0.0f ;
		Target_Transform = null ;
		Target_Root_Transform = null ;
		Detect_Flag = false ; // Referred to from "Cannon_Vertical".
		Lost_Time = 0.0f ;
		Near_Flag = false ;
		Toward_Flag = false ;
		// Send Message to "Turret_Horizontal".
		if ( Parent_Transform ) {
			Parent_Transform.BroadcastMessage ( "Reset_Lock_On" , SendMessageOptions.DontRequireReceiver ) ;
		}
		// Update Text.
		Text_Update ( "Search" , Default_Text_Color ) ;
		//
		if ( Follow_Target == null ) { // WayPoint mode.
			This_Agent.SetDestination ( WayPoints [ Next_WayPoint_Num ] ) ;
		}
	}

	void Follow_Mode () {
		if ( Follow_MainBody ) {
			// Update the destination.
			if ( SetDestination_Count <= 0.0f ) {
				This_Agent.SetDestination ( Follow_MainBody.position ) ;
				SetDestination_Count = 2.0f ;
			} else {
				SetDestination_Count -= Time.deltaTime ;
			}
			//
			Auto_Drive () ;
			// Stop near the Follow_Target, and turn toward the same direction.
			float Temp_Distance = Vector3.Distance ( This_Transform.position , Follow_MainBody.position ) ;
			float Follow_Distance = 15.0f ;
			if ( Toward_Flag ) {
				if ( Temp_Distance < Follow_Distance + 5.0f ) { // Keep staying.
					Toward_Pos = Follow_MainBody.position + ( Follow_MainBody.forward * 250.0f ) ;
				} else {
					Toward_Flag = false ;
				}
			} else {
				if ( Temp_Distance < Follow_Distance ) { // Stay here.
					Toward_Pos = Follow_MainBody.position + ( Follow_MainBody.forward * 250.0f ) ;
					Toward_Flag = true ;
				} else if ( Temp_Distance < Follow_Distance + 5.0f ) { // Slow down.
					float Temp_Speed_Order = Mathf.Lerp ( 0.0f , Max_Speed_Rate , ( Temp_Distance - ( Follow_Distance - 1.0f ) ) / 10.0f ) ;
					if ( Speed_Order > Temp_Speed_Order ) {
						Speed_Order = Temp_Speed_Order ;
					}
				}
			}
		} else { // Find the "MainBody" of Follow_Target.
			Follow_Target_Settings () ;
		}
	}

	void WayPoint_Mode () {
		// Check the Path Status.
		if ( This_Agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid ) {
			This_Agent.ResetPath () ;
			This_Agent.SetDestination ( WayPoints [ Next_WayPoint_Num ] ) ;
		}
		if ( WayPoints.Length > 1 ) { // Normal.
			if ( Vector3.Distance ( This_Transform.position , WayPoints [ Next_WayPoint_Num ] ) < WayPoint_Radius ) { // Arrived at the WayPoint.
				Update_Next_WayPoint () ;
			}
		} else { // Only one waypoint.
			if ( Vector3.Distance ( This_Transform.position , WayPoints [ 0 ] ) < WayPoint_Radius ) { // Arrived at the WayPoint (initial position).
				if ( !Toward_Flag ) {
					Toward_Flag = true ;
				}
			} else {  // Not arrived.
				if ( Toward_Flag ) {
					Toward_Flag = false ;
				}
			}
		}
		Auto_Drive () ;
	}

	void Chase_Mode () {
		// Update the destination.
		if ( SetDestination_Count <= 0.0f ) {
			This_Agent.SetDestination ( Target_Transform.position ) ;
			SetDestination_Count = 5.0f ;
		} else {
			SetDestination_Count -= Time.deltaTime ;
		}
		// Set 'Near_Flag'.
		if ( Target_Distance < Approach_Distance ) { // Near Approach_Distance.
			if ( Fire_Flag || Approach_Distance == Mathf.Infinity ) { // Cannon can aim the target, or Approach_Distance is set to infinity.
				Near_Flag = true ;
			} else {
				Near_Flag = false ;
			}
		} else { // Far from Approach_Distance.
			Near_Flag = false ;
		}
		Auto_Drive () ;
	}

	void Update_Next_WayPoint () {
		switch ( Patrol_Type ) {
		case 0 :
			Next_WayPoint_Num += Step ;
			if ( Next_WayPoint_Num >= WayPoints.Length ) {
				Next_WayPoint_Num = 0 ;
			} else if ( Next_WayPoint_Num < 0 ) {
				Next_WayPoint_Num = WayPoints.Length - 1 ;
			}
			break ;
		case 1 :
			Next_WayPoint_Num = Random.Range ( 0 , WayPoints.Length ) ;
			break ;
		}
		This_Agent.SetDestination ( WayPoints [ Next_WayPoint_Num ] ) ;
	}
	
	void Auto_Drive () {
		// Get the next corner position.
		Vector3 Next_Corner_Pos ;
		if ( Near_Flag && Face_Enemy ) { // 'Near_Flag' is true only when the cannon can aim the target.
			Next_Corner_Pos = Target_Transform.position ;
		} else if ( Toward_Flag ) { // Follow mode, or have only one waypoint.
			Next_Corner_Pos = Toward_Pos ;
		} else if ( This_Agent.path.corners.Length > 1 ) {
			Next_Corner_Pos = This_Agent.path.corners [ 1 ] ;
		} else {
			Next_Corner_Pos = This_Agent.path.corners [ 0 ] ;
		}
		// Calculate the direction to the next corner.
		float Temp_Angle ;
		Vector3 Local_Pos = This_Transform.InverseTransformPoint ( Next_Corner_Pos ) ;
		Temp_Angle = Vector2.Angle ( Vector2.up , new Vector2 ( Local_Pos.x , Local_Pos.z ) ) ;
		// Left or Right.
		if ( Local_Pos.x < 0.0f ) {
			Temp_Angle = -Temp_Angle ;
		}
		// Face the target.
		if ( Near_Flag && Face_Enemy ) {
			if ( Temp_Angle >= 0.0f ) { // Left
				Temp_Angle -= Face_Offest_Angle ;
			} else { // Right
				Temp_Angle += Face_Offest_Angle ;
			}
		}
		// Calculate Turn rate and Speed rate.
		if ( Temp_Angle < -Min_Turn_Angle ) { // Left
			Turn_Order = Mathf.Lerp ( -Min_Turn_Rate , -Max_Turn_Rate , Temp_Angle / -Pivot_Turn_Angle ) ;
			if ( Near_Flag || Toward_Flag ) { // Stay here.
				Speed_Order = 0.0f ;
				return ;
			}
			Speed_Order = Mathf.Clamp ( Max_Speed_Rate + Turn_Order , 0.0f , Max_Speed_Rate ) ;
			if ( Temp_Angle < -Slow_Turn_Angle ) { // Slow Turn
				Turn_Order *= Slow_Turn_Rate ;
				Speed_Order *= Min_Speed_Rate ;
				Slow_Turn_Flag = true ;
			} else {
				Slow_Turn_Flag = false ;
			}
			float Temp_Distance = Vector3.Distance ( This_Transform.position , Next_Corner_Pos ) ; // Distance to the next corner.
			if ( Temp_Distance < 10.0f ) { // Near to the next corner.
				Speed_Order *= Mathf.Lerp ( Min_Speed_Rate , 1.0f , Temp_Distance / 10.0f ) ;
			}
		} else if ( Temp_Angle > Min_Turn_Angle ) { // Right
			Turn_Order = Mathf.Lerp ( Min_Turn_Rate , Max_Turn_Rate , Temp_Angle / Pivot_Turn_Angle ) ;
			if ( Near_Flag || Toward_Flag ) { // Stay here.
				Speed_Order = 0.0f ;
				return ;
			}
			Speed_Order = Mathf.Clamp ( Max_Speed_Rate - Turn_Order , 0.0f , Max_Speed_Rate ) ;
			if ( Temp_Angle > Slow_Turn_Angle ) { // Slow Turn
				Turn_Order *= Slow_Turn_Rate ;
				Speed_Order *= Min_Speed_Rate ;
				Slow_Turn_Flag = true ;
			} else {
				Slow_Turn_Flag = false ;
			}
			float Temp_Distance = Vector3.Distance ( This_Transform.position , Next_Corner_Pos ) ; // Distance to the next corner.
			if ( Temp_Distance < 10.0f ) { // Near to the next corner.
				Speed_Order *= Mathf.Lerp ( Min_Speed_Rate , 1.0f , Temp_Distance / 10.0f ) ;
			}
		} else { // No Turn
			Turn_Order = 0.0f ;
			Slow_Turn_Flag = false ;
			if ( Near_Flag || Toward_Flag ) { // Stay here.
				Speed_Order = 0.0f ;
				return ;
			} else { // Far from the target.
				float Temp_Distance = Vector3.Distance ( This_Transform.position , Next_Corner_Pos ) ; // Distance to the next corner.
				Speed_Order = Mathf.Lerp ( Min_Speed_Rate , Max_Speed_Rate , Temp_Distance / 50.0f ) ;
			}
		}
	}

	bool Obstacle_Flag = false ;
	public void Escape_Stuck () { // Called from "AI_Hand" when stuck.
		if ( Follow_Target == null ) {
			if ( Random.Range ( 0 , 3 ) == 0 ) {
				if ( Obstacle_Flag == false ) {
					GameObject Obstacle_Object = new GameObject ( "Obstacle_Object" ) ;
					Obstacle_Object.transform.position = Hand_Script.transform.position ;
					Obstacle_Object.transform.rotation = This_Transform.rotation * Quaternion.Euler ( 0.0f , 45.0f , 0.0f ) ;
					UnityEngine.AI.NavMeshObstacle Temp_NavMeshObstacle = Obstacle_Object.AddComponent < UnityEngine.AI.NavMeshObstacle > () ;
					Temp_NavMeshObstacle.carving = true ;
					Obstacle_Object.AddComponent < Delete_Timer_CS > ().Count = 20.0f ;
					Obstacle_Flag = true ;
					StartCoroutine ( "Obstacle_Flag_Timer" ) ;
				}
			} else if ( Action_Type == 0 && Patrol_Type == 1 ) {
				Update_Next_WayPoint () ;
			}
		}
	}

	IEnumerator Obstacle_Flag_Timer () {
		yield return new WaitForSeconds ( 20.0f ) ;
		Obstacle_Flag = false ;
	}

	void Text_Update ( string Temp_String , Color Temp_Color ) {
		if ( AI_State_Text ) {
			AI_State_Text.text = Tank_Name + " = " + Temp_String ;
			Temp_Color.a = Default_Text_Color.a ;
			AI_State_Text.color = Temp_Color ;
		}
	}

	public bool RayCast_Check ( Transform Temp_Transform , MainBody_Setting_CS Temp_Script ) { // Called from Game_Controller.
		This_Transform.localPosition = Initial_Position ; // Fix this position and rotation.
		Vector3 Temp_Pos = Temp_Transform.position + ( Temp_Transform.up * Temp_Script.AI_Upper_Offset ) ;
		Ray Temp_Ray = new Ray ( Eye_Transform.position , Temp_Pos - Eye_Transform.position ) ;
		RaycastHit Temp_RaycastHit ;
		if ( Physics.Raycast ( Temp_Ray , out Temp_RaycastHit , Visibility_Radius , Ray_LayerMask ) ) { // Ray hits anything.
			if ( Temp_RaycastHit.transform.root == Temp_Transform.root ) { // Ray hits the target.
				return true ;
			} else { // Ray does not hit the target.
				return false ;
			}
		} else { // Ray does not hit anything.
			return false ;
		}
	}

	void MainBody_Linkage () {
		Text_Update ( "Dead." , Color.black ) ; // Update Text.
		Destroy ( this.gameObject ) ; // Bye.
	}

	public void Change_AI_Settings ( Event_Controller_CS Temp_Event_Script ) { // Called from "Event_Controller".
		if ( Temp_Event_Script ) {
			// Change variables.
			WayPoint_Pack = Temp_Event_Script.New_WayPoint_Pack ;
			Patrol_Type = Temp_Event_Script.New_Patrol_Type ;
			Follow_Target = Temp_Event_Script.New_Follow_Target ;
			No_Attack = Temp_Event_Script.New_No_Attack ;
			// WayPoints settings.
			WayPoints_Settings () ;
			// Follow_Target settings.
			if ( Follow_Target ) {
				Follow_Target_Settings () ;
			} else {
				Next_WayPoint_Num = -1 ;
				Step = 1 ;
				Update_Next_WayPoint () ; // Set the first waypoint.
			}
			//
			Action_Type = 0 ;
			Lost_Target () ;
		}
	}

}
