//(c) 2014 DaiMangou
// By Ruchmair Dixon
// Game: WISDOM of The First Heavens
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class CharacterMotion : MonoBehaviour
{
   

    public int force = 30;

    public float speed = 0.0f;


    private Vector3 locVel;

    public List<float> PYR = new List<float>();



    void FixedUpdate()
    {


        locVel = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
        GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(0, 0, -Input.GetAxis("Horizontal") * PYR[0] * GetComponent<Rigidbody>().mass));
        GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(0, Input.GetAxis("Horizontal") * PYR[1] * GetComponent<Rigidbody>().mass, 0));
        GetComponent<Rigidbody>().AddRelativeTorque(new Vector3(Input.GetAxis("Vertical") * PYR[2] * GetComponent<Rigidbody>().mass, 0, 0));
        speed = locVel.z;


        GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, 0, Input.GetAxis("Jump") * force * GetComponent<Rigidbody>().mass));
    }



}
