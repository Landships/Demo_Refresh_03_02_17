using UnityEngine;
using System.Collections;

public class LookatRotate : MonoBehaviour {

    public Transform target;
	void Update () {
        transform.LookAt(target);
        transform.RotateAround(target.transform.position, Vector3.up, -20 * Time.deltaTime);
	}
}
