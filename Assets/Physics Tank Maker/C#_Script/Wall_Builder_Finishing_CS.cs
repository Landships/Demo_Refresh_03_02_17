using UnityEngine;
using System.Collections;

public class Wall_Builder_Finishing_CS : MonoBehaviour {
	
	public float Mass = 10.0f ;

	void Start () {
		if ( GetComponent < Rigidbody > () ==null ) {
			Rigidbody Temp_Rigidbody = gameObject.AddComponent < Rigidbody > () ;
			Temp_Rigidbody.mass = Mass ;
			gameObject.name = "Block(Work)" ;
		}
		Destroy ( this ) ;
	}
	
	public void Set_Mass ( float Temp_Mass ) {
		Mass = Temp_Mass ;
	}
}