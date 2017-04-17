using UnityEngine;
using System.Collections;

public class start_game : MonoBehaviour {

    public GameObject game;
    public GameObject net;
    
    // Use this for initialization
    void Start ()
    {
        Instantiate(game, transform.position, Quaternion.identity);
        Instantiate(net, transform.position, Quaternion.identity);

    }

}
