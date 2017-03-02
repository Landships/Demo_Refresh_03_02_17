using UnityEngine;
using System.Collections;

[ RequireComponent ( typeof ( MeshFilter ) ) ]
[ RequireComponent ( typeof ( MeshRenderer ) ) ]
[ RequireComponent ( typeof ( Rigidbody ) ) ]

public class MainBody_Setting_CS : MonoBehaviour {

	public float Body_Mass = 2000.0f ;
	public Mesh Body_Mesh ;

	public int Materials_Num = 1 ;
	public Material[] Materials ;
	public Material Body_Material ;

	public Mesh Collider_Mesh ;
	public Mesh Sub_Collider_Mesh ;
	public float Durability = 150000.0f ;
	public int Turret_Number = 1 ;

	public int SIC = 14 ;
	public bool Soft_Landing_Flag ;
	public float Landing_Drag = 20.0f ;
	public float Landing_Time = 1.5f ;

	public float AI_Upper_Offset = 1.5f ;
	public float AI_Lower_Offset = 0.3f ;

	// Referred to Static_Track.
	public bool Visible_Flag ;
	
	Rigidbody This_Rigidbody ;

	void Start () {
		// Layer Collision Settings.
		Layer_Collision_Settings () ;
		// Solver Iteration Count setting.
		This_Rigidbody = GetComponent < Rigidbody > () ;
		This_Rigidbody.solverIterations = SIC ;
		// Attach NavMeshObstacle for Bunker Tank.
		if ( This_Rigidbody.isKinematic ) {
			UnityEngine.AI.NavMeshObstacle Temp_NavMeshObstacle = gameObject.AddComponent < UnityEngine.AI.NavMeshObstacle > () ;
			Temp_NavMeshObstacle.carvingMoveThreshold = 1.0f ;
			Temp_NavMeshObstacle.carving = true ;
		}
		// Soft Landing.
		if ( Soft_Landing_Flag ) {
			StartCoroutine ( "Soft_Landing" ) ;
		}
	}

	void Layer_Collision_Settings () {
		// Layer Collision Settings.
		// Layer8 >> for CrossHair Image.
		// Layer9 >> for wheels.
		// Layer10 >> for Suspensions and Track Reinforce.
		// Layer11 >> for MainBody.
		for ( int i =0 ; i <= 11 ; i ++ ) {
			Physics.IgnoreLayerCollision ( 9 , i , false ) ;
			Physics.IgnoreLayerCollision ( 11 , i , false ) ;
		}
		Physics.IgnoreLayerCollision ( 9 , 9 , true ) ; // Wheels ignore each other.
		Physics.IgnoreLayerCollision ( 9 , 11 , true ) ; // Wheels ignore MainBody.
		for ( int i =0 ; i <= 11 ; i ++ ) {
			Physics.IgnoreLayerCollision ( 10 , i , true ) ; // Suspensions and Track Reinforce are ignore all.
		}
	}

	IEnumerator Soft_Landing () {
		float Default_Drag= This_Rigidbody.drag ;
		This_Rigidbody.drag = Landing_Drag ;
		This_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation ;
		yield return new WaitForSeconds ( Landing_Time ) ;
		This_Rigidbody.drag = Default_Drag ;
		This_Rigidbody.constraints = RigidbodyConstraints.None ;
	}

	void OnBecameVisible () {
		Visible_Flag = true ;
	}
	
	void OnBecameInvisible () {
		Visible_Flag = false ;
	}

}