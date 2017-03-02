using UnityEngine;
using System.Collections;

public class Recoil_Brake_CS : MonoBehaviour {
	
	public float Recoil_Time = 0.25f ;
	public float Return_Time = 0.45f ;
	public float Recoil_Length = 0.2f ;

	int Barrel_Type = 0 ;
	float Temp_Time = 0.0f ;
	bool Brake_Flag = false ;
	bool Return_Flag = false ;
	Vector3 Default_Position ;
	
	const float PI = 3.14f ;

	void Complete_Turret () { // Called from 'Turret_Finishing".
		Default_Position = transform.localPosition ;
	}

	void Update () {
		if ( Brake_Flag ) {
			if ( Temp_Time <= Recoil_Time ) {
				float Temp_Position = Mathf.Sin ( ( PI / 2 ) * ( Temp_Time / Recoil_Time ) ) ;
				transform.localPosition = new Vector3 ( Default_Position.x , Default_Position.y , Default_Position.z - ( Temp_Position * Recoil_Length ) ) ;
				Temp_Time += Time.deltaTime ;
			} else {
				Brake_Flag = false ;
				Return_Flag = true ;
				Temp_Time = 0.0f ;
			}
		}
		//
		if ( Return_Flag ) {
			if ( Temp_Time <= Return_Time ) {
				float Temp_Position = Mathf.Sin ( ( PI / 2 ) * ( Temp_Time / Return_Time ) + ( PI / 2 ) ) ;
				transform.localPosition = new Vector3 ( Default_Position.x , Default_Position.y ,Default_Position.z - ( Temp_Position * Recoil_Length ) ) ;
				Temp_Time += Time.deltaTime ;
			} else {
				transform.localPosition = Default_Position ;
				Return_Flag = false ;
				Temp_Time = 0 ;
			}
		}
	}
	
	void Fire_Linkage ( int Select_LR ){
		if ( Recoil_Time == 0 || Return_Time == 0 ) {
			Brake_Flag = false ;
		} else if ( Barrel_Type == 0 || Barrel_Type == Select_LR ) {
			Brake_Flag = true ;
		} else {
			Brake_Flag = false ;
		}
	}

	void Turret_Linkage () {
		Destroy ( this ) ;
	}

	void Set_Barrel_Type ( int Temp_Type ) { // Called from "Barrel_Base".
		Barrel_Type = Temp_Type ;
	}
	
}