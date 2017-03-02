using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class RossLever : NetworkBehaviour {

    private GameObject HandObject;
    private Position Hand;

    public enum detectionaxis { x, y, z };
    public enum rotationaxis { x, y, z};


    public float uppercap;
    public float lowercap;
    public detectionaxis d_axis;
    public rotationaxis r_axis;
   
    private Renderer rend;

    public GameObject leverbase;
    public float length;
    private float abslength;
    private float prevmouseinout;
    public float lever_init;
    public float leverpos;
    private float prevangle;
    private bool firsttime;

    public float angle = 0;
    public float lever_x = 0;
    public float lever_y = 0;
    
    public bool intersecting = false;
    public bool interacting = false;
    public bool holdintersecting = false;

    private bool is_grabbing = false;



    public void Start()
    {

        firsttime = true;
        prevangle = 0;
        abslength = 2*(leverbase.transform.position - transform.position).magnitude;
        length = 2*(leverbase.transform.localPosition - transform.localPosition).magnitude;


        rend = GetComponent<Renderer>();
        rend.material.color = Color.red;

        gameObject.tag = "Interactable";

    }



    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("lever is colliding with: " + other.gameObject.name);
        if (other.gameObject.tag == "hand" )
        {
            HandObject = other.gameObject;

            intersecting = true;

        }
    }

        void OnTriggerExit(Collider other)
    {
        rend.material.color = Color.red;

        intersecting = false;

        
    }


    void Update()
    {

        if (intersecting || interacting)
        {

            if (HandObject.GetComponent<PseudoHand>().trigger_on)
            {
                interacting = true;
                rend.material.color = Color.magenta;
                Normalize();
                Calculate();
                Rotate();
                is_grabbing = true;
                HandObject.GetComponent<PseudoHand>().isGrabbing();
            }

            else
            {
                interacting = false;
                is_grabbing = false;
                if (intersecting)
                    rend.material.color = Color.green;
                else
                {
                    rend.material.color = Color.red;
                }
            }
        }


    }

 

    void Normalize()
    {
        if (firsttime)
        {
            switch (d_axis)
            {
                case detectionaxis.x:
                    lever_init = HandObject.transform.localPosition.x;
                    leverpos = (HandObject.transform.localPosition.x - lever_init);
                    break;
                case detectionaxis.y:
                    lever_init = HandObject.transform.localPosition.y;
                    leverpos = (HandObject.transform.localPosition.y - lever_init);
                    break;
                case detectionaxis.z:
                    lever_init = HandObject.transform.localPosition.z;
                    leverpos = (HandObject.transform.localPosition.z - lever_init);
                    break;
            }
            firsttime = false;

        }

        else
        {
            switch (d_axis)
            {
                case detectionaxis.x:
                    leverpos = (HandObject.transform.localPosition.x - lever_init);
                    break;
                case detectionaxis.y:
                    leverpos = (HandObject.transform.localPosition.y - lever_init);
                    break;
                case detectionaxis.z:
                    leverpos = (HandObject.transform.localPosition.z - lever_init);
                    break;
            }


            if (leverpos > uppercap)
            {
                leverpos = uppercap;
            }
            else if (leverpos < -lowercap)
            {
                leverpos = -lowercap;
            }

        }

    }


    void Calculate()
    {
        // lever is orientated such that rotation is along x-axis.
        // -circumference/4 < arclength < circumference/4, arclength =  leverpos * (circumference/4) 
        angle =  (leverpos * (2 * length * Mathf.PI) / 4) / length;                           // angle (from neutral) = arclength/length
        /*lever_x = length * Mathf.Cos(Mathf.PI / 2 - angle)/2;                                // x = length * cosine ( pi/2 - angle)
        lever_y = Mathf.Sqrt((length / 2) * (length / 2) - lever_x*lever_x);                 // y = SQRT(r^2 - x^2)
        lever_y = lever_y - length / 2;*/                                                      //  
    }


    void Rotate()
    {
        switch (r_axis)
        {
            case rotationaxis.x:
                transform.Rotate((angle * 180 / Mathf.PI) - (prevangle * 180 / Mathf.PI), 0, 0);
                break;
            case rotationaxis.y:
                transform.Rotate(0, (angle * 180 / Mathf.PI) - (prevangle * 180 / Mathf.PI), 0);
                break;
            case rotationaxis.z:
                transform.Rotate(0, 0, (angle * 180 / Mathf.PI) - (prevangle * 180 / Mathf.PI));
                break;
        }
        prevangle = angle;
    }

}