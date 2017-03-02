using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System;


public class SP_PlayerController_VR : MonoBehaviour
{
    byte current_player = 1;

    public GameObject camera_rig;
    public GameObject left_controller;
    public GameObject right_controller;

    public GameObject left_hand;
    public GameObject right_hand;

    void Update()
    {
        update_world_state();
    }



//if not owner and not host, do nothing, else:
void update_world_state()
    {
        Read_Camera_Rig();
    }

    void Read_Camera_Rig()
    {
        left_hand.transform.position = left_controller.transform.position;
        right_hand.transform.position = right_controller.transform.position;
    }

    public byte get_client_player_number()
    {
        return current_player;
    }
}