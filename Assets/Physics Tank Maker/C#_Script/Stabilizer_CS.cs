using UnityEngine;
using System.Collections;

public class Stabilizer_CS : MonoBehaviour {


	Transform This_Transform ;
	float Default_Pos ;
	Vector3 Default_Ang ;
	
	void Start () {
		This_Transform = transform ;
		Default_Pos = This_Transform.localPosition.y ;
		Default_Ang = This_Transform.localEulerAngles ;
	}

	void Update () {
		// Stabilize position.
		float Temp_X = This_Transform.localPosition.x ;
		float Temp_Z = This_Transform.localPosition.z ;
		This_Transform.localPosition = new Vector3 ( Temp_X , Default_Pos , Temp_Z ) ;
		// Stabilize angle.
		float Temp_Y = This_Transform.localEulerAngles.y ;
		This_Transform.localEulerAngles = new Vector3 ( Default_Ang.x , Temp_Y , Default_Ang.z ) ;
	}

}
