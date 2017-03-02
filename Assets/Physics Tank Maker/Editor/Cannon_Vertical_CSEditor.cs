using UnityEngine;
using System.Collections;
using UnityEditor ;
/*
[ CustomEditor ( typeof ( Cannon_Vertical_CS ) ) ]

public class Cannon_Vertical_CSEditor : Editor {
	
	SerializedProperty Max_ElevationProp ;
	SerializedProperty Max_DepressionProp ;
	SerializedProperty Speed_MagProp ;	
	SerializedProperty Buffer_AngleProp ;
	SerializedProperty Auto_Angle_FlagProp ;
	SerializedProperty Upper_CourseProp ;
	SerializedProperty AI_ReferenceProp ;

	void  OnEnable () {
		Max_ElevationProp = serializedObject.FindProperty ( "Max_Elevation" ) ;
		Max_DepressionProp = serializedObject.FindProperty ( "Max_Depression" ) ;
		Speed_MagProp = serializedObject.FindProperty ( "Speed_Mag" ) ;
		Buffer_AngleProp = serializedObject.FindProperty ( "Buffer_Angle" ) ;
		Auto_Angle_FlagProp = serializedObject.FindProperty ( "Auto_Angle_Flag" ) ;
		Upper_CourseProp = serializedObject.FindProperty ( "Upper_Course" ) ;
		AI_ReferenceProp = serializedObject.FindProperty ( "AI_Reference" ) ;
	}
	
	public override void  OnInspectorGUI (){
		GUI.backgroundColor = new Color ( 1.0f , 1.0f , 0.5f , 1.0f ) ;
		serializedObject.Update () ;
		
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Cannon Elevation settings", MessageType.None, true );
		if ( EditorApplication.isPlaying == false ) {
			EditorGUILayout.Slider ( Max_ElevationProp , 0.0f , 180.0f , "Max Angle" ) ;
			EditorGUILayout.Slider ( Max_DepressionProp , 0.0f , 180.0f , "Min Angle" ) ;
		}
		EditorGUILayout.Slider ( Speed_MagProp , 1.0f , 360.0f , "Speed" ) ;
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;

		EditorGUILayout.HelpBox( "Following options are reflected only in 'Mouse + KeyBoard operation'.", MessageType.None, true ) ;
		EditorGUILayout.Slider ( Buffer_AngleProp , 0.0f , 180.0f , "Angle of Buffer" ) ;
		Auto_Angle_FlagProp.boolValue = EditorGUILayout.Toggle ( "Auto Angle" , Auto_Angle_FlagProp.boolValue ) ;
		if ( Auto_Angle_FlagProp.boolValue ) {
			Upper_CourseProp.boolValue = EditorGUILayout.Toggle ( "Upper Course" , Upper_CourseProp.boolValue ) ;
		}

		EditorGUILayout.HelpBox( "Following option is reflected only in AI tank.", MessageType.None, true ) ;
		AI_ReferenceProp.boolValue = EditorGUILayout.Toggle ( "Referred to from AI" , AI_ReferenceProp.boolValue ) ;
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		
		serializedObject.ApplyModifiedProperties ();
	}
	
}*/
