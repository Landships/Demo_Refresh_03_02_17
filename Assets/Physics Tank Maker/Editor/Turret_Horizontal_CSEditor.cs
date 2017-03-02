using UnityEngine;
using System.Collections;
using UnityEditor ;

[ CustomEditor ( typeof ( Turret_Horizontal_CS ) ) ]

public class Turret_Horizontal_CSEditor : Editor {
	
	SerializedProperty Limit_FlagProp ;
	SerializedProperty Max_RightProp ;
	SerializedProperty Max_LeftProp ;
	SerializedProperty Speed_MagProp ;
	SerializedProperty Buffer_AngleProp ;
	SerializedProperty Acceleration_TimeProp ;
	SerializedProperty Deceleration_TimeProp ;
	SerializedProperty OpenFire_AngleProp ;
	SerializedProperty Marker_PrefabProp ;

	void OnEnable () {
		Limit_FlagProp = serializedObject.FindProperty ( "Limit_Flag" ) ;
		Max_RightProp = serializedObject.FindProperty ( "Max_Right" ) ;
		Max_LeftProp = serializedObject.FindProperty ( "Max_Left" ) ;
		Speed_MagProp = serializedObject.FindProperty ( "Speed_Mag" ) ;
		Buffer_AngleProp = serializedObject.FindProperty ( "Buffer_Angle" ) ;
		Acceleration_TimeProp = serializedObject.FindProperty ( "Acceleration_Time" ) ;
		Deceleration_TimeProp = serializedObject.FindProperty ( "Deceleration_Time" ) ;
		OpenFire_AngleProp = serializedObject.FindProperty ( "OpenFire_Angle" ) ;
		Marker_PrefabProp = serializedObject.FindProperty ( "Marker_Prefab" ) ;
	}
	
	public override void OnInspectorGUI () {
		GUI.backgroundColor = new Color ( 1.0f , 1.0f , 0.5f , 1.0f ) ;
		serializedObject.Update () ;
		
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Turret Rotation settings", MessageType.None, true ) ;
		if ( EditorApplication.isPlaying == false ) {
			Limit_FlagProp.boolValue = EditorGUILayout.Toggle ( "Limit" , Limit_FlagProp.boolValue ) ;
			if ( Limit_FlagProp.boolValue ) {
				EditorGUILayout.Slider ( Max_RightProp , 0.0f , 180.0f , "Max Rigth Angle" ) ;
				EditorGUILayout.Slider ( Max_LeftProp , 0.0f , 180.0f , "Max Left Angle" ) ;
			}
		}
		EditorGUILayout.Slider ( Speed_MagProp , 1.0f , 360.0f , "Speed" ) ;
		EditorGUILayout.Slider ( Buffer_AngleProp , 0.0f , 180.0f , "Buffer Angle" ) ;
		EditorGUILayout.Slider ( Acceleration_TimeProp , 0.01f , 5.0f , "Acceleration Time" ) ;
		EditorGUILayout.Slider ( Deceleration_TimeProp , 0.01f , 5.0f , "Deceleration Time" ) ;

		EditorGUILayout.Space () ;
		EditorGUILayout.Slider ( OpenFire_AngleProp , 1.0f , 180.0f , "Open Fire Angle" ) ;

		EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Marker settings", MessageType.None, true ) ;
		Marker_PrefabProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Marker Prefab" , Marker_PrefabProp.objectReferenceValue , typeof ( GameObject ) , true ) ;
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		
		serializedObject.ApplyModifiedProperties ();
	}
	
}