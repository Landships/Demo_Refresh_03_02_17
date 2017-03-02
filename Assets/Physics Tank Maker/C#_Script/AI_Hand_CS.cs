using UnityEngine;
using System.Collections;

public class AI_Hand_CS : MonoBehaviour {

	public bool Work_Flag = false ; // Referred to from AI_CS.
	bool Touch_Flag = false ;
	float Count ;
	Collider Touch_Collider ;
	AI_CS AI_Script ;

	void Start () {
		gameObject.layer = 2 ; // "Ignore Raycast" layer.
		Renderer Temp_Renderer = GetComponent < Renderer > () ;
		if ( Temp_Renderer ) {
			Temp_Renderer.enabled = false ;
		}
		Collider Temp_Collider = GetComponent < Collider > () ;
		if ( Temp_Collider ) {
			Temp_Collider.isTrigger = true ;
		}
		// Remove Rigidbody for old versions.
		Rigidbody Temp_Rigidbody = GetComponent < Rigidbody > () ;
		if ( Temp_Rigidbody ) {
			Destroy ( Temp_Rigidbody ) ;
		}
	}

	void Update () {
		if ( Work_Flag ) {
			if ( Touch_Flag ) {
				if ( Touch_Collider == null ) { // The touched tank may be removed by respawn.
					Touch_Flag = false ;
					return ;
				}
				Count += Time.deltaTime ;
				if ( Count > AI_Script.Stuck_Count ) {
					AI_Script.Escape_Stuck () ;
					Count = 0.0f ;
				}
				return ;
			} else {
				Count -= Time.deltaTime ;
				if ( Count < 0.0f ) {
					Count = 0.0f ;
					Work_Flag = false ;
				}
			}
		}
	}

	void OnTriggerStay ( Collider Temp_Collider ) {
		if ( !Touch_Flag && Temp_Collider.attachedRigidbody ) {
			if ( Temp_Collider.transform.root.tag != "Finish" ) {
				Work_Flag = true ;
				Touch_Flag = true ;
				Touch_Collider = Temp_Collider ;
			}
		}
	}
	
	void OnTriggerExit () {
		Touch_Flag = false ;
	}

	void Get_AI ( AI_CS Temp_Script ) {
		AI_Script =Temp_Script ;
	}
	
}
