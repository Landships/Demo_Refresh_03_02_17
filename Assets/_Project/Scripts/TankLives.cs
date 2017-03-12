using UnityEngine;
using System.Collections;

public class TankLives : MonoBehaviour {
	public int lives = 1;
	public bool isPlayer;

	void Start() {
		if (isPlayer) {
			lives = 3;
		}
	}
	public int getLives() {
		return lives;
	}

	public void loseLife() {
		lives -= 1;
	}
}
