using UnityEngine;
using System ;
using System.Collections;
using UnityEditor ;

[ CustomEditor ( typeof ( Create_TrackBelt_CS ) ) ]

public class Create_TrackBelt_CSEditor : Editor {

	SerializedProperty Rear_FlagProp ;
	SerializedProperty SelectedAngleProp ;
	SerializedProperty Angle_RearProp ;
	SerializedProperty NumberProp ;
	SerializedProperty SpacingProp ;
	SerializedProperty DistanceProp ;
	SerializedProperty Track_MassProp ;
	SerializedProperty Collider_InfoProp ;
	SerializedProperty Collider_MaterialProp ;
	SerializedProperty Track_R_MeshProp ;
	SerializedProperty Track_L_MeshProp ;
	SerializedProperty Track_R_MaterialProp ;
	SerializedProperty Track_L_MaterialProp ;

	SerializedProperty SubJoint_TypeProp ;
	SerializedProperty Reinforce_RadiusProp ;

	SerializedProperty Use_InterpolationProp ;
	SerializedProperty Joint_OffsetProp ;
	SerializedProperty Interpolation_R_MeshProp ;
	SerializedProperty Interpolation_L_MeshProp ;
	SerializedProperty Interpolation_R_MaterialProp ;
	SerializedProperty Interpolation_L_MaterialProp ;

	SerializedProperty Special_OffsetProp ;

	SerializedProperty Track_DurabilityProp ;
	SerializedProperty BreakForceProp ;

	SerializedProperty RealTime_FlagProp ;
	SerializedProperty Static_FlagProp ;
	SerializedProperty Prefab_FlagProp ;
	SerializedProperty Parent_TransformProp ;
	
	string[] AngleNames = { "10" , "11.25" , "12" , "15" , "18" , "20" , "22.5" , "25.71" , "30" , "36" , "45" , "60" , "90" } ;
	int[] AngleValues = { 1000 , 1125 , 1200 , 1500 , 1800 , 2000 , 2250 , 2571 , 3000 , 3600 , 4500 , 6000 , 9000 } ;
	string[] SubJointNames = { "All" , "Every Two pieces" , "None" } ;

	Transform Parent_Transform ;
	// for Normal Track.
	float Pos_Y ;
	float Pos_X ;
	float Pos_Z ;
	float Ang_Z ; 
	int Count = 1 ;

	void OnEnable () {
		Rear_FlagProp = serializedObject.FindProperty ( "Rear_Flag" ) ;
		SelectedAngleProp = serializedObject.FindProperty ( "SelectedAngle" ) ;
		Angle_RearProp = serializedObject.FindProperty ( "Angle_Rear" ) ;
		NumberProp = serializedObject.FindProperty ( "Number_Straight" ) ;
		SpacingProp = serializedObject.FindProperty ( "Spacing" ) ;
		DistanceProp = serializedObject.FindProperty ( "Distance" ) ;
		Track_MassProp = serializedObject.FindProperty ( "Track_Mass" ) ;
		Collider_InfoProp = serializedObject.FindProperty ( "Collider_Info" ) ;
		Collider_MaterialProp = serializedObject.FindProperty ( "Collider_Material" ) ;
		Track_R_MeshProp = serializedObject.FindProperty ( "Track_R_Mesh" ) ;
		Track_L_MeshProp = serializedObject.FindProperty ( "Track_L_Mesh" ) ;
		Track_R_MaterialProp = serializedObject.FindProperty ( "Track_R_Material" ) ;
		Track_L_MaterialProp = serializedObject.FindProperty ( "Track_L_Material" ) ;

		SubJoint_TypeProp = serializedObject.FindProperty ( "SubJoint_Type" ) ;
		Reinforce_RadiusProp = serializedObject.FindProperty ( "Reinforce_Radius" ) ;

		Use_InterpolationProp = serializedObject.FindProperty ( "Use_Interpolation" ) ;
		Joint_OffsetProp = serializedObject.FindProperty ( "Joint_Offset" ) ;
		Interpolation_R_MeshProp = serializedObject.FindProperty ( "Interpolation_R_Mesh" ) ;
		Interpolation_L_MeshProp = serializedObject.FindProperty ( "Interpolation_L_Mesh" ) ;
		Interpolation_R_MaterialProp = serializedObject.FindProperty ( "Interpolation_R_Material" ) ;
		Interpolation_L_MaterialProp = serializedObject.FindProperty ( "Interpolation_L_Material" ) ;

		Special_OffsetProp = serializedObject.FindProperty ( "Special_Offset" ) ;

		Track_DurabilityProp = serializedObject.FindProperty ( "Track_Durability" ) ;
		BreakForceProp = serializedObject.FindProperty ( "BreakForce" ) ;

		RealTime_FlagProp = serializedObject.FindProperty ( "RealTime_Flag" ) ;
		Static_FlagProp = serializedObject.FindProperty ( "Static_Flag" ) ;
		Prefab_FlagProp = serializedObject.FindProperty ( "Prefab_Flag" ) ;
		Parent_TransformProp = serializedObject.FindProperty ( "Parent_Transform" ) ;
		
		Parent_Transform = Parent_TransformProp.objectReferenceValue as Transform ;
	}
	
	public override void OnInspectorGUI () {
		bool Work_Flag ;
		if ( Parent_Transform.parent == null || Parent_Transform.parent.gameObject.GetComponent<Rigidbody>() == null ) {
			Work_Flag = false ;
		} else {
			Work_Flag = true ;
		}
		
		if ( Work_Flag ) {
			Set_Inspector () ;
			if ( GUI.changed && RealTime_FlagProp.boolValue ) {
				Create () ;
			}
			if (Event.current.commandName == "UndoRedoPerformed" ) {
				Create () ;
			}
		}
	}
	
	void Set_Inspector () {
		GUI.backgroundColor = new Color ( 1.0f , 1.0f , 0.5f , 1.0f ) ;
		serializedObject.Update () ;
		if ( EditorApplication.isPlaying == false ) {

			// Basic settings
			EditorGUILayout.Space () ; EditorGUILayout.Space () ;
			EditorGUILayout.HelpBox( "Basic settings", MessageType.None, true ) ;
			EditorGUILayout.Slider ( DistanceProp , 0.1f , 10.0f , "Distance" ) ;
			EditorGUILayout.Slider ( SpacingProp , 0.05f , 1.0f , "Spacing" ) ;
			EditorGUILayout.Slider ( Track_MassProp , 0.1f , 100.0f , "Mass" ) ;
			// Shape settings
			EditorGUILayout.Space () ;
			EditorGUILayout.HelpBox( "Shape settings", MessageType.None, true ) ;
			SelectedAngleProp.intValue = EditorGUILayout.IntPopup ( "Angle of Front Arc" , SelectedAngleProp.intValue , AngleNames , AngleValues ) ;
			Rear_FlagProp.boolValue = EditorGUILayout.Toggle ( "Set Rear Arc" , Rear_FlagProp.boolValue ) ;
			if ( Rear_FlagProp.boolValue ) {
				Angle_RearProp.intValue = EditorGUILayout.IntPopup ( "Angle of Rear Arc" , Angle_RearProp.intValue , AngleNames , AngleValues ) ;
			}
			EditorGUILayout.IntSlider ( NumberProp , 0 , 80 , "Number of Straight" ) ;
			// Collider settings
			EditorGUILayout.Space () ;
			EditorGUILayout.HelpBox( "Collider settings", MessageType.None, true ) ;
			Collider_InfoProp.boundsValue = EditorGUILayout.BoundsField ( "Box Collider" , Collider_InfoProp.boundsValue ) ;
			Collider_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Physic Material" , Collider_MaterialProp.objectReferenceValue , typeof ( PhysicMaterial ) , false ) ;
			// Mesh settings
			EditorGUILayout.Space () ;
			EditorGUILayout.HelpBox( "Mesh settings", MessageType.None, true ) ;
			Track_L_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh of Left" , Track_L_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
			Track_R_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh of Right" , Track_R_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
			Track_L_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Material of Left" , Track_L_MaterialProp.objectReferenceValue , typeof ( Material ) , false ) ;
			Track_R_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Material of Right" , Track_R_MaterialProp.objectReferenceValue , typeof ( Material ) , false ) ;
			// Reinforce settings
			EditorGUILayout.Space () ;
			EditorGUILayout.HelpBox( "Reinforce settings", MessageType.None, true ) ;
			SubJoint_TypeProp.intValue = EditorGUILayout.Popup ( "Reinforce Type" , SubJoint_TypeProp.intValue , SubJointNames ) ;
			if ( SubJoint_TypeProp.intValue != 2 ) {
				EditorGUILayout.Slider ( Reinforce_RadiusProp , 0.1f , 10.0f , "Radius of SphereCollider" ) ;
			}
			// Interpolation settings
			EditorGUILayout.Space () ;
			EditorGUILayout.HelpBox( "Interpolation settings", MessageType.None, true ) ;
			Use_InterpolationProp.boolValue = EditorGUILayout.Toggle ( "Use Interpolation" , Use_InterpolationProp.boolValue ) ;
			if ( Use_InterpolationProp.boolValue ) {
				EditorGUILayout.Slider ( Joint_OffsetProp , 0.0f , 1.0f , "Joint Offset" ) ;
				Interpolation_L_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh of Left" , Interpolation_L_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
				Interpolation_R_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh of Right" , Interpolation_R_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
				Interpolation_L_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Material of Left" , Interpolation_L_MaterialProp.objectReferenceValue , typeof ( Material ) , false ) ;
				Interpolation_R_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Material of Right" , Interpolation_R_MaterialProp.objectReferenceValue , typeof ( Material ) , false ) ;
			}
			// Special settings
			EditorGUILayout.Space () ;
			EditorGUILayout.HelpBox( "Specail settings for Unity5", MessageType.None, true ) ;
			EditorGUILayout.Slider ( Special_OffsetProp , -0.1f , 0.1f , "Offset for Unity5" ) ;
			// Durability settings
			EditorGUILayout.Space () ;
			EditorGUILayout.HelpBox( "Durability settings", MessageType.None, true ) ;
			EditorGUILayout.Slider ( Track_DurabilityProp , 1.0f , 1000000.0f , "Track Durability" ) ;
			if ( Track_DurabilityProp.floatValue >= 1000000 ) {
				Track_DurabilityProp.floatValue = Mathf.Infinity ;
			}
			EditorGUILayout.Slider ( BreakForceProp , 10000.0f , 1000000.0f , "HingeJoint BreakForce" ) ;
			if ( BreakForceProp.floatValue >= 1000000 ) {
				BreakForceProp.floatValue = Mathf.Infinity ;
			}
			// for Static Track
			EditorGUILayout.Space () ; EditorGUILayout.Space () ;
			EditorGUILayout.HelpBox( "Edit Static Track", MessageType.None, true ) ;
			Static_FlagProp.boolValue = EditorGUILayout.Toggle ( "for Static Track" , Static_FlagProp.boolValue ) ;

			// Update Value
			EditorGUILayout.Space () ; EditorGUILayout.Space () ;
			RealTime_FlagProp.boolValue = EditorGUILayout.Toggle ( "Real Time Update" , RealTime_FlagProp.boolValue ) ;
			if ( GUILayout.Button ( "Update Value" ) ) {
				if ( RealTime_FlagProp.boolValue == false ) {
					Create () ;
				}
			}
			EditorGUILayout.Space () ; EditorGUILayout.Space () ;

		} else { // in PlayMode.
			EditorGUILayout.Space () ; EditorGUILayout.Space () ;
			EditorGUILayout.HelpBox( "Edit Static Track", MessageType.None, true ) ;
			if ( Static_FlagProp.boolValue ) { // for making Static_Track
				if ( !Prefab_FlagProp.boolValue ) { // Static_Track is not prepared yet.
					RealTime_FlagProp.boolValue = false ;
					EditorGUILayout.Space () ; EditorGUILayout.Space () ;
					if ( GUILayout.Button ( "Change into Static Track" ) ) {
						Change_Static_Track () ;
						Prefab_FlagProp.boolValue = true ;
					}
					EditorGUILayout.Space () ; EditorGUILayout.Space () ;
				} else { // Static_Track has been prepared.
					EditorGUILayout.Space () ; EditorGUILayout.Space () ;
					if ( GUILayout.Button ( "Create Prefab in 'Assets' folder" ) ) {
						Create_Prefab () ;
					}
					EditorGUILayout.Space () ; EditorGUILayout.Space () ;
				}
			}
		}

		//
		serializedObject.ApplyModifiedProperties ();
	}
	
	
	void Create () {			
		// Delete Objects
		int Temp_Num = Parent_Transform.childCount ;
		for ( int i = 0 ;  i  < Temp_Num ; i++ ) {
			DestroyImmediate ( Parent_Transform.GetChild ( 0 ).gameObject ) ;
		}
		// Create Track Pieces	(Preparation)
		Count = 0 ;
		if ( Rear_FlagProp.boolValue == false ) {
			Angle_RearProp.intValue = SelectedAngleProp.intValue ;
		}
		float TrueAngle_F = SelectedAngleProp.intValue / 100.0f ;
		float TrueAngle_R = Angle_RearProp.intValue / 100.0f ;
		
		float Radius_F = SpacingProp.floatValue / ( 2.0f * Mathf.Tan ( 3.1415f / ( 360.0f / TrueAngle_F ) ) ) ;
		float Radius_R = SpacingProp.floatValue / ( 2.0f * Mathf.Tan ( 3.1415f / ( 360.0f / TrueAngle_R ) ) ) ;
		
		float Slope_Height = Radius_F - Radius_R ;
		float Slope_Bottom ;
		float Slope_Angle ;
		if ( Mathf.Abs ( Slope_Height ) > SpacingProp.floatValue * NumberProp.intValue || NumberProp.intValue == 0 ) {
			Slope_Bottom = 0.0f ;
			Slope_Angle = 0.0f ;
		} else {
			Slope_Angle = Mathf.Asin ( Slope_Height / ( SpacingProp.floatValue * NumberProp.intValue ) ) ;
			if ( Slope_Angle != 0.0f ) {
				Slope_Bottom = Slope_Height / Mathf.Tan ( Slope_Angle ) ;
			} else {
				Slope_Bottom = SpacingProp.floatValue * NumberProp.intValue ;
			}
			Slope_Angle *= Mathf.Rad2Deg ;
		}
		Pos_Y = DistanceProp.floatValue / 2.0f ;
		
		// Create Front Arc				
		float Initial_Pos_X = Radius_F ;
		float Initial_Pos_Z = 0.0f ;
		for ( int i = 0 ; i <= 180 / TrueAngle_F ; i++ ) {
			Count ++ ;
			Pos_X = Radius_F * Mathf.Sin ( Mathf.Deg2Rad * ( 270.0f + ( TrueAngle_F * i ) ) ) ;
			Pos_X += Initial_Pos_X ;
			Pos_Z = Radius_F * Mathf.Cos ( Mathf.Deg2Rad * ( 270.0f + ( TrueAngle_F * i ) ) ) ;
			Ang_Z = i * TrueAngle_F ;
			Set_Track ( "R" ) ;
			Set_Track ( "L" ) ;
		}
		
		// Create Upper Straight
		if ( Slope_Bottom != 0.0f ) {
			Initial_Pos_X = ( Radius_F * 2.0f ) - ( Slope_Height / NumberProp.intValue / 2.0f ) ;
			Initial_Pos_Z = -( ( SpacingProp.floatValue / 2.0f ) + ( Slope_Bottom / NumberProp.intValue / 2.0f ) ) ;
			Ang_Z = 180.0f + Slope_Angle ;
			for ( int i = 0 ;  i  < NumberProp.intValue ; i++ ) {
				Count ++ ;
				Pos_X = Initial_Pos_X - ( Slope_Height / NumberProp.intValue * i ) ;
				Pos_Z = Initial_Pos_Z - ( Slope_Bottom / NumberProp.intValue * i ) ;
				Set_Track ( "R" ) ;
				Set_Track ( "L" ) ;
			}
		}
		
		// Create Rear Arc
		Initial_Pos_X = Radius_F ;
		Initial_Pos_Z = -( Slope_Bottom + SpacingProp.floatValue ) ;
		for ( int i = 0 ; i <= 180 / TrueAngle_R ; i++ ) {
			Count ++ ;
			Pos_X = Radius_R * Mathf.Sin ( Mathf.Deg2Rad * ( 90.0f + ( TrueAngle_R * i ) ) ) ;
			Pos_X += Initial_Pos_X ;
			Pos_Z = Radius_R * Mathf.Cos ( Mathf.Deg2Rad * ( 90.0f + ( TrueAngle_R * i ) ) ) ;
			Pos_Z += Initial_Pos_Z ;
			Ang_Z = 180.0f + ( i * TrueAngle_R ) ;
			Set_Track ( "R" ) ;
			Set_Track ( "L" ) ;
		}
		
		// Create lower Straight
		if ( Slope_Bottom != 0.0f ) {
			Initial_Pos_X = ( Radius_F - Radius_R ) - ( Slope_Height / NumberProp.intValue / 2.0f ) ;
			Initial_Pos_Z = -( Slope_Bottom + ( SpacingProp.floatValue / 2.0f ) ) + ( Slope_Bottom / NumberProp.intValue / 2.0f ) ;
			Ang_Z = -Slope_Angle ;
			for ( int i = 0 ;  i  < NumberProp.intValue ; i++ ) {
				Count ++ ;
				Pos_X = Initial_Pos_X - ( Slope_Height / NumberProp.intValue * i ) ;
				Pos_Z = Initial_Pos_Z + ( Slope_Bottom / NumberProp.intValue * i ) ;
				Set_Track ( "R" ) ;
				Set_Track ( "L" ) ;
			}
		}

		// Create Reinforce Collider.
		int Total_Number = Count ;
		if ( SubJoint_TypeProp.intValue != 2 ) {
			Count = 0 ;
			for ( int i = 0 ; i < Total_Number ; i ++ ) {
				Count ++ ;
				if ( SubJoint_TypeProp.intValue == 0 || Count % 2 == 0 ) {
					Set_SubJoint ( "R" ) ;
					Set_SubJoint ( "L" ) ;
				}
			}
		}
		
		// Create Interpolation
		if ( Use_InterpolationProp.boolValue ) {
			Count = 0 ;
			for ( int i = 0 ; i < Total_Number ; i ++ ) {
				Count += 1 ;
				Set_Interpolator ( "R" ) ;
				Set_Interpolator ( "L" ) ;
			}
		}

		// Add RigidBody and Joint.
		Finishing ( "L" ) ;
		Finishing ( "R" ) ;

	}
	
	
	void Set_Track ( string Direction ) {
		//Create gameobject & Set transform
		GameObject Temp_Object = new GameObject ( "TrackBelt_" + Direction + "_" + Count ) ;
		Temp_Object.transform.parent = Parent_Transform ;
		if ( Direction == "R" ) {
			Temp_Object.transform.localPosition = new Vector3 ( Pos_X , -Pos_Y , Pos_Z ) ;
		} else {
			Temp_Object.transform.localPosition = new Vector3 ( Pos_X , Pos_Y , Pos_Z ) ;
		}
		Temp_Object.transform.localRotation = Quaternion.Euler ( 0.0f , Ang_Z , -90.0f ) ;
		// Mesh
		Temp_Object.AddComponent < MeshRenderer > () ;
		MeshFilter Temp_MeshFilter ;
		Temp_MeshFilter = Temp_Object.AddComponent < MeshFilter > () ;
		if ( Direction == "R" ) {
			Temp_MeshFilter.mesh = Track_R_MeshProp.objectReferenceValue as Mesh ;
			Temp_Object.GetComponent<Renderer>().material = Track_R_MaterialProp.objectReferenceValue as Material ;
		} else {
			Temp_MeshFilter.mesh = Track_L_MeshProp.objectReferenceValue as Mesh ;
			Temp_Object.GetComponent<Renderer>().material = Track_L_MaterialProp.objectReferenceValue as Material ;
		}
		// BoxCollider
		BoxCollider Temp_BoxCollider ;
		Temp_BoxCollider = Temp_Object.AddComponent < BoxCollider > () ;
		Temp_BoxCollider.center = Collider_InfoProp.boundsValue.center ;
		if ( Direction == "R" ) {
			float Temp_Center_X = -Temp_BoxCollider.center.x ;
			float Temp_Center_Y = Temp_BoxCollider.center.y ;
			float Temp_Center_Z = Temp_BoxCollider.center.z ;
			Temp_BoxCollider.center = new Vector3 ( Temp_Center_X , Temp_Center_Y , Temp_Center_Z ) ;
		}
		Temp_BoxCollider.size = Collider_InfoProp.boundsValue.size ;
		Temp_BoxCollider.material = Collider_MaterialProp.objectReferenceValue as PhysicMaterial ;
		// Stabilizer_CS
		Temp_Object.AddComponent < Stabilizer_CS > () ;
		// Damage_Control_CS
		Damage_Control_CS Temp_Damage_Control_CS ;
		Temp_Damage_Control_CS = Temp_Object.AddComponent < Damage_Control_CS > () ;
		Temp_Damage_Control_CS.Type = 6 ; // 6 = Track in "Damage_Control"
		Temp_Damage_Control_CS.Durability = Track_DurabilityProp.floatValue ;
		if ( Direction == "L" ) {
			Temp_Damage_Control_CS.Direction = 0 ;
		} else {
			Temp_Damage_Control_CS.Direction = 1 ;
		}
		// Static_Track_Setting_CS
		if ( Static_FlagProp.boolValue ) {
			Temp_Object.AddComponent < Static_Track_Setting_CS > () ;
		}
		// Set Layer
		Temp_Object.layer = 0 ;
	}

	void Set_SubJoint ( string Direction ) {
		//Create gameobject & Set transform
		Transform Base_Transform = Parent_Transform.Find ( "TrackBelt_" + Direction +"_" + Count ) ;
		GameObject Temp_Object = new GameObject ( "Reinforce_" + Direction + "_" + Count ) ;
		Temp_Object.transform.position = Base_Transform.position ;
		Temp_Object.transform.rotation = Base_Transform.rotation ;
		Temp_Object.transform.parent = Base_Transform ;
		// SphereCollider
		SphereCollider Temp_SphereCollider ;
		Temp_SphereCollider = Temp_Object.AddComponent < SphereCollider > () ;
		Temp_SphereCollider.radius = Reinforce_RadiusProp.floatValue ;
		// Damage_Control_CS
		Damage_Control_CS Temp_Damage_Control_CS ;
		Temp_Damage_Control_CS = Temp_Object.AddComponent < Damage_Control_CS > () ;
		Temp_Damage_Control_CS.Type = 7 ; // 7 = Reinforce in "Damage_Control"
		if ( Direction == "L" ) {
			Temp_Damage_Control_CS.Direction = 0 ;
		} else {
			Temp_Damage_Control_CS.Direction = 1 ;
		}
		// Set Layer
		Temp_Object.layer = 10 ; // Ignore All.
	}
	
	void Set_Interpolator ( string Direction ) {
		//Create gameobject & Set transform
		Transform Base_Transform = Parent_Transform.Find ( "TrackBelt_" + Direction +"_" + Count ) ;
		Transform Front_Transform = Parent_Transform.Find ( "TrackBelt_" + Direction +"_" + ( Count + 1 ) ) ;
		if ( Front_Transform == null ) {
			Front_Transform = Parent_Transform.Find ( "TrackBelt_" + Direction +"_1" ) ;
		}
		GameObject Temp_Object = new GameObject ( "Interpolator" ) ;
		Temp_Object.transform.parent = Base_Transform ;
		Vector3 Base_Pos = Base_Transform.position + ( Base_Transform.forward * Joint_OffsetProp.floatValue ) ;
		Vector3 Front_Pos = Front_Transform.position - ( Front_Transform.forward * Joint_OffsetProp.floatValue ) ;
		Temp_Object.transform.position = Vector3.Slerp ( Base_Pos , Front_Pos , 0.5f ) ;
		Temp_Object.transform.rotation = Quaternion.Slerp ( Base_Transform.rotation , Front_Transform.rotation , 0.5f ) ;		
		// Mesh
		Temp_Object.AddComponent < MeshRenderer > () ;
		MeshFilter Temp_MeshFilter ;
		Temp_MeshFilter = Temp_Object.AddComponent < MeshFilter > () ;
		if ( Direction == "R" ) {
			Temp_MeshFilter.mesh = Interpolation_R_MeshProp.objectReferenceValue as Mesh ;
			Temp_Object.GetComponent<Renderer>().material = Interpolation_R_MaterialProp.objectReferenceValue as Material ;
		} else {
			Temp_MeshFilter.mesh = Interpolation_L_MeshProp.objectReferenceValue as Mesh ;
			Temp_Object.GetComponent<Renderer>().material = Interpolation_L_MaterialProp.objectReferenceValue as Material ;
		}
		// Track_Interpolation_CS
		Track_Interpolation_CS Temp_Script ;
		Temp_Script = Temp_Object.AddComponent < Track_Interpolation_CS > () ;
		Temp_Script.Set_Value ( Base_Transform , Front_Transform , Joint_OffsetProp.floatValue , SpacingProp.floatValue , Direction ) ;
	}

	void Finishing ( string Direction ) {
		// Add RigidBody.
		for ( int i = 1 ;  i <= Parent_Transform.childCount ; i++ ) {
			Transform Temp_Transform = Parent_Transform.Find ( "TrackBelt_" + Direction + "_" + i ) ;
			if ( Temp_Transform ) {
				GameObject Temp_Object = Temp_Transform.gameObject ;
				// Add RigidBody.
				Rigidbody Temp_RigidBody = Temp_Object.AddComponent < Rigidbody > () ;
				Temp_RigidBody.mass = Track_MassProp.floatValue ;
				// for Static_Track
				if ( Static_FlagProp.boolValue ) {
					Temp_RigidBody.drag = 10.0f ;
				}
				// Special Offset for Unity5.
				if ( i % 2 == 0 ) {
					Temp_Transform.position += Temp_Transform.forward * Special_OffsetProp.floatValue ;
				}
			}
		}
		// Add HingeJoint.
		for ( int i = 1 ;  i <= Parent_Transform.childCount ; i++ ) {
			Transform Temp_Transform = Parent_Transform.Find ( "TrackBelt_" + Direction + "_" + i ) ;
			if ( Temp_Transform ) {
				GameObject Temp_Object = Temp_Transform.gameObject ;
				HingeJoint Temp_HingeJoint ;
				Temp_HingeJoint = Temp_Object.AddComponent < HingeJoint > () ;
				Temp_HingeJoint.anchor = new Vector3 ( 0.0f , 0.0f , SpacingProp.floatValue / 2.0f ) ;
				Temp_HingeJoint.axis = new Vector3 ( 1.0f , 0.0f , 0.0f ) ;
				Temp_HingeJoint.breakForce = BreakForceProp.floatValue ;
				Transform Temp_Connected_Transform ;
				Temp_Connected_Transform = Parent_Transform.Find ( "TrackBelt_" + Direction + "_" + ( i + 1 ) ) ;
				if ( Temp_Connected_Transform ) {
					Temp_HingeJoint.connectedBody = Temp_Connected_Transform.GetComponent < Rigidbody > () ;
				} else {
					Temp_Connected_Transform = Parent_Transform.Find ( "TrackBelt_" + Direction + "_1" ) ;
					if ( Temp_Connected_Transform ) {
						Temp_HingeJoint.connectedBody = Temp_Connected_Transform.GetComponent < Rigidbody > () ;
					}
				}
			}
		}
	}

	void Change_Static_Track () {
		Parent_Transform.BroadcastMessage ( "Set_Static_Track_Value" , SendMessageOptions.DontRequireReceiver ) ;
		Time.timeScale = 0.0f ;
	}

	void Create_Prefab () {
		GameObject Temp_Object = new GameObject ( "Temp_Object" ) ;
		Temp_Object.transform.parent = Parent_Transform.parent ;
		Temp_Object.transform.localPosition = Parent_Transform.localPosition ;
		Temp_Object.transform.localRotation = Parent_Transform.localRotation ;
		Static_Track_CS Temp_Script ;
		Temp_Script = Temp_Object.AddComponent < Static_Track_CS > () ;
		Temp_Script.Type = 9 ; // 9=Parent ;
		Temp_Script.Length = Collider_InfoProp.boundsValue.size.z ;
		// Enable "Static_Track_CS".
		int Temp_Count = Parent_Transform.childCount ;
		for ( int i = 0 ;  i  < Temp_Count ; i++ ) {
			Transform Temp_Child = Parent_Transform.GetChild ( 0 ) ;
			Destroy ( Temp_Child.GetComponent < Static_Track_Setting_CS > () ) ;
			Temp_Child.GetComponent < Static_Track_CS > ().enabled = true ;
			Temp_Child.parent = Temp_Object.transform ;
		}
		// Create Prefab.
		string Temp_String = DateTime.Now.ToString ( "yyMMdd_HHmmss" ) ;
		PrefabUtility.CreatePrefab ( "Assets/" + "Static_Track" + Temp_String + ".prefab" , Temp_Object ) ;
		// Message.
		if ( Parent_Transform.childCount == 0 ) {
			Debug.Log ( "New 'Static_Track' has been created in 'Assets' folder." ) ;
		} else {
			Debug.LogWarning ( "The script fails to create a prefab of 'Static_Track'." ) ;
		}
		//
		DestroyImmediate ( Temp_Object ) ;
	}
	
}
