using UnityEngine;
using System.Collections;

public class TargetHitDetection : MonoBehaviour {

    public TargetManager managerScript;
    public GameObject parent;



    void OnTriggerEnter(Collider other) {

        // Would be ideal to have a check here so only bullets (instead of any object) can destroy targets
        Destroy(other.gameObject);
        Destroy(parent);
        managerScript.decreaseCount();
      
    }


}
