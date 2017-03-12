using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class FiringState : NetworkBehaviour {

	PlaneLever detectionScript;

	[SyncVar]
	public bool fireState = false;

	void Start()
	{
		detectionScript = GetComponent<PlaneLever> ();
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log (detectionScript.intersecting);
		if (detectionScript.intersecting && detectionScript.HandObject.GetComponent<PseudoHand> ().dpad_on) 
		{
			fireState = true;
		} 
		else 
		{
			fireState = false;
		}
	}
}
