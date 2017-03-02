using UnityEngine;
using System.Collections;

[ ExecuteInEditMode ]

public class Create_RoadWheel_CS : MonoBehaviour {

	public bool Fit_ST_Flag = false ;

	public float Sus_Distance = 2.06f ;
	public int Num = 6 ;
	public float Spacing = 0.88f ;
	public float Sus_Length = 0.5f ;
	public float Sus_Angle = 0.0f ;
	public float Sus_Anchor = 0.0f ;
	public float Sus_Mass = 30.0f ;
	public float Sus_Spring = 900.0f ;
	public float Sus_Damper = 20.0f ;
	public float Sus_Target = 30.0f ;
	public float Sus_Forward_Limit = 30.0f ;
	public float Sus_Backward_Limit = 30.0f ;
	public Mesh Sus_L_Mesh ;
	public Mesh Sus_R_Mesh ;
	public Material Sus_L_Material ;
	public Material Sus_R_Material ;
	public float Reinforce_Radius = 0.5f ;

	public float Wheel_Distance = 2.7f ;
	public float Wheel_Mass = 30.0f ;
	public float Wheel_Radius = 0.3f ;
	public PhysicMaterial Collider_Material ;
	public Mesh Wheel_Mesh ;
	public Material Wheel_Material ;
	public Mesh Collider_Mesh ;
	public Mesh Collider_Mesh_Sub ;

	public bool  Drive_Wheel = true ;
	public bool  Wheel_Resize = false ;
	public float ScaleDown_Size = 0.5f ;
	public float Return_Speed = 0.05f ;
	public float Wheel_Durability = 55000.0f ;

	public bool  RealTime_Flag = true ;
	public Transform Parent_Transform ;
	
	void Start () {
		Parent_Transform = this.transform ;
	}
	
	void Update () {
		if ( transform.localEulerAngles.z != 90.0f ) {
			float Temp_X = transform.localEulerAngles.x ;
			float Temp_Y = transform.localEulerAngles.y ;
			transform.localEulerAngles = new Vector3 ( Temp_X , Temp_Y , 90.0f ) ;
		}
		if ( Application.isPlaying ) {
			Destroy ( this ) ;
		}
	}
	
	void  Reset (){
		Start () ;
	}

}