using UnityEngine;
using System.Collections;

public class Wheel_Resize_CS : MonoBehaviour {
	
	public float ScaleDown_Size = 0.5f ;
	public float Return_Speed = 0.05f ;
	
	bool Small_Flag ;
	bool Flag = false ;

	void Start () {
		if ( ScaleDown_Size <= 1.0f ) {
			Small_Flag = true ; // from small to original
		} else {
			Small_Flag = false ; // from large to original
		}
		//
		transform.localScale = new Vector3 ( ScaleDown_Size , ScaleDown_Size , ScaleDown_Size ) ;
		Flag = true ;
	}

	void FixedUpdate () {
		if ( Flag ) {
			transform.localScale = new Vector3 ( ScaleDown_Size , ScaleDown_Size , ScaleDown_Size ) ;
			if ( Small_Flag ) {
				if ( ScaleDown_Size >= 1.0f ) {
					transform.localScale = Vector3.one ;
					Destroy ( this ) ;
				} else {
					ScaleDown_Size += Return_Speed ;
				}
			} else {
				if ( ScaleDown_Size <= 1.0f ) {
					transform.localScale = Vector3.one ;
					Destroy ( this ) ;
				} else {
					ScaleDown_Size -= Return_Speed ;
				}
			}
		}
	}

	public void Set_Value ( float Size_Value , float Speed_Value ) {
		ScaleDown_Size = Size_Value ;
		Return_Speed = Speed_Value ;
	}
}