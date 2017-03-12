using UnityEngine;
using System.Collections;

public class MagneticCompass : MonoBehaviour {

	public GameObject target;
	public float needleSpeed;
	
	// Update is called once per frame
	void Update () {

		Vector3 vector = target.transform.position - transform.position;
		Rigidbody needle = transform.GetComponent<Rigidbody> ();
		needle.AddForce (needleSpeed * vector.normalized);

	}
}
