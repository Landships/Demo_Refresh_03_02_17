using UnityEngine;
using System.Collections;
using UnityEditor ;

[ CustomEditor ( typeof ( Wall_Builder_CS ) ) ]

public class Wall_Builder_CSEditor : Editor {
	
	SerializedProperty TypeProp ;
	SerializedProperty X_NumberProp ;
	SerializedProperty Y_NumberProp ;
	SerializedProperty Z_NumberProp ;
	SerializedProperty X_SpacingProp ;
	SerializedProperty Y_SpacingProp ;
	SerializedProperty Z_SpacingProp ;
	SerializedProperty ScaleProp ;
	SerializedProperty Block_MassProp ;
	SerializedProperty Block_MeshProp ;
	SerializedProperty Block_MaterialProp ;
	SerializedProperty Collider_MeshProp ;
	SerializedProperty Collider_MaterialProp ;
	SerializedProperty ConvexProp ;
	
	SerializedProperty Parent_TransformProp ;
	
	GameObject Temp_Object ;
	Transform Temp_Transform ;
	Transform Parent_Transform ;
	string[] ID_Names = { "Simple" , "Header Bond" , "Gable" , "Pyramid" } ;
	float Pos_X ;
	float Pos_Y ;
	float Pos_Z ;
	
	void OnEnable () {
		TypeProp = serializedObject.FindProperty ( "Type" ) ;
		X_NumberProp = serializedObject.FindProperty ( "X_Number" ) ;
		Y_NumberProp = serializedObject.FindProperty ( "Y_Number" ) ;
		Z_NumberProp = serializedObject.FindProperty ( "Z_Number" ) ;
		X_SpacingProp = serializedObject.FindProperty ( "X_Spacing" ) ;
		Y_SpacingProp = serializedObject.FindProperty ( "Y_Spacing" ) ;
		Z_SpacingProp = serializedObject.FindProperty ( "Z_Spacing" ) ;
		ScaleProp = serializedObject.FindProperty ( "Scale" ) ;
		Block_MassProp = serializedObject.FindProperty ( "Block_Mass" ) ;
		Block_MeshProp = serializedObject.FindProperty ( "Block_Mesh" ) ;
		Block_MaterialProp = serializedObject.FindProperty ( "Block_Material" ) ;
		Collider_MeshProp = serializedObject.FindProperty ( "Collider_Mesh" ) ;
		Collider_MaterialProp = serializedObject.FindProperty ( "Collider_Material" ) ;
		ConvexProp = serializedObject.FindProperty ( "Convex" ) ;
		
		Parent_TransformProp = serializedObject.FindProperty ( "Parent_Transform" ) ;
		
		Parent_Transform = Parent_TransformProp.objectReferenceValue as Transform ;
	}
	
	public override void OnInspectorGUI () {
		Set_Inspector () ;
		if (Event.current.commandName == "UndoRedoPerformed" ) {
			Create () ;
		}
	}
	
	void Set_Inspector () {
		GUI.backgroundColor = new Color ( 1.0f , 1.0f , 0.5f , 1.0f ) ;
		serializedObject.Update () ;
		
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Select the type of wall.", MessageType.None, true );
		TypeProp.intValue = EditorGUILayout.Popup ( "Type" , TypeProp.intValue , ID_Names ) ;
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Set the number of blocks.", MessageType.None, true );
		EditorGUILayout.IntSlider ( X_NumberProp , 1 , 30 , "Number of X" ) ;
		EditorGUILayout.IntSlider ( Y_NumberProp , 1 , 30 , "Number of Y" ) ;
		EditorGUILayout.IntSlider ( Z_NumberProp , 1 , 30 , "Number of Z" ) ;
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "Set the spacing.", MessageType.None, true );
		EditorGUILayout.Slider ( X_SpacingProp , 0.1f , 100.0f , "Spacing of X" ) ;
		EditorGUILayout.Slider ( Y_SpacingProp , 0.1f , 100.0f , "Spacing of Y" ) ;
		EditorGUILayout.Slider ( Z_SpacingProp , 0.1f , 100.0f , "Spacing of Z" ) ;
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		ScaleProp.vector3Value = EditorGUILayout.Vector3Field( "Scale", ScaleProp.vector3Value ) ;
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		EditorGUILayout.Slider ( Block_MassProp , 0.1f , 500.0f , "Mass" ) ;
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		Block_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh" , Block_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
		Block_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Material" , Block_MaterialProp.objectReferenceValue , typeof ( Material ) , false ) ;
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		Collider_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh Collider" , Collider_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
		Collider_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Physic Material" , Collider_MaterialProp.objectReferenceValue , typeof ( PhysicMaterial ) , false ) ;
		ConvexProp.boolValue = EditorGUILayout.Toggle ( "Convex" , ConvexProp.boolValue ) ;
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		if ( GUILayout.Button ( "Update Value" ) ) {
			Create () ;
		}
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		
		serializedObject.ApplyModifiedProperties ();
	}
	
	void Create () {	
		// Delete Objects
		int Temp_Num = Parent_Transform.childCount ;
		for ( int i = 0 ;  i  < Temp_Num ; i++ ) {
			DestroyImmediate ( Parent_Transform.GetChild ( 0 ).gameObject ) ;
		}
		
		// Create Blocks
		switch ( TypeProp.intValue ) {
		case 0 :
			Type_1 () ;
			break ;
		case 1 :
			Type_2 () ;
			break ;
		case 2 :
			Type_3 () ;
			break ;
		case 3 :
			Type_4 () ;
			break ;
		}
	}
	
	void Type_1 () {
		for ( int i = 0 ; i < Z_NumberProp.intValue ; i++ ) {
			Pos_Z = Z_SpacingProp.floatValue * i ;
			for ( int j = 0 ; j < Y_NumberProp.intValue ; j++ ) {
				Pos_Y = Y_SpacingProp.floatValue * j ;
				for ( int k = 0 ; k < X_NumberProp.intValue ; k++ ) {
					Pos_X = ( X_SpacingProp.floatValue * k ) ;
					Arrange () ;
				}
			}
		}
	}
	
	void Type_2 () {
		for ( int i = 0 ; i < Z_NumberProp.intValue ; i++ ) {
			Pos_Z = Z_SpacingProp.floatValue * i ;
			for ( int j = 0 ; j < Y_NumberProp.intValue ; j++ ) {
				Pos_Y = Y_SpacingProp.floatValue * j ;
				if ( j % 2 == 0 ) {
					for ( int k = 0 ; k < X_NumberProp.intValue ; k++ ) {
						Pos_X = X_SpacingProp.floatValue * k ;
						Arrange () ;
					}
				} else {
					for ( int k = 0 ; k < X_NumberProp.intValue - 1 ; k++ ) {
						Pos_X = ( X_SpacingProp.floatValue * k ) + ( X_SpacingProp.floatValue / 2 ) ;
						Arrange () ;
					}
				}
			}
		}
	}
	
	void Type_3 () {
		for ( int i = 0 ; i < Z_NumberProp.intValue ; i++ ) {
			Pos_Z = Z_SpacingProp.floatValue * i ;
			for ( int j = 0 ; j < Y_NumberProp.intValue ; j++ ) {
				Pos_Y = Y_SpacingProp.floatValue * j ;
				for ( int k = 0 ; k < X_NumberProp.intValue - j ; k++ ) {
					Pos_X = ( X_SpacingProp.floatValue * k ) + ( ( X_SpacingProp.floatValue / 2 ) * j ) ;
					Arrange () ;
				}
			}
		}
	}
	
	void Type_4 () {
		for ( int i = 0 ; i < Y_NumberProp.intValue ; i++ ) {
			Pos_Y = Y_SpacingProp.floatValue * i ;
			for ( int j = 0 ; j < X_NumberProp.intValue - i ; j++ ) {
				Pos_X = ( X_SpacingProp.floatValue * j ) + ( ( X_SpacingProp.floatValue / 2 ) * i ) ;
				for ( int k = 0 ; k < Z_NumberProp.intValue - i ; k++ ) {
					Pos_Z = ( Z_SpacingProp.floatValue * k ) + ( ( Z_SpacingProp.floatValue / 2 ) * i ) ;
					Arrange () ;		
				}
			}
		}
	}
	
	void Arrange () {
		//Create gameobject & Set transform
		Temp_Object = new GameObject ( "Block(Rest)" ) ;
		Temp_Object.transform.parent = Parent_Transform ;
		Temp_Object.transform.localPosition = new Vector3 ( Pos_X , Pos_Y , Pos_Z ) - new Vector3 ( X_SpacingProp.floatValue * ( ( X_NumberProp.intValue - 1 ) ) / 2 , -ScaleProp.vector3Value.y / 2 , Z_SpacingProp.floatValue * ( ( Z_NumberProp.intValue - 1 ) ) / 2 );
		Temp_Object.transform.localRotation = Quaternion.identity ;
		Temp_Object.transform.localScale = ScaleProp.vector3Value ;
		// Add components
		Temp_Object.AddComponent < MeshRenderer > () ;
		Temp_Object.GetComponent<Renderer>().material = Block_MaterialProp.objectReferenceValue as Material ;
		MeshFilter Temp_MeshFilter ;
		Temp_MeshFilter = Temp_Object.AddComponent < MeshFilter > () ;
		Temp_MeshFilter.mesh = Block_MeshProp.objectReferenceValue as Mesh ;
		MeshCollider Temp_MeshCollider ;
		Temp_MeshCollider = Temp_Object.AddComponent < MeshCollider > () ;
		Temp_MeshCollider.material = Collider_MaterialProp.objectReferenceValue as PhysicMaterial ;
		Temp_MeshCollider.sharedMesh = Collider_MeshProp.objectReferenceValue as Mesh ;
		Temp_MeshCollider.convex = ConvexProp.boolValue ;
		// Add Script
		Wall_Builder_Finishing_CS Temp_Script ;
		Temp_Script = Temp_Object.AddComponent < Wall_Builder_Finishing_CS > () ;
		Temp_Script.Set_Mass ( Block_MassProp.floatValue ) ;
	}
}