using UnityEngine;
using System.Collections;

public class Trigger_Collider_CS : MonoBehaviour {

	public Transform Collide_Transform ;

	void Awake () {
		this.gameObject.layer = 2 ; // Ignore Raycast.
		Collider [] Temp_Colliders = GetComponents < Collider > () ;
		for ( int i = 0 ; i < Temp_Colliders.Length ; i++ ) {
			Temp_Colliders [ i ].isTrigger = true ;
		}
		MeshRenderer Temp_MeshRenderer = GetComponent < MeshRenderer > () ;
		if ( Temp_MeshRenderer ) {
			Temp_MeshRenderer.enabled = false ;
		}
	}

	void OnTriggerEnter ( Collider Temp_Collider ) {
		Collide_Transform = Temp_Collider.transform.root ;
	}

	void OnTriggerExit () {
		Collide_Transform = null ;
	}

}
