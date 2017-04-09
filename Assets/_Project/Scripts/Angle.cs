using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Angle : MonoBehaviour {
	Text myAngle;
	private int upAngle;
	private int rightAngle;
	public GameObject myBarral;
	public GameObject myBarralv;
	// Use this for initialization
	void Start () {
		myAngle = GetComponent<Text>();
	}

	// Update is called once per frame
	void Update () {
		upAngle = - (int) myBarralv.GetComponent<Transform> ().localEulerAngles.x % 360;
		rightAngle = (int) myBarral.GetComponent<Transform> ().localEulerAngles.y % 360;
		if (upAngle < -250) {
			upAngle = upAngle + 360;
		}
		if (rightAngle > 180) {
			rightAngle = rightAngle - 360;
		}
		myAngle.text = upAngle.ToString () + "°     " + rightAngle.ToString () + "°";

	}
}
