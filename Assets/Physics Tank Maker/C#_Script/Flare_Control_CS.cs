using UnityEngine;
using System.Collections;

public class Flare_Control_CS : MonoBehaviour {

	public float Min_Interval = 1.0f ;
	public float Max_Interval = 10.0f ;

	ParticleSystem This_ParticleSystem ;
	float LifeTime ;
	Light This_Light ;
	float Target_Intensity ;
	float Current_Intensity ;
	bool Work_Flag = false ;
	bool Intarval_Flag = false ;
	bool Phase_Flag = true ;

	void Start () {
		This_ParticleSystem = GetComponent < ParticleSystem > () ;
		if ( This_ParticleSystem ) {
			LifeTime = This_ParticleSystem.startLifetime ;
		} else {
			Destroy ( this ) ;
		}
		//
		This_Light = GetComponent < Light > () ;
		if ( This_Light ) {
			Target_Intensity = This_Light.intensity ;
			This_Light.intensity = 0.0f ;
		} else {
			Destroy ( this ) ;
		}
	}
	
	void Update () {
		if ( Work_Flag ) {
			Light_Control () ;
		} else if ( !Intarval_Flag ) {
			StartCoroutine ( "Interval" ) ;
			Intarval_Flag = true ;
		}
	}

	IEnumerator Interval () {
		yield return new WaitForSeconds ( Random.Range ( Min_Interval , Max_Interval ) ) ;
		Work_Flag = true ;
		This_ParticleSystem.Play () ;
	}

	void Light_Control () {
		if ( Phase_Flag ) {
			Current_Intensity = Mathf.MoveTowards ( Current_Intensity , Target_Intensity , Target_Intensity * Time.deltaTime * 2.0f ) ;
			if ( Current_Intensity >= Target_Intensity ) {
				Phase_Flag = false ;
			}
		} else {
			Current_Intensity = Mathf.MoveTowards ( Current_Intensity , 0.0f , Target_Intensity / LifeTime * Time.deltaTime ) ;
			if ( Current_Intensity <= 0.0f ) {
				Phase_Flag = true ;
				Work_Flag = false ;
				Intarval_Flag = false ;
			}
		}
		This_Light.intensity = Current_Intensity ;
	}
}
