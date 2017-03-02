/*using UnityEngine;
using System.Collections;
using UnityEditor ;

[ CustomEditor ( typeof ( Damage_Control_CS ) ) ]

public class Damage_Control_CSEditor : Editor {
	
	SerializedProperty TypeProp ;
	SerializedProperty MassProp ;
	SerializedProperty DirectionProp ;
	SerializedProperty DurabilityProp ;
	SerializedProperty Sub_DurabilityProp ;
	SerializedProperty Trouble_TimeProp ;
	SerializedProperty Linked_TransformProp ;

	string[] TypeNames = { "" , "Armor_Collider" , "Turret" , "Cannon" , "Barrel" , "MainBody" , "Track" , "SubJoint" , "Wheel" , "StaticTrack_Collider" } ;

	void OnEnable () {
		TypeProp = serializedObject.FindProperty ( "Type" ) ;
		MassProp = serializedObject.FindProperty ( "Mass" ) ;
		DirectionProp = serializedObject.FindProperty ( "Direction" ) ;
		DurabilityProp = serializedObject.FindProperty ( "Durability" ) ;
		Sub_DurabilityProp = serializedObject.FindProperty ( "Sub_Durability" ) ;
		Trouble_TimeProp = serializedObject.FindProperty ( "Trouble_Time" ) ;
		Linked_TransformProp = serializedObject.FindProperty ( "Linked_Transform" ) ;
	}
	
	public override void OnInspectorGUI () {
		GUI.backgroundColor = new Color ( 1.0f , 1.0f , 0.5f , 1.0f ) ;
		if ( EditorApplication.isPlaying == false ) {
			serializedObject.Update () ;
			
			EditorGUILayout.Space () ; EditorGUILayout.Space () ;	
			TypeProp.intValue = EditorGUILayout.Popup ( "Type" , TypeProp.intValue , TypeNames ) ;

			switch ( TypeProp.intValue ) {
			case 1 : // Armor_Collider
				EditorGUILayout.HelpBox( "Type : Armor_Collider" , MessageType.None, true );
				EditorGUILayout.Slider ( DurabilityProp , 1.0f , 1000000.0f , "Durability" ) ;
				break ;
			case 2 : // Turret
				EditorGUILayout.HelpBox( "Type : Turret" , MessageType.None, true );
				EditorGUILayout.Slider ( MassProp , 1.0f , 10000.0f , "Mass" ) ;
				EditorGUILayout.Slider ( DurabilityProp , 1.0f , 1000000.0f , "Durability" ) ;
				EditorGUILayout.Slider ( Sub_DurabilityProp , 1.0f , 1000000.0f , "Sub Durability" ) ;
				EditorGUILayout.Slider ( Trouble_TimeProp , 0.0f , 60.0f , "Trouble Time" ) ;
				break ;
			case 3 : // Cannon
				EditorGUILayout.HelpBox( "Type : Cannon" , MessageType.None, true );
				EditorGUILayout.Slider ( DurabilityProp , 1.0f , 1000000.0f , "Durability" ) ;
				EditorGUILayout.Slider ( Sub_DurabilityProp , 1.0f , 1000000.0f , "Sub Durability" ) ;
				EditorGUILayout.Slider ( Trouble_TimeProp , 0.0f , 60.0f , "Trouble Time" ) ;
				break ;
			case 4 : // Barrel
				EditorGUILayout.HelpBox( "Type : Barrel" , MessageType.None, true );
				EditorGUILayout.Slider ( DurabilityProp , 1.0f , 1000000.0f , "Durability" ) ;
				EditorGUILayout.Slider ( Sub_DurabilityProp , 1.0f , 1000000.0f , "Sub Durability" ) ;
				EditorGUILayout.Slider ( Trouble_TimeProp , 0.0f , 60.0f , "Trouble Time" ) ;
				break ;
			case 5 : // MainBody
				EditorGUILayout.HelpBox( "Type : MainBody" , MessageType.None, true );
				EditorGUILayout.Slider ( DurabilityProp , 1.0f , 1000000.0f , "Durability" ) ;
				break ;
			case 6 : // Track
				EditorGUILayout.HelpBox( "Type : Track" , MessageType.None, true );
				if ( DirectionProp.intValue == 0 ) {
					EditorGUILayout.HelpBox( "Direction : Left" , MessageType.None, true );
				} else {
					EditorGUILayout.HelpBox( "Direction : Right" , MessageType.None, true );
				}
				EditorGUILayout.Slider ( DurabilityProp , 1.0f , 1000000.0f , "Durability" ) ;
				break ;
			case 7 : // SubJoint
				EditorGUILayout.HelpBox( "Type : SubJoint" , MessageType.None, true );
				EditorGUILayout.HelpBox( "Direction : " + DirectionProp.intValue , MessageType.None, true );
				break ;
			case 8 : // Wheel
				EditorGUILayout.HelpBox( "Type : Wheel" , MessageType.None, true );
				if ( DirectionProp.intValue == 0 ) {
					EditorGUILayout.HelpBox( "Direction : Left" , MessageType.None, true );
				} else {
					EditorGUILayout.HelpBox( "Direction : Right" , MessageType.None, true );
				}
				EditorGUILayout.Slider ( DurabilityProp , 1.0f , 1000000.0f , "Durability" ) ;
				break ;
			case 9 : // Static Track
				EditorGUILayout.HelpBox( "Type : StaticTrack_Collider" , MessageType.None, true );
				Linked_TransformProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Linked Piece" , Linked_TransformProp.objectReferenceValue , typeof ( Transform ) , true ) ;
				EditorGUILayout.Slider ( DurabilityProp , 1.0f , 1000000.0f , "Durability" ) ;
				break ;
			}
			if ( DurabilityProp.floatValue >= 1000000.0f ) {
				DurabilityProp.floatValue = Mathf.Infinity ;
			}
		}
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		serializedObject.ApplyModifiedProperties () ;
	}
}*/
