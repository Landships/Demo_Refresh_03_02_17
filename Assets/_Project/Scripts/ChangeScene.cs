using UnityEngine;
using System.Collections;

public class ChangeScene : MonoBehaviour {
    public void changeScene(string sceneName) {
        Application.LoadLevel(sceneName);
    }

    void Update() {
        if (Input.GetKeyDown("d")) {
            Application.LoadLevel("DriveTutorial");
        } else if (Input.GetKeyDown("g")) {
            Application.LoadLevel("ShootingTutorial");
        }
    }


}
