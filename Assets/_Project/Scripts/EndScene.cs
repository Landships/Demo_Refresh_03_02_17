using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.LoadLevel("Menu");
		}
		if (GameObject.Find ("AI_1") == null && GameObject.Find ("AI_2") == null && GameObject.Find ("AI_3") == null && GameObject.Find ("AI_4") == null) {
			//Application.LoadLevel("Menu");
		}
	}
}
