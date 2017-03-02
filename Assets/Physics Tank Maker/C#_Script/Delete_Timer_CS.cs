using UnityEngine;
using System.Collections;

public class Delete_Timer_CS : MonoBehaviour {

	public float Count ;

	void Start () {
		Destroy ( this.gameObject , Count ) ;
	}

}
