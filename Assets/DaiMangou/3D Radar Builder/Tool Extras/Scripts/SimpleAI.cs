using UnityEngine;
using System.Collections;

public class SimpleAI : MonoBehaviour {

    void Start()
    {
        gameObject.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * Random.Range(200, 800));
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.transform.tag != "Untagged")
        {
            transform.Rotate(Vector3.up * Time.deltaTime, Random.Range(0, 200));
            gameObject.GetComponent<Rigidbody>().AddRelativeForce(transform.forward * Random.Range(200, 800));
        }
    }
}
