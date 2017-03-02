using UnityEngine;
using System.Collections;
using UnityEditor ;

[ CustomEditor ( typeof ( Create_RoadWheel_CS ) ) ]

public class Create_RoadWheel_CSEditor : Editor {

	SerializedProperty Fit_ST_FlagProp ;

	SerializedProperty Sus_DistanceProp ;
	SerializedProperty NumProp ;
	SerializedProperty SpacingProp ;
	SerializedProperty Sus_LengthProp ;
	SerializedProperty Sus_AngleProp ;
	SerializedProperty Sus_AnchorProp ;
	SerializedProperty Sus_MassProp ;
	SerializedProperty Sus_SpringProp ;
	SerializedProperty Sus_DamperProp ;
	SerializedProperty Sus_TargetProp ;
	SerializedProperty Sus_Forward_LimitProp ;
	SerializedProperty Sus_Backward_LimitProp ;
	SerializedProperty Sus_L_MeshProp ;
	SerializedProperty Sus_R_MeshProp ;
	SerializedProperty Sus_L_MaterialProp ;
	SerializedProperty Sus_R_MaterialProp ;
	SerializedProperty Reinforce_RadiusProp ;
	
	SerializedProperty Wheel_DistanceProp ;
	SerializedProperty Wheel_MassProp ;
	SerializedProperty Wheel_RadiusProp ;
	SerializedProperty Collider_MaterialProp ;
	SerializedProperty Wheel_MeshProp ;
	SerializedProperty Wheel_MaterialProp ;
	SerializedProperty Collider_MeshProp ;
	SerializedProperty Collider_Mesh_SubProp ;

	SerializedProperty Drive_WheelProp ;
	SerializedProperty Wheel_ResizeProp ;
	SerializedProperty ScaleDown_SizeProp ;
	SerializedProperty Return_SpeedProp ;
	SerializedProperty Wheel_DurabilityProp ;

	SerializedProperty RealTime_FlagProp ;
	SerializedProperty Parent_TransformProp ;
	
	Transform Parent_Transform ;
	float WheelPos_X ;
	float WheelPos_Y ;
	float WheelPos_Z ;
	float SusPos_X ;
	float SusPos_Y ;
	float SusPos_Z ;
	int Count = 1 ;
	
	void OnEnable () {
		Fit_ST_FlagProp = serializedObject.FindProperty ( "Fit_ST_Flag" ) ;

		Sus_DistanceProp = serializedObject.FindProperty ( "Sus_Distance" ) ;
		NumProp = serializedObject.FindProperty ( "Num" ) ;
		SpacingProp = serializedObject.FindProperty ( "Spacing" ) ;
		Sus_LengthProp = serializedObject.FindProperty ( "Sus_Length" ) ;
		Sus_AngleProp = serializedObject.FindProperty ( "Sus_Angle" ) ;
		Sus_AnchorProp = serializedObject.FindProperty ( "Sus_Anchor" ) ;
		Sus_MassProp = serializedObject.FindProperty ( "Sus_Mass" ) ;
		Sus_SpringProp = serializedObject.FindProperty ( "Sus_Spring" ) ;
		Sus_DamperProp = serializedObject.FindProperty ( "Sus_Damper" ) ;
		Sus_TargetProp = serializedObject.FindProperty ( "Sus_Target" ) ;
		Sus_Forward_LimitProp = serializedObject.FindProperty ( "Sus_Forward_Limit" ) ;
		Sus_Backward_LimitProp = serializedObject.FindProperty ( "Sus_Backward_Limit" ) ;
		Sus_L_MeshProp = serializedObject.FindProperty ( "Sus_L_Mesh" ) ;
		Sus_R_MeshProp = serializedObject.FindProperty ( "Sus_R_Mesh" ) ;
		Sus_L_MaterialProp = serializedObject.FindProperty ( "Sus_L_Material" ) ;
		Sus_R_MaterialProp = serializedObject.FindProperty ( "Sus_R_Material" ) ;
		Reinforce_RadiusProp = serializedObject.FindProperty ( "Reinforce_Radius" ) ;

		Wheel_DistanceProp = serializedObject.FindProperty ( "Wheel_Distance" ) ;
		Wheel_MassProp = serializedObject.FindProperty ( "Wheel_Mass" ) ;		
		Wheel_RadiusProp = serializedObject.FindProperty ( "Wheel_Radius" ) ;
		Collider_MaterialProp = serializedObject.FindProperty ( "Collider_Material" ) ;
		Wheel_MeshProp = serializedObject.FindProperty ( "Wheel_Mesh" ) ;
		Wheel_MaterialProp = serializedObject.FindProperty ( "Wheel_Material" ) ;
		Collider_MeshProp = serializedObject.FindProperty ( "Collider_Mesh" ) ;
		Collider_Mesh_SubProp = serializedObject.FindProperty ( "Collider_Mesh_Sub" ) ;

		Drive_WheelProp = serializedObject.FindProperty ( "Drive_Wheel" ) ;
		Wheel_ResizeProp = serializedObject.FindProperty ( "Wheel_Resize" ) ;
		ScaleDown_SizeProp = serializedObject.FindProperty ( "ScaleDown_Size" ) ;
		Return_SpeedProp = serializedObject.FindProperty ( "Return_Speed" ) ;
		Wheel_DurabilityProp = serializedObject.FindProperty ( "Wheel_Durability" ) ;

		RealTime_FlagProp = serializedObject.FindProperty ( "RealTime_Flag" ) ;
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

		// for Static Wheel
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Wheel Type", MessageType.None, true ) ;
		Fit_ST_FlagProp.boolValue = EditorGUILayout.Toggle ( "Fit for Static Tracks" , Fit_ST_FlagProp.boolValue ) ;

		// Suspension settings
		EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Suspension settings", MessageType.None, true );
		EditorGUILayout.Slider ( Sus_DistanceProp , 0.1f , 10.0f , "Distance" ) ;
		EditorGUILayout.IntSlider ( NumProp , 0 , 30 , "Number" ) ;
		EditorGUILayout.Slider ( SpacingProp , 0.1f , 10.0f , "Spacing" ) ;
		EditorGUILayout.Slider ( Sus_LengthProp , -1.0f , 1.0f , "Length" ) ;
		EditorGUILayout.Slider ( Sus_AngleProp , -180.0f , 180.0f , "Angle" ) ;
		EditorGUILayout.Slider ( Sus_AnchorProp , -1.0f , 1.0f , "Anchor Offset" ) ;
		EditorGUILayout.Slider ( Sus_MassProp , 0.1f , 300.0f , "Mass" ) ;
		EditorGUILayout.Slider ( Sus_SpringProp , 0.0f , 100000.0f , "Sus Spring Force" ) ;
		EditorGUILayout.Slider ( Sus_DamperProp , 0.0f , 10000.0f , "Sus Damper Force" ) ;
		EditorGUILayout.Slider ( Sus_TargetProp , -90.0f , 90.0f , "Sus Spring Target Angle" ) ;
		EditorGUILayout.Slider ( Sus_Forward_LimitProp , 0.0f , 90.0f , "Forward Limit Angle" ) ;
		EditorGUILayout.Slider ( Sus_Backward_LimitProp , 0.0f , 90.0f , "Backward Limit Angle" ) ;
		EditorGUILayout.Space () ;
		Sus_L_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh of Left" , Sus_L_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
		Sus_R_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh of Right" , Sus_R_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
		Sus_L_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Material of Left" , Sus_L_MaterialProp.objectReferenceValue , typeof ( Material ) , false ) ;
		Sus_R_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Material of Right" , Sus_R_MaterialProp.objectReferenceValue , typeof ( Material ) , false ) ;
		EditorGUILayout.Space () ;
		EditorGUILayout.Slider ( Reinforce_RadiusProp , 0.1f , 1.0f , "SphereCollider Radius" ) ;

		// Wheels settings
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Wheels settings", MessageType.None, true );
		EditorGUILayout.Slider ( Wheel_DistanceProp , 0.1f , 10.0f , "Distance" ) ;
		EditorGUILayout.Slider ( Wheel_MassProp , 0.1f , 300.0f , "Mass" ) ;
		EditorGUILayout.Space () ;
		GUI.backgroundColor = new Color ( 1.0f , 0.5f , 0.5f , 1.0f ) ;
		EditorGUILayout.Slider ( Wheel_RadiusProp , 0.01f , 1.0f , "SphereCollider Radius" ) ;
		GUI.backgroundColor = new Color ( 1.0f , 1.0f , 0.5f , 1.0f ) ;
		Collider_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Physic Material" , Collider_MaterialProp.objectReferenceValue , typeof ( PhysicMaterial ) , false ) ;
		EditorGUILayout.Space () ;
		Wheel_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh" , Wheel_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
		Wheel_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Material" , Wheel_MaterialProp.objectReferenceValue , typeof ( Material ) , false ) ;

		// Mesh Collider
		if ( Fit_ST_FlagProp.boolValue == false ) { // for Physics Tracks
			EditorGUILayout.Space () ;
			EditorGUILayout.HelpBox( "'MeshCollider settings", MessageType.None, true );
			Collider_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh Collider" , Collider_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
			Collider_Mesh_SubProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh Sub Collider" , Collider_Mesh_SubProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
		}

		// Scripts settings
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Scripts settings", MessageType.None, true );
		// Drive Wheel
		Drive_WheelProp.boolValue = EditorGUILayout.Toggle ( "Drive Wheel" , Drive_WheelProp.boolValue ) ;
		EditorGUILayout.Space () ;
		if ( Fit_ST_FlagProp.boolValue == false ) { // for Physics Tracks
			// Wheel Resize
			Wheel_ResizeProp.boolValue = EditorGUILayout.Toggle ( "Wheel Resize Script" , Wheel_ResizeProp.boolValue ) ;
			if ( Wheel_ResizeProp.boolValue ) {
				EditorGUILayout.Slider ( ScaleDown_SizeProp , 0.1f , 3.0f , "Scale Size" ) ;
				EditorGUILayout.Slider ( Return_SpeedProp , 0.01f , 0.1f , "Return Speed" ) ;
			}
			EditorGUILayout.Space () ;
			// Durability
			EditorGUILayout.Slider ( Wheel_DurabilityProp , 1 , 1000000 , "Wheel Durability" ) ;
			if ( Wheel_DurabilityProp.floatValue >= 1000000 ) {
				Wheel_DurabilityProp.floatValue = Mathf.Infinity ;
			}
		}

		// Update Value
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		RealTime_FlagProp.boolValue = EditorGUILayout.Toggle ( "Real Time Update" , RealTime_FlagProp.boolValue ) ;
		if ( GUILayout.Button ( "Update Value" ) ) {
			if ( RealTime_FlagProp.boolValue == false ) {
				Create () ;
			}
		}
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;

		//
		serializedObject.ApplyModifiedProperties ();
	}
	
	
	void Create () {
		// Delete Objects
		int Temp_Num = Parent_Transform.childCount ;
		for ( int i = 0 ;  i  < Temp_Num ; i++ ) {
			DestroyImmediate ( Parent_Transform.GetChild ( 0 ).gameObject ) ;
		}
		// Create Suspension & Wheel
		Count = 1 ;
		for ( int i = 0 ;  i  < NumProp.intValue ; i++ ) {
			SusPos_X = 0.0f ;		
			SusPos_Z = -SpacingProp.floatValue * i ;
			
			WheelPos_X = Mathf.Sin ( Mathf.Deg2Rad * ( 180.0f + Sus_AngleProp.floatValue ) ) * Sus_LengthProp.floatValue ;
			WheelPos_Z = -SpacingProp.floatValue * i ;
			WheelPos_Z += Mathf.Cos ( Mathf.Deg2Rad * ( 180.0f + Sus_AngleProp.floatValue ) ) * Sus_LengthProp.floatValue ;
			
			SusPos_Y = -Sus_DistanceProp.floatValue / 2.0f ;
			WheelPos_Y = -Wheel_DistanceProp.floatValue / 2.0f ;
			SetSusValue ( "R" ) ;
			SetWheelValue ( "R" ) ;
			
			SusPos_Y = Sus_DistanceProp.floatValue / 2.0f ;
			WheelPos_Y = Wheel_DistanceProp.floatValue / 2.0f ;
			SetSusValue ( "L" ) ;
			SetWheelValue ( "L" ) ;
			
			Count ++ ;
		}
	}
	
	void SetSusValue ( string Direction ) {
		//Create gameobject & Set transform
		GameObject Temp_Object = new GameObject ( "Suspension_" + Direction + "_" + Count ) ;
		Temp_Object.transform.parent = Parent_Transform ;
		Temp_Object.transform.localPosition = new Vector3 ( SusPos_X , SusPos_Y , SusPos_Z ) ;
		Temp_Object.transform.localRotation = Quaternion.Euler ( 0.0f , Sus_AngleProp.floatValue , -90.0f ) ;
		// Mesh
		Temp_Object.AddComponent < MeshRenderer > () ;
		MeshFilter Temp_MeshFilter ;
		Temp_MeshFilter = Temp_Object.AddComponent < MeshFilter > () ;
		if ( Direction == "R" ) {
			Temp_MeshFilter.mesh = Sus_R_MeshProp.objectReferenceValue as Mesh ;
			Temp_Object.GetComponent<Renderer>().material = Sus_R_MaterialProp.objectReferenceValue as Material ;
		} else {
			Temp_MeshFilter.mesh = Sus_L_MeshProp.objectReferenceValue as Mesh ;
			Temp_Object.GetComponent<Renderer>().material = Sus_L_MaterialProp.objectReferenceValue as Material ;
		}
		// Rigidbody
		Temp_Object.AddComponent < Rigidbody > () ;
		Temp_Object.GetComponent<Rigidbody>().mass = Sus_MassProp.floatValue ;
		// HingeJoint
		HingeJoint Temp_HingeJoint ;
		Temp_HingeJoint = Temp_Object.AddComponent < HingeJoint > () ;
		Temp_HingeJoint.connectedBody = Parent_Transform.parent.gameObject.GetComponent<Rigidbody>() ;
		Temp_HingeJoint.anchor = new Vector3 ( 0.0f , 0.0f , Sus_AnchorProp.floatValue ) ;
		Temp_HingeJoint.axis = new Vector3 ( 1.0f , 0.0f , 0.0f ) ;
		Temp_HingeJoint.useSpring = true ;
		JointSpring Temp_JointSpring = Temp_HingeJoint.spring ;
		Temp_JointSpring.spring = Sus_SpringProp.floatValue ;
		Temp_JointSpring.damper = Sus_DamperProp.floatValue ;
		Temp_JointSpring.targetPosition = Sus_TargetProp.floatValue ;
		Temp_HingeJoint.spring = Temp_JointSpring ;
		Temp_HingeJoint.useLimits = true ;
		JointLimits Temp_JointLimits = Temp_HingeJoint.limits ;
		Temp_JointLimits.max = Sus_Forward_LimitProp.floatValue ;
		Temp_JointLimits.min = -Sus_Backward_LimitProp.floatValue ;
		Temp_HingeJoint.limits = Temp_JointLimits ;
		// Reinforce SphereCollider
		SphereCollider Temp_SphereCollider ;
		Temp_SphereCollider = Temp_Object.AddComponent < SphereCollider > () ;
		Temp_SphereCollider.radius = Reinforce_RadiusProp.floatValue ;
		// Set Layer
		Temp_Object.layer = 10 ; // Ignore All
	}
	
	void SetWheelValue ( string Direction ) {
		//Create gameobject & Set transform
		GameObject Temp_Object = new GameObject ( "RoadWheel_" + Direction + "_" + Count ) ;
		Temp_Object.transform.parent = Parent_Transform ;
		Temp_Object.transform.localPosition = new Vector3 ( WheelPos_X , WheelPos_Y , WheelPos_Z ) ;
		if ( Direction == "R" ) {
			Temp_Object.transform.localRotation = Quaternion.Euler ( 0.0f , 0.0f , 180 ) ;
		} else {
			Temp_Object.transform.localRotation = Quaternion.Euler ( Vector3.zero ) ;
		}
		// Mesh
		Temp_Object.AddComponent < MeshRenderer > () ;
		Temp_Object.GetComponent<Renderer>().material = Wheel_MaterialProp.objectReferenceValue as Material ;
		MeshFilter Temp_MeshFilter ;
		Temp_MeshFilter = Temp_Object.AddComponent < MeshFilter > () ;
		Temp_MeshFilter.mesh = Wheel_MeshProp.objectReferenceValue as Mesh ;
		// Rigidbody
		Temp_Object.AddComponent < Rigidbody > () ;
		Temp_Object.GetComponent<Rigidbody>().mass = Wheel_MassProp.floatValue ;
		// HingeJoint
		HingeJoint Temp_HingeJoint ;
		Temp_HingeJoint = Temp_Object.AddComponent < HingeJoint > () ;
		Temp_HingeJoint.anchor = Vector3.zero ;
		Temp_HingeJoint.axis = new Vector3 ( 0.0f , 1.0f , 0.0f ) ;
		Temp_HingeJoint.connectedBody = Parent_Transform.Find ( "Suspension_" + Direction + "_" + Count ).gameObject.GetComponent<Rigidbody>() ;
		// SphereCollider
		SphereCollider Temp_SphereCollider ;
		Temp_SphereCollider = Temp_Object.AddComponent < SphereCollider > () ;
		Temp_SphereCollider.radius = Wheel_RadiusProp.floatValue ;
		Temp_SphereCollider.material = Collider_MaterialProp.objectReferenceValue as PhysicMaterial ;
		// MeshCollider
		if ( Fit_ST_FlagProp.boolValue == false ) { // for Physics Tracks
			if ( Collider_MeshProp.objectReferenceValue != null ) {
				MeshCollider Temp_MeshCollider ;
				Temp_MeshCollider = Temp_Object.AddComponent < MeshCollider > () ;
				Temp_MeshCollider.material = Collider_MaterialProp.objectReferenceValue as PhysicMaterial ;
				Temp_MeshCollider.sharedMesh = Collider_MeshProp.objectReferenceValue as Mesh ;
				Temp_MeshCollider.convex = true ;
				Temp_MeshCollider.enabled = false ;
			}
			// Sub MeshCollider
			if  ( Collider_Mesh_SubProp.objectReferenceValue != null ) {
				MeshCollider Temp_MeshCollider_Sub ;
				Temp_MeshCollider_Sub = Temp_Object.AddComponent < MeshCollider > () ;
				Temp_MeshCollider_Sub.material = Collider_MaterialProp.objectReferenceValue as PhysicMaterial ;
				Temp_MeshCollider_Sub.sharedMesh = Collider_Mesh_SubProp.objectReferenceValue as Mesh ;
				Temp_MeshCollider_Sub.convex = true ;
				Temp_MeshCollider_Sub.enabled = false ;
			}
		}
		// Drive_Wheel_CS
		Drive_Wheel_CS Temp_Drive_Wheel_CS ;
		Temp_Drive_Wheel_CS = Temp_Object.AddComponent < Drive_Wheel_CS > () ;
		Temp_Drive_Wheel_CS.Radius = Wheel_RadiusProp.floatValue ;
		// Wheel_Resize_CS
		if ( Fit_ST_FlagProp.boolValue == false ) { // for Physics Tracks
			if ( Wheel_ResizeProp.boolValue ) {
				Wheel_Resize_CS Temp_Wheel_Resize_CS ;
				Temp_Wheel_Resize_CS = Temp_Object.AddComponent < Wheel_Resize_CS > () ;
				Temp_Wheel_Resize_CS.ScaleDown_Size = ScaleDown_SizeProp.floatValue ;
				Temp_Wheel_Resize_CS.Return_Speed = Return_SpeedProp.floatValue ;
			}
		}
		// Damage_Control_CS
		Damage_Control_CS Temp_Damage_Control_CS ;
		Temp_Damage_Control_CS = Temp_Object.AddComponent < Damage_Control_CS > () ;
		Temp_Damage_Control_CS.Type = 8 ; // 8 = Wheel in "Damage_Control"
		if ( Fit_ST_FlagProp.boolValue == false ) { // for Physics Tracks
			Temp_Damage_Control_CS.Durability = Wheel_DurabilityProp.floatValue ;
		} else { // for Static Tracks
			Temp_Damage_Control_CS.Durability = Mathf.Infinity ;
		}
		// Set direction.
		if ( Direction == "L" ) {
			Temp_Damage_Control_CS.Direction = 0 ;
		} else {
			Temp_Damage_Control_CS.Direction = 1 ;
		}
		// Stabilizer_CS
		Temp_Object.AddComponent < Stabilizer_CS > () ;
		// Set Layer
		Temp_Object.layer = 9 ;
	}
	
}
