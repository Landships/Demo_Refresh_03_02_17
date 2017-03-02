using UnityEngine;
using System.Collections;

public class CompassControl : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (transform.position + Vector3.up);
	}
}
