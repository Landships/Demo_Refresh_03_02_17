using UnityEngine;
using System.Collections;

public class Set_UI_Position : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject damageDisplay = GameObject.Find ("DamageDisplay");
		this.transform.localRotation = new Quaternion(0,0,0,0);
		this.transform.position = damageDisplay.transform.position;
		this.transform.Translate (0, 0, - (.001f + damageDisplay.transform.lossyScale[2] / 2));
			
	}

}
