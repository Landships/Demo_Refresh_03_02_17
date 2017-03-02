using UnityEngine;
using System.Collections;

[ ExecuteInEditMode ]

public class Create_SwingBall_CS : MonoBehaviour {
	
	public float Distance = 2.7f ;
	public int Num = 1 ;
	public float Spacing = 1.7f ;
	public float Mass = 10.0f ;
	public bool Gravity = false ;
	public float Radius = 0.1f ;
	public float Range = 0.3f ;
	public float Spring = 500.0f ;
	public float Damper = 100.0f ;
	public int Layer = 0 ;
	public PhysicMaterial Collider_Material ;

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