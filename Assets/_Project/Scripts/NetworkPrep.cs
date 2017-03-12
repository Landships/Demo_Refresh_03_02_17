using UnityEngine;
using System.Collections;

public class NetworkPrep : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void BroadCast()
    {
        BroadcastMessage("Prep");
    }
}
