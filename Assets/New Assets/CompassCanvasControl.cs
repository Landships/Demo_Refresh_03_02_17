using UnityEngine;
using System.Collections;

public class CompassCanvasControl : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler (0, transform.rotation.eulerAngles.y, 0);
	}
}
