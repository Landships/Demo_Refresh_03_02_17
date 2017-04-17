using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChangeIp : MonoBehaviour {

	static public string ipAddress = "-1";

	public void changeIp(string ip) {
		
	}


		

	void Update() {
		if (Input.GetKeyDown ("return")) {
			GameObject inputFieldGo = GameObject.Find("InputField");
			InputField inputFieldCo = inputFieldGo.GetComponent<InputField>();
			ipAddress = inputFieldCo.text;
			Application.LoadLevel("Menu");
		}
	}


}
