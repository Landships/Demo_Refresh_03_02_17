using UnityEngine;
using System.Collections;
using UnityEditor ;

[ CustomEditor ( typeof ( Recoil_Brake_CS ) ) ]

public class Recoil_Brake_CSEditor : Editor {
	
	SerializedProperty Recoil_TimeProp ;
	SerializedProperty Return_TimeProp ;
	SerializedProperty Recoil_LengthProp ;
	
	void OnEnable () {
		Recoil_TimeProp = serializedObject.FindProperty ( "Recoil_Time" ) ;
		Return_TimeProp = serializedObject.FindProperty ( "Return_Time" ) ;
		Recoil_LengthProp = serializedObject.FindProperty ( "Recoil_Length" ) ;
	}

	public override void OnInspectorGUI () {
		GUI.backgroundColor = new Color ( 1.0f , 1.0f , 0.5f , 1.0f ) ;
		serializedObject.Update () ;
		
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Recoil Brake settings", MessageType.None, true );
		EditorGUILayout.Slider ( Recoil_TimeProp , 0.01f , 2.0f , "Recoil Time" ) ;
		EditorGUILayout.Slider ( Return_TimeProp , 0.01f , 4.0f , "Return Time" ) ;
		EditorGUILayout.Slider ( Recoil_LengthProp , 0.0f , 2.0f , "Length" ) ;
		
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		
		serializedObject.ApplyModifiedProperties ();
	}
	
}