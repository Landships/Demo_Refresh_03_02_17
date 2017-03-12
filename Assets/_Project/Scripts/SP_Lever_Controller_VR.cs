using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System;

/// <summary>
/// THIS IS OWNED BY PLAYER 1/SERVER
/// </summary>

public class SP_Lever_Controller_VR : MonoBehaviour
{
    byte current_player = 1;

    public GameObject left_lever;
    public GameObject right_lever;

 

    public byte get_client_player_number()
    {
        return current_player;
    }

}