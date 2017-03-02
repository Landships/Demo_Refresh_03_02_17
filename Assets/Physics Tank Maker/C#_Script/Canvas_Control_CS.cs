using UnityEngine;
using System.Collections;

public class Canvas_Control_CS : MonoBehaviour {

	Canvas This_Canvas ;
	bool Flag ;

	void Start () {
		This_Canvas = GetComponent < Canvas > () ;
		Flag = This_Canvas.enabled ;
	}
	
	void Update () {
		if ( Input.GetKeyDown ( KeyCode.Delete ) ) {
			if ( Flag ) {
				Flag = false ;
			} else {
				Flag = true ;
			}
			This_Canvas.enabled = Flag ;
		}
	}
}
