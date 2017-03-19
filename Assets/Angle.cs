using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Angle : MonoBehaviour {
	Text myAngle;
	private int upAngle;
	private int rightAngle;
	GameObject myBarral;
	// Use this for initialization
	void Start () {
		myAngle = GetComponent<Text>();
		myBarral = GameObject.Find ("Barrel_Base");
	}
	
	// Update is called once per frame
	void Update () {
		upAngle = - (int) myBarral.GetComponent<Transform> ().localEulerAngles.x;
		rightAngle = (int) myBarral.GetComponent<Transform> ().localEulerAngles.y;
		if (upAngle < -250) {
			upAngle = upAngle + 360;
		}
		if (rightAngle > 250) {
			rightAngle = rightAngle - 360;
		}
		myAngle.text = upAngle.ToString () + "    " + rightAngle.ToString ();

	}
}
