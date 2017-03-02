using UnityEngine;
using System.Collections;

public class Static_Track_CS : MonoBehaviour {

	public int Type ; // 0=Static, 1=Anchor, 2=Dynamic, 9=Parent.
	public Transform Front_Transform ;
	public Transform Rear_Transform ;
	public string Anchor_Name ;
	public string Anchor_Parent_Name ;
	public Transform Anchor_Transform ;
	public Transform Reference_L ;
	public Transform Reference_R ;
	public string Reference_Name_L ;
	public string Reference_Name_R ;
	public string Reference_Parent_Name_L ;
	public string Reference_Parent_Name_R ;
	public float Length ;
	public float Radius_Offset ;
	public float Mass = 30.0f ;
	// Referred to from all pieces.
	public float Rate_L ;
	public float Rate_R ;
	// Referred to from all Static_Wheels.
	public float Reference_Radius_L ;
	public float Reference_Radius_R ;
	public float Diff_Ang_L ;
	public float Diff_Ang_R ;

	Transform This_Transform ;
	bool Direction ; // Left = true.
	float Invert_Value ; // Lower piece = 0.0f, Upper pieces = 180.0f.
	Vector3 Invisible_Pos ;
	float Invisible_Ang ;
	Static_Track_CS Front_Script ;
	Static_Track_CS Rear_Script ;
	Static_Track_CS Parent_Script ;
	MainBody_Setting_CS MainBody_Script ;
	int Number_of_Pieces ;
	float Half_Length ;
	// only for Anchor.
	float Default_This_X ;
	float Default_Anchor_X ;
	// only for Parent.
	float Last_Ang_L ;
	float Last_Ang_R ;
	float Rate_Ang_L ;
	float Rate_Ang_R ;
	bool Parent_Flag ;
	
	void Start () {
		This_Transform = transform ;
		MainBody_Script = GetComponentInParent < MainBody_Setting_CS > () ;
		switch ( Type ) {
		case 0 : // Static.
			Initial_Settings () ;
			Parent_Flag = false ;
			break ;
		case 1 : // Anchor.
			Initial_Settings () ;
			Find_Anchor () ;
			Parent_Flag = false ;
			break ;
		case 2 : // Dynamic.
			Initial_Settings () ;
			Parent_Flag = false ;
			break ;
		case 9 : // Parent.
			Parent_Settings () ;
			Parent_Flag = true ;
			break ;
		}
	}

	void Initial_Settings () {
		// Set direction.
		if ( This_Transform.localPosition.y > 0.0f ) {
			Direction = true ; // Left
		} else {
			Direction = false ; // Right
		}
		// Set Invert_Value.
		if ( This_Transform.localEulerAngles.y > 90.0f && This_Transform.localEulerAngles.y < 270.0f ) {
			Invert_Value = 180.0f ; // Upper.
		} else {
			Invert_Value = 0.0f ; // Lower.
		}
		// Set initial position and angle.
		Invisible_Pos = This_Transform.localPosition ;
		Invisible_Ang = This_Transform.localRotation.eulerAngles.y ;
		// Find Front, Rear, Parent scripts.
		if ( Front_Transform ) {
			Front_Script = Front_Transform.GetComponent < Static_Track_CS > () ;
		}
		if ( Rear_Transform ) {
			Rear_Script = Rear_Transform.GetComponent < Static_Track_CS > () ;
		}
		Parent_Script = This_Transform.parent.GetComponent < Static_Track_CS > () ;
		// Get the half length of the piece.
		Half_Length = Parent_Script.Length * 0.5f ;
		// Set Number_of_Pieces.
		for ( int i = 0 ; i < This_Transform.parent.childCount ; i++ ) {
			if ( This_Transform.parent.GetChild ( i ).GetComponent < Static_Track_CS > () ) {
				Number_of_Pieces += 1 ;
			}
		}
		Number_of_Pieces /= 2 ;
	}

	void Find_Anchor () {
		if ( Anchor_Transform == null ) { // Anchor_Transform is not assigned.
			// Find Anchor with reference to the name.
			if ( !string.IsNullOrEmpty ( Anchor_Name ) && !string.IsNullOrEmpty ( Anchor_Parent_Name ) && This_Transform.parent.parent ) {
				Anchor_Transform = This_Transform.parent.parent.Find ( Anchor_Parent_Name + "/" + Anchor_Name ) ;
			}
		}
		// Set initial hight.
		if ( Anchor_Transform ) {
			Default_This_X = This_Transform.localPosition.x ; // Axis X = hight.
			Default_Anchor_X = Anchor_Transform.localPosition.x ;
		}
	}

	void Parent_Settings () {
		// Find Reference Wheels.
		if ( Reference_L == null ) { // Left Reference Wheel is not assigned.
			if ( !string.IsNullOrEmpty ( Reference_Name_L ) && !string.IsNullOrEmpty ( Reference_Parent_Name_L ) ) { // Its names are assigned.
				Reference_L = This_Transform.parent.Find ( Reference_Parent_Name_L + "/" + Reference_Name_L ) ;
			}
		}
		if ( Reference_R == null ) { // Right Reference Wheel is not assigned.
			if ( !string.IsNullOrEmpty ( Reference_Name_R ) && !string.IsNullOrEmpty ( Reference_Parent_Name_R ) ) { // Its names are assigned.
				Reference_R = This_Transform.parent.Find ( Reference_Parent_Name_R + "/" + Reference_Name_R ) ;
			}
		}
		// Check
		if ( Reference_L == null ) {
			Debug.LogWarning ( "'Left Reference Wheel' for Static_Track is not found. (Physics Tank Maker)" ) ;
		}
		if ( Reference_R == null ) {
			Debug.LogWarning ( "'Right Reference Wheel' for Static_Track is not found. (Physics Tank Maker)" ) ;
		}
		// Set "Reference_Radius" and "Rate_Ang".
		if ( Reference_L ) {
			if ( Reference_L.GetComponent < MeshFilter > () ) {
				Reference_Radius_L = Reference_L.GetComponent < MeshFilter > ().mesh.bounds.extents.x + Radius_Offset ;
				if ( Reference_Radius_L > 0.0f ) {
					Rate_Ang_L = 360.0f / ( ( 2.0f * 3.14f * Reference_Radius_L ) / Length ) ;
				}
			}
		}
		if ( Reference_R ) {
			if ( Reference_R.GetComponent < MeshFilter > () ) {
				Reference_Radius_R = Reference_R.GetComponent < MeshFilter > ().mesh.bounds.extents.x + Radius_Offset ;
				if ( Reference_Radius_R > 0.0f ) {
					Rate_Ang_R= 360.0f / ( ( 2.0f * 3.14f * Reference_Radius_R ) / Length ) ;
				}
			}
		}
		// Send message to all the "Static_Wheels".
		This_Transform.parent.BroadcastMessage ( "Get_Static_Track" , this , SendMessageOptions.DontRequireReceiver ) ;
	}

	void Update () {
		if ( Parent_Flag ) { // Parent
			if ( MainBody_Script.Visible_Flag ) { // MainBody is visible by any camera.
				Speed_Control () ;
			}
		}
	}

	void LateUpdate () {
		if ( MainBody_Script.Visible_Flag ) { // MainBody is visible by any camera.
			switch ( Type ) {
			case 0 : // Static.
				Slide_Control () ;
				break ;
			case 1 : // Anchor.
				Anchor_Control () ;
				Slide_Control () ;
				break ;
			case 2 : // Dynamic.
				Dynamic_Control () ;
				Slide_Control () ;
				break ;
			}
		}
	}

	void Anchor_Control () {
		// Set position.
		if ( Anchor_Transform ) {
			float Offset = Anchor_Transform.localPosition.x - Default_Anchor_X ;
			Invisible_Pos = new Vector3 ( Default_This_X + Offset , Invisible_Pos.y , Invisible_Pos.z ) ;
		}
		// Set angle.
		float Ang_Y = Mathf.Rad2Deg * Mathf.Atan ( ( Front_Script.Invisible_Pos.x - Rear_Script.Invisible_Pos.x ) / ( Front_Script.Invisible_Pos.z - Rear_Script.Invisible_Pos.z) ) ;
		Invisible_Ang = Ang_Y + Invert_Value ;
	}

	void Dynamic_Control () {
		// Set position.
		float Temp_Rad = Rear_Script.Invisible_Ang * Mathf.Deg2Rad ;
		float Rear_Offset_Z = Half_Length * Mathf.Cos ( Temp_Rad ) ;
		float Rear_Offset_X = Half_Length * Mathf.Sin ( Temp_Rad ) ;
		Vector3 Rear_Piece_Front = new Vector3 ( Rear_Script.Invisible_Pos.x + Rear_Offset_X , Rear_Script.Invisible_Pos.y , Rear_Script.Invisible_Pos.z + Rear_Offset_Z ) ;
		Temp_Rad = Front_Script.Invisible_Ang * Mathf.Deg2Rad ;
		float Front_Offset_Z = Half_Length * Mathf.Cos ( Temp_Rad ) ;
		float Front_Offset_X = Half_Length * Mathf.Sin ( Temp_Rad ) ;
		Vector3 Front_Piece_End = new Vector3 ( Front_Script.Invisible_Pos.x - Front_Offset_X , Front_Script.Invisible_Pos.y , Front_Script.Invisible_Pos.z - Front_Offset_Z ) ;
		Invisible_Pos = Vector3.Lerp ( Rear_Piece_Front , Front_Piece_End , 0.5f ) ;
		// Set angle.
		float Ang_Y = Mathf.Rad2Deg * Mathf.Atan ( ( Front_Script.Invisible_Pos.x - Rear_Script.Invisible_Pos.x ) / ( Front_Script.Invisible_Pos.z - Rear_Script.Invisible_Pos.z) ) ;
		Invisible_Ang = Ang_Y + Invert_Value ;
	}

	void Slide_Control () {
		if ( Direction ) { // Left
			This_Transform.localPosition = Vector3.Lerp ( Invisible_Pos , Rear_Script.Invisible_Pos , Parent_Script.Rate_L ) ;
			This_Transform.localRotation = Quaternion.Euler ( new Vector3 ( 0.0f , Mathf.LerpAngle ( Invisible_Ang , Rear_Script.Invisible_Ang , Parent_Script.Rate_L ) , 270.0f ) ) ;
		} else { // Right
			This_Transform.localPosition = Vector3.Lerp ( Invisible_Pos , Rear_Script.Invisible_Pos , Parent_Script.Rate_R ) ;
			This_Transform.localRotation = Quaternion.Euler ( new Vector3 ( 0.0f , Mathf.LerpAngle ( Invisible_Ang , Rear_Script.Invisible_Ang , Parent_Script.Rate_R ) , 270.0f ) ) ;
		}
	}

	void Speed_Control () {
		// Left
		if ( Reference_L ) {
			float Current_Ang_L = Reference_L.localEulerAngles.y ;
			Diff_Ang_L = Mathf.DeltaAngle ( Current_Ang_L , Last_Ang_L ) ; // This value is referred to also from Static_Wheels.
			Rate_L = Calculate_Rate ( Rate_L , Diff_Ang_L , Rate_Ang_L ) ;
			Last_Ang_L = Current_Ang_L ;
		}
		// Right
		if ( Reference_R ) {
			float Current_Ang_R = Reference_R.localEulerAngles.y ;
			Diff_Ang_R = Mathf.DeltaAngle ( Current_Ang_R , Last_Ang_R ) ; // This value is referred to also from Static_Wheels.
			Rate_R = Calculate_Rate ( Rate_R , Diff_Ang_R , Rate_Ang_R ) ;
			Last_Ang_R = Current_Ang_R ;
		}
	}

	float Calculate_Rate ( float Temp_Rate , float Temp_Diff_Ang , float Temp_Rate_Ang ) {
		float Pitch ;
		Pitch = ( Temp_Diff_Ang / Temp_Rate_Ang ) % 1.0f ;
		Temp_Rate += Pitch ;
		if ( Temp_Rate > 1.0f ) {
			Temp_Rate -= 1.0f ;
		} else if ( Temp_Rate < 0.0f ) {
			Temp_Rate += 1.0f ;
		}
		return Temp_Rate ;
	}

	void StaticTrack_Linkage () { // Called from Damage_Control.
		if ( this.enabled ) {
			// Reset parent script.
			if ( Parent_Script ) {
				Parent_Script.Rate_L = 0.0f ;
				Parent_Script.Rate_R = 0.0f ;
			}
			// Add Components into this piece.
			Add_Components ( This_Transform ) ;
			// Add Components into other pieces.
			Static_Track_CS Temp_Script = this ;
			for ( int i = 0 ; i < Number_of_Pieces ; i++ ) {
				Transform Temp_Transform = Temp_Script.Front_Transform ;
				Add_Components ( Temp_Transform ) ;
				Temp_Script = Temp_Script.Front_Script ;
			}
			// Add HingeJoint into front pieces.
			Temp_Script = this ;
			for ( int i = 0 ; i < Number_of_Pieces - 1 ; i++ ) { // Add HingeJoint except for this piece.
				Transform Temp_Transform = Temp_Script.Front_Transform ;
				Transform Connected_Transform = Temp_Script.Front_Script.Front_Transform ;
				Add_HingeJoint ( Temp_Transform , Connected_Transform ) ;
				Temp_Script = Temp_Script.Front_Script ;
			}
			// Disable and Destroy the pieces on the same side.
			This_Transform.parent.BroadcastMessage ( "Disable_and_Destroy" , Direction , SendMessageOptions.DontRequireReceiver ) ;
		}
	}

	void Add_Components ( Transform Temp_Transform ) {
		// Add RigidBody.
		if ( !Temp_Transform.gameObject.GetComponent<Rigidbody>() ) {
			Temp_Transform.gameObject.AddComponent < Rigidbody > () ;
			Temp_Transform.GetComponent<Rigidbody>().mass = Mass ;
		}
		BoxCollider Temp_BoxCollider = Temp_Transform.GetComponent < BoxCollider > () ;
		if ( Temp_BoxCollider ) {
			Temp_BoxCollider.enabled = true ;
		}
	}

	void Add_HingeJoint ( Transform Temp_Transform , Transform Connected_Transform ) {
		HingeJoint Temp_HingeJoint =  Temp_Transform.gameObject.AddComponent < HingeJoint > () ;
		if ( Connected_Transform.GetComponent<Rigidbody>() ) {
			Temp_HingeJoint.connectedBody = Connected_Transform.GetComponent<Rigidbody>() ;
		}
		float Anchor_Z = GetComponent < BoxCollider > ().size.z / 2.0f ;
		Temp_HingeJoint.anchor = new Vector3 ( 0.0f , 0.0f , Anchor_Z ) ;
		Temp_HingeJoint.axis = new Vector3 ( 1.0f , 0.0f , 0.0f ) ;
	}

	IEnumerator Disable_and_Destroy ( bool Temp_Direction ) {
		if ( Type != 9 && Temp_Direction == Direction ) {
			this.enabled = false ;
			yield return new WaitForSeconds ( 1.0f ) ;
			This_Transform.parent = null ;
			Destroy ( this.gameObject , 20.0f ) ;
		}
	}
	
}
