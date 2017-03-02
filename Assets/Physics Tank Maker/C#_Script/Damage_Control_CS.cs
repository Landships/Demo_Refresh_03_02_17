using UnityEngine;
using System.Collections;

public class Damage_Control_CS : MonoBehaviour {
	
	public int Type = 1 ;
	public float Mass = 200.0f ; // for Turret.
	public int Direction ; // for Tracks, Subjoints, Wheels and StaticTrack_Collider. 0=Left, 1=Right.
	public float Durability = 130000.0f ;
	public float Sub_Durability = 100000.0f ;
	public float Trouble_Time = 20.0f ; 
	public bool Coming_Off = true ; // for Turret
	public GameObject Damage_Effect_Object ; // for Turret
	public GameObject Trouble_Effect_Object ; // for Turret, Cannon and Barrel.
	public int Turret_Number = 1 ; // for MainBody.
	public Transform Linked_Transform ; // for StaticTrack_Collider.

	bool Live_Flag = true ;

	Transform MainBody_Transform ; // for Turret and StaticTrack_Collider.
	Transform Parent_Transform ;
	Cannon_Fire_CS  Cannon_Fire_Script ; // for Cannon and Barrel.
	Turret_Horizontal_CS  Turret_Horizontal_Script ; // for Turret.
	bool Child_Turret_Flag = false ; // for Turret.
	Rigidbody This_Rigidbody ; // for MainBody and Wheels.
	Stabilizer_CS Stabilizer_Script ; // for Tracks and Wheels.
	SphereCollider Wheel_SphereCollider ; // for Wheels.
	Drive_Wheel_CS Drive_Wheel_Script ; // for Wheels.

    public int ai_id = 0;

	//added TankLives
	public TankLives lives;

    void Set_Ai_Id(int id)
    {
        ai_id = id;
    }

    void Alert(int type)
    {
        if (type == Type)
        {
            Penetration();
        }
    }

    void Start (){
		switch ( Type ) {
		case 1 : // Armor_Collider
			Parent_Transform = transform.parent ;
			Renderer Temp_Renderer = GetComponent < Renderer >() ;
			if ( Temp_Renderer ) {
				Temp_Renderer.enabled = false ;
			}
			break ;
		case 2 : // Turret
			break ;
		case 3 : // Cannon
			break ;
		case 4 : // Barrel
			break ;
		case 5 : // MainBody
			Parent_Transform = transform.parent ;
			This_Rigidbody = GetComponent < Rigidbody > () ;
			break ;
		case 6 : // Track
			Stabilizer_Script = GetComponent < Stabilizer_CS > () ;
			Parent_Transform = transform.parent ;
			MainBody_Transform = GetComponentInParent < MainBody_Setting_CS > ().transform ;
			break ;
		case 7 : // SubJoint ( Reinforce piece ).
			break ;
		case 8 : // Wheel
			This_Rigidbody = GetComponent < Rigidbody > () ;
			Stabilizer_Script = GetComponent < Stabilizer_CS > () ;
			Drive_Wheel_Script = GetComponent < Drive_Wheel_CS > () ;
			Wheel_SphereCollider = GetComponent < SphereCollider > () ;
			break ;
		case 9 : // StaticTrack_Collider
			Collider Temp_Collider = GetComponent < Collider > () ;
			if ( Temp_Collider ) {
				Temp_Collider.isTrigger = true ;
			}
			MainBody_Transform = GetComponentInParent < MainBody_Setting_CS > ().transform ;
			Parent_Transform = transform.parent ;
			Temp_Renderer = GetComponent < Renderer >() ;
			if ( Temp_Renderer ) {
				Temp_Renderer.enabled = false ;
			}
			if ( Linked_Transform ) {
				if ( Linked_Transform.localPosition.y > 0.0f ) {
					Direction = 0 ;
				} else {
					Direction = 1 ;
				}
			}
			break ;
		}
		lives = transform.root.GetChild (0).gameObject.GetComponent<TankLives> ();
	}

	void Complete_Turret () { // Called from 'Turret_Finishing".
		switch ( Type ) {
		case 2 : // Turret
			MainBody_Transform = GetComponentInParent < MainBody_Setting_CS > ().transform ;
			Parent_Transform = transform.parent ; // Turret_Base
			Turret_Horizontal_Script = Parent_Transform.GetComponent < Turret_Horizontal_CS > () ;
			break ;
		case 3 : // Cannon
			Parent_Transform = transform.parent ; // Cannon_Base
			Cannon_Fire_Script = Parent_Transform.GetComponent < Cannon_Fire_CS > () ;
			break ;
		case 4 : // Barrel
			Parent_Transform = transform.parent.parent ; // Cannon_Base
			Cannon_Fire_Script = Parent_Transform.GetComponent < Cannon_Fire_CS > () ;
			break ;
		}
	}

	public bool Breaker ( float Hit_Energy ) { 
        Debug.Log("is this hitting? Hit = " + Hit_Energy + " Durability = " + Durability + "ai id = " + ai_id);
        if (Hit_Energy >= Durability)
        {
            Debug.Log("PENETRATION");
            if (ai_id == 0) // own tank
            {
                Debug.Log("Player Penetration");
                transform.root.GetChild(0).GetComponent<Turret_Controller_VR>().Alert_Penetration(Type);
            }
            else // ai on authority
                transform.root.GetChild(0).GetComponent<AI_Controller_VR>().Alert_Penetration(ai_id, Type);

            return true;
        }
        else {
            return false;
        }
		/*
		if ( Hit_Energy >= Durability ) {
            Penetration();
			return true ;
		} else if ( Hit_Energy >= Sub_Durability ) {
            Trouble();
			return false ;
		} else {
			return false ;
		}*/
	}

    
	public void  Penetration () {

		switch ( Type ) {
		case 1 : // Armor_Collider
			Armor_Collider_Broken () ;
			break ;
		case 2 : // Turret
			Turret_Broken () ;
			break ;
		case 3 : // Cannon
			// Send Message to Turret's "Damage_Control".
			Parent_Transform.parent.BroadcastMessage ( "Turret_Broken" , SendMessageOptions.DontRequireReceiver ) ;
			Destroy ( this ) ;
			break ;
		case 4 : // Barrel
			// Send Message to Turret's "Damage_Control". ( Barrel's 'Parent_Transform' is set to Cannon_Base.)
			Parent_Transform.parent.BroadcastMessage ( "Turret_Broken" , SendMessageOptions.DontRequireReceiver ) ;
			Destroy ( this ) ;
			break ;
		case 5 : // MainBody
			MainBody_Broken () ;
			break ;
		case 6 : // Track
			Track_Broken () ;
			break ;
		case 7 : // SubJoint
			break ;
		case 8 : // Wheel
			Wheel_Broken () ;
			break ;
		case 9 : // StaticTrack_Collider
			StaticTrack_Broken () ;
			break ;
		}
	}

    
	void Trouble () {
		switch ( Type ) {
		case 1 : // Armor_Collider
			break ;
		case 2 : // Turret
			Turret_Trouble () ;
			break ;
		case 3 : // Cannon
			Cannon_Trouble () ;
			break ;
		case 4 : // Barrel
			Cannon_Trouble () ;
			break ;
		case 5 : // MainBody
			break ;
		case 6 : // Track
			break ;
		case 7 : // SubJoint
			break ;
		case 8 : // Wheel
			break ;
		case 9 : // StaticTrack_Collider
			break ;
		}
	}

	void Cannon_Trouble () {
		if ( Parent_Transform ) {
			if ( Cannon_Fire_Script ) {
				// Check 'Trouble_Flag' in "Cannon_Fire".
				if ( Cannon_Fire_Script.Trouble ( Trouble_Time ) ) {
					Create_Trouble_Effect () ;
				}
			}
		}
	}

	void Turret_Trouble () {
		if ( Parent_Transform ) {
			if ( Turret_Horizontal_Script ) {
				// Check 'Trouble_Flag' in "Cannon_Fire".
				if ( Turret_Horizontal_Script.Trouble ( Trouble_Time ) ) {
					// Create Trouble Effect.
					Create_Trouble_Effect () ;
				}
			}
		}
	}

	void Create_Trouble_Effect () {
		GameObject Temp_Object ;
		if ( Trouble_Effect_Object ) {
			Temp_Object = Instantiate ( Trouble_Effect_Object , Parent_Transform.position , Parent_Transform.rotation ) as GameObject;
			Temp_Object.transform.parent = Parent_Transform ;
			Temp_Object.SendMessage ( "Delete_Timer" , Trouble_Time ) ;
		} else {
			Temp_Object = null ;
		}
	}

	void Armor_Collider_Broken () {
		if ( Live_Flag ) {
			Live_Flag = false ;
			if ( Parent_Transform ) {
				Parent_Transform.SendMessage ( "Penetration" , SendMessageOptions.DontRequireReceiver ) ;
			}
			Destroy ( this.gameObject ) ;
		}
	}

	void Turret_Broken () {
		if ( Type == 2 && Live_Flag ) { // Turret
			Live_Flag = false ;
			// Create Damage Effect.
			if ( Damage_Effect_Object ) {
				GameObject Temp_Object = Instantiate ( Damage_Effect_Object , Parent_Transform.position , Parent_Transform.rotation ) as GameObject;
				Temp_Object.transform.parent = MainBody_Transform ;
			}
			// Send Message to "Damage_Control" in the MainBody.
			if ( Child_Turret_Flag == false ) {
				MainBody_Transform.SendMessage ( "Reduce_Turret_Number" , SendMessageOptions.DontRequireReceiver ) ;
			}
			// Send Message to "Turret_Horizontal", "Cannon_Vertical", "Cannon_Fire", "Recoil_Brake", "Look_At_Point", "Gun_Camera", "Sound_Control", "Damage_Control" in cannon and barrel.
			Parent_Transform.BroadcastMessage ( "Turret_Linkage" , SendMessageOptions.DontRequireReceiver ) ;
			// Coming off.
			if ( Coming_Off ) {
				// Add RigidBody to the "Turret_Base".
				if ( !Parent_Transform.GetComponent < Rigidbody > () ) {
					Rigidbody Temp_Parent_Rigidbody = Parent_Transform.gameObject.AddComponent < Rigidbody > () ;
					Temp_Parent_Rigidbody.mass = Mass ;
				}
				// Change the hierarchy.
				Parent_Transform.parent = MainBody_Transform.parent ;
			}
			//
			Destroy ( this ) ;
		}
	}

	void Turret_Linkage () {
		if ( Type == 1 && Live_Flag ) { // Armor_Collider.
			Destroy ( this.gameObject ) ;
		} else if ( Type == 3 || Type == 4 ) { // Cannon or Barrel.
			Destroy ( this ) ;
		}
	}

	void Reduce_Turret_Number () {
		if ( Type == 5 && Live_Flag ) { // MainBody.
			Turret_Number -= 1 ;
			if ( Turret_Number <=0 ) {
				MainBody_Broken () ;
			}
		}
	}

	void MainBody_Broken () {
		Live_Flag = false ;
		// Send Message to Turret's "Damage_Control".
		BroadcastMessage ( "Turret_Broken" , SendMessageOptions.DontRequireReceiver ) ;
		// Stop Wheels.
		BroadcastMessage ( "Wheel_Linkage" , 0 , SendMessageOptions.DontRequireReceiver ) ;
		BroadcastMessage ( "Wheel_Linkage" , 1 , SendMessageOptions.DontRequireReceiver ) ;
		// Send Message to "Tank_ID_Control", "Sound_Control", Armor_Collider, "AI", "Drive_Control", "Steer_Wheel", "PT_ParkingBrake".
		Parent_Transform.BroadcastMessage ( "MainBody_Linkage" , SendMessageOptions.DontRequireReceiver ) ;
		// Add NavMeshObstacle.
		if ( gameObject.GetComponent < UnityEngine.AI.NavMeshObstacle > () == null ) {
			UnityEngine.AI.NavMeshObstacle Temp_NavMeshObstacle = gameObject.AddComponent < UnityEngine.AI.NavMeshObstacle > () ;
			Temp_NavMeshObstacle.carvingMoveThreshold = 1.0f ;
			Temp_NavMeshObstacle.carving = true ;
		}
		/// Release the parking brake, and Destroy this script.
		StartCoroutine ( "Disable_Constraints" ) ;
	}

	IEnumerator Disable_Constraints () {
		yield return new WaitForFixedUpdate () ;
		This_Rigidbody.constraints = RigidbodyConstraints.None ;
		Destroy ( this ) ;
	}

	void  MainBody_Linkage () {
		if ( Type == 1 && Live_Flag ) { // Armor_Collider.
			Destroy ( this.gameObject ) ;
		}
	}

	void Track_Broken () {
		HingeJoint Temp_HingeJoint = GetComponent < HingeJoint > () ;
		if ( Temp_HingeJoint ) {
			Destroy ( Temp_HingeJoint ) ;
		}
		if ( Live_Flag && Parent_Transform ) {
			// Send message to Damage_Control in the track pieces and SubJoints (Reinforce), and PT_ParkingBrake_CS and Track_Interpolation_CS.
			Parent_Transform.BroadcastMessage ( "Track_Linkage" , Direction , SendMessageOptions.DontRequireReceiver ) ;
			// Send message to the wheels from the MainBody.
			MainBody_Transform.BroadcastMessage ( "Wheel_Linkage" , Direction , SendMessageOptions.DontRequireReceiver ) ;
		}
	}

	IEnumerator Track_Linkage ( int Temp_Direction ) {
		if ( Temp_Direction == Direction ) {
			if ( Type == 6 ) { // Track.
				Live_Flag = false ;
				if ( Stabilizer_Script ) {
					Destroy ( Stabilizer_Script ) ;
				}
				yield return new WaitForSeconds ( 1.0f ) ;
				transform.parent = null ;
				Destroy ( this.gameObject , 20.0f ) ;
			} else if ( Type == 7 ) { // SubJoint.
				Destroy ( this.gameObject ) ;
			}
		}
	}
	
	void SubJoint_Linkage ( int Temp_Direction ) {
		if ( Type == 7 && Temp_Direction == Direction ) { // SubJoint.
			Destroy ( this.gameObject ) ;
		}
	}

	void Wheel_Broken () {
		HingeJoint Temp_HingeJoint = GetComponent < HingeJoint > () ;
		if ( Temp_HingeJoint ) {
			Destroy ( Temp_HingeJoint ) ;
		}
		if ( Stabilizer_Script ) {
			Destroy ( Stabilizer_Script ) ;
		}
		if ( Drive_Wheel_Script ) {
			Destroy ( Drive_Wheel_Script ) ;
		}
		// Release the brake.
		if ( This_Rigidbody ) {
			This_Rigidbody.angularDrag = 0.05f ;
		}
		// Switch the collider.
		MeshCollider[] Temp_MeshColliders = GetComponents < MeshCollider > () ;
		if ( Temp_MeshColliders.Length != 0 ) {
			if ( Wheel_SphereCollider ) {
				Destroy ( Wheel_SphereCollider ) ;
			}
			foreach ( MeshCollider Temp_MeshCollider in Temp_MeshColliders ) {
				Temp_MeshCollider.enabled = true ;
			}
		}
		gameObject.layer = 0 ;
		transform.parent = null ;
		Destroy ( this.gameObject , 20.0f ) ;
	}
	
	void Wheel_Linkage ( int Temp_Direction ) {
		if ( Type == 8 && Temp_Direction == Direction ) { // Wheel.
			if ( Drive_Wheel_Script ) {
				Destroy ( Drive_Wheel_Script ) ;
			}
			// Lock the wheels.
			if ( This_Rigidbody ) {
				This_Rigidbody.angularDrag = Mathf.Infinity ; // Constraint spin.
			}
		}
	}

	void Wheel_Linkage_for_StaticTrack ( int Temp_Direction ) {
		if ( Type == 8 && Temp_Direction == Direction ) { // Wheel.
			if ( Drive_Wheel_Script ) {
				Destroy ( Drive_Wheel_Script ) ;
			}
			// Lock the wheels.
			if ( This_Rigidbody ) {
				This_Rigidbody.angularDrag = Mathf.Infinity ; // Constraint spin.
			}
			// Switch the collider.
			MeshCollider[] Temp_MeshColliders = GetComponents < MeshCollider > () ;
			if ( Temp_MeshColliders.Length != 0 ) {
				if ( Wheel_SphereCollider ) {
					Destroy ( Wheel_SphereCollider ) ;
				}
				foreach ( MeshCollider Temp_MeshCollider in Temp_MeshColliders ) {
					Temp_MeshCollider.enabled = true ;
				}
			}
			// Remove useless script.
			Static_Wheel_CS Temp_Script = GetComponent < Static_Wheel_CS > () ;
			if ( Temp_Script ) {
				Destroy ( Temp_Script ) ;
			}
		}
	}

	void StaticTrack_Broken () {
		// Stop Wheels on the same side.
		MainBody_Transform.BroadcastMessage ( "Wheel_Linkage_for_StaticTrack" , Direction , SendMessageOptions.DontRequireReceiver ) ;
		// Send Message to "Static_Track" script in the Linked piece.
		if ( Linked_Transform ) {
			Linked_Transform.SendMessage ( "StaticTrack_Linkage" , SendMessageOptions.DontRequireReceiver ) ;
		}
		// Destroy other StaticTrack_Colliders on the same side.
		if ( Parent_Transform ) {
			Parent_Transform.BroadcastMessage ( "Destroy_StaticTrack_Collider" , Direction , SendMessageOptions.DontRequireReceiver ) ;
		}
		//
		Destroy ( this.gameObject ) ;
	}

	void Destroy_StaticTrack_Collider ( int Temp_Direction ) {
		if ( Type == 9 && Temp_Direction == Direction) {
			Destroy ( this.gameObject ) ;
		}
	}
	
	void OnJointBreak () { // for physics track.
		if ( Type == 6 ) { // Track.
			Track_Broken () ;
		}
	}
	
	void Child_Turret () {
		Child_Turret_Flag = true ;
	}
	
}