using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System;

/// <summary>
/// THIS IS OWNED BY PLAYER 2
/// </summary>

public class SP_Crank_Controller_VR : MonoBehaviour
{
    byte current_player = 2; // owner = player 2

    public GameObject vertical_crank;
    public GameObject horizontal_crank;

    public byte get_client_player_number()
    {
        return current_player;
    }

}