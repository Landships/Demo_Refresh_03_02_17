using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PseudoHand : NetworkBehaviour {


    public bool trigger_on;
    public bool dpad_on;
    public void isGrabbing()
    {
        gameObject.GetComponentInChildren<handScript>().hapticFeedBack();
    }


}
