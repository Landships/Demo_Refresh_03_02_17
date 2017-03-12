using UnityEngine;
using System.Collections;

public class SnapLeverToNeutral : MonoBehaviour {

    public float threshold = 1f; // Degrees for which the lever will be snapped to the neutral position
    private Quaternion neutralPos;
    public VRTK.VRTK_InteractableObject lever;

	// Use this for initialization
	void Start () {
        neutralPos = transform.rotation;
	
	}
	
	// Update is called once per frame
	void Update () {

        
        Vector3 currAngle = transform.rotation.eulerAngles;
        float degreesDifferent = Mathf.Abs(Quaternion.Angle(transform.rotation, neutralPos));

        if (degreesDifferent < threshold && !lever.IsGrabbed()) {
            transform.rotation = neutralPos;
        }
	
	}
}
