using UnityEngine;
using System.Collections;

public class Static_Track_Setting_CS : MonoBehaviour {

	public Transform Front_Transform ;
	public Transform Rear_Transform ;

	public int Type ;
	public string Anchor_Name ;
	public string Anchor_Parent_Name ;

	Transform Parent_Transform ;

	void Start () {
		Parent_Transform = transform.parent ;
		string Base_Name = this.name.Substring ( 0 , 12 ) ; // e.g. "TrackBelt_L_"
		int This_Num = int.Parse ( this.name.Substring ( 12 ) ) ; // e.g. "1"
		// Find front piece.
		Front_Transform = Parent_Transform.Find ( Base_Name + ( This_Num + 1 ) ) ; // Find a piece having next number.
		if ( !Front_Transform ) { // It must be the last piece.
			Front_Transform = Parent_Transform.Find ( Base_Name + 1 ) ; // The 1st piece.
		}
		// Find rear piece.
		Rear_Transform = Parent_Transform.Find ( Base_Name + ( This_Num - 1 ) ) ; // Find a piece having previous number.
		if ( !Rear_Transform ) { // It must be the 1st piece.
			Rear_Transform = transform.parent.Find ( Base_Name + ( transform.parent.childCount / 2 ) ) ; // The last piece.
		}
	}

	void OnCollisionStay ( Collision Temp_Collision ) {
		foreach ( ContactPoint Temp_ContactPoint in Temp_Collision.contacts ) {
			if ( Temp_ContactPoint.otherCollider.transform.parent ) {
				if ( Temp_ContactPoint.otherCollider.name.Length >= 9 && Temp_ContactPoint.otherCollider.name.Substring ( 0 , 9 ) == "RoadWheel" ) {
					Anchor_Name = Temp_ContactPoint.otherCollider.name ;
					Anchor_Parent_Name = Temp_ContactPoint.otherCollider.transform.parent.name ;
					Type = 1 ;
				}
			}
		}
	}
	
	void OnCollisionExit () {
		Anchor_Name = null ;
		Anchor_Parent_Name = null ;
		Type = 0 ;
	}

	void Set_Static_Track_Value () {
		// Add Script.
		Static_Track_CS Temp_Script ;
		Temp_Script = gameObject.AddComponent < Static_Track_CS > () ;
		Temp_Script.Front_Transform = Front_Transform ;
		Temp_Script.Rear_Transform = Rear_Transform ;
		if ( Type ==1 ) {
			Temp_Script.Type = 1 ;
			Temp_Script.Anchor_Name = Anchor_Name ;
			Temp_Script.Anchor_Parent_Name = Anchor_Parent_Name ;
		} else if ( Front_Transform.GetComponent < Static_Track_Setting_CS > ().Type == 1 ) {
			Temp_Script.Type = 2 ;
		} else if ( Rear_Transform.GetComponent < Static_Track_Setting_CS > ().Type == 1 ) {
			Temp_Script.Type = 2 ;
		} else {
			Temp_Script.Type = 0 ;
		}
		Temp_Script.enabled = false ;
		// Remove_Components.
		if ( gameObject.GetComponent<HingeJoint>() ) {
			Destroy ( gameObject.GetComponent<HingeJoint>() ) ;
		}
		if ( GetComponent<Rigidbody>() ) {
			Destroy ( GetComponent<Rigidbody>() ) ;
		}
		Damage_Control_CS Temp_Damage_Control_Script = GetComponent < Damage_Control_CS > () ;
		if ( Temp_Damage_Control_Script ) {
			Destroy ( Temp_Damage_Control_Script ) ;
		}
		Stabilizer_CS Temp_Stabilizer_Script = GetComponent < Stabilizer_CS > () ;
		if ( Temp_Stabilizer_Script ) {
			Destroy ( Temp_Stabilizer_Script ) ;
		}
		// Disable BoxCollider.
		BoxCollider Temp_BoxCollider = GetComponent < BoxCollider > () ;
		if ( Temp_BoxCollider ) {
			Temp_BoxCollider.enabled = false ;
		}
		// Remove_Child.
		for ( int i = 0 ;  i < transform.childCount ; i++ ) {
			Destroy ( transform.GetChild ( 0 ).gameObject ) ;
		}
		// Destroy this script/
		Destroy ( this ) ;
	}

}
