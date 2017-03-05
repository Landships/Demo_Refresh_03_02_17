using UnityEngine;
using System.Collections;

public class TriggerExplosion : MonoBehaviour {

    public string mytag;
    public GameObject child;
    public float time = 0.1f;

    void OnCollisionEnter(Collision col)
    {
        if(col.transform.tag == mytag)
        {
            child.SetActive(true);
            Destroy(gameObject, time);
        }

    }
}
