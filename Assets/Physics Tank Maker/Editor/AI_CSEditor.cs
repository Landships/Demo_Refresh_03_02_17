using UnityEngine;
using System.Collections;
using UnityEditor ;

[ CustomEditor ( typeof ( AI_CS ) ) ]

public class AI_CSEditor : Editor {

	SerializedProperty Eye_TransformProp ;

	SerializedProperty WayPoint_RadiusProp ;
	SerializedProperty Min_Turn_AngleProp ;
	SerializedProperty Pivot_Turn_AngleProp ;
	SerializedProperty Slow_Turn_AngleProp ;
	SerializedProperty Slow_Turn_RateProp ;
	SerializedProperty Min_Turn_RateProp ;
	SerializedProperty Max_Turn_RateProp ;
	SerializedProperty Min_Speed_RateProp ;
	SerializedProperty Max_Speed_RateProp ;
	SerializedProperty Stuck_CountProp ;
	
	SerializedProperty Direct_FireProp ;
	SerializedProperty Fire_AngleProp ;
	SerializedProperty Fire_CountProp ;
	SerializedProperty Bullet_TypeProp ;
	
	string[] Bullet_Type_Names = { "AP" , "HE" } ;

	void OnEnable () {
		Eye_TransformProp = serializedObject.FindProperty ( "Eye_Transform" ) ;

		WayPoint_RadiusProp = serializedObject.FindProperty ( "WayPoint_Radius" ) ;
		Min_Turn_AngleProp = serializedObject.FindProperty ( "Min_Turn_Angle" ) ;
		Pivot_Turn_AngleProp = serializedObject.FindProperty ( "Pivot_Turn_Angle" ) ;
		Slow_Turn_AngleProp = serializedObject.FindProperty ( "Slow_Turn_Angle" ) ;
		Slow_Turn_RateProp = serializedObject.FindProperty ( "Slow_Turn_Rate" ) ;

		Min_Turn_RateProp = serializedObject.FindProperty ( "Min_Turn_Rate" ) ;
		Max_Turn_RateProp = serializedObject.FindProperty ( "Max_Turn_Rate" ) ;
		Min_Speed_RateProp = serializedObject.FindProperty ( "Min_Speed_Rate" ) ;
		Max_Speed_RateProp = serializedObject.FindProperty ( "Max_Speed_Rate" ) ;
		Stuck_CountProp = serializedObject.FindProperty ( "Stuck_Count" ) ;

		Direct_FireProp = serializedObject.FindProperty ( "Direct_Fire" ) ;
		Fire_AngleProp = serializedObject.FindProperty ( "Fire_Angle" ) ;
		Fire_CountProp = serializedObject.FindProperty ( "Fire_Count" ) ;
		Bullet_TypeProp = serializedObject.FindProperty ( "Bullet_Type" ) ;

	}
	
	public override void OnInspectorGUI () {
		GUI.backgroundColor = new Color ( 1.0f , 1.0f , 0.5f , 1.0f ) ;
		serializedObject.Update () ;

		EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Basic Settings", MessageType.None, true );
		Eye_TransformProp.objectReferenceValue = EditorGUILayout.ObjectField ( "AI_Eye" , Eye_TransformProp.objectReferenceValue , typeof ( Transform ) , true ) ;

		EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Drive Settings", MessageType.None, true );
		EditorGUILayout.Slider ( WayPoint_RadiusProp , 0.0f , 1000.0f , "WayPoint Radius" ) ;
		EditorGUILayout.Space () ;
		EditorGUILayout.Slider ( Min_Turn_AngleProp , 0.0f , 10.0f , "Min Turn Angle" ) ;
		EditorGUILayout.Slider ( Pivot_Turn_AngleProp , 0.0f , 360.0f , "Pivot Turn Angle" ) ;
		EditorGUILayout.Slider ( Slow_Turn_AngleProp , 0.0f , 360.0f , "Slow Turn Angle" ) ;
		EditorGUILayout.Slider ( Slow_Turn_RateProp , 0.0f , 1.0f , "Slow Turn Rate" ) ;
		EditorGUILayout.Space () ;
		EditorGUILayout.Slider ( Min_Turn_RateProp , 0.0f , 1.0f , "Min Turn Rate" ) ;
		EditorGUILayout.Slider ( Max_Turn_RateProp , 0.0f , 1.0f , "Max Turn Rate" ) ;
		EditorGUILayout.Slider ( Min_Speed_RateProp , 0.0f , 1.0f , "Min Speed Rate" ) ;
		EditorGUILayout.Slider ( Max_Speed_RateProp , 0.0f , 1.0f , "Max Speed Rate" ) ;
		EditorGUILayout.Space () ;
		EditorGUILayout.Slider ( Stuck_CountProp , 1.0f , 10.0f , "Stuck Count" ) ;

		EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Combat Settings", MessageType.None, true );
		Direct_FireProp.boolValue = EditorGUILayout.Toggle ( "Direct Fire" , Direct_FireProp.boolValue ) ;
		EditorGUILayout.Slider ( Fire_AngleProp , 0.0f , 45.0f , "Fire Angle" ) ;
		EditorGUILayout.Slider ( Fire_CountProp , 0.0f , 10.0f , "Fire Count" ) ;
		Bullet_TypeProp.intValue = EditorGUILayout.Popup ( "Bullet Type" , Bullet_TypeProp.intValue , Bullet_Type_Names ) ;
		EditorGUILayout.Space () ;

		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		serializedObject.ApplyModifiedProperties ();
	}
	
}