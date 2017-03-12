using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AIhandler : NetworkBehaviour {

    public GameObject[] AI1;


    // Use this for initialization
    void Start () {
        Teleport();
	}

    // Update is called once per frame
    [Server]
    void Teleport()
    {
        Vector3 Displacement = new Vector3(100, 0, -100);
        for (int i=0; i < AI1.Length; i++ )
        {
            AI1[i].transform.position = AI1[i].transform.position + Displacement;
        }
    }
}
