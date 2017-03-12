using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
#if UNITY_EDITOR 
using UnityEditor;
#endif

public class Position : NetworkBehaviour
{

    [SyncVar]
    public string position;
    [SyncVar]
    public NetworkInstanceId parentid;
    public bool assigned = false;

    public GameObject MainCamera;

    public GameObject controller1;
    public GameObject controller2;

    public GameObject Hand1;
    public GameObject Hand2;

    public Vector3 Hand1pos;
    public Vector3 Hand2pos;

    [SyncVar]
    public bool trigger_on1 = false;
    [SyncVar]
    public bool trigger_on2 = false;
    [SyncVar]
    public bool dpad1 = false;
    [SyncVar]
    public bool dpad2 = false;

    public void Update()
    {
        if (!assigned)
        {
            if (position == "DRIVER")
            {
                AddPlayerasDriver(parentid);
                assigned = true;
            }
            else if (position == "TURRET")
            {
                AddPlayerasTurret(parentid);
                assigned = true;
            }
        }

        if (controller1)
        {

            Hand1.transform.position = controller1.transform.position;
            Hand1.transform.rotation = controller1.transform.rotation;
            transform.position = MainCamera.transform.GetChild(0).position;
            transform.rotation = MainCamera.transform.GetChild(0).rotation;
            /*if (position == "TURRET")
            {
                if (GetComponent<HandScript>().hand_hold)
                {
                    trigger_on1 = true;

                }
                else
                {
                    trigger_on1 = false;

                }
                Hand1.GetComponent<PseudoHand>().trigger_on = trigger_on1;
            }
            else if (position == "DRIVER")
            {*/
            if ((int)controller1.GetComponent<SteamVR_TrackedObject>().index != -1 && SteamVR_Controller.Input((int)controller1.GetComponent<SteamVR_TrackedObject>().index).GetPress(SteamVR_Controller.ButtonMask.Trigger))
            {
                trigger_on1 = true;
            }
            else
            {
                trigger_on1 = false;
            }
            if ((int)controller1.GetComponent<SteamVR_TrackedObject>().index != -1 && SteamVR_Controller.Input((int)controller1.GetComponent<SteamVR_TrackedObject>().index).GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                Debug.Log("DPAD WAS CLICKED IN POSITION SCRIPT - Controller 1");
                dpad1 = true;
            }
            else
            {
                //Debug.Log("DPAD WAS ~~~NOT~~~ CLICKED IN POSITION SCRIPT - Controller 1");
                dpad1 = false;
            }
            //Hand1.GetComponent<PseudoHand>().trigger_on = trigger_on1;
            //Hand1.GetComponent<PseudoHand>().dpad_on = dpad1;

        }
        if (controller2)
        {
            Hand2.transform.position = controller2.transform.position;
            Hand2.transform.rotation = controller2.transform.rotation;
            if ((int)controller2.GetComponent<SteamVR_TrackedObject>().index != -1 && SteamVR_Controller.Input((int)controller2.GetComponent<SteamVR_TrackedObject>().index).GetPress(SteamVR_Controller.ButtonMask.Trigger))
            {
            
                trigger_on2 = true;
            }
            else
            {
               //Debug.Log("DPAD WAS ~~~NOT~~~ CLICKED IN POSITION SCRIPT - Controller 2");
                trigger_on2 = false;
            }
            if ((int)controller2.GetComponent<SteamVR_TrackedObject>().index != -1 && SteamVR_Controller.Input((int)controller2.GetComponent<SteamVR_TrackedObject>().index).GetPress(SteamVR_Controller.ButtonMask.Touchpad))
            {
                Debug.Log("DPAD WAS CLICKED IN POSITION SCRIPT - Controller 2");
                dpad2 = true;
            }
            else
            {
                dpad2 = false;
            }
            if (trigger_on2 == true)
            {
                //Debug.Log("Value passed");
            }
           //Hand2.GetComponent<PseudoHand>().trigger_on = trigger_on2;
            //Hand2.GetComponent<PseudoHand>().dpad_on = dpad2;
        }

		CmdUpdateBooleans (trigger_on1, trigger_on2, dpad1, dpad2);


        Hand2.GetComponent<PseudoHand>().trigger_on = trigger_on2;
        Hand2.GetComponent<PseudoHand>().dpad_on = dpad2;

        
        Hand1.GetComponent<PseudoHand>().trigger_on = trigger_on1;
        Hand1.GetComponent<PseudoHand>().dpad_on = dpad1;
    }

	[Command]
	void CmdUpdateBooleans(bool t1, bool t2, bool d1, bool d2)
	{
		trigger_on1 = t1;
		trigger_on2 = t2;
		dpad1 = d1;
		dpad2 = d2;
	}



    public void AddPlayerasDriver(NetworkInstanceId parent)
    {
        
        GameObject tank = ClientScene.FindLocalObject(parent);
        transform.parent = tank.transform.GetChild(0);
        Transform turretSpawnPos = tank.transform.GetChild(0).GetChild(12);
        Transform driverSpawnPos = tank.transform.GetChild(0).GetChild(13);
        Hand1 = transform.GetChild(0).gameObject;
        Hand2 = transform.GetChild(1).gameObject;
        //Hand1.transform.parent = transform.parent;
        //Hand2.transform.parent = transform.parent;

        if (isLocalPlayer)
        {

            MainCamera = GameObject.Find("Main Camera (origin)");
            MainCamera.transform.parent = tank.transform.GetChild(0);
            MainCamera.transform.position = driverSpawnPos.position;
            MainCamera.transform.rotation = driverSpawnPos.rotation;
            controller1 = MainCamera.transform.GetChild(2).GetChild(1).gameObject;
            controller2 = MainCamera.transform.GetChild(3).GetChild(1).gameObject;

        }


    }


    public void AddPlayerasTurret(NetworkInstanceId parent)
    {
        GameObject tank = ClientScene.FindLocalObject(parent);
        transform.parent = tank.transform.GetChild(0).GetChild(9).GetChild(0).GetChild(1);
        Transform turretSpawnPos = tank.transform.GetChild(0).GetChild(12);
        Transform driverSpawnPos = tank.transform.GetChild(0).GetChild(13);
        
        Hand1 = transform.GetChild(0).gameObject;
        Hand2 = transform.GetChild(1).gameObject;
        //Hand1.transform.parent = transform.parent;
        //Hand2.transform.parent = transform.parent;

        if (isLocalPlayer)
        {

            MainCamera = GameObject.Find("Main Camera (origin)");
            MainCamera.transform.parent = transform.parent;
            MainCamera.transform.position = turretSpawnPos.position;
            MainCamera.transform.rotation = turretSpawnPos.rotation;
            controller1 = MainCamera.transform.GetChild(2).GetChild(1).gameObject;
            controller2 = MainCamera.transform.GetChild(3).GetChild(1).gameObject;

        }
        /* MainCamera = GameObject.Find("Main Camera (origin)");

         controller1 = MainCamera.transform.GetChild(2).GetChild(1).gameObject;
         controller2 = MainCamera.transform.GetChild(3).GetChild(1).gameObject;
         Hand1 = transform.GetChild(0).gameObject;
         Hand2 = transform.GetChild(1).gameObject;

         GameObject tank = ClientScene.FindLocalObject(parent);
         transform.parent = tank.transform.GetChild(0).GetChild(9).GetChild(0).GetChild(1);
         Transform turretSpawnPos = tank.transform.GetChild(0).GetChild(12);
         Transform driverSpawnPos = tank.transform.GetChild(0).GetChild(13);


         MainCamera.transform.parent = tank.transform.GetChild(0);
         MainCamera.transform.position = turretSpawnPos.position;
         MainCamera.transform.rotation = turretSpawnPos.rotation;*/
    }

}