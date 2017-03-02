using UnityEngine;
using System.Collections;

public class Turret_Finishing_CS : MonoBehaviour {

	public bool Child_Flag = false ;
	public Transform Parent_Transform ;
	
	Transform Turret_Base ;
	Transform Cannon_Base ;
	Transform Barrel_Base ;
	
	void Start () {
		// Count the number of barrels.
		int Count = 0 ;
		for ( int i = 0 ; i < transform.childCount ; i++ ) {
			Transform Temp_Transform = transform.GetChild ( i ) ;
			if ( Temp_Transform.name.Length >= 11 && Temp_Transform.name.Substring ( 0 , 11 ) == "Barrel_Base" ) {
				Count += 1 ;
			}
		}
		// Change the hierarchy.
		if ( Count == 1 ) {
			Single_Barrel () ; // Single Barrel
		} else {
			Multiple_Barrels () ; // Multiple Barrels
		}
	}
	
	void Single_Barrel () {
		Turret_Base = transform.Find ( "Turret_Base" ) ;
		Cannon_Base = transform.Find ( "Cannon_Base" ) ;
		Barrel_Base = transform.Find ( "Barrel_Base" ) ;
		if ( Turret_Base && Cannon_Base && Barrel_Base ) {
			Barrel_Base.parent = Cannon_Base ;
			Cannon_Base.parent = Turret_Base ;
			Finishing () ;
		} else {
			Error_Message () ;
		}
	}
	
	void Multiple_Barrels () {
		Turret_Base = transform.Find ( "Turret_Base" ) ;
		if ( Turret_Base ) {
			for ( int i = 1 ; i <= transform.childCount ; i++ ) {
				Cannon_Base = transform.Find ( "Cannon_Base_" + i ) ;
				if ( Cannon_Base ) {
					Cannon_Base.parent = Turret_Base ;
					for ( int j = 1 ; j <= transform.childCount ; j++ ) {
						Barrel_Base = transform.Find ( "Barrel_Base_" + i ) ;
						if ( Barrel_Base ) {
							Barrel_Base.parent = Cannon_Base ;
						}
					}
				}
			}
		} else {
			Error_Message () ;
		}
		// Check the new hierarchy.
		for ( int i = 0 ; i < transform.childCount ; i++ ) {
			Transform Temp_Transform = transform.GetChild ( i ) ;
			if ( Temp_Transform.name.Substring ( 0 , 11 ) == "Barrel_Base" ) {
				Error_Message () ;
			} else if ( Temp_Transform.name.Substring ( 0 , 11 ) == "Cannon_Base" ) {
				Error_Message () ;
			}
		}
		Finishing () ;
	}

	void Finishing () {
		// Send message to "Turret_Horizontal", "Cannon_Vertical", "Recoil Brake", "Bullet_Generator", "Damage_Control".
		BroadcastMessage ( "Complete_Turret" , SendMessageOptions.DontRequireReceiver ) ;
		//
		if ( Child_Flag ) { // Child turret.
			// Send message to "Damage_Control" in the Turret. 
			Turret_Base.BroadcastMessage ( "Child_Turret" , SendMessageOptions.DontRequireReceiver ) ;
		} else { // Normal or Parent turret.
			Destroy ( this ) ;
		}
	}

	void Update () { // Only for child turret.
		if ( Child_Flag ) {
			if ( Parent_Transform ) {
			// Change the parent.
			Transform Temp_Transform =  Parent_Transform.Find ( "Turret_Base" ) ;
			transform.parent = Temp_Transform ;
			} else {
				Debug.LogError ( "'Turret_Finishing_CS(Script)' could not change the hierarchy of the Turret. (Physics Tank Maker)" ) ;
				Debug.LogWarning ( "'Parent Transform' for the child turret is not assigned." ) ;
			}
		}
		Destroy ( this ) ;
	}

	void Error_Message () {
		Debug.LogError ( "'Turret_Finishing_CS(Script)' could not change the hierarchy of the Turret. (Physics Tank Maker)" ) ;
		Debug.LogWarning ( "Please confirm the names of 'Turret_Base', 'Cannon_Base', 'Barrel_Base'." ) ;
		Destroy ( this ) ;
	}
	
}