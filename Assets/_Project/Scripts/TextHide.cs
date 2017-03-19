using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextHide : MonoBehaviour {
	public GameObject[] canvas = new GameObject[8];
	int delayTime = 20;

	public void DelayHide() {
		StartCoroutine(ExecuteAfterTime(delayTime));
	}

	IEnumerator ExecuteAfterTime(float time)
	{
		yield return new WaitForSeconds(time);
		foreach (GameObject text in canvas) {
			text.SetActive (false);
		}
	}
}
