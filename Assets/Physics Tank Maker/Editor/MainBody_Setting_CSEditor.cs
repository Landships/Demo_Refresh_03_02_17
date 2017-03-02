using UnityEngine;
using System.Collections;
using UnityEditor ;

[ CustomEditor ( typeof ( MainBody_Setting_CS ) ) ]

public class MainBody_Setting_CSEditor : Editor {
	
	SerializedProperty Body_MassProp ;
	SerializedProperty Body_MeshProp ;

	SerializedProperty Materials_NumProp ;
	SerializedProperty MaterialsProp ;
	SerializedProperty Body_MaterialProp ;

	SerializedProperty Collider_MeshProp ;
	SerializedProperty Sub_Collider_MeshProp ;
	SerializedProperty DurabilityProp ;
	SerializedProperty Turret_NumberProp ;

	SerializedProperty SICProp ;
	SerializedProperty Soft_Landing_FlagProp ;
	SerializedProperty Landing_DragProp ;
	SerializedProperty Landing_TimeProp ;

	SerializedProperty AI_Upper_OffsetProp ;
	SerializedProperty AI_Lower_OffsetProp ;

	GameObject MainBody_Object ;
	bool Work_Flag ;
	
	void OnEnable () {
		Body_MassProp = serializedObject.FindProperty ( "Body_Mass" ) ;
		Body_MeshProp = serializedObject.FindProperty ( "Body_Mesh" ) ;

		Materials_NumProp = serializedObject.FindProperty ( "Materials_Num" ) ;
		MaterialsProp = serializedObject.FindProperty ( "Materials" ) ;
		Body_MaterialProp = serializedObject.FindProperty ( "Body_Material" ) ;

		Collider_MeshProp = serializedObject.FindProperty ( "Collider_Mesh" ) ;
		Sub_Collider_MeshProp = serializedObject.FindProperty ( "Sub_Collider_Mesh" ) ;
		DurabilityProp = serializedObject.FindProperty ( "Durability" ) ;
		Turret_NumberProp = serializedObject.FindProperty ( "Turret_Number" ) ;

		SICProp = serializedObject.FindProperty ( "SIC" ) ;
		Soft_Landing_FlagProp = serializedObject.FindProperty ( "Soft_Landing_Flag" ) ;
		Landing_DragProp = serializedObject.FindProperty ( "Landing_Drag" ) ;
		Landing_TimeProp = serializedObject.FindProperty ( "Landing_Time" ) ;

		AI_Upper_OffsetProp = serializedObject.FindProperty ( "AI_Upper_Offset" ) ;
		AI_Lower_OffsetProp = serializedObject.FindProperty ( "AI_Lower_Offset" ) ;

		MainBody_Object = Selection.activeGameObject ;

		//Even if the prefab is selected in "Project view", this script dose not work.
		if ( MainBody_Object.activeInHierarchy ) {
			Work_Flag = true ;
		} else {
			Work_Flag = false ;
		}
	}
	
	public override void OnInspectorGUI () {
		if ( Work_Flag && !EditorApplication.isPlaying ) {
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

		// Basic Settings
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Basic Settings", MessageType.None, true );
		EditorGUILayout.Slider ( Body_MassProp , 1.0f , 100000.0f , "Mass" ) ;
		EditorGUILayout.Space () ;
		Body_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh" , Body_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
		EditorGUILayout.Space () ;
		EditorGUILayout.IntSlider ( Materials_NumProp , 1 , 10 , "Number of Materials" ) ;
		MaterialsProp.arraySize = Materials_NumProp.intValue ;
		if ( Materials_NumProp.intValue == 1 && Body_MaterialProp.objectReferenceValue != null ) {
			if ( MaterialsProp.GetArrayElementAtIndex ( 0 ).objectReferenceValue == null ) {
				MaterialsProp.GetArrayElementAtIndex ( 0 ).objectReferenceValue = Body_MaterialProp.objectReferenceValue ;
			}
			Body_MaterialProp.objectReferenceValue = null ;
		}
		for ( int i = 0 ; i < Materials_NumProp.intValue ; i++ ) {
			MaterialsProp.GetArrayElementAtIndex ( i ).objectReferenceValue = EditorGUILayout.ObjectField ( "Material" , MaterialsProp.GetArrayElementAtIndex ( i ).objectReferenceValue , typeof ( Material ) , false ) ;
		}

		// Collider settings
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Collider settings", MessageType.None, true );
		Collider_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "MeshCollider" , Collider_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
		EditorGUILayout.Space () ;
		Sub_Collider_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Sub MeshCollider" , Sub_Collider_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;

		// Physics settings
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Physics settings", MessageType.None, true );
		EditorGUILayout.IntSlider ( SICProp , 1 , 100 , "Solver Iteration Count" ) ;
		EditorGUILayout.Space () ;
		Soft_Landing_FlagProp.boolValue = EditorGUILayout.Toggle ( "Soft Landing" , Soft_Landing_FlagProp.boolValue ) ;
		if ( Soft_Landing_FlagProp.boolValue ) {
			EditorGUILayout.Slider ( Landing_DragProp , 0.1f , 10000.0f , "Landing Drag" ) ;
			EditorGUILayout.Slider ( Landing_TimeProp , 0.1f , 30.0f , "Landing Time" ) ;
		}

		// Damage settings
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Damage settings", MessageType.None, true );
		// Durability
		EditorGUILayout.Slider ( DurabilityProp , 1.0f , 1000000.0f , "Durability" ) ;
		if ( DurabilityProp.floatValue >= 1000000.0f ) {
			DurabilityProp.floatValue = Mathf.Infinity ;
		}
		EditorGUILayout.Space () ;
		// Number of Turrret(s)
		EditorGUILayout.IntSlider ( Turret_NumberProp , 1 , 10 , "Number of Turrret(s)" ) ;
		EditorGUILayout.Space () ;
		// Offset for AI
		EditorGUILayout.Space () ;
		EditorGUILayout.Slider ( AI_Upper_OffsetProp , 0.0f , 10.0f , "Upper Offset for AI" ) ;
		EditorGUILayout.Slider ( AI_Lower_OffsetProp , 0.0f , 10.0f , "Lower Offset for AI" ) ;

		// Update Value
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		if ( GUILayout.Button ( "Update Value" ) ) {
			Create () ;
		}
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;

		//
		serializedObject.ApplyModifiedProperties ();
	}
	
	void Create () {
		MainBody_Object.GetComponent<Rigidbody>().mass = Body_MassProp.floatValue ;
		MainBody_Object.GetComponent < MeshFilter > ().mesh = Body_MeshProp.objectReferenceValue as Mesh ;

		Material [] Temp_Materials = new Material [ Materials_NumProp.intValue ] ;
		for ( int i = 0 ; i < Temp_Materials.Length ; i++ ) {
			Temp_Materials [ i ] = MaterialsProp.GetArrayElementAtIndex ( i ).objectReferenceValue as Material ;
		}
		MainBody_Object.GetComponent<Renderer>().materials = Temp_Materials ;

		MeshCollider[] Temp_MeshColliders ;
		Temp_MeshColliders = MainBody_Object.GetComponents < MeshCollider > () ;
		MeshCollider Main_MeshCollider ;
		MeshCollider Sub_MeshCollider ;
		switch ( Temp_MeshColliders.Length ) {
		case 0 :
			Main_MeshCollider = MainBody_Object.AddComponent < MeshCollider > () ;
			Sub_MeshCollider = MainBody_Object.AddComponent < MeshCollider > () ;
			break ;
		case 1 :
			Main_MeshCollider = Temp_MeshColliders [0] as MeshCollider ;
			Sub_MeshCollider = MainBody_Object.AddComponent < MeshCollider > () ;
			break ;
		default :
			Main_MeshCollider = Temp_MeshColliders [0] as MeshCollider ;
			Sub_MeshCollider = Temp_MeshColliders [1] as MeshCollider ;
			break ;
		}
		if ( Collider_MeshProp.objectReferenceValue ) {
			Main_MeshCollider.enabled = true ;
			Main_MeshCollider.sharedMesh = Collider_MeshProp.objectReferenceValue as Mesh ;
			Main_MeshCollider.convex = true ;
		} else {
			Main_MeshCollider.enabled = false ;
		}
		if ( Sub_Collider_MeshProp.objectReferenceValue ) {
			Sub_MeshCollider.enabled = true ;
			Sub_MeshCollider.sharedMesh = Sub_Collider_MeshProp.objectReferenceValue as Mesh ;
			Sub_MeshCollider.convex = true ;
		} else {
			Sub_MeshCollider.enabled = false ;
		}
		// Add script
		Damage_Control_CS Temp_Damage_Control_CS = MainBody_Object.GetComponent < Damage_Control_CS > () ;
		if ( Temp_Damage_Control_CS == null ) {
			Temp_Damage_Control_CS = MainBody_Object.AddComponent < Damage_Control_CS > () ;
		}
		Temp_Damage_Control_CS.Type = 5 ;  // 5 = MainBody in "Damage_Control"
		Temp_Damage_Control_CS.Durability = DurabilityProp.floatValue ;
		Temp_Damage_Control_CS.Turret_Number = Turret_NumberProp.intValue ;
		// Set Layer.
		MainBody_Object.layer = 11 ; // Ignore wheels.
	}
	
}