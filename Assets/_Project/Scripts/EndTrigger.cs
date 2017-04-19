using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class EndTrigger : MonoBehaviour {
    public Text message;
    public float waitTime = 3f;

    private void Start() {
        //message.text = "Your Mission: Follow the Red Path And Collect the Yellow Ball";
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            Application.LoadLevel("GameScene");
        }
    }


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
          
            print("Target reached, starting countdown to scene switch");
            StartCoroutine(switchScenes());
            // message.text = "Mission Completed! Nice Job";

        }
    }


    IEnumerator switchScenes() {
        yield return new WaitForSeconds(waitTime);
        Application.LoadLevel("GameScene");

    }
}
