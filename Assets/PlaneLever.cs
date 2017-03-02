using UnityEngine;
using System.Collections;

// This class handles objects that act as a lever or a crank. Player hands that intersect with it can interact with it, turning it along a designated axis. 
// The condition of requiring contact or not relates to user testing with the crank. Crank rotation worked best when users are required to keep their hands within the crank collider.

public class PlaneLever : MonoBehaviour
{
    public GameObject HandObject;

    public enum detectionplane { xy, xz, yz };
    public enum rotationaxis { x, y, z };
    public enum contact { required, not_required };

    public float uppercap_degrees;
    public float lowercap_degrees;
    public detectionplane d_plane;
    public rotationaxis r_axis;
    public contact contact_required;

    private Renderer rend;

    public GameObject center;
    public Vector3 prevpos;
    public Vector3 currentpos;
    private bool firsttime;


    public float d_angle;

    public float angle;


    public bool leavingcollider = false;

    public bool intersecting = false;

    public bool interacting = false;


    public void Start()
    {
        d_angle = 0;
        firsttime = true;
       
        // Interactable objects are displayed red. They turn green when intersected, and magenta when interacted with.
        rend = GetComponent<Renderer>();
        rend.material.color = Color.red;
        gameObject.tag = "Interactable";
    }


    void OnTriggerExit(Collider other)
    {
        rend.material.color = Color.red;

        intersecting = false;
        if (leavingcollider)
        {
            leavingcollider = false;
        }
    }

    // Depending on intersection/interaction state, perform rotation on the crank to follow the hand.
    void Update()
    {
        if (contact_required == contact.not_required)
        {
            if ((intersecting || interacting) && !leavingcollider)
            {
                if (HandObject.GetComponent<PseudoHand>().trigger_on)
                {
                    interacting = true;
                    rend.material.color = Color.magenta;
                    Calculate();
                }
                else
                {
                    interacting = false;
                    if (intersecting)
                        rend.material.color = Color.green;
                    else
                    {
                        rend.material.color = Color.red;
                    }
                }
            }
        }
        else
        {
            if (intersecting && !leavingcollider)
            {
                if (HandObject.GetComponent<PseudoHand>().trigger_on)
                {
                    rend.material.color = Color.magenta;
                    Calculate();
                }
                else
                {
                    rend.material.color = Color.green;
                }
            }
        }
    }

    // Calculates rotation of the lever/crank dependent on the position of the hand.
    void Calculate()
    {
        if (firsttime)
        {
            prevpos = center.transform.InverseTransformPoint(HandObject.transform.position);
            currentpos = prevpos;
            firsttime = false;
        }

        else
        {
            currentpos = center.transform.InverseTransformPoint(HandObject.transform.position);

            switch (d_plane)
            {
                case detectionplane.xy:
                    d_angle = Mathf.Atan2(prevpos.x * currentpos.y - prevpos.y * currentpos.x, prevpos.x * currentpos.x + prevpos.y * currentpos.y);
                    break;
                case detectionplane.xz:
                    d_angle = Mathf.Atan2(prevpos.z * currentpos.x - prevpos.x * currentpos.z, prevpos.z * currentpos.z + prevpos.x * currentpos.x);
                    break;
                case detectionplane.yz:
                    d_angle = Mathf.Atan2(prevpos.y * currentpos.z - prevpos.z * currentpos.y, prevpos.y * currentpos.y + prevpos.z * currentpos.z);
                    break;
            }

            prevpos = currentpos;

            if (angle+d_angle < uppercap_degrees*Mathf.PI/180 && angle+d_angle > -lowercap_degrees*Mathf.PI / 180)
            {
                Rotate();
            }
            else
            {
                // do nothing
            }
        }

    }


    void Rotate()
    {
        float capped_d_angle = Mathf.Clamp(d_angle, -6*Mathf.PI / 180, 6*Mathf.PI / 180);
        switch (r_axis)
        {
            case rotationaxis.x:
                transform.Rotate((capped_d_angle * 180 / Mathf.PI), 0, 0);
                break;
            case rotationaxis.y:
                transform.Rotate(0, (capped_d_angle * 180 / Mathf.PI), 0);
                break;
            case rotationaxis.z:
                transform.Rotate(0, 0, (capped_d_angle * 180 / Mathf.PI));
                break;
        }
        angle += capped_d_angle;
    }

}
