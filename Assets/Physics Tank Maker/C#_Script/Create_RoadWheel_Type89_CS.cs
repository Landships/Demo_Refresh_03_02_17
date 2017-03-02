using UnityEngine;
using System.Collections;

[ ExecuteInEditMode ]

public class Create_RoadWheel_Type89_CS : MonoBehaviour {

	public bool Fit_ST_Flag = false ;

	public float Distance = 1.78f ;
	public float Spring = 400000.0f ;
	public int ParentArm_Num = 2 ;
	public float ParentArm_Spacing = 1.6f ;
	public float ParentArm_Offset_Y = 0.0f ;
	public float ParentArm_AngleLimit = 25.0f ;
	public float ParentArm_Mass = 20.0f ;
	public Mesh ParentArm_L_Mesh ;
	public Mesh ParentArm_R_Mesh ;
	public Material ParentArm_L_Material ;
	public Material ParentArm_R_Material ;
	
	public int ChildArm_Num = 2 ;
	public float ChildArm_Spacing = 0.8f ;
	public 	float ChildArm_Offset_Y = 0.0f ;
	public float ChildArm_AngleLimit = 25.0f ;
	public float ChildArm_Mass = 20.0f ;
	public Mesh ChildArm_L_Mesh ;
	public Mesh ChildArm_R_Mesh ;
	public Material ChildArm_L_Material ;
	public Material ChildArm_R_Material ;
	
	public int Wheel_Num = 2 ;
	public float Wheel_Spacing = 0.4f ;
	public float Wheel_Offset_Y = 0.0f ;
	public float Wheel_Mass = 20.0f ;
	public float Wheel_Radius = 0.12f ;
	public PhysicMaterial Wheel_Collider_Material ;
	public Mesh Wheel_Mesh ;
	public Material Wheel_Material ;
	public Mesh Wheel_Collider_Mesh ;

	public bool Drive_Wheel = true ;
	public bool Wheel_Resize = false ;
	public float ScaleDown_Size = 0.5f ;
	public float Return_Speed = 0.05f ;
	public float Wheel_Durability = 55000.0f ;
	public bool RealTime_Flag = true ;
	
	public Transform Parent_Transform ;
	
	void Start () {
		Parent_Transform = this.transform ;
		if ( Application.isPlaying ) {
			Destroy ( this ) ;
		}
	}
	
	void Update () {
		if ( transform.localEulerAngles.z != 90.0f ) {
			float Temp_X = transform.localEulerAngles.x ;
			float Temp_Y = transform.localEulerAngles.y ;
			transform.localEulerAngles = new Vector3 ( Temp_X , Temp_Y , 90.0f ) ;
		}
	}
	
	void Reset () {
		Start () ;
	}
	
}