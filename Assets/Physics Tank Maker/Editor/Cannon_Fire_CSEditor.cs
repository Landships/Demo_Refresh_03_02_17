using UnityEngine;
using System.Collections;
using UnityEditor ;

[ CustomEditor ( typeof ( Cannon_Fire_CS ) ) ]

public class Cannon_Fire_CSEditor : Editor {
	
	SerializedProperty Reload_TimeProp ;
	SerializedProperty Recoil_ForceProp ;
	SerializedProperty Karl_FlagProp ;

	void  OnEnable () {
		Reload_TimeProp = serializedObject.FindProperty ( "Reload_Time" ) ;
		Recoil_ForceProp = serializedObject.FindProperty ( "Recoil_Force" ) ;
		Karl_FlagProp = serializedObject.FindProperty ( "Karl_Flag" ) ;
	}
	
	public override void  OnInspectorGUI () {
		GUI.backgroundColor = new Color ( 1.0f , 1.0f , 0.5f , 1.0f ) ;
		serializedObject.Update () ;
		
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Firing settings", MessageType.None, true );
		EditorGUILayout.Slider ( Reload_TimeProp , 0.01f , 60.0f , "Reload Time" ) ;
		EditorGUILayout.Slider ( Recoil_ForceProp , 0.0f , 30000.0f , "Recoil Force" ) ;

		EditorGUILayout.Space () ;
		Karl_FlagProp.boolValue = EditorGUILayout.Toggle ( "for 'Moser Karl'" , Karl_FlagProp.boolValue ) ;
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		
		serializedObject.ApplyModifiedProperties ();
	}
}
