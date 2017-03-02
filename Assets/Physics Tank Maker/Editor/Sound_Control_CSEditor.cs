using UnityEngine;
using System.Collections;
using UnityEditor ;

[ CustomEditor ( typeof ( Sound_Control_CS ) ) ]

public class Sound_Control_CSEditor : Editor {
	
	SerializedProperty TypeProp ;
	SerializedProperty Min_Engine_PitchProp ;
	SerializedProperty Max_Engine_PitchProp ;
	SerializedProperty Min_Engine_VolumeProp ;
	SerializedProperty Max_Engine_VolumeProp ;
	SerializedProperty Max_VelocityProp ;
	SerializedProperty Min_ImpactProp ;
	SerializedProperty Max_ImpactProp ;
	SerializedProperty Min_Impact_PitchProp ;
	SerializedProperty Max_Impact_PitchProp ;
	SerializedProperty Min_Impact_VolumeProp ;
	SerializedProperty Max_Impact_VolumeProp ;
	SerializedProperty Max_Motor_VolumeProp ;

	string[] Type_Names = { "Engine Sound" , "Impact Sound" , "Turret Sound" , "Cannon Sound" } ;
	
	void  OnEnable (){
		TypeProp = serializedObject.FindProperty ( "Type" ) ;
		Min_Engine_PitchProp = serializedObject.FindProperty ( "Min_Engine_Pitch" ) ;
		Max_Engine_PitchProp = serializedObject.FindProperty ( "Max_Engine_Pitch" ) ;
		Min_Engine_VolumeProp = serializedObject.FindProperty ( "Min_Engine_Volume" ) ;
		Max_Engine_VolumeProp = serializedObject.FindProperty ( "Max_Engine_Volume" ) ;
		Max_VelocityProp = serializedObject.FindProperty ( "Max_Velocity" ) ;	
		Min_ImpactProp = serializedObject.FindProperty ( "Min_Impact" ) ;
		Max_ImpactProp = serializedObject.FindProperty ( "Max_Impact" ) ;
		Min_Impact_PitchProp = serializedObject.FindProperty ( "Min_Impact_Pitch" ) ;
		Max_Impact_PitchProp = serializedObject.FindProperty ( "Max_Impact_Pitch" ) ;
		Min_Impact_VolumeProp = serializedObject.FindProperty ( "Min_Impact_Volume" ) ;
		Max_Impact_VolumeProp = serializedObject.FindProperty ( "Max_Impact_Volume" ) ;
		Max_Motor_VolumeProp = serializedObject.FindProperty ( "Max_Motor_Volume" ) ;
	}
	
	public override void  OnInspectorGUI (){
		GUI.backgroundColor = new Color ( 1.0f , 1.0f , 0.5f , 1.0f ) ;
		serializedObject.Update () ;
		
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Select the type of sound.", MessageType.None, true );
		TypeProp.intValue = EditorGUILayout.Popup ( "Type" , TypeProp.intValue , Type_Names ) ;
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		
		switch ( TypeProp.intValue ) {
		case 0 :
			EditorGUILayout.HelpBox( "This script must be attached to the object under the 'MainBody'.", MessageType.None, true );
			EditorGUILayout.Slider ( Min_Engine_PitchProp , 0.1f , 10.0f , "Idling Pitch" ) ;
			EditorGUILayout.Slider ( Max_Engine_PitchProp , 0.1f , 10.0f , "Max Pitch" ) ;
			EditorGUILayout.Slider ( Min_Engine_VolumeProp , 0.0f , 1.0f , "Idling Volume" ) ;
			EditorGUILayout.Slider ( Max_Engine_VolumeProp , 0.0f , 1.0f , "Max Volume" ) ;
			EditorGUILayout.Slider ( Max_VelocityProp , 1.0f , 100.0f , "Max Speed" ) ;
			break ;
		case 1 :
			EditorGUILayout.HelpBox( "This script must be attached to 'MainBody'", MessageType.None, true );
			EditorGUILayout.Slider ( Min_ImpactProp , 0.1f , 5.0f , "Min Impact" ) ;
			EditorGUILayout.Slider ( Max_ImpactProp , 0.1f , 5.0f , "Max Impact" ) ;
			EditorGUILayout.Slider ( Min_Impact_PitchProp , 0.1f , 10.0f , "Min Pitch" ) ;
			EditorGUILayout.Slider ( Max_Impact_PitchProp , 0.1f , 10.0f , "Max Pitch" ) ;
			EditorGUILayout.Slider ( Min_Impact_VolumeProp , 0.0f , 1.0f , "Min Volume" ) ;
			EditorGUILayout.Slider ( Max_Impact_VolumeProp , 0.0f , 1.0f , "Max Volume" ) ;
			break ;
		case 2 :
			EditorGUILayout.HelpBox( "This script must be attached to 'Turret_Base'", MessageType.None, true );
			EditorGUILayout.Slider ( Max_Motor_VolumeProp , 0.0f , 1.0f , "Max Volume" ) ;
			break ;
		case 3 :
			EditorGUILayout.HelpBox( "This script must be attached to 'Cannon_Base'", MessageType.None, true );
			EditorGUILayout.Slider ( Max_Motor_VolumeProp , 0.0f , 1.0f , "Max Volume" ) ;
			break ;
		}
		
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		
		serializedObject.ApplyModifiedProperties () ;
	}
}