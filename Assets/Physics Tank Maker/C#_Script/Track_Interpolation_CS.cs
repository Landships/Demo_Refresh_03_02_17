using UnityEngine;
using System.Collections;

public class Track_Interpolation_CS : MonoBehaviour {
	
	public Transform Base_Transform ;
	public Transform Front_Transform ;
	public float Joint_Offset ;
	public float Broken_Offset ;
	public int Direction ; // 0=Left, 1=Right.

	MainBody_Setting_CS MainBody_Script ;
	Transform This_Transform ;

	void Start () {
		This_Transform = transform ;
		MainBody_Script = GetComponentInParent < MainBody_Setting_CS > () ;
	}
	
	void Update () {
		if ( MainBody_Script.Visible_Flag ) { // MainBody is visible by any camera.
			Vector3 Base_Pos = Base_Transform.position + ( Base_Transform.forward * Joint_Offset ) ;
			Vector3 Front_Pos = Front_Transform.position - ( Front_Transform.forward * Joint_Offset ) ;
			This_Transform.position = Vector3.Slerp ( Base_Pos , Front_Pos , 0.5f ) ;
			This_Transform.rotation = Quaternion.Slerp ( Base_Transform.rotation , Front_Transform.rotation , 0.5f ) ;
		}
	}

	void Track_Linkage ( int Temp_Direction ) {
		if ( Temp_Direction == Direction ) {
			This_Transform.localPosition = new Vector3 ( 0.0f , 0.0f , Broken_Offset ) ;
			This_Transform.localEulerAngles = Vector3.zero ;
			Destroy ( this ) ;
		}
	}
	
	public void Set_Value ( Transform Temp_Base_Transform , Transform Temp_Front_Transform ,  float Temp_Joint_Offset , float Temp_Spacing , string Temp_Direction ) {
		Base_Transform = Temp_Base_Transform ;
		Front_Transform = Temp_Front_Transform ;
		Joint_Offset = Temp_Joint_Offset ;
		Broken_Offset = Temp_Spacing / 2 ;
		if ( Temp_Direction == "L" ) {
			Direction = 0 ;
		} else {
			Direction = 1 ;
		}
	}

}