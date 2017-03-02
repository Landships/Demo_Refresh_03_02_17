using UnityEngine;
using System.Collections;

[ ExecuteInEditMode ]

public class Create_SteeredWheel_CS : MonoBehaviour {
	
	public float Shaft_Mass = 300.0f ;
	public Mesh Shaft_Mesh ;
	public Material Shaft_Material ;
	public float Shaft_Collider_Radius = 0.5f ;

	public float Sus_Vertical_Range = 0.1f ;
	public float Sus_Vertical_Spring = 100000.0f ;
	public float Sus_Vertical_Damper = 250.0f ;
	public float Sus_Torsion_Range = 20.0f ;
	public float Sus_Torsion_Spring = 100000.0f ;
	public float Sus_Torsion_Damper = 250.0f ;
	public float Sus_Anchor_Offset_Y = 0.0f ;
	public float Sus_Anchor_Offset_Z = 0.0f ;
	
	public float Hub_Distance = 1.4f ;
	public float Hub_Offset_Y = 0.0f ;
	public float Hub_Offset_Z = 0.0f ;
	public float Hub_Mass = 50.0f ;
	public float Hub_Spring = 2000.0f ;
	public Mesh Hub_Mesh_L ;
	public Mesh Hub_Mesh_R ;
	public Material Hub_Material_L ;
	public Material Hub_Material_R ;
	public float Hub_Anchor_Offset_X = 0.0f ;
	public float Hub_Anchor_Offset_Z = 0.0f ;
	public float Hub_Collider_Radius = 0.15f ;
	
	public float Wheel_Distance = 1.64f ;
	public float Wheel_Radius = 0.41f ;
	public PhysicMaterial Wheel_Collider_Material ;
	public float Wheel_Offset_Y = 0.0f ;
	public float Wheel_Offset_Z = 0.0f ;
	public float Wheel_Mass = 50.0f ;
	public Mesh Wheel_Mesh ;
	public Material Wheel_Material ;
	public Mesh Wheel_Collider_Mesh ;

	public bool Steer_Flag = true ;
	public bool Reverse_Flag = false ;
	public float Max_Angle = 35.0f ;
	public float Rotation_Speed = 45.0f ;
	
	public bool Drive_Wheel = false ;
	public float Wheel_Durability = 30000.0f ;
	
	public Transform Parent_Transform ;
	
	void Start () {
		Parent_Transform = this.transform ;
		if ( Application.isPlaying ) {
			Destroy ( this ) ;
		}
	}
	
	void Reset () {
		Start () ;
	}
}