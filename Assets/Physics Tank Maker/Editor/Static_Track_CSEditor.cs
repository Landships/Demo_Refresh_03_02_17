using UnityEngine;
using System.Collections;
using UnityEditor ;

[ CustomEditor ( typeof ( Static_Track_CS ) ) ]

public class Static_Track_CSEditor : Editor {
	
	SerializedProperty TypeProp ;
	SerializedProperty Front_TransformProp ;
	SerializedProperty Rear_TransformProp ;
	SerializedProperty Anchor_NameProp ;
	SerializedProperty Anchor_Parent_NameProp ;
	SerializedProperty Anchor_TransformProp ;
	SerializedProperty Reference_LProp ;
	SerializedProperty Reference_RProp ;
	SerializedProperty Reference_Name_LProp ;
	SerializedProperty Reference_Parent_Name_LProp ;
	SerializedProperty Reference_Name_RProp ;
	SerializedProperty Reference_Parent_Name_RProp ;
	SerializedProperty Radius_OffsetProp ;
	SerializedProperty MassProp ;

	string[] TypeNames = { "Static" , "Anchor" , "Dynamic" , "" , "" , "" , "" , "" , "" , "Parent" } ;

	void  OnEnable () {
		TypeProp = serializedObject.FindProperty ( "Type" ) ;
		Front_TransformProp = serializedObject.FindProperty ( "Front_Transform" ) ;
		Rear_TransformProp = serializedObject.FindProperty ( "Rear_Transform" ) ;
		Anchor_NameProp = serializedObject.FindProperty ( "Anchor_Name" ) ;
		Anchor_Parent_NameProp = serializedObject.FindProperty ( "Anchor_Parent_Name" ) ;
		Anchor_TransformProp = serializedObject.FindProperty ( "Anchor_Transform" ) ;
		Reference_LProp = serializedObject.FindProperty ( "Reference_L" ) ;
		Reference_RProp = serializedObject.FindProperty ( "Reference_R" ) ;
		Reference_Name_LProp = serializedObject.FindProperty ( "Reference_Name_L" ) ;
		Reference_Parent_Name_LProp = serializedObject.FindProperty ( "Reference_Parent_Name_L" ) ;
		Reference_Name_RProp = serializedObject.FindProperty ( "Reference_Name_R" ) ;
		Reference_Parent_Name_RProp = serializedObject.FindProperty ( "Reference_Parent_Name_R" ) ;
		Radius_OffsetProp = serializedObject.FindProperty ( "Radius_Offset" ) ;
		MassProp = serializedObject.FindProperty ( "Mass" ) ;
	}
	
	public override void  OnInspectorGUI () {
		GUI.backgroundColor = new Color ( 1.0f , 1.0f , 0.5f , 1.0f ) ;
		serializedObject.Update () ;
		
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Static Track settings", MessageType.None, true );
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;

		if ( TypeProp.intValue != 9 ) { // Except for Parent type.
			Front_TransformProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Front Piece" , Front_TransformProp.objectReferenceValue , typeof ( Transform ) , true ) ;
			Rear_TransformProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Rear Piece" , Rear_TransformProp.objectReferenceValue , typeof ( Transform ) , true ) ;
			EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		}

		TypeProp.intValue = EditorGUILayout.Popup ( "Type" , TypeProp.intValue , TypeNames ) ;
		EditorGUILayout.Space () ;
		switch ( TypeProp.intValue ) {
		case 0 :
			break ;
		case 1 :
			Anchor_TransformProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Anchor Transform" , Anchor_TransformProp.objectReferenceValue , typeof ( Transform ) , true ) ;
			if ( Anchor_TransformProp.objectReferenceValue != null ) {
				Anchor_NameProp.stringValue = Anchor_TransformProp.objectReferenceValue.name ;
				Transform Temp_Transform = Anchor_TransformProp.objectReferenceValue as Transform ;
				if ( Temp_Transform.parent ) {
					Anchor_Parent_NameProp.stringValue = Temp_Transform.parent.name ;
				}
			}
			EditorGUILayout.Space () ;
			GUI.backgroundColor = new Color ( 0.7f , 0.7f , 0.7f , 1.0f ) ;
			Anchor_NameProp.stringValue = EditorGUILayout.TextField ( "Anchor Name" , Anchor_NameProp.stringValue ) ;
			Anchor_Parent_NameProp.stringValue = EditorGUILayout.TextField ( "Anchor Parent Name" , Anchor_Parent_NameProp.stringValue ) ;
			GUI.backgroundColor = new Color ( 1.0f , 1.0f , 0.5f , 1.0f ) ;
			break ;
		case 2 :
			break ;
		case 9 :
			EditorGUILayout.Slider ( MassProp , 1.0f , 100.0f , "Mass" ) ;
			EditorGUILayout.Slider ( Radius_OffsetProp , -0.5f , 0.5f , "Radius Offset" ) ;
			EditorGUILayout.Space () ;
			Reference_RProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Right Reference Wheel" , Reference_RProp.objectReferenceValue , typeof ( Transform ) , true ) ;
			Reference_LProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Left Reference Wheel" , Reference_LProp.objectReferenceValue , typeof ( Transform ) , true ) ;
			if ( Reference_RProp.objectReferenceValue != null ) {
				Reference_Name_RProp.stringValue =Reference_RProp.objectReferenceValue.name ;
				Transform Temp_Transform = Reference_RProp.objectReferenceValue as Transform ;
				if ( Temp_Transform.parent ) {
					Reference_Parent_Name_RProp.stringValue = Temp_Transform.parent.name ;
				}
			}
			if ( Reference_LProp.objectReferenceValue != null ) {
				Reference_Name_LProp.stringValue =Reference_LProp.objectReferenceValue.name ;
				Transform Temp_Transform = Reference_LProp.objectReferenceValue as Transform ;
				if ( Temp_Transform.parent ) {
					Reference_Parent_Name_LProp.stringValue = Temp_Transform.parent.name ;
				}
			}
			EditorGUILayout.Space () ;
			GUI.backgroundColor = new Color ( 0.7f , 0.7f , 0.7f , 1.0f ) ;
			Reference_Parent_Name_RProp.stringValue = EditorGUILayout.TextField ( "Right Parent Name" , Reference_Parent_Name_RProp.stringValue ) ;
			Reference_Name_RProp.stringValue = EditorGUILayout.TextField ( "Right Wheel Name" , Reference_Name_RProp.stringValue ) ;
			Reference_Parent_Name_LProp.stringValue = EditorGUILayout.TextField ( "Left Parent Name" , Reference_Parent_Name_LProp.stringValue ) ;
			Reference_Name_LProp.stringValue = EditorGUILayout.TextField ( "Left Wheel Name" , Reference_Name_LProp.stringValue ) ;
			GUI.backgroundColor = new Color ( 1.0f , 1.0f , 0.5f , 1.0f ) ;
			break ;
		}
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		
		serializedObject.ApplyModifiedProperties ();
	}
}