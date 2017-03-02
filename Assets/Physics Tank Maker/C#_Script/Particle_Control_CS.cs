using UnityEngine;
using System.Collections;

public class Particle_Control_CS : MonoBehaviour {

	float Distance ;
	bool Flag_Playing ;
	Transform This_Transform ;
	ParticleSystem This_ParticleSystem ;
	AudioSource This_AudioSource ;
	Light This_Light ;

	void Awake () {
		This_Transform = transform ;
		This_ParticleSystem = GetComponent<ParticleSystem>() ;
		This_AudioSource = GetComponent<AudioSource>() ;
		This_Light = GetComponent<Light>() ;
		if ( This_AudioSource ) {
			This_AudioSource.playOnAwake = false ;
			Flag_Playing = true ;
		} else {
			Flag_Playing = false ;
		}
	}
	
	void Start () {
		// Set play speed.
		This_ParticleSystem.playbackSpeed = 1.0f / Time.timeScale ;
		// Set children's play speed.
		if ( This_Transform.childCount > 0 ) {
			for ( int i = 0 ; i < This_Transform.childCount ; i ++ ) {
				ParticleSystem Temp_ParticleSystem = This_Transform.GetChild ( i ).GetComponent < ParticleSystem > () ;
				if ( Temp_ParticleSystem ) {
					Temp_ParticleSystem.playbackSpeed = 1.0f / Time.timeScale ;
				}
			}
		}
		//
		if ( This_Light ) {
			StartCoroutine ( "Flash" ) ;
		}
		if ( This_AudioSource && Camera.main ) {
			StartCoroutine ( "Play_Audio" ) ;
		}
	}
	
	void Update () {
		if ( This_ParticleSystem.isStopped && Flag_Playing == false ) {
			Destroy ( this.gameObject ) ;
		}
	}
	
	IEnumerator Flash () {
		This_Light.enabled = true ;
		yield return new WaitForSeconds ( 0.08f ) ;
		This_Light.enabled = false ;
	}
	
	IEnumerator Play_Audio () {
		Distance = Vector3.Distance ( This_Transform.position , Camera.main.transform.position ) ;
		This_AudioSource.pitch = Mathf.Lerp ( 1.0f , 0.1f , Distance / This_AudioSource.maxDistance ) ;
		yield return new WaitForSeconds ( Distance / 340.29f * Time.timeScale ) ;
		This_AudioSource.Play () ;
		yield return new WaitForSeconds ( This_AudioSource.clip.length * 0.95f ) ;
		This_AudioSource.Stop () ;
		Flag_Playing = false ;
	}

	public void Delete_Timer ( float Temp_Time ) {
		Destroy ( this.gameObject , Temp_Time ) ;
	}

}