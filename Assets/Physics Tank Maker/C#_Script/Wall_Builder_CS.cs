using UnityEngine;
using System.Collections;

[ ExecuteInEditMode ]

public class Wall_Builder_CS : MonoBehaviour {
	
	public int Type = 3 ;
	public int X_Number = 5 ;
	public int Y_Number = 5 ;
	public int Z_Number = 5 ;
	public float X_Spacing = 0.8f ;
	public float Y_Spacing = 0.5f ;
	public float Z_Spacing = 0.8f ;
	public Vector3 Scale = new Vector3 ( 0.5f , 0.5f , 0.5f ) ;
	public float Block_Mass = 20.0f ;
	public Mesh Block_Mesh ;
	public Material Block_Material ;
	public Mesh Collider_Mesh ;
	public PhysicMaterial Collider_Material ;
	public bool  Convex = true ;
	
	public Transform Parent_Transform ;
	
	void Start () {
		Parent_Transform = this.transform ;
	}
	
}