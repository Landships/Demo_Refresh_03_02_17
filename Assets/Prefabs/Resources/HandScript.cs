using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class HandScript : NetworkBehaviour
{

    public Camera Playercam;
    public float FOV;
    private int screen_width;
    private int screen_height;

    //[SyncVar]
    //public bool hand_on = false;                //whether renderer is enabled  
    [SyncVar]
    public bool hand_hold = false;              //whether button 0 is being held
    [SyncVar]
    public bool hand_holdfirst = false;         ///currently not used, might be helpful later
    [SyncVar]
    public bool hand_click = false;             //whether a click was just concluded (button 0 release)

    public float distance;
    private float distance_init;
    public float distanceuplimit;
    public float distancelowlimit;

    public float mouseupdown = 0;               ///mouse tracking up/down normalized to screen res (-1,1)
    public float mouseleftright = 0;            ///mouse tracking left/right normalized to screen res(-1,1)
    public float mouseinout = 0;                ///scroll wheel tracking normalized to distanceuplimit and distancelowlimit (-1,1) with distance_init as 0.
    private float prevleftright = 0;
    private float prevupdown = 0;
    private float previnout = 0;
    public float inoutchange = 0;
    public float updownchange = 0;
    public float leftrightchange = 0;
    public float scrollincrement = 0.1f;        //set this to something smaller manually when game starts...

    public Vector3 newPos;

    // Use this for initialization
    void Start()
    {
        if (isLocalPlayer)
        {
            Playercam.enabled = true;

            screen_width = Screen.width;
            screen_height = Screen.height;
            FOV = Playercam.fieldOfView;
            prevleftright = mouseleftright;
            prevupdown = mouseupdown;
            distance_init = (Playercam.transform.position - transform.position).magnitude;
            distance = distance_init;
            distanceuplimit = 5f;
            distancelowlimit = 1f;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            GetInput();
            updateTransform();
        }


    }

    void GetInput()
    {

        ///////////For translating mouse position

        screen_width = Screen.width;
        screen_height = Screen.height;
        FOV = Playercam.fieldOfView;

        ///////////////////////////////////////////////



        /*
        //////////Show/Hide "hand" , q key by default

        transform.GetChild(1).GetComponent<MeshRenderer>().enabled = hand_on;

        if (Input.GetButtonDown("hand"))
        {
            hand_on = !hand_on;
        }
        ////////////////////////////////////////////////////////
        */



        //// Normalizes mousepos -1<x<1, -1<y<1, where center = (0,0)

        Vector3 mousepos = Input.mousePosition;

        float mouse_x = mousepos.x;
        float mouse_y = mousepos.y;


        mouseleftright = 2 * (mouse_x / screen_width - 0.5f);
        mouseupdown = 2 * (mouse_y / screen_height - 0.5f);

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (distance + scrollincrement < distanceuplimit)
                distance = distance + scrollincrement;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (distance - scrollincrement > distancelowlimit)
                distance = distance - scrollincrement;
        }

        mouseinout = distance - distance_init;
        if (mouseinout > 0f)
        {
            mouseinout = mouseinout / (distanceuplimit - distance_init);
        }
        else
        {
            mouseinout = mouseinout / (distance_init - distancelowlimit);
        }


        ////////////////////////////////////////////////////////////


        /////////////////////Some Maths to get postion


        float handx = distance * Mathf.Sin(mouseleftright * FOV * Mathf.PI / (180 * 2));
        float handy = distance * Mathf.Sin(mouseupdown * FOV * Mathf.PI / (180 * 2));
        float handz = Mathf.Sqrt(distance * distance - handx * handx - handy * handy);
        newPos = new Vector3(handx, handy, handz);


        //////////////////////////////////////////////////////////////


        ////////Interactions

        if (hand_hold)
        {
            hand_holdfirst = false;
        }

        if (Input.GetButtonDown("Interact"))
        {
            hand_hold = true;
            hand_holdfirst = true;

        }

        if (Input.GetButtonUp("Interact"))
        {
            hand_hold = false;
            hand_click = true;
        }


        /////////not used atm
        leftrightchange = mouseleftright - prevleftright;
        updownchange = mouseupdown - prevupdown;
        inoutchange = mouseinout - previnout;
        prevleftright = mouseleftright;
        prevupdown = mouseupdown;
        previnout = mouseinout;

    }


    public void RegisterClick()
    {
        hand_click = false;
    }

    void updateTransform()
    {
        transform.GetChild(1).transform.localPosition = newPos;
    }
}
