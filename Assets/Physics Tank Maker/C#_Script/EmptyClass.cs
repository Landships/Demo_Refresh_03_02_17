namespace VRTK {
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;
	public class bulletready : MonoBehaviour {
		public Button buttonScript;

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {
			bool canfire = buttonScript.ReadyFire();
			if (canfire) {
				GetComponent<SpriteRenderer> ().color = Color.black;
			} else {
				GetComponent<SpriteRenderer> ().color = Color.red;
			}


		}
	}
}