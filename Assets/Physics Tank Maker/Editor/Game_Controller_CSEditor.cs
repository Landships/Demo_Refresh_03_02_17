/*using UnityEngine;
using System.Collections;
using UnityEditor ;

[ CustomEditor ( typeof ( Game_Controller_CS ) ) ]

public class Game_Controller_CSEditor : Editor {

	SerializedProperty Assign_FrequencyProp ;

	SerializedProperty Time_ScaleProp ;
	SerializedProperty GravityProp ;
	SerializedProperty Fixed_TimeStepProp ;

	void OnEnable () {
		Assign_FrequencyProp = serializedObject.FindProperty ( "Assign_Frequency" ) ;

		Time_ScaleProp = serializedObject.FindProperty ( "Time_Scale" ) ;
		GravityProp = serializedObject.FindProperty ( "Gravity" ) ;
		Fixed_TimeStepProp = serializedObject.FindProperty ( "Fixed_TimeStep" ) ;
	}
	
	public override void OnInspectorGUI () {
		if ( EditorApplication.isPlaying == false ) {
			GUI.backgroundColor = new Color ( 1.0f , 1.0f , 0.5f , 1.0f ) ;
			serializedObject.Update () ;

			// Scene settings.
			EditorGUILayout.Space () ; EditorGUILayout.Space () ;
			EditorGUILayout.HelpBox( "Scene settings", MessageType.None, true );
			EditorGUILayout.Slider ( Assign_FrequencyProp , 0.1f , 10.0f , "Assign Target Interval" ) ;

			// Time Manager and Gravity settings.
			EditorGUILayout.Space () ; EditorGUILayout.Space () ;
			EditorGUILayout.HelpBox( "Time Manager and Gravity settings", MessageType.None, true );
			EditorGUILayout.Slider ( Time_ScaleProp , 0.1f , 5.0f , "Time Scale" ) ;
			EditorGUILayout.Slider ( GravityProp , -9.81f , -1.0f , "Gravity" ) ;
			EditorGUILayout.Slider ( Fixed_TimeStepProp , 0.01f , 0.05f , "Fixed TimeStep" ) ;

			EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		
			serializedObject.ApplyModifiedProperties ();
		}
	}
	
}*/