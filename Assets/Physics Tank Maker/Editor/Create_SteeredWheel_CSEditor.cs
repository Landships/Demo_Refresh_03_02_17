using UnityEngine;
using System.Collections;
using UnityEditor ;

[ CustomEditor ( typeof ( Create_SteeredWheel_CS ) ) ]

public class Create_SteeredWheel_CSEditor : Editor {
	
	SerializedProperty Shaft_MassProp ;
	SerializedProperty Shaft_MeshProp ;
	SerializedProperty Shaft_MaterialProp ;
	SerializedProperty Shaft_Collider_RadiusProp ;

	SerializedProperty Sus_Vertical_RangeProp ;
	SerializedProperty Sus_Vertical_SpringProp ;
	SerializedProperty Sus_Vertical_DamperProp ;
	SerializedProperty Sus_Torsion_RangeProp ;
	SerializedProperty Sus_Torsion_SpringProp ;
	SerializedProperty Sus_Torsion_DamperProp ;
	SerializedProperty Sus_Anchor_Offset_YProp ;
	SerializedProperty Sus_Anchor_Offset_ZProp ;
	
	SerializedProperty Hub_DistanceProp ;
	SerializedProperty Hub_Offset_YProp ;
	SerializedProperty Hub_Offset_ZProp ;
	SerializedProperty Hub_MassProp ;
	SerializedProperty Hub_SpringProp ;
	SerializedProperty Hub_Mesh_LProp ;
	SerializedProperty Hub_Mesh_RProp ;
	SerializedProperty Hub_Material_LProp ;
	SerializedProperty Hub_Material_RProp ;
	SerializedProperty Hub_Anchor_Offset_XProp ;
	SerializedProperty Hub_Anchor_Offset_ZProp ;
	SerializedProperty Hub_Collider_RadiusProp ;

	SerializedProperty Wheel_DistanceProp ;
	SerializedProperty Wheel_RadiusProp ;
	SerializedProperty Wheel_Collider_MaterialProp ;
	SerializedProperty Wheel_Offset_YProp ;
	SerializedProperty Wheel_Offset_ZProp ;
	SerializedProperty Wheel_MassProp ;
	SerializedProperty Wheel_MeshProp ;
	SerializedProperty Wheel_MaterialProp ;
	SerializedProperty Wheel_Collider_MeshProp ;

	SerializedProperty Steer_FlagProp ;
	SerializedProperty Reverse_FlagProp ;
	SerializedProperty Max_AngleProp ;
	SerializedProperty Rotation_SpeedProp ;
	
	SerializedProperty Drive_WheelProp ;
	SerializedProperty Wheel_DurabilityProp ;

	SerializedProperty Parent_TransformProp ;
	
	Transform Parent_Transform ;
	float Pos_X ;
	float Pos_Y ;
	float Pos_Z ;
	
	void OnEnable () {
		Shaft_MassProp = serializedObject.FindProperty ( "Shaft_Mass" ) ;
		Shaft_MeshProp = serializedObject.FindProperty ( "Shaft_Mesh" ) ;
		Shaft_MaterialProp = serializedObject.FindProperty ( "Shaft_Material" ) ;
		Shaft_Collider_RadiusProp = serializedObject.FindProperty ( "Shaft_Collider_Radius" ) ;

		Sus_Vertical_RangeProp = serializedObject.FindProperty ( "Sus_Vertical_Range" ) ;
		Sus_Vertical_SpringProp = serializedObject.FindProperty ( "Sus_Vertical_Spring" ) ;
		Sus_Vertical_DamperProp = serializedObject.FindProperty ( "Sus_Vertical_Damper" ) ;
		Sus_Torsion_RangeProp = serializedObject.FindProperty ( "Sus_Torsion_Range" ) ;
		Sus_Torsion_SpringProp = serializedObject.FindProperty ( "Sus_Torsion_Spring" ) ;
		Sus_Torsion_DamperProp = serializedObject.FindProperty ( "Sus_Torsion_Damper" ) ;
		Sus_Anchor_Offset_YProp = serializedObject.FindProperty ( "Sus_Anchor_Offset_Y" ) ;
		Sus_Anchor_Offset_ZProp = serializedObject.FindProperty ( "Sus_Anchor_Offset_Z" ) ;
		
		Hub_DistanceProp = serializedObject.FindProperty ( "Hub_Distance" ) ;
		Hub_Offset_YProp = serializedObject.FindProperty ( "Hub_Offset_Y" ) ;
		Hub_Offset_ZProp = serializedObject.FindProperty ( "Hub_Offset_Z" ) ;
		Hub_MassProp = serializedObject.FindProperty ( "Hub_Mass" ) ;
		Hub_SpringProp = serializedObject.FindProperty ( "Hub_Spring" ) ;
		Hub_Mesh_LProp = serializedObject.FindProperty ( "Hub_Mesh_L" ) ;
		Hub_Mesh_RProp = serializedObject.FindProperty ( "Hub_Mesh_R" ) ;
		Hub_Material_LProp = serializedObject.FindProperty ( "Hub_Material_L" ) ;
		Hub_Material_RProp = serializedObject.FindProperty ( "Hub_Material_R" ) ;
		Hub_Anchor_Offset_XProp = serializedObject.FindProperty ( "Hub_Anchor_Offset_X" ) ;
		Hub_Anchor_Offset_ZProp = serializedObject.FindProperty ( "Hub_Anchor_Offset_Z" ) ;
		Hub_Collider_RadiusProp = serializedObject.FindProperty ( "Hub_Collider_Radius" ) ;

		Wheel_DistanceProp = serializedObject.FindProperty ( "Wheel_Distance" ) ;
		Wheel_RadiusProp = serializedObject.FindProperty ( "Wheel_Radius" ) ;
		Wheel_Collider_MaterialProp = serializedObject.FindProperty ( "Wheel_Collider_Material" ) ;
		Wheel_MassProp = serializedObject.FindProperty ( "Wheel_Mass" ) ;
		Wheel_MeshProp = serializedObject.FindProperty ( "Wheel_Mesh" ) ;
		Wheel_MaterialProp = serializedObject.FindProperty ( "Wheel_Material" ) ;
		Wheel_Collider_MeshProp = serializedObject.FindProperty ( "Wheel_Collider_Mesh" ) ;
		Wheel_Offset_YProp = serializedObject.FindProperty ( "Wheel_Offset_Y" ) ;
		Wheel_Offset_ZProp = serializedObject.FindProperty ( "Wheel_Offset_Z" ) ;

		Steer_FlagProp = serializedObject.FindProperty ( "Steer_Flag" ) ;
		Reverse_FlagProp = serializedObject.FindProperty ( "Reverse_Flag" ) ;
		Max_AngleProp = serializedObject.FindProperty ( "Max_Angle" ) ;
		Rotation_SpeedProp = serializedObject.FindProperty ( "Rotation_Speed" ) ;
		
		Drive_WheelProp = serializedObject.FindProperty ( "Drive_Wheel" ) ;
		Wheel_DurabilityProp = serializedObject.FindProperty ( "Wheel_Durability" ) ;
		
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
			if ( GUI.changed ) {
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

		// Axle Shaft settings
		EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Axle Shaft settings", MessageType.None, true );
		EditorGUILayout.Slider ( Shaft_MassProp , 1.0f , 3000.0f , "Mass" ) ;
		Shaft_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh" , Shaft_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
		Shaft_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Material" , Shaft_MaterialProp.objectReferenceValue , typeof ( Material ) , false ) ;
		EditorGUILayout.Slider ( Shaft_Collider_RadiusProp , 0.1f , 3.0f , "SphereCollider Radius" ) ;

		// Suspension settings
		EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Suspension settings", MessageType.None, true );
		EditorGUILayout.Slider ( Sus_Vertical_RangeProp , 0.0f , 1.0f , "Vertical Range" ) ;
		EditorGUILayout.Slider ( Sus_Vertical_SpringProp , 0.0f , 1000000.0f , "Vertical Spring Force" ) ;
		EditorGUILayout.Slider ( Sus_Vertical_DamperProp , 0.0f , 1000000.0f , "Vertical Damper Force" ) ;
		EditorGUILayout.Slider ( Sus_Torsion_RangeProp , 0.0f , 90.0f , "Torsion Range" ) ;
		EditorGUILayout.Slider ( Sus_Torsion_SpringProp , 0.0f , 1000000.0f , "Torsion Spring Force" ) ;
		EditorGUILayout.Slider ( Sus_Torsion_DamperProp , 0.0f , 1000000.0f , "Torsion Damper Force" ) ;
		EditorGUILayout.Slider ( Sus_Anchor_Offset_YProp , -10.0f , 10.0f , "Anchor Offset Y" ) ;
		EditorGUILayout.Slider ( Sus_Anchor_Offset_ZProp , -10.0f , 10.0f , "Anchor Offset Z" ) ;

		// Hubs settings
		EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Hubs settings", MessageType.None, true );
		EditorGUILayout.Slider ( Hub_DistanceProp , 0.1f , 10.0f , "Distance" ) ;
		EditorGUILayout.Slider ( Hub_Offset_YProp , -10.0f , 10.0f , "Position Offset Y" ) ;
		EditorGUILayout.Slider ( Hub_Offset_ZProp , -10.0f , 10.0f , "Position Offset Z" ) ;
		EditorGUILayout.Slider ( Hub_MassProp , 1.0f , 3000.0f , "Mass" ) ;
		EditorGUILayout.Slider ( Hub_SpringProp , 0.0f , 100000.0f , "Fixing Force" ) ;
		Hub_Mesh_LProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh of Left" , Hub_Mesh_LProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
		Hub_Mesh_RProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh of Right" , Hub_Mesh_RProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
		Hub_Material_LProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Material of Left" , Hub_Material_LProp.objectReferenceValue , typeof ( Material ) , false ) ;
		Hub_Material_RProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Material of Right" , Hub_Material_RProp.objectReferenceValue , typeof ( Material ) , false ) ;
		EditorGUILayout.Slider ( Hub_Anchor_Offset_XProp , -10.0f , 10.0f , "Anchor Offset X" ) ;
		EditorGUILayout.Slider ( Hub_Anchor_Offset_ZProp , -10.0f , 10.0f , "Anchor Offset Z" ) ;
		EditorGUILayout.Slider ( Hub_Collider_RadiusProp , 0.1f , 1.0f , "SphereCollider Radius" ) ;

		// Wheels settings
		EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Wheels settings", MessageType.None, true );
		EditorGUILayout.Slider ( Wheel_DistanceProp , 0.1f , 10.0f , "Distance" ) ;
		GUI.backgroundColor = new Color ( 1.0f , 0.5f , 0.5f , 1.0f ) ;
		EditorGUILayout.Slider ( Wheel_RadiusProp , 0.01f , 1.0f , "SphereCollider Radius" ) ;
		GUI.backgroundColor = new Color ( 1.0f , 1.0f , 0.5f , 1.0f ) ;
		Wheel_Collider_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Physic Material" , Wheel_Collider_MaterialProp.objectReferenceValue , typeof ( PhysicMaterial ) , false ) ;
		EditorGUILayout.Slider ( Wheel_Offset_YProp , -10.0f , 10.0f , "Position Offset Y" ) ;
		EditorGUILayout.Slider ( Wheel_Offset_ZProp , -10.0f , 10.0f , "Position Offset Z" ) ;
		EditorGUILayout.Slider ( Wheel_MassProp , 1.0f , 3000.0f , "Mass" ) ;
		Wheel_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh" , Wheel_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
		Wheel_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Material" , Wheel_MaterialProp.objectReferenceValue , typeof ( Material ) , false ) ;

		// Mesh Collider
		EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "'MeshCollider settings", MessageType.None, true );
		Wheel_Collider_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh Collider" , Wheel_Collider_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;

		// Scripts settings
		EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Scripts settings", MessageType.None, true );
		// Steering
		Steer_FlagProp.boolValue = EditorGUILayout.Toggle ( "Steer" , Steer_FlagProp.boolValue ) ;
		if ( Steer_FlagProp.boolValue ) {
			Reverse_FlagProp.boolValue = EditorGUILayout.Toggle ( "Reverse" , Reverse_FlagProp.boolValue ) ;
			EditorGUILayout.Slider ( Max_AngleProp , 0.0f , 180.0f , "Max Steering Angle" ) ;
			EditorGUILayout.Slider ( Rotation_SpeedProp , 1.0f , 90.0f , "Steering Speed" ) ;
		}
		EditorGUILayout.Space () ;
		// Drive Wheel
		Drive_WheelProp.boolValue = EditorGUILayout.Toggle ( "Drive Wheel" , Drive_WheelProp.boolValue ) ;
		EditorGUILayout.Space () ;
		// Durability
		EditorGUILayout.Slider ( Wheel_DurabilityProp , 1 , 1000000 , "Wheel Durability" ) ;
		if ( Wheel_DurabilityProp.floatValue >= 1000000 ) {
			Wheel_DurabilityProp.floatValue = Mathf.Infinity ;
		}

		// Update Value
		EditorGUILayout.Space () ;
		if ( GUILayout.Button ( "Update Value" ) ) {
			Create () ;
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
		// Create Axle Shaft
		Set_ShaftValue () ;
		// Create Hubs
		Pos_X = Hub_DistanceProp.floatValue / 2.0f ;
		Pos_Y = Hub_Offset_YProp.floatValue ;
		Pos_Z = Hub_Offset_ZProp.floatValue ;
		Set_HubValue ( "R" ) ;
		Pos_X = -Hub_DistanceProp.floatValue / 2.0f ;
		Set_HubValue ( "L" ) ;
		// Create Wheels
		Pos_X = ( Wheel_DistanceProp.floatValue - Hub_DistanceProp.floatValue ) / 2.0f ;
		Pos_Y = Wheel_Offset_YProp.floatValue ;
		Pos_Z = Wheel_Offset_ZProp.floatValue ;
		Set_WheelValue ( "R" ) ;
		Pos_X = -( Wheel_DistanceProp.floatValue - Hub_DistanceProp.floatValue ) / 2.0f ;
		Set_WheelValue ( "L" ) ;
	}
	
	void Set_ShaftValue () {
		//Create gameobject & Set transform
		GameObject Temp_Object = new GameObject ( "Axle_Shaft" ) ;
		Temp_Object.transform.parent = Parent_Transform ;
		Temp_Object.transform.localPosition = Vector3.zero ;
		Temp_Object.transform.localRotation = Quaternion.identity ;
		// Mesh
		Temp_Object.AddComponent < MeshRenderer > () ;
		MeshFilter Temp_MeshFilter ;
		Temp_MeshFilter = Temp_Object.AddComponent < MeshFilter > () ;
		Temp_MeshFilter.mesh = Shaft_MeshProp.objectReferenceValue as Mesh ;
		Temp_Object.GetComponent<Renderer>().material = Shaft_MaterialProp.objectReferenceValue as Material ;
		// Rigidbody
		Temp_Object.AddComponent < Rigidbody > () ;
		Temp_Object.GetComponent<Rigidbody>().mass = Shaft_MassProp.floatValue ;
		// SphereCollider
		SphereCollider Temp_SphereCollider ;
		Temp_SphereCollider = Temp_Object.AddComponent < SphereCollider > () ;
		Temp_SphereCollider.radius = Shaft_Collider_RadiusProp.floatValue ;
		// ConfigurableJoint
		ConfigurableJoint Temp_Joint ;
		Temp_Joint = Temp_Object.AddComponent < ConfigurableJoint > () ;
		Temp_Joint.connectedBody = Parent_Transform.parent.GetComponent<Rigidbody>() ;
		Temp_Joint.anchor = new Vector3 ( 0.0f , Sus_Anchor_Offset_YProp.floatValue , Sus_Anchor_Offset_ZProp.floatValue ) ;
		Temp_Joint.axis = Vector3.zero ;
		Temp_Joint.secondaryAxis = Vector3.zero ;
		Temp_Joint.xMotion = ConfigurableJointMotion.Locked ;
		Temp_Joint.yMotion = ConfigurableJointMotion.Limited ;
		Temp_Joint.zMotion = ConfigurableJointMotion.Locked ;
		Temp_Joint.angularXMotion = ConfigurableJointMotion.Locked ;
		Temp_Joint.angularYMotion = ConfigurableJointMotion.Locked ;
		Temp_Joint.angularZMotion = ConfigurableJointMotion.Limited ;
		//
		SoftJointLimit Temp_SoftJointLimit = Temp_Joint.linearLimit ; // Set Vertical Range
		Temp_SoftJointLimit.limit = Sus_Vertical_RangeProp.floatValue ;
		Temp_Joint.linearLimit = Temp_SoftJointLimit ;
		//
		JointDrive Temp_JointDrive = Temp_Joint.yDrive ; // Set Vertical Spring.
		Temp_JointDrive.mode = JointDriveMode.Position ;
		Temp_JointDrive.positionSpring = Sus_Vertical_SpringProp.floatValue ;
		Temp_JointDrive.positionDamper = Sus_Vertical_DamperProp.floatValue ;
		Temp_Joint.yDrive = Temp_JointDrive ;
		//
		Temp_SoftJointLimit = Temp_Joint.angularZLimit ; // Set Torsion Range
		Temp_SoftJointLimit.limit = Sus_Torsion_RangeProp.floatValue ;
		Temp_Joint.angularZLimit = Temp_SoftJointLimit ;
		//
		Temp_JointDrive = Temp_Joint.angularYZDrive ; // Set Torsion Spring.
		Temp_JointDrive.mode = JointDriveMode.Position ;
		Temp_JointDrive.positionSpring = Sus_Torsion_SpringProp.floatValue ;
		Temp_JointDrive.positionDamper = Sus_Torsion_DamperProp.floatValue ;
		Temp_Joint.angularYZDrive = Temp_JointDrive ;
		// Set Layer
		Temp_Object.layer = 10 ; // Ignore All
	}
	
	void Set_HubValue ( string Direction ){
		//Create gameobject & Set transform
		GameObject Temp_Object = new GameObject ( "Hub_" + Direction ) ;
		Temp_Object.transform.parent = Parent_Transform ;
		Temp_Object.transform.localPosition = new Vector3 ( Pos_X , Pos_Y , Pos_Z ) ;
		Temp_Object.transform.localRotation = Quaternion.identity ;
		// Mesh
		Temp_Object.AddComponent < MeshRenderer > () ;
		MeshFilter Temp_MeshFilter ;
		Temp_MeshFilter = Temp_Object.AddComponent < MeshFilter > () ;
		if ( Direction == "R" ) {
			Temp_MeshFilter.mesh = Hub_Mesh_RProp.objectReferenceValue as Mesh ;
			Temp_Object.GetComponent<Renderer>().material = Hub_Material_RProp.objectReferenceValue as Material ;
		} else {
			Temp_MeshFilter.mesh = Hub_Mesh_LProp.objectReferenceValue as Mesh ;
			Temp_Object.GetComponent<Renderer>().material = Hub_Material_LProp.objectReferenceValue as Material ;
		}
		// Rigidbody
		Temp_Object.AddComponent < Rigidbody > () ;
		Temp_Object.GetComponent<Rigidbody>().mass = Hub_MassProp.floatValue ;
		// HingeJoint
		HingeJoint Temp_Joint ;
		Temp_Joint = Temp_Object.AddComponent < HingeJoint > () ;
		Temp_Joint.connectedBody = Parent_Transform.Find ( "Axle_Shaft" ).GetComponent<Rigidbody>() ;
		if ( Direction == "R" ) {
			Temp_Joint.anchor = new Vector3 ( Hub_Anchor_Offset_XProp.floatValue , 0.0f , Hub_Anchor_Offset_ZProp.floatValue ) ;
		} else {
			Temp_Joint.anchor = new Vector3 ( -Hub_Anchor_Offset_XProp.floatValue , 0.0f , Hub_Anchor_Offset_ZProp.floatValue ) ;
		}
		Temp_Joint.axis = new Vector3 ( 0.0f , 1.0f , 0.0f ) ;
		Temp_Joint.useSpring = true ;
		JointSpring Temp_JointSpring = Temp_Joint.spring ;
		Temp_JointSpring.spring = Hub_SpringProp.floatValue ;
		Temp_Joint.spring = Temp_JointSpring ;
		// SphereCollider
		SphereCollider Temp_SphereCollider ;
		Temp_SphereCollider = Temp_Object.AddComponent < SphereCollider > () ;
		Temp_SphereCollider.radius = Hub_Collider_RadiusProp.floatValue ;
		// Steer_Wheel_CS
		if ( Steer_FlagProp.boolValue ) {
			Steer_Wheel_CS Temp_Steer_Wheel_CS ;
			Temp_Steer_Wheel_CS = Temp_Object.AddComponent < Steer_Wheel_CS > () ;
			if ( Reverse_FlagProp.boolValue ) {
				Temp_Steer_Wheel_CS.Reverse = -1.0f ;
			} else {
				Temp_Steer_Wheel_CS.Reverse = 1.0f ;
			}
			Temp_Steer_Wheel_CS.Max_Angle = Max_AngleProp.floatValue ;
			Temp_Steer_Wheel_CS.Rotation_Speed = Rotation_SpeedProp.floatValue ;
		}
		// Set Layer
		Temp_Object.layer = 10 ; // Ignore All
	}
	
	void Set_WheelValue (  string Direction   ){
		// Create gameobject & Set transform
		// Create 'Parent_of_Wheel'
		GameObject Temp_Parent_Object = new GameObject ( "Parent_of_Wheel_" + Direction ) ;
		Temp_Parent_Object.transform.parent = Parent_Transform.Find ( "Hub_" + Direction ) ;
		Temp_Parent_Object.transform.localPosition = new Vector3 ( Pos_X , Pos_Y , Pos_Z ) ;
		Temp_Parent_Object.transform.localRotation = Quaternion.Euler ( 0 , 0 , 90 ) ;
		// Create Wheel
		GameObject Temp_Object = new GameObject ( "SteeredWheel_" + Direction ) ;
		Temp_Object.transform.parent = Temp_Parent_Object.transform ;
		Temp_Object.transform.localPosition = Vector3.zero ;
		if ( Direction == "R" ) {
			Temp_Object.transform.localRotation = Quaternion.Euler ( 0 , 0 , 180 ) ;
		} else {
			Temp_Object.transform.localRotation = Quaternion.Euler ( 0 , 0 , 0 ) ;
		}
		// Mesh
		Temp_Object.AddComponent < MeshRenderer > () ;
		MeshFilter Temp_MeshFilter ;
		Temp_MeshFilter = Temp_Object.AddComponent < MeshFilter > () ;
		Temp_MeshFilter.mesh = Wheel_MeshProp.objectReferenceValue as Mesh ;
		Temp_Object.GetComponent<Renderer>().material = Wheel_MaterialProp.objectReferenceValue as Material ;
		// Rigidbody
		Temp_Object.AddComponent < Rigidbody > () ;
		Temp_Object.GetComponent<Rigidbody>().mass = Wheel_MassProp.floatValue ;
		// SphereCollider
		SphereCollider Temp_SphereCollider ;
		Temp_SphereCollider = Temp_Object.AddComponent < SphereCollider > () ;
		Temp_SphereCollider.radius = Wheel_RadiusProp.floatValue ;
		Temp_SphereCollider.material = Wheel_Collider_MaterialProp.objectReferenceValue as PhysicMaterial ;
		// MeshCollider
		MeshCollider Temp_MeshCollider ;
		Temp_MeshCollider = Temp_Object.AddComponent < MeshCollider > () ;
		Temp_MeshCollider.sharedMesh = Wheel_Collider_MeshProp.objectReferenceValue as Mesh ;
		Temp_MeshCollider.material = Wheel_Collider_MaterialProp.objectReferenceValue as PhysicMaterial ;
		Temp_MeshCollider.convex = true ;
		Temp_MeshCollider.enabled = false ;
		// HingeJoint
		HingeJoint Temp_Joint ;
		Temp_Joint = Temp_Object.AddComponent < HingeJoint > () ;
		Temp_Joint.connectedBody = Temp_Parent_Object.transform.parent.GetComponent<Rigidbody>() ;
		Temp_Joint.anchor = Vector3.zero ;
		Temp_Joint.axis = new Vector3 ( 0.0f , 1.0f , 0.0f ) ;
		// Drive_Wheel_CS
		Drive_Wheel_CS Temp_Drive_Wheel_CS ;
		Temp_Drive_Wheel_CS = Temp_Object.AddComponent < Drive_Wheel_CS > () ;
		Temp_Drive_Wheel_CS.Radius = Wheel_RadiusProp.floatValue ;
		Temp_Drive_Wheel_CS.Drive_Flag = Drive_WheelProp.boolValue ;
		// Damage_Control_CS
		Damage_Control_CS Temp_Damage_Control_CS ;
		Temp_Damage_Control_CS = Temp_Object.AddComponent < Damage_Control_CS > () ;
		Temp_Damage_Control_CS.Type = 8 ; // 8 = Wheel in "Damage_Control"
		Temp_Damage_Control_CS.Durability = Wheel_DurabilityProp.floatValue ;
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
