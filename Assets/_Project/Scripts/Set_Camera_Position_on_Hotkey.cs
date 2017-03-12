using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set_Camera_Position_on_Hotkey : MonoBehaviour {

    public GameObject cameraRig;
    public GameObject correctDriverPos;
    public GameObject correctGunnerPos;
    public GameObject cameraHead;
    byte current_player = 1;
    GameObject n_manager;
    network_manager n_manager_script;

	
	void Update () {

        if (Input.GetKeyDown("c") == true) {

            Vector3 distanceToMove = new Vector3(0, 0, 0);

            if (current_player == 1) {

                distanceToMove = correctDriverPos.transform.position - cameraHead.transform.position;

            } else if (current_player == 2) {

                distanceToMove = correctGunnerPos.transform.position - cameraHead.transform.position;

            } else {
                Debug.Log("If you got here it means we added more players which is pretty cool :)");
                // If there are more players, just add an else if condition for each where the correct
                // position is used to calculate distanceToMove
            }

            cameraRig.transform.Translate(distanceToMove, Space.World);

        }

    }

    public void Prep() {
        n_manager = GameObject.Find("Custom Network Manager(Clone)");
        n_manager_script = n_manager.GetComponent<network_manager>();
        current_player = (byte)(n_manager_script.client_players_amount);
    }


}
