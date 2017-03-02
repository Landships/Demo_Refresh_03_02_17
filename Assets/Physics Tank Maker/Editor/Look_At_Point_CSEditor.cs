using UnityEngine;
using System.Collections;
using UnityEditor ;

[ CustomEditor ( typeof ( Look_At_Point_CS ) ) ]

public class Look_At_Point_CSEditor : Editor {
	
	SerializedProperty Offset_XProp ;
	SerializedProperty Offset_YProp ;
	SerializedProperty Offset_ZProp ;
	SerializedProperty Horizontal_SpeedProp ;
	SerializedProperty Vertical_SpeedProp ;
	SerializedProperty Invert_FlagProp ;
	
	void  OnEnable (){
		Offset_XProp = serializedObject.FindProperty ( "Offset_X" ) ;
		Offset_YProp = serializedObject.FindProperty ( "Offset_Y" ) ;
		Offset_ZProp = serializedObject.FindProperty ( "Offset_Z" ) ;
		Horizontal_SpeedProp = serializedObject.FindProperty ( "Horizontal_Speed" ) ;
		Vertical_SpeedProp = serializedObject.FindProperty ( "Vertical_Speed" ) ;
		Invert_FlagProp = serializedObject.FindProperty ( "Invert_Flag" ) ;
	}
	
	public override void OnInspectorGUI () {
		GUI.backgroundColor = new Color ( 1.0f , 1.0f , 0.5f , 1.0f ) ;
		serializedObject.Update () ;

		if ( EditorApplication.isPlaying == false ) {
			EditorGUILayout.Space () ; EditorGUILayout.Space () ;
			EditorGUILayout.HelpBox( "Set the offset position for third person view.", MessageType.None, true ) ;
			EditorGUILayout.Slider ( Offset_XProp , -10.0f , 10.0f , "Offset X" ) ;
			EditorGUILayout.Slider ( Offset_YProp , -10.0f , 10.0f , "Offset Y" ) ;
			EditorGUILayout.Slider ( Offset_ZProp , -10.0f , 10.0f , "Offset Z" ) ;
		}
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Rotation speed settings.", MessageType.None, true ) ;
		EditorGUILayout.Slider ( Horizontal_SpeedProp , 0.1f , 10.0f , "Horizontal Speed" ) ;
		EditorGUILayout.Slider ( Vertical_SpeedProp , 0.1f , 10.0f , "Vertical Speed" ) ;
		Invert_FlagProp.boolValue = EditorGUILayout.Toggle ( "Invert Camera Vertical" , Invert_FlagProp.boolValue ) ;
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;

		serializedObject.ApplyModifiedProperties () ;
	}
}