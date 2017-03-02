using UnityEngine;
using System.Collections;

[ ExecuteInEditMode ]

public class Create_SupportWheel_CS : MonoBehaviour {

	public float Wheel_Distance = 2.7f ;
	public int Num = 3 ;
	public float Spacing = 1.7f ;
	public float Wheel_Mass = 20.0f ;
	public float Wheel_Radius = 0.21f ;
	public PhysicMaterial Collider_Material ;
	public Mesh Wheel_Mesh ;
	public Material Wheel_Material ;
	public Mesh Collider_Mesh ;
	public Mesh Collider_Mesh_Sub ;
	public bool Drive_Wheel = true ;
	public bool Wheel_Resize = false ;
	public float ScaleDown_Size = 0.5f ;
	public float Return_Speed = 0.05f ;
	public float Wheel_Durability = 55000.0f ;
	public bool Static_Flag = false ;
	public float Radius_Offset ;

	public bool RealTime_Flag = false ;
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