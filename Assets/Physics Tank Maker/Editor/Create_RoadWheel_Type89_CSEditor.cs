using UnityEngine;
using System.Collections;
using UnityEditor ;

[ CustomEditor ( typeof ( Create_RoadWheel_Type89_CS ) ) ]

public class Create_RoadWheel_Type89_CSEditor : Editor {

	SerializedProperty Fit_ST_FlagProp ;

	SerializedProperty DistanceProp ;
	SerializedProperty SpringProp ;
	SerializedProperty ParentArm_NumProp ;
	SerializedProperty ParentArm_SpacingProp ;
	SerializedProperty ParentArm_Offset_YProp ;
	SerializedProperty ParentArm_AngleLimitProp ;
	SerializedProperty ParentArm_MassProp ;
	SerializedProperty ParentArm_L_MeshProp ;
	SerializedProperty ParentArm_R_MeshProp ;
	SerializedProperty ParentArm_L_MaterialProp ;
	SerializedProperty ParentArm_R_MaterialProp ;
	
	SerializedProperty ChildArm_NumProp ;
	SerializedProperty ChildArm_SpacingProp ;
	SerializedProperty ChildArm_Offset_YProp ;
	SerializedProperty ChildArm_AngleLimitProp ;
	SerializedProperty ChildArm_MassProp ;
	SerializedProperty ChildArm_L_MeshProp ;
	SerializedProperty ChildArm_R_MeshProp ;
	SerializedProperty ChildArm_L_MaterialProp ;
	SerializedProperty ChildArm_R_MaterialProp ;
	
	SerializedProperty Wheel_NumProp ;
	SerializedProperty Wheel_SpacingProp ;
	SerializedProperty Wheel_Offset_YProp ;
	SerializedProperty Wheel_MassProp ;
	SerializedProperty Wheel_RadiusProp ;
	SerializedProperty Wheel_Collider_MaterialProp ;
	SerializedProperty Wheel_MeshProp ;
	SerializedProperty Wheel_MaterialProp ;
	SerializedProperty Wheel_Collider_MeshProp ;

	SerializedProperty Drive_WheelProp ;
	SerializedProperty Wheel_ResizeProp ;
	SerializedProperty ScaleDown_SizeProp ;
	SerializedProperty Return_SpeedProp ;
	SerializedProperty Wheel_DurabilityProp ;
	SerializedProperty RealTime_FlagProp ;
	SerializedProperty Parent_TransformProp ;
	
	Transform Parent_Transform ;
	float ParentArmPos_X ;
	float ParentArmPos_Y ;
	float ParentArmPos_Z ;
	float ChildArmPos_X ;
	float ChildArmPos_Y ;
	float ChildArmPos_Z ;
	float WheelPos_X ;
	float WheelPos_Y ;
	float WheelPos_Z ;
	int Count_Parent ;
	int Count_Child ;
	int Count_Wheel ;
	
	void OnEnable () {
		Fit_ST_FlagProp = serializedObject.FindProperty ( "Fit_ST_Flag" ) ;

		DistanceProp = serializedObject.FindProperty ( "Distance" ) ;
		SpringProp = serializedObject.FindProperty ( "Spring" ) ;
		ParentArm_NumProp = serializedObject.FindProperty ( "ParentArm_Num" ) ;
		ParentArm_SpacingProp = serializedObject.FindProperty ( "ParentArm_Spacing" ) ;
		ParentArm_Offset_YProp = serializedObject.FindProperty ( "ParentArm_Offset_Y" ) ;
		ParentArm_AngleLimitProp = serializedObject.FindProperty ( "ParentArm_AngleLimit" ) ;
		ParentArm_MassProp = serializedObject.FindProperty ( "ParentArm_Mass" ) ;
		ParentArm_L_MeshProp = serializedObject.FindProperty ( "ParentArm_L_Mesh" ) ;
		ParentArm_R_MeshProp = serializedObject.FindProperty ( "ParentArm_R_Mesh" ) ;
		ParentArm_L_MaterialProp = serializedObject.FindProperty ( "ParentArm_L_Material" ) ;
		ParentArm_R_MaterialProp = serializedObject.FindProperty ( "ParentArm_R_Material" ) ;
		
		ChildArm_NumProp = serializedObject.FindProperty ( "ChildArm_Num" ) ;
		ChildArm_SpacingProp = serializedObject.FindProperty ( "ChildArm_Spacing" ) ;
		ChildArm_Offset_YProp = serializedObject.FindProperty ( "ChildArm_Offset_Y" ) ;
		ChildArm_AngleLimitProp = serializedObject.FindProperty ( "ChildArm_AngleLimit" ) ;
		ChildArm_MassProp = serializedObject.FindProperty ( "ChildArm_Mass" ) ;
		ChildArm_L_MeshProp = serializedObject.FindProperty ( "ChildArm_L_Mesh" ) ;
		ChildArm_R_MeshProp = serializedObject.FindProperty ( "ChildArm_R_Mesh" ) ;
		ChildArm_L_MaterialProp = serializedObject.FindProperty ( "ChildArm_L_Material" ) ;
		ChildArm_R_MaterialProp = serializedObject.FindProperty ( "ChildArm_R_Material" ) ;
		
		Wheel_NumProp = serializedObject.FindProperty ( "Wheel_Num" ) ;
		Wheel_SpacingProp = serializedObject.FindProperty ( "Wheel_Spacing" ) ;
		Wheel_Offset_YProp = serializedObject.FindProperty ( "Wheel_Offset_Y" ) ;
		Wheel_MassProp = serializedObject.FindProperty ( "Wheel_Mass" ) ;
		Wheel_RadiusProp = serializedObject.FindProperty ( "Wheel_Radius" ) ;
		Wheel_Collider_MaterialProp = serializedObject.FindProperty ( "Wheel_Collider_Material" ) ;
		Wheel_MeshProp = serializedObject.FindProperty ( "Wheel_Mesh" ) ;
		Wheel_MaterialProp = serializedObject.FindProperty ( "Wheel_Material" ) ;
		Wheel_Collider_MeshProp = serializedObject.FindProperty ( "Wheel_Collider_Mesh" ) ;

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

		// 'Parent Arm' settings
		EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "'Parent Arm' settings", MessageType.None, true );
		EditorGUILayout.Slider ( DistanceProp , 0.1f , 10.0f , "Distance" ) ;
		EditorGUILayout.Slider ( SpringProp , 0.0f , 1000000.0f , "Spring Force" ) ;
		EditorGUILayout.IntSlider ( ParentArm_NumProp , 0 , 3 , "Number" ) ;
		EditorGUILayout.Slider ( ParentArm_SpacingProp , 0.1f , 10.0f , "Spacing" ) ;
		EditorGUILayout.Slider ( ParentArm_Offset_YProp , -1.0f , 1.0f , "Anchor Offset" ) ;
		EditorGUILayout.Slider ( ParentArm_AngleLimitProp , 0.0f , 90.0f , "Limit Angle" ) ;
		EditorGUILayout.Slider ( ParentArm_MassProp , 0.1f , 300.0f , "Mass" ) ;
		ParentArm_L_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh of Left" , ParentArm_L_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
		ParentArm_R_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh of Right" , ParentArm_R_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
		ParentArm_L_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Material of Left" , ParentArm_L_MaterialProp.objectReferenceValue , typeof ( Material ) , false ) ;
		ParentArm_R_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Material of Right" , ParentArm_R_MaterialProp.objectReferenceValue , typeof ( Material ) , false ) ;

		// 'Child Arm' settings
		EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "'Child Arm' settings", MessageType.None, true );
		EditorGUILayout.IntSlider ( ChildArm_NumProp , 0 , 3 , "Number" ) ;
		EditorGUILayout.Slider ( ChildArm_SpacingProp , 0.1f , 10.0f , "Spacing" ) ;
		EditorGUILayout.Slider ( ChildArm_Offset_YProp , -1.0f , 1.0f , "Anchor Offset" ) ;
		EditorGUILayout.Slider ( ChildArm_AngleLimitProp , 0.0f , 90.0f , "Limit Angle" ) ;
		EditorGUILayout.Slider ( ChildArm_MassProp , 0.1f , 300.0f , "Mass" ) ;
		ChildArm_L_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh of Left" , ChildArm_L_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
		ChildArm_R_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh of Right" , ChildArm_R_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
		ChildArm_L_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Material of Left" , ChildArm_L_MaterialProp.objectReferenceValue , typeof ( Material ) , false ) ;
		ChildArm_R_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Material of Right" , ChildArm_R_MaterialProp.objectReferenceValue , typeof ( Material ) , false ) ;

		// Wheels settings
		EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Wheels settings", MessageType.None, true );
		EditorGUILayout.IntSlider ( Wheel_NumProp , 0 , 3 , "Number" ) ;
		EditorGUILayout.Slider ( Wheel_SpacingProp , 0.1f , 10.0f , "Spacing" ) ;
		EditorGUILayout.Slider ( Wheel_Offset_YProp , -1.0f , 1.0f , "Anchor Offset" ) ;
		EditorGUILayout.Slider ( Wheel_MassProp , 0.1f , 300.0f , "Mass" ) ;
		GUI.backgroundColor = new Color ( 1.0f , 0.5f , 0.5f , 1.0f ) ;
		EditorGUILayout.Slider ( Wheel_RadiusProp , 0.01f , 1.0f , "SphereCollider Radius" ) ;
		GUI.backgroundColor = new Color ( 1.0f , 1.0f , 0.5f , 1.0f ) ;
		Wheel_Collider_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Physic Material" , Wheel_Collider_MaterialProp.objectReferenceValue , typeof ( PhysicMaterial ) , false ) ;
		Wheel_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh" , Wheel_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
		Wheel_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Material" , Wheel_MaterialProp.objectReferenceValue , typeof ( Material ) , false ) ;
		if ( Fit_ST_FlagProp.boolValue == false ) { // for Physics Tracks
			Wheel_Collider_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh Collider" , Wheel_Collider_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
		}
		// Scripts settings
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Scripts settings", MessageType.None, true );
		// Drive Wheel
		Drive_WheelProp.boolValue = EditorGUILayout.Toggle ( "Drive Wheel" , Drive_WheelProp.boolValue ) ;
		EditorGUILayout.Space () ;
		// Wheel Resize
		if ( Fit_ST_FlagProp.boolValue == false ) { // for Physics Tracks
			Wheel_ResizeProp.boolValue = EditorGUILayout.Toggle ( "Wheel Resize Script" , Wheel_ResizeProp.boolValue ) ;
			if ( Wheel_ResizeProp.boolValue ) {
				EditorGUILayout.Slider ( ScaleDown_SizeProp , 0.1f , 3.0f , "Scale Size" ) ;
				EditorGUILayout.Slider ( Return_SpeedProp , 0.01f , 0.1f , "Return Speed" ) ;
			}
			EditorGUILayout.Space () ;
		}
		// Durability
		if ( Fit_ST_FlagProp.boolValue == false ) { // for Physics Tracks
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
		// Create Arm & Wheels
		for ( int i = 0 ;  i  < ParentArm_NumProp.intValue ; i++ ) {
			Count_Parent = i ;
			ParentArmPos_X = 0.0f ;		
			ParentArmPos_Z = -ParentArm_SpacingProp.floatValue * i ;
			
			ParentArmPos_Y = -DistanceProp.floatValue / 2.0f ;
			Set_ParentArm ( "R" ) ;
			
			ParentArmPos_Y = DistanceProp.floatValue / 2.0f ;
			Set_ParentArm ( "L" ) ;
			
			for ( int j = 0 ; j < ChildArm_NumProp.intValue ; j++ ) {
				Count_Child = j ;
				ChildArmPos_X = ChildArm_Offset_YProp.floatValue ;
				ChildArmPos_Z = ( ChildArm_NumProp.intValue - 1 ) * ChildArm_SpacingProp.floatValue / 2.0f ;
				ChildArmPos_Z += ParentArmPos_Z - ( ChildArm_SpacingProp.floatValue * j ) ;
				
				ChildArmPos_Y = -DistanceProp.floatValue / 2.0f ;
				Set_ChildArm ( "R" ) ;
				
				ChildArmPos_Y = DistanceProp.floatValue / 2.0f ;
				Set_ChildArm ( "L" ) ;
				
				for ( int k = 0 ; k < Wheel_NumProp.intValue ; k++ ) {
					Count_Wheel = k ;
					WheelPos_X = ChildArmPos_X + Wheel_Offset_YProp.floatValue ;
					WheelPos_Z = ( Wheel_NumProp.intValue - 1 ) * Wheel_SpacingProp.floatValue / 2.0f ;
					WheelPos_Z += ChildArmPos_Z - ( Wheel_SpacingProp.floatValue * k ) ;
					
					WheelPos_Y = -DistanceProp.floatValue / 2.0f ;
					Set_Wheel ( "R" ) ;
					
					WheelPos_Y = DistanceProp.floatValue / 2.0f ;
					Set_Wheel ( "L" ) ;
				}
			}
		}
	}
	
	void Set_ParentArm ( string Direction ) {
		// Create ParentArm GameObject
		GameObject Temp_Object = new GameObject ( "ParentArm_" + Direction + "_" + ( Count_Parent + 1 ) ) ;
		Temp_Object.transform.parent = Parent_Transform ;
		Temp_Object.transform.localPosition = new Vector3 ( ParentArmPos_X , ParentArmPos_Y , ParentArmPos_Z ) ;
		Temp_Object.transform.localRotation = Quaternion.Euler ( 0.0f , 0.0f , -90.0f ) ;
		// Mesh
		Temp_Object.AddComponent < MeshRenderer > () ;
		MeshFilter Temp_MeshFilter ;
		Temp_MeshFilter = Temp_Object.AddComponent < MeshFilter > () ;
		if ( Direction == "R" ) {
			Temp_MeshFilter.mesh = ParentArm_R_MeshProp.objectReferenceValue as Mesh ;
			Temp_Object.GetComponent<Renderer>().material = ParentArm_R_MaterialProp.objectReferenceValue as Material ;
		} else {
			Temp_MeshFilter.mesh = ParentArm_L_MeshProp.objectReferenceValue as Mesh ;
			Temp_Object.GetComponent<Renderer>().material = ParentArm_L_MaterialProp.objectReferenceValue as Material ;
		}
		// Rigidbody
		Temp_Object.AddComponent < Rigidbody > () ;
		Temp_Object.GetComponent<Rigidbody>().mass = ParentArm_MassProp.floatValue ;
		// Reinforce SphereCollider
		SphereCollider Temp_SphereCollider ;
		Temp_SphereCollider = Temp_Object.AddComponent < SphereCollider > () ;
		Temp_SphereCollider.radius = 0.45f ;
		// ConfigurableJoint
		ConfigurableJoint Temp_Joint ;
		Temp_Joint = Temp_Object.AddComponent < ConfigurableJoint > () ;
		Temp_Joint.connectedBody = Parent_Transform.parent.gameObject.GetComponent<Rigidbody>() ;
		Temp_Joint.anchor = new Vector3 ( 0.0f , ParentArm_Offset_YProp.floatValue , 0.0f ) ;
		Temp_Joint.axis = Vector3.zero ;
		Temp_Joint.secondaryAxis = Vector3.zero ;
		Temp_Joint.xMotion = ConfigurableJointMotion.Locked ;
		Temp_Joint.yMotion = ConfigurableJointMotion.Limited ;
		Temp_Joint.zMotion = ConfigurableJointMotion.Locked ;
		Temp_Joint.angularXMotion = ConfigurableJointMotion.Limited ;
		Temp_Joint.angularYMotion = ConfigurableJointMotion.Locked ;
		Temp_Joint.angularZMotion = ConfigurableJointMotion.Locked ;
		SoftJointLimit Temp_SoftJointLimit = Temp_Joint.linearLimit ; // Set Linear Limit
		Temp_SoftJointLimit.limit = 0.1f ;
		Temp_Joint.linearLimit = Temp_SoftJointLimit ;
		Temp_SoftJointLimit = Temp_Joint.lowAngularXLimit ; // Set Low Angular XLimit
		Temp_SoftJointLimit.limit = -ParentArm_AngleLimitProp.floatValue ;
		Temp_Joint.lowAngularXLimit = Temp_SoftJointLimit ;
		Temp_SoftJointLimit = Temp_Joint.highAngularXLimit ; // Set High Angular XLimit
		Temp_SoftJointLimit.limit = ParentArm_AngleLimitProp.floatValue ;
		Temp_Joint.highAngularXLimit = Temp_SoftJointLimit ;
		JointDrive Temp_JointDrive = Temp_Joint.yDrive ; // Set Vertical Spring.
		Temp_JointDrive.mode = JointDriveMode.Position ;
		Temp_JointDrive.positionSpring = SpringProp.floatValue ;
		Temp_Joint.yDrive = Temp_JointDrive ;
		// Set Layer
		Temp_Object.layer = 10 ; // Ignore All
	}
	
	void Set_ChildArm ( string Direction ) {
		// Create ChildArm GameObject
		GameObject Temp_Object = new GameObject ( "ChildArm_" + Direction + "_" + ( ( Count_Parent * ChildArm_NumProp.intValue ) + ( Count_Child + 1 ) ) ) ;
		Temp_Object.transform.parent = Parent_Transform ;
		Temp_Object.transform.localPosition = new Vector3 ( ChildArmPos_X , ChildArmPos_Y , ChildArmPos_Z ) ;
		Temp_Object.transform.localRotation = Quaternion.Euler ( 0.0f , 0.0f , -90.0f ) ;
		// Add components
		Temp_Object.AddComponent < MeshRenderer > () ;
		MeshFilter Temp_MeshFilter ;
		Temp_MeshFilter = Temp_Object.AddComponent < MeshFilter > () ;
		if ( Direction == "R" ) {
			Temp_MeshFilter.mesh = ChildArm_R_MeshProp.objectReferenceValue as Mesh ;
			Temp_Object.GetComponent<Renderer>().material = ChildArm_R_MaterialProp.objectReferenceValue as Material ;
		} else {
			Temp_MeshFilter.mesh = ChildArm_L_MeshProp.objectReferenceValue as Mesh ;
			Temp_Object.GetComponent<Renderer>().material = ChildArm_L_MaterialProp.objectReferenceValue as Material ;
		}
		// Rigidbody
		Temp_Object.AddComponent < Rigidbody > () ;
		Temp_Object.GetComponent<Rigidbody>().mass = ChildArm_MassProp.floatValue ;
		// Reinforce SphereCollider
		SphereCollider Temp_SphereCollider ;
		Temp_SphereCollider = Temp_Object.AddComponent < SphereCollider > () ;
		Temp_SphereCollider.radius = 0.3f ;
		// HingeJoint
		HingeJoint Temp_Joint ;
		Temp_Joint = Temp_Object.AddComponent < HingeJoint > () ;
		Temp_Joint.connectedBody = Parent_Transform.Find ( "ParentArm_" + Direction + "_" + ( Count_Parent + 1 ) ).gameObject.GetComponent<Rigidbody>() ;
		Temp_Joint.anchor = new Vector3 ( 0.0f , ChildArm_Offset_YProp.floatValue , 0.0f ) ;
		Temp_Joint.axis = new Vector3 ( 1.0f , 0.0f , 0.0f ) ;
		Temp_Joint.useLimits = true ;
		JointLimits Temp_JointLimits = Temp_Joint.limits ;
		Temp_JointLimits.max = ChildArm_AngleLimitProp.floatValue ;
		Temp_JointLimits.min = -ChildArm_AngleLimitProp.floatValue ;
		Temp_Joint.limits = Temp_JointLimits ;
		// Set Layer
		Temp_Object.layer = 10 ; // Ignore All
	}
	
	void Set_Wheel ( string Direction ) {
		// Create RoadWheel GameObject
		GameObject Temp_Object = new GameObject ( "RoadWheel_" + Direction + "_" + ( ( Count_Parent * ChildArm_NumProp.intValue * Wheel_NumProp.intValue ) + ( Count_Child * Wheel_NumProp.intValue ) + ( Count_Wheel + 1 ) ) ) ;
		Temp_Object.transform.parent = Parent_Transform ;
		Temp_Object.transform.localPosition = new Vector3 ( WheelPos_X , WheelPos_Y , WheelPos_Z ) ;
		if ( Direction == "R" ) {
			Temp_Object.transform.localRotation = Quaternion.Euler ( 0.0f , 0.0f , 180.0f ) ;
		} else {
			Temp_Object.transform.localRotation = Quaternion.identity ;
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
		// SphereCollider
		SphereCollider Temp_SphereCollider ;
		Temp_SphereCollider = Temp_Object.AddComponent < SphereCollider > () ;
		Temp_SphereCollider.radius = Wheel_RadiusProp.floatValue ;
		Temp_SphereCollider.material = Wheel_Collider_MaterialProp.objectReferenceValue as PhysicMaterial ;
		// MeshCollider
		if ( Fit_ST_FlagProp.boolValue == false ) { // for Physics Tracks
			if ( Wheel_Collider_MeshProp.objectReferenceValue != null ) {
				MeshCollider Temp_MeshCollider ;
				Temp_MeshCollider = Temp_Object.AddComponent < MeshCollider > () ;
				Temp_MeshCollider.sharedMesh = Wheel_Collider_MeshProp.objectReferenceValue as Mesh ;
				Temp_MeshCollider.material = Wheel_Collider_MaterialProp.objectReferenceValue as PhysicMaterial ;
				Temp_MeshCollider.convex = true ;
				Temp_MeshCollider.enabled = false ;
			}
		}
		// HingeJoint
		HingeJoint Temp_Joint ;
		Temp_Joint = Temp_Object.AddComponent < HingeJoint > () ;
		Temp_Joint.anchor = Vector3.zero ;
		Temp_Joint.axis = new Vector3 ( 0.0f , 1.0f , 0.0f ) ;
		Temp_Joint.connectedBody = Parent_Transform.Find ( "ChildArm_" + Direction + "_" + ( ( Count_Parent * ChildArm_NumProp.intValue ) + ( Count_Child + 1 ) ) ).gameObject.GetComponent<Rigidbody>() ;
		// Drive_Wheel_CS
		Drive_Wheel_CS Temp_Drive_Wheel_CS ;
		Temp_Drive_Wheel_CS = Temp_Object.AddComponent < Drive_Wheel_CS > () ;
		Temp_Drive_Wheel_CS.Radius = Wheel_RadiusProp.floatValue ;
		Temp_Drive_Wheel_CS.Drive_Flag = Drive_WheelProp.boolValue ;
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
		// Set direction
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