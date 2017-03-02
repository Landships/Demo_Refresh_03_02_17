using UnityEngine;
using System.Collections;
using UnityEditor ;

[ CustomEditor ( typeof ( Create_SupportWheel_CS ) ) ]

public class Create_SupportWheel_CSEditor : Editor {
	
	SerializedProperty Wheel_DistanceProp ;
	SerializedProperty NumProp ;
	SerializedProperty SpacingProp ;
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
	SerializedProperty Static_FlagProp ;
	SerializedProperty Radius_OffsetProp ;

	SerializedProperty RealTime_FlagProp ;
	SerializedProperty Parent_TransformProp ;
	
	Transform Parent_Transform ;
	
	void OnEnable () {
		Wheel_DistanceProp = serializedObject.FindProperty ( "Wheel_Distance" ) ;
		NumProp = serializedObject.FindProperty ( "Num" ) ;
		SpacingProp = serializedObject.FindProperty ( "Spacing" ) ;
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
		Static_FlagProp = serializedObject.FindProperty ( "Static_Flag" ) ;
		Radius_OffsetProp = serializedObject.FindProperty ( "Radius_Offset" ) ;

		RealTime_FlagProp = serializedObject.FindProperty ( "RealTime_Flag" ) ;
		Parent_TransformProp = serializedObject.FindProperty ( "Parent_Transform" ) ;
		
		Parent_Transform = Parent_TransformProp.objectReferenceValue as Transform ;
	}
	
	public override void OnInspectorGUI () {
		bool Work_Flag ;
		if ( Parent_Transform.parent == null || Parent_Transform.parent.gameObject.GetComponent < Rigidbody > () == null ) {
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
		Static_FlagProp.boolValue = EditorGUILayout.Toggle ( "Static Wheel" , Static_FlagProp.boolValue ) ;
		if ( Static_FlagProp.boolValue ) {
			EditorGUILayout.Slider ( Radius_OffsetProp , -0.5f , 0.5f , "Radius Offset" ) ;
		}
		// Wheels settings
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Wheels settings", MessageType.None, true );
		EditorGUILayout.Slider ( Wheel_DistanceProp , 0.1f , 10.0f , "Distance" ) ;
		EditorGUILayout.IntSlider ( NumProp , 0 , 30 , "Number" ) ;
		EditorGUILayout.Slider ( SpacingProp , 0.1f , 10.0f , "Spacing" ) ;
		if ( !Static_FlagProp.boolValue ) { // Physics Wheel.
			EditorGUILayout.Slider ( Wheel_MassProp , 0.1f , 300.0f , "Mass" ) ;
		}
		EditorGUILayout.Space () ;
		GUI.backgroundColor = new Color ( 1.0f , 0.5f , 0.5f , 1.0f ) ;
		EditorGUILayout.Slider ( Wheel_RadiusProp , 0.01f , 1.0f , "SphereCollider Radius" ) ;
		GUI.backgroundColor = new Color ( 1.0f , 1.0f , 0.5f , 1.0f ) ;
		Collider_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Physic Material" , Collider_MaterialProp.objectReferenceValue , typeof ( PhysicMaterial ) , false ) ;
		EditorGUILayout.Space () ;
		Wheel_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh" , Wheel_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
		Wheel_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Material" , Wheel_MaterialProp.objectReferenceValue , typeof ( Material ) , false ) ;

		if ( !Static_FlagProp.boolValue ) {
			EditorGUILayout.Space () ; EditorGUILayout.Space () ;
			// Mesh Collider
			EditorGUILayout.HelpBox( "MeshCollider settings", MessageType.None, true );
			Collider_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh Collider" , Collider_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
			Collider_Mesh_SubProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh Sub Collider" , Collider_Mesh_SubProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
			// Scripts settings
			EditorGUILayout.Space () ; EditorGUILayout.Space () ;
			EditorGUILayout.HelpBox( "Scripts settings", MessageType.None, true );
			// Drive Wheel
			Drive_WheelProp.boolValue = EditorGUILayout.Toggle ( "Drive Wheel" , Drive_WheelProp.boolValue ) ;
			EditorGUILayout.Space () ;
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
		// Create Wheel	
		float Temp_X ;
		float Temp_Y ;
		float Temp_Z ;
		for ( int i = 0 ;  i  < NumProp.intValue ; i++ ) {
			Temp_X = 0.0f ;
			Temp_Y = Wheel_DistanceProp.floatValue / 2.0f ;
			Temp_Z = -SpacingProp.floatValue * i ;
			Create_Wheel ( "R" , new Vector3 ( Temp_X , -Temp_Y , Temp_Z ) , i + 1 ) ;
			Create_Wheel ( "L" , new Vector3 ( Temp_X , Temp_Y , Temp_Z ) , i + 1 ) ;
		}
	}
	
	void Create_Wheel ( string Temp_Direction , Vector3 Temp_Pos , int Temp_Count ) {
		//Create_Gameobject
		GameObject Temp_Object = new GameObject ( "SupportWheel_" + Temp_Direction + "_" + Temp_Count ) ;
		Temp_Object.transform.parent = Parent_Transform ;
		Temp_Object.transform.localPosition = Temp_Pos ;
		if ( Temp_Direction == "R" ) {
			Temp_Object.transform.localRotation = Quaternion.Euler ( 0.0f , 0.0f , 180.0f ) ;
		} else {
			Temp_Object.transform.localRotation = Quaternion.Euler ( Vector3.zero ) ;
		}
		Temp_Object.layer = 9 ; // Ignore eachother.
		// Mesh
		MeshRenderer Temp_MeshRenderer = Temp_Object.AddComponent < MeshRenderer > () ;
		Temp_MeshRenderer.material = Wheel_MaterialProp.objectReferenceValue as Material ;
		MeshFilter Temp_MeshFilter ;
		Temp_MeshFilter = Temp_Object.AddComponent < MeshFilter > () ;
		Temp_MeshFilter.mesh = Wheel_MeshProp.objectReferenceValue as Mesh ;
		// SphereCollider
		SphereCollider Temp_SphereCollider ;
		Temp_SphereCollider = Temp_Object.AddComponent < SphereCollider > () ;
		Temp_SphereCollider.radius = Wheel_RadiusProp.floatValue ;
		Temp_SphereCollider.material = Collider_MaterialProp.objectReferenceValue as PhysicMaterial ;

		if ( Static_FlagProp.boolValue == false ) { // for Physics Wheel.
			// MeshCollider
			if  ( Collider_MeshProp.objectReferenceValue != null ) {
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
			// Rigidbody
			Rigidbody Temp_Rigidbody = Temp_Object.AddComponent < Rigidbody > () ;
			Temp_Rigidbody.mass = Wheel_MassProp.floatValue ;
			// HingeJoint
			HingeJoint Temp_HingeJoint ;
			Temp_HingeJoint = Temp_Object.AddComponent < HingeJoint > () ;
			Temp_HingeJoint.anchor = Vector3.zero ;
			Temp_HingeJoint.axis = new Vector3 ( 0.0f , 1.0f , 0.0f ) ;
			Temp_HingeJoint.connectedBody = Parent_Transform.parent.gameObject.GetComponent < Rigidbody > () ;
			// Drive_Wheel_CS
			Drive_Wheel_CS Temp_Drive_Wheel_CS ;
			Temp_Drive_Wheel_CS = Temp_Object.AddComponent < Drive_Wheel_CS > () ;
			Temp_Drive_Wheel_CS.Radius = Wheel_RadiusProp.floatValue ;
			Temp_Drive_Wheel_CS.Drive_Flag = Drive_WheelProp.boolValue ;
			// Wheel_Resize_CS
			if ( Wheel_ResizeProp.boolValue ) {
				Wheel_Resize_CS Temp_Wheel_Resize_CS ;
				Temp_Wheel_Resize_CS = Temp_Object.AddComponent < Wheel_Resize_CS > () ;
				Temp_Wheel_Resize_CS.ScaleDown_Size = ScaleDown_SizeProp.floatValue ;
				Temp_Wheel_Resize_CS.Return_Speed = Return_SpeedProp.floatValue ;
			}
			// Stabilizer_CS
			Temp_Object.AddComponent < Stabilizer_CS > () ;
		} else { // for Static Wheel
			Static_Wheel_CS Temp_Script = Temp_Object.AddComponent < Static_Wheel_CS > () ;
			Temp_Script.Radius_Offset = Radius_OffsetProp.floatValue ;
		}
		// Damage_Control_CS
		Damage_Control_CS Temp_Damage_Control_CS ;
		Temp_Damage_Control_CS = Temp_Object.AddComponent < Damage_Control_CS > () ;
		Temp_Damage_Control_CS.Type = 8 ; // 8 = Wheel in "Damage_Control"
		if ( Static_FlagProp.boolValue ) {
			Temp_Damage_Control_CS.Durability = Mathf.Infinity ;
		} else {
			Temp_Damage_Control_CS.Durability = Wheel_DurabilityProp.floatValue ;
		} 
		if ( Temp_Direction == "L" ) {
			Temp_Damage_Control_CS.Direction = 0 ;
		} else {
			Temp_Damage_Control_CS.Direction = 1 ;
		}
	}
}