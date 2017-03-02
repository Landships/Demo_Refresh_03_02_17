using UnityEngine;
using System.Collections;
using UnityEditor ;

[ CustomEditor ( typeof ( Bullet_Generator_CS ) ) ]

public class Bullet_Generator_CSEditor : Editor {
	
	SerializedProperty Bullet_MeshProp ;
	SerializedProperty Bullet_MaterialProp ;
	SerializedProperty Bullet_MassProp ;
	SerializedProperty Bullet_DragProp ;
	SerializedProperty Bullet_PhysicMatProp ;
	SerializedProperty Bullet_ScaleProp ;
	SerializedProperty Bullet_ForceProp ;
	SerializedProperty BoxCollider_ScaleProp ;
	SerializedProperty Delete_TimeProp ;
	SerializedProperty MuzzleFire_ObjectProp ;
	SerializedProperty Impact_ObjectProp ;
	SerializedProperty Ricochet_ObjectProp ;
	SerializedProperty Trail_FlagProp ;
	SerializedProperty Trail_MaterialProp ;
	SerializedProperty Trail_Start_WidthProp ;
	SerializedProperty Trail_End_WidthProp ;
	SerializedProperty Trail_TimeProp ;

	SerializedProperty Bullet_Mesh_HEProp ;
	SerializedProperty Bullet_Material_HEProp ;
	SerializedProperty Bullet_Mass_HEProp ;
	SerializedProperty Bullet_Drag_HEProp ;
	SerializedProperty Bullet_Scale_HEProp ;
	SerializedProperty Bullet_Force_HEProp ;
	SerializedProperty BoxCollider_Scale_HEProp ;
	SerializedProperty Delete_Time_HEProp ;
	SerializedProperty MuzzleFire_Object_HEProp ;
	SerializedProperty Explosion_ForceProp ;
	SerializedProperty Explosion_RadiusProp ;
	SerializedProperty Explosion_ObjectProp ;
	SerializedProperty Trail_Flag_HEProp ;
	SerializedProperty Trail_Material_HEProp ;
	SerializedProperty Trail_Start_Width_HEProp ;
	SerializedProperty Trail_End_Width_HEProp ;
	SerializedProperty Trail_Time_HEProp ;

	SerializedProperty OffsetProp ;
	SerializedProperty Debug_FlagProp ;

	void  OnEnable (){
		Bullet_MeshProp = serializedObject.FindProperty ( "Bullet_Mesh" ) ;
		Bullet_MaterialProp = serializedObject.FindProperty ( "Bullet_Material" ) ;
		Bullet_MassProp = serializedObject.FindProperty ( "Bullet_Mass" ) ;
		Bullet_DragProp = serializedObject.FindProperty ( "Bullet_Drag" ) ;
		Bullet_PhysicMatProp = serializedObject.FindProperty ( "Bullet_PhysicMat" ) ;
		Bullet_ScaleProp = serializedObject.FindProperty ( "Bullet_Scale" ) ;
		Bullet_ForceProp = serializedObject.FindProperty ( "Bullet_Force" ) ;
		BoxCollider_ScaleProp = serializedObject.FindProperty ( "BoxCollider_Scale" ) ;
		Delete_TimeProp = serializedObject.FindProperty ( "Delete_Time" ) ;
		MuzzleFire_ObjectProp = serializedObject.FindProperty ( "MuzzleFire_Object" ) ;
		Impact_ObjectProp = serializedObject.FindProperty ( "Impact_Object" ) ;
		Ricochet_ObjectProp = serializedObject.FindProperty ( "Ricochet_Object" ) ;
		Trail_FlagProp = serializedObject.FindProperty ( "Trail_Flag" ) ;
		Trail_MaterialProp = serializedObject.FindProperty ( "Trail_Material" ) ;
		Trail_Start_WidthProp = serializedObject.FindProperty ( "Trail_Start_Width" ) ;
		Trail_End_WidthProp = serializedObject.FindProperty ( "Trail_End_Width" ) ;
		Trail_TimeProp = serializedObject.FindProperty ( "Trail_Time" ) ;

		Bullet_Mesh_HEProp = serializedObject.FindProperty ( "Bullet_Mesh_HE" ) ;
		Bullet_Material_HEProp = serializedObject.FindProperty ( "Bullet_Material_HE" ) ;
		Bullet_Mass_HEProp = serializedObject.FindProperty ( "Bullet_Mass_HE" ) ;
		Bullet_Drag_HEProp = serializedObject.FindProperty ( "Bullet_Drag_HE" ) ;
		Bullet_Scale_HEProp = serializedObject.FindProperty ( "Bullet_Scale_HE" ) ;
		Bullet_Force_HEProp = serializedObject.FindProperty ( "Bullet_Force_HE" ) ;
		BoxCollider_Scale_HEProp = serializedObject.FindProperty ( "BoxCollider_Scale_HE" ) ;
		Delete_Time_HEProp = serializedObject.FindProperty ( "Delete_Time_HE" ) ;
		MuzzleFire_Object_HEProp = serializedObject.FindProperty ( "MuzzleFire_Object_HE" ) ;
		Explosion_ForceProp = serializedObject.FindProperty ( "Explosion_Force" ) ;
		Explosion_RadiusProp = serializedObject.FindProperty ( "Explosion_Radius" ) ;
		Explosion_ObjectProp = serializedObject.FindProperty ( "Explosion_Object" ) ;
		Trail_Flag_HEProp = serializedObject.FindProperty ( "Trail_Flag_HE" ) ;
		Trail_Material_HEProp = serializedObject.FindProperty ( "Trail_Material_HE" ) ;
		Trail_Start_Width_HEProp = serializedObject.FindProperty ( "Trail_Start_Width_HE" ) ;
		Trail_End_Width_HEProp = serializedObject.FindProperty ( "Trail_End_Width_HE" ) ;
		Trail_Time_HEProp = serializedObject.FindProperty ( "Trail_Time_HE" ) ;

		OffsetProp = serializedObject.FindProperty ( "Offset" ) ;
		Debug_FlagProp = serializedObject.FindProperty ( "Debug_Flag" ) ;
	}
	
	public override void  OnInspectorGUI (){
		GUI.backgroundColor = new Color ( 1.0f , 1.0f , 0.5f , 1.0f ) ;
		serializedObject.Update () ;
		
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		EditorGUILayout.HelpBox( "AP Bullet setting", MessageType.None, true ) ;
		Bullet_MeshProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh" , Bullet_MeshProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
		Bullet_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Material" , Bullet_MaterialProp.objectReferenceValue , typeof ( Material ) , false ) ;
		Bullet_PhysicMatProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Physic Material" , Bullet_PhysicMatProp.objectReferenceValue , typeof ( PhysicMaterial ) , false ) ;
		EditorGUILayout.Space () ;
		EditorGUILayout.Slider ( Bullet_MassProp , 0.1f , 100.0f , "Mass" ) ;
		EditorGUILayout.Slider ( Bullet_DragProp , 0.0f , 1.0f , "Drag (Air Resistance)" ) ;
		EditorGUILayout.Slider ( Bullet_ForceProp , 10.0f , 1000.0f , "Initial Velocity" ) ;
		EditorGUILayout.Space () ;
		Bullet_ScaleProp.vector3Value = EditorGUILayout.Vector3Field( "Scale", Bullet_ScaleProp.vector3Value ) ;
		BoxCollider_ScaleProp.vector3Value = EditorGUILayout.Vector3Field( "BoxCollider Scale", BoxCollider_ScaleProp.vector3Value ) ;
		EditorGUILayout.Space () ;
		EditorGUILayout.Slider ( Delete_TimeProp , 1.0f , 180.0f , "Life Time" ) ;
		EditorGUILayout.Space () ;
		MuzzleFire_ObjectProp.objectReferenceValue = EditorGUILayout.ObjectField ( "MuzzleFire Prefab" , MuzzleFire_ObjectProp.objectReferenceValue , typeof ( GameObject ) , true ) ;
		EditorGUILayout.Space () ;
		Impact_ObjectProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Impact Prefab" , Impact_ObjectProp.objectReferenceValue , typeof ( GameObject ) , true ) ;
		Ricochet_ObjectProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Ricochet Prefab" , Ricochet_ObjectProp.objectReferenceValue , typeof ( GameObject ) , true ) ;
		EditorGUILayout.Space () ;
		Trail_FlagProp.boolValue = EditorGUILayout.Toggle ( "Trail" , Trail_FlagProp.boolValue ) ;
		if ( Trail_FlagProp.boolValue ) {
			Trail_MaterialProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Trail Material" , Trail_MaterialProp.objectReferenceValue , typeof ( Material ) , false ) ;
			EditorGUILayout.Slider ( Trail_Start_WidthProp , 0.0f , 10.0f , "Start Width" ) ;
			EditorGUILayout.Slider ( Trail_End_WidthProp , 0.0f , 10.0f , "End Width" ) ;
			EditorGUILayout.Slider ( Trail_TimeProp , 0.0f , 10.0f , "Time" ) ;
		}
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;
		
		EditorGUILayout.HelpBox( "HE Bullet setting", MessageType.None, true ) ;
		Bullet_Mesh_HEProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Mesh" , Bullet_Mesh_HEProp.objectReferenceValue , typeof ( Mesh ) , false ) ;
		Bullet_Material_HEProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Material" , Bullet_Material_HEProp.objectReferenceValue , typeof ( Material ) , false ) ;
		EditorGUILayout.Space () ;
		EditorGUILayout.Slider ( Bullet_Mass_HEProp , 0.1f , 100.0f , "Mass" ) ;
		EditorGUILayout.Slider ( Bullet_Drag_HEProp , 0.0f , 1.0f , "Drag (Air Resistance)" ) ;
		EditorGUILayout.Slider ( Bullet_Force_HEProp , 10.0f , 1000.0f , "Initial Velocity" ) ;
		EditorGUILayout.Space () ;
		Bullet_Scale_HEProp.vector3Value = EditorGUILayout.Vector3Field( "Scale", Bullet_Scale_HEProp.vector3Value ) ;
		BoxCollider_Scale_HEProp.vector3Value = EditorGUILayout.Vector3Field( "BoxCollider Scale", BoxCollider_Scale_HEProp.vector3Value ) ;
		EditorGUILayout.Space () ;
		EditorGUILayout.Slider ( Delete_Time_HEProp , 1.0f , 180.0f , "Life Time" ) ;
		EditorGUILayout.Space () ;
		MuzzleFire_Object_HEProp.objectReferenceValue = EditorGUILayout.ObjectField ( "MuzzleFire Prefab" , MuzzleFire_Object_HEProp.objectReferenceValue , typeof ( GameObject ) , true ) ;
		EditorGUILayout.Space () ;
		EditorGUILayout.Slider ( Explosion_ForceProp , 10.0f , 1000000.0f , "Explosion Force" ) ;
		EditorGUILayout.Slider ( Explosion_RadiusProp , 0.1f , 1000.0f , "Explosion Radius" ) ;
		Explosion_ObjectProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Explosion Prefab" , Explosion_ObjectProp.objectReferenceValue , typeof ( GameObject ) , true ) ;
		EditorGUILayout.Space () ;
		Trail_Flag_HEProp.boolValue = EditorGUILayout.Toggle ( "Trail" , Trail_Flag_HEProp.boolValue ) ;
		if ( Trail_Flag_HEProp.boolValue ) {
			Trail_Material_HEProp.objectReferenceValue = EditorGUILayout.ObjectField ( "Trail Material" , Trail_Material_HEProp.objectReferenceValue , typeof ( Material ) , false ) ;
			EditorGUILayout.Slider ( Trail_Start_Width_HEProp , 0.0f , 10.0f , "Start Width" ) ;
			EditorGUILayout.Slider ( Trail_End_Width_HEProp , 0.0f , 10.0f , "End Width" ) ;
			EditorGUILayout.Slider ( Trail_Time_HEProp , 0.0f , 10.0f , "Time" ) ;
		}
		EditorGUILayout.Space () ; EditorGUILayout.Space () ;


		EditorGUILayout.HelpBox( "Special setting", MessageType.None, true ) ;
		EditorGUILayout.Slider ( OffsetProp , 0.0f , 10.0f , "Offset" ) ;
		Debug_FlagProp.boolValue = EditorGUILayout.Toggle ( "Debug Mode" , Debug_FlagProp.boolValue ) ;
		
		serializedObject.ApplyModifiedProperties ();
	}
}
