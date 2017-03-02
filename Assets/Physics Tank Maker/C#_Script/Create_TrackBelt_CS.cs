using UnityEngine;
using System.Collections;

[ ExecuteInEditMode ]

public class Create_TrackBelt_CS : MonoBehaviour {
	public bool Rear_Flag = false ;
	public int SelectedAngle = 3600 ;
	public int Angle_Rear = 4500 ;
	public int Number_Straight = 17 ;
	public float Spacing = 0.3f ;
	public float Distance = 2.7f ;
	public float Track_Mass = 30.0f ;
	public Bounds Collider_Info = new Bounds ( new Vector3 ( 0.0f , -0.016f , 0.0f ) , new Vector3 ( 0.65f , 0.08f , 0.3f ) ) ;
	public PhysicMaterial Collider_Material ;
	public Mesh Track_R_Mesh ;
	public Mesh Track_L_Mesh ;
	public Material Track_R_Material ;
	public Material Track_L_Material ;

	public int SubJoint_Type = 1 ;
	public float Reinforce_Radius = 0.3f ;

	public bool Use_Interpolation = false ;
	public float Joint_Offset ;
	public Mesh Interpolation_R_Mesh ;
	public Mesh Interpolation_L_Mesh ;
	public Material Interpolation_R_Material ;
	public Material Interpolation_L_Material ;

	public float Special_Offset ;

	public float Track_Durability = 55000.0f ;
	public float BreakForce = 5000.0f ;

	public bool RealTime_Flag = true ;
	public bool Static_Flag = false ;
	public bool Prefab_Flag = false ;

	public Transform Parent_Transform ;

	void Awake () {
		// for Unity 5.1
		if ( GetComponentInChildren < Rigidbody > () == false ) {
			Finishing ( "L" ) ;
			Finishing ( "R" ) ;
		}
	}

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
			if ( Static_Flag ) {
				Rigidbody MainBody_RigidBody = transform.parent.GetComponent < Rigidbody > () ;
				MainBody_RigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ ;
				MainBody_RigidBody.drag = 15.0f ;
			} else {
				Destroy ( this ) ;
			}
		}
	}
	
	void Reset () {
		Start () ;
	}

	void Finishing ( string Direction ) {
		// Add RigidBody and BoxCollider.
		for ( int i = 1 ;  i <= transform.childCount ; i++ ) {
			Transform Temp_Transform = transform.Find ( "TrackBelt_" + Direction + "_" + i ) ;
			if ( Temp_Transform ) {
				GameObject Temp_Object = Temp_Transform.gameObject ;
				// Add RigidBody.
				if ( Temp_Object.GetComponent<Rigidbody>() == null ) {
					Rigidbody Temp_RigidBody = Temp_Object.AddComponent < Rigidbody > () ;
					Temp_RigidBody.mass = Track_Mass ;
				}
				// Special Offset for Unity5.
				if ( i % 2 == 0 ) {
					Temp_Transform.position += Temp_Transform.forward * Special_Offset ;
				}
			}
		}
		// Add HingeJoint.
		for ( int i = 1 ;  i <= transform.childCount ; i++ ) {
			Transform Temp_Transform = transform.Find ( "TrackBelt_" + Direction + "_" + i ) ;
			if ( Temp_Transform ) {
				GameObject Temp_Object = Temp_Transform.gameObject ;
				if ( Temp_Object.GetComponent<HingeJoint>() == null ) {
					HingeJoint Temp_HingeJoint ;
					Temp_HingeJoint = Temp_Object.AddComponent < HingeJoint > () ;
					Temp_HingeJoint.anchor = new Vector3 ( 0.0f , 0.0f , Spacing / 2.0f ) ;
					Temp_HingeJoint.axis = new Vector3 ( 1.0f , 0.0f , 0.0f ) ;
					Temp_HingeJoint.breakForce = BreakForce ;
					Transform Temp_Connected_Transform ;
					Temp_Connected_Transform = transform.Find ( "TrackBelt_" + Direction + "_" + ( i + 1 ) ) ;
					if ( Temp_Connected_Transform ) {
						if ( Temp_Connected_Transform.GetComponent<Rigidbody>() ) {
							Temp_HingeJoint.connectedBody = Temp_Connected_Transform.GetComponent<Rigidbody>() ;
						}
					} else {
						Temp_Connected_Transform = transform.Find ( "TrackBelt_" + Direction + "_1" ) ;
						if ( Temp_Connected_Transform ) {
							if ( Temp_Connected_Transform.GetComponent<Rigidbody>() ) {
								Temp_HingeJoint.connectedBody = Temp_Connected_Transform.GetComponent<Rigidbody>() ;
							}
						}
					}
				}
			}
		}
	}
}
	
