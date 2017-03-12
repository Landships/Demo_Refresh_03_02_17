using UnityEngine;
using System.Collections;

public class WheelSetChild : MonoBehaviour {
    public GameObject wheel;
    public bool set = false;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	    if (!set) {
            wheel.transform.parent = this.gameObject.transform;
            set = true;
        }
	}
}
