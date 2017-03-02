using UnityEngine;
using System.Collections;
using UnityEditor ;

using UnityEngine.UI;

[ CustomEditor ( typeof ( Tank_ID_Control_CS ) ) ]

public class Tank_ID_Control_CSEditor : Editor {
	/*
	SerializedProperty Tank_IDProp ;
	SerializedProperty RelationshipProp ;
	SerializedProperty ReSpawn_TimesProp ;
	SerializedProperty Attack_MultiplierProp ;
	SerializedProperty Input_TypeProp ;
	SerializedProperty Turn_TypeProp ;

	SerializedProperty WayPoint_PackProp ;
	SerializedProperty Patrol_TypeProp ;
	SerializedProperty Follow_TargetProp ;

	SerializedProperty No_AttackProp ;
	SerializedProperty Visibility_RadiusProp ;
	SerializedProperty Approach_DistanceProp ;
	SerializedProperty OpenFire_DistanceProp ;
	SerializedProperty Lost_CountProp ;
	SerializedProperty Face_EnemyProp ;
	SerializedProperty Face_Offest_AngleProp ;

	SerializedProperty AI_State_TextProp ;
	SerializedProperty Tank_NameProp ;

	SerializedProperty ReSpawn_IntervalProp ;
	SerializedProperty Remove_TimeProp ;

	SerializedProperty ReSpawn_FlagProp ;
	SerializedProperty Prefab_PathProp ;

	string[] ID_Names = { "Not Operable" , "[ 1 ]" , "[ 2 ]" , "[ 3 ]" , "[ 4 ]" , "[ 5 ]" , "[ 6 ]" , "[ 7 ]" , "[ 8 ]" , "[ 9 ]" , "[10]" } ;
	string[] Relationship_Names = { "Friendly" , "Hostile" } ;
	string[] Input_Type_Names = { "Keyboard ( Keyboard only )" , "GamePad ( Stick operation )" , "GamePad ( Trigger operation )" ,  "GamePad ( Stick+Trigger operation )" , "Mouse + Keyboard ( Default )" , "Mouse + Keyboard ( Easy )" , "" , "" , "" , "" , "AI" } ;
	string[] Turn_Type_Names = { "Easy Turn (Pivot Turn)" , "Classic Turn (only Brake-Turn)" } ;

	string[] Patrol_Type_Names = { "Order" , "Random" } ;

	void OnEnable () {
		Tank_IDProp = serializedObject.FindProperty ( "Tank_ID" ) ;
		RelationshipProp = serializedObject.FindProperty ( "Relationship" ) ;
		ReSpawn_TimesProp = serializedObject.FindProperty ( "ReSpawn_Times" ) ;
		Attack_MultiplierProp = serializedObject.FindProperty ( "Attack_Multiplier" ) ;
		Input_TypeProp = serializedObject.FindProperty ( "Input_Type" ) ;
		Turn_TypeProp = serializedObject.FindProperty ( "Turn_Type" ) ;

		WayPoint_PackProp = serializedObject.FindProperty ( "WayPoint_Pack" ) ;
		Patrol_TypeProp = serializedObject.FindProperty ( "Patrol_Type" ) ;
		Follow_TargetProp = serializedObject.FindProperty ( "Follow_Target" ) ;

		No_AttackProp = serializedObject.FindProperty ( "No_Attack" ) ;
		Visibility_RadiusProp = serializedObject.FindProperty ( "Visibility_Radius" ) ;
		Approach_DistanceProp = serializedObject.FindProperty ( "Approach_Distance" ) ;
		OpenFire_DistanceProp = serializedObject.FindProperty ( "OpenFire_Distance" ) ;
		Lost_CountProp = serializedObject.FindProperty ( "Lost_Count" ) ;
		Face_EnemyProp = serializedObject.FindProperty ( "Face_Enemy" ) ;
		Face_Offest_AngleProp = serializedObject.FindProperty ( "Face_Offest_Angle" ) ;

		AI_State_TextProp = serializedObject.FindProperty ( "AI_State_Text" ) ;
		Tank_NameProp = serializedObject.FindProperty ( "Tank_Name" ) ;

		ReSpawn_IntervalProp = serializedObject.FindProperty ( "ReSpawn_Interval" ) ;
		Remove_TimeProp = serializedObject.FindProperty ( "Remove_Time" ) ;

		ReSpawn_FlagProp = serializedObject.FindProperty ( "ReSpawn_Flag" ) ;
		Prefab_PathProp = serializedObject.FindProperty ( "Prefab_Path" ) ;
	}
	
	public override void OnInspectorGUI () {
		if ( EditorApplication.isPlaying == false ) {
			GUI.backgroundColor = new Color ( 1.0f , 1.0f , 0.5f , 1.0f ) ;
			serializedObject.Update () ;

			// Set Prefab_Path and ReSpawn_Flag.
			Get_Prefab_Path () ;

			EditorGUILayout.Space () ; EditorGUILayout.Space () ;	
			EditorGUILayout.HelpBox( "Basic settings.", MessageType.None, true );
			Tank_IDProp.intValue = EditorGUILayout.Popup ( "Tank ID" , Tank_IDProp.intValue , ID_Names ) ;
			EditorGUILayout.Space () ;
			RelationshipProp.intValue = EditorGUILayout.Popup ( "Relationship" , RelationshipProp.intValue , Relationship_Names ) ;

			EditorGUILayout.Space () ;
			GUI.backgroundColor = new Color ( 1.0f , 0.5f , 0.5f , 1.0f ) ;
			EditorGUILayout.IntSlider ( ReSpawn_TimesProp , 0 , 100 , "ReSpawn Times" ) ;
			GUI.backgroundColor = new Color ( 1.0f , 1.0f , 0.5f , 1.0f ) ;
			EditorGUILayout.Slider ( Attack_MultiplierProp , 0.1f , 2.0f , "Attack Multiplier" ) ;

			EditorGUILayout.Space () ;
			EditorGUILayout.HelpBox( "'Input Device Type' is ignored when this tank has AI.", MessageType.Warning, true );
			if ( Input_TypeProp.intValue < 4 ) {
				EditorGUILayout.HelpBox( "Have you finished setting up 'Input Manager' ?", MessageType.Warning , true );
				EditorGUILayout.Space () ;
			}
			Input_TypeProp.intValue = EditorGUILayout.Popup ( "Input Device Type" , Input_TypeProp.intValue , Input_Type_Names ) ;
			
			if ( Input_TypeProp.intValue == 0 || Input_TypeProp.intValue == 1 || Input_TypeProp.intValue == 5 ) {
				Turn_TypeProp.intValue = EditorGUILayout.Popup ( "Turn Type" , Turn_TypeProp.intValue , Turn_Type_Names ) ;
			}

			EditorGUILayout.Space () ; EditorGUILayout.Space () ;

			// for AI.
			if ( Input_TypeProp.intValue == 10 ) {
				EditorGUILayout.HelpBox( "AI Patrol Settings", MessageType.None, true );
				WayPoint_PackProp.objectReferenceValue = EditorGUILayout.ObjectField ( "WayPoint Pack" , WayPoint_PackProp.objectReferenceValue , typeof ( GameObject ) , true ) ;
				Patrol_TypeProp.intValue = EditorGUILayout.Popup ( "Patrol Type" , Patrol_TypeProp.intValue , Patrol_Type_Names ) ;
				Follow_TargetProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Follow Target" , Follow_TargetProp.objectReferenceValue , typeof ( Transform ) , true ) ;
				EditorGUILayout.Space () ;
				EditorGUILayout.HelpBox( "AI Combat Settings", MessageType.None, true );
				No_AttackProp.boolValue = EditorGUILayout.Toggle ( "No Attack" , No_AttackProp.boolValue ) ;
				EditorGUILayout.Slider ( Visibility_RadiusProp , 0.1f , 10000.0f , "Visibility Radius" ) ;
				EditorGUILayout.Slider ( Approach_DistanceProp , 1.0f , 10000.0f , "Approach Distance" ) ;
				if ( Approach_DistanceProp.floatValue == 10000.0f ) {
					Approach_DistanceProp.floatValue = Mathf.Infinity ;
				}
				EditorGUILayout.Slider ( OpenFire_DistanceProp , 1.0f , 10000.0f , "Open Fire Distance" ) ;
				if ( OpenFire_DistanceProp.floatValue == 10000.0f ) {
					OpenFire_DistanceProp.floatValue = Mathf.Infinity ;
				}
				EditorGUILayout.Slider ( Lost_CountProp , 0.0f , 100.0f , "Lost Count" ) ;
				Face_EnemyProp.boolValue = EditorGUILayout.Toggle ( "Face the Enemy" , Face_EnemyProp.boolValue ) ;
				if ( Face_EnemyProp.boolValue ) {
					EditorGUILayout.Slider ( Face_Offest_AngleProp , 0.0f , 90.0f , "Face Offest Angle" ) ;
				}
				EditorGUILayout.Space () ;
				EditorGUILayout.HelpBox( "AI State Text Settings", MessageType.None, true );
				AI_State_TextProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Text" , AI_State_TextProp.objectReferenceValue , typeof ( Text ) , true ) ;
				Tank_NameProp.stringValue = EditorGUILayout.TextField ( "Tank Name" , Tank_NameProp.stringValue ) ;
				EditorGUILayout.Space () ;
				EditorGUILayout.HelpBox( "Auto ReSpawn Settings", MessageType.None, true );
				EditorGUILayout.Slider ( ReSpawn_IntervalProp , 1.0f , 100.0f , "Interval Time" ) ;
				EditorGUILayout.Slider ( Remove_TimeProp , 10.0f , 120.0f , "Remove Time" ) ;
				if ( Remove_TimeProp.floatValue >= 120.0f ) {
					Remove_TimeProp.floatValue = Mathf.Infinity ;
				}
			}

			EditorGUILayout.Space () ; EditorGUILayout.Space () ;	
			if ( ReSpawn_FlagProp.boolValue ) {
				EditorGUILayout.HelpBox( "Path of the prefab in 'Resources' folder." , MessageType.None , true ) ;
				EditorGUILayout.HelpBox( Prefab_PathProp.stringValue , MessageType.None, true ) ;
			} else {
				EditorGUILayout.HelpBox( "Tank prefab must be placed under 'Resources' folder." , MessageType.Error , true ) ;
			}

			EditorGUILayout.Space () ; EditorGUILayout.Space () ;

			serializedObject.ApplyModifiedProperties () ;
		}
	}

	void Get_Prefab_Path () {
		Object Temp_Object = PrefabUtility.GetPrefabParent ( Selection.activeGameObject ) ;
		if ( Temp_Object ) {
			string Temp_Path = AssetDatabase.GetAssetPath ( Temp_Object ) ;
			if ( !string.IsNullOrEmpty ( Temp_Path ) ) {
				int Index = Temp_Path.IndexOf ( "Resources/" ) ;
				if ( Index < 0 ) { // This prefab is not placed in 'Resources' folder.
					ReSpawn_FlagProp.boolValue = false ;
					Prefab_PathProp.stringValue = null ;
				} else {
					Temp_Path = Temp_Path.Substring ( Index + 10 ) ;
					Index = Temp_Path.IndexOf ( ".prefab" ) ;
					if ( Index < 0 ) {
						ReSpawn_FlagProp.boolValue = false ;
						Prefab_PathProp.stringValue = null ;
					} else {
						ReSpawn_FlagProp.boolValue = true ;
						Temp_Path = Temp_Path.Substring ( 0 , Temp_Path.Length - 7 ) ;
						Prefab_PathProp.stringValue = Temp_Path ;
					}
				}
			}
		}
	}*/
}
