using UnityEngine;
using System.Collections;

public class Sound_Control_CS : MonoBehaviour {
	public int Type = 0 ;
	// Engine Sound from Driving Wheels.
	public float Min_Engine_Pitch = 1.0f ;
	public float Max_Engine_Pitch = 2.0f ;
	public float Min_Engine_Volume = 0.1f ;
	public float Max_Engine_Volume = 0.3f ;
	public float Max_Velocity = 7.0f ;
	public Transform Wheel_Transform_L ;
	public Transform Wheel_Transform_R ;
	public Rigidbody Wheel_RigidBody_L ;
	public Rigidbody Wheel_RigidBody_R ;

	float Circumference_L ;
	float Circumference_R ;
	float Engine_Rate ;

	// Impact Sound from MainBody.
	public float Min_Impact = 0.25f ;
	public float Max_Impact = 0.5f ;
	public float Min_Impact_Pitch = 0.3f ;
	public float Max_Impact_Pitch = 1.0f ;
	public float Min_Impact_Volume = 0.1f ;
	public float Max_Impact_Volume = 0.5f ;
	float Last_Velocity ;
	bool  Interval_Flag = true ;
	Rigidbody MainBody_RigidBody ;

	// Turret Motor Sound.
	public float Max_Motor_Volume = 0.5f ;
	Turret_Horizontal_CS Turret_Script ;
	Cannon_Vertical_CS Cannon_Script ;

	AudioSource This_AudioSource ;

	bool Flag = true ;
	int Tank_ID ;

	void Awake () {
		This_AudioSource = GetComponent < AudioSource > () ;
		if ( !This_AudioSource ) {
			Destroy ( this );
		}
		This_AudioSource.playOnAwake = false ;
	}

	void Start () {
		switch ( Type ) {
		case 0 : // Engine Sound.
			This_AudioSource.loop = true ;
			This_AudioSource.volume = 0.0f ;
			This_AudioSource.Play () ;
			// In case of "Physics_Track".
			if ( transform.parent.GetComponentInChildren < Static_Track_CS > () == null ) {
				Set_Reference () ;
			}
			break ;
		case 1 : // Impact Sound.
			This_AudioSource.loop = false ;
			MainBody_RigidBody = GetComponent < Rigidbody > () ;
			break ;
		case 2 : // Turret Motor Sound.
			This_AudioSource.loop = true ;
			Turret_Script = GetComponent < Turret_Horizontal_CS > () ;
			break ;
		case 3 : // Cannon Motor Sound.
			This_AudioSource.loop = true ;
			Cannon_Script = GetComponent < Cannon_Vertical_CS > () ;
			break ;
		}
	}

	// In case of "Physics_Track".
	void Set_Reference () {
		Drive_Wheel_CS [] Drive_Wheel_Scripts = GetComponentsInChildren < Drive_Wheel_CS > () ;
		foreach ( Drive_Wheel_CS Temp_Script in Drive_Wheel_Scripts ) {
			if ( Temp_Script.transform.localEulerAngles.z == 0.0f ) {
				if ( Wheel_Transform_L == null ) {
					Wheel_Transform_L = Temp_Script.transform ;
					Wheel_RigidBody_L = Temp_Script.GetComponent<Rigidbody>() ;
				}
			} else {
				if ( Wheel_Transform_R == null ) {
					Wheel_Transform_R = Temp_Script.transform ;
					Wheel_RigidBody_R = Temp_Script.GetComponent<Rigidbody>() ;
				}
			}
		}
		//
		if ( Wheel_Transform_L && Wheel_Transform_R ) {
			Circumference_L = Wheel_Transform_L.GetComponent < SphereCollider > ().radius * 6.28f ;
			Circumference_R = Wheel_Transform_R.GetComponent < SphereCollider > ().radius * 6.28f ;
		} else {
			Debug.LogWarning ( "Reference Wheels for the engine sound can not be found. (Physics Tank Maker)" ) ;
			Destroy ( this ) ;
		}
	}
	
	// In case of "Static_Track".
	void Get_Static_Track ( Static_Track_CS Temp_Script ) { // Called from "Static_Track".
		if ( Type == 0 ) { // Engine Sound.
			Wheel_Transform_L = Temp_Script.Reference_L ;
			Wheel_Transform_R = Temp_Script.Reference_R ;
			if ( Wheel_Transform_L && Wheel_Transform_R ) {
				Wheel_RigidBody_L = Temp_Script.Reference_L.GetComponent<Rigidbody>() ;
				Wheel_RigidBody_R = Temp_Script.Reference_R.GetComponent<Rigidbody>() ;
				Circumference_L = Temp_Script.Reference_L.GetComponent < SphereCollider > ().radius * 6.28f ;
				Circumference_R = Temp_Script.Reference_R.GetComponent < SphereCollider > ().radius * 6.28f ;
			} else {
				Debug.LogWarning ( "Reference Wheels for the engine sound can not be found. (Physics Tank Maker)" ) ;
				Destroy ( this ) ;
			}
		}
	}

	void FixedUpdate (){
		switch ( Type ) {
		case 0 :
			Engine_Sound () ;
			break ;
		case 1 :
			if ( Interval_Flag ) {
				StartCoroutine ( "Impact_Sound" ) ;
			}
			break ;
		case 2 :
			if ( Flag ) {
				Turret_Motor_Sound () ;
			}
			break ;
		case 3 :
			if ( Flag ) {
				Cannon_Motor_Sound () ;
			}
			break ;
		}
	}

	void Engine_Sound () {
		float Velocity_L ;
		float Velocity_R ;
		if ( Wheel_Transform_L && Wheel_Transform_L.parent ) {
			Velocity_L = Wheel_RigidBody_L.angularVelocity.magnitude / 6.28f * Circumference_L ;
		} else {
			Velocity_L = 0.0f ;
		}
		if ( Wheel_Transform_R && Wheel_Transform_R.parent ) {
			Velocity_R = Wheel_RigidBody_R.angularVelocity.magnitude / 6.28f * Circumference_R ;
		} else {
			Velocity_R = 0.0f ;
		}
		float Target_Rate = ( Velocity_L + Velocity_R ) / 2.0f / Max_Velocity ;
		Engine_Rate = Mathf.MoveTowards ( Engine_Rate , Target_Rate , 0.02f ) ;
		This_AudioSource.pitch = Mathf.Lerp ( Min_Engine_Pitch , Max_Engine_Pitch , Engine_Rate ) ;
		This_AudioSource.volume = Mathf.Lerp ( Min_Engine_Volume , Max_Engine_Volume , Engine_Rate ) ;
	}

	IEnumerator Impact_Sound () {
		float Current_Velocity = MainBody_RigidBody.velocity.y ;
		float Impact = Mathf.Abs ( Last_Velocity - Current_Velocity ) ;
		if ( Impact > Min_Impact ) {
			Interval_Flag = false ;
			float Temp = Impact / Max_Impact ;
			This_AudioSource.pitch = Mathf.Lerp ( Min_Impact_Pitch , Max_Impact_Pitch , Temp ) ;
			This_AudioSource.volume = Mathf.Lerp ( Min_Impact_Volume , Max_Impact_Volume , Temp ) ;
			This_AudioSource.Play () ;
			yield return new WaitForSeconds ( This_AudioSource.clip.length * 0.95f ) ;
			This_AudioSource.volume = 0.0f ;
			Interval_Flag = true ;
		}
		Last_Velocity = MainBody_RigidBody.velocity.y ;
	}
	
	void  Turret_Motor_Sound () {
		if ( Turret_Script.Current_Horizontal != 0.0f ) {
			This_AudioSource.volume = Mathf.Lerp ( 0.0f , Max_Motor_Volume ,Mathf.Abs ( Turret_Script.Current_Horizontal ) ) ;
			if ( This_AudioSource.isPlaying == false ) {
				This_AudioSource.Play () ;
			}
		} else if ( This_AudioSource.volume != 0 ) {
			This_AudioSource.volume = 0 ;
		} else {
			This_AudioSource.Stop () ;
		}
	}
	
	void  Cannon_Motor_Sound () {
		if ( Cannon_Script.Temp_Vertical != 0.0f ) {
			This_AudioSource.volume = Mathf.Lerp ( 0.0f , Max_Motor_Volume ,Mathf.Abs ( Cannon_Script.Temp_Vertical ) ) ;
			if ( This_AudioSource.isPlaying == false ) {
				This_AudioSource.Play () ;
			}
		} else if ( This_AudioSource.volume != 0 ) {
			This_AudioSource.volume = 0 ;
		} else {
			This_AudioSource.Stop () ;
		}
	}

	void Turret_Linkage () { // Called from Turret's "Damage_Control".
		This_AudioSource.Stop () ; // Stop the sound. ( turret and cannon motor )
		Destroy ( this ) ;
	}

	void MainBody_Linkage () { // Called from MainBody's "Damage_Control".
		if ( Type != 1 ) {
			This_AudioSource.Stop () ; // Stop the sound. ( all sounds except for Impact )
			Destroy ( this ) ;
		}
	}

	void Set_Tank_ID ( int Temp_Tank_ID ) {
		Tank_ID = Temp_Tank_ID ;
	}
	
	void Receive_Current_ID ( int Temp_Current_ID ) {
		if ( Temp_Current_ID == Tank_ID ) {
			Flag = true ;
		} else {
			Flag = false ;
		}
	}

}