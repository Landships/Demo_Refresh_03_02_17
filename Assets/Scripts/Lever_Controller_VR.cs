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

public class Lever_Controller_VR : MonoBehaviour
{
    public int designated_player;
    byte current_player; // owner = player 1

    public GameObject left_lever;
    public GameObject right_lever;

    // Client Queue
    int frame = 0;

    int server_player;

    // general
    float left_x;

    float right_x;


    //trigger

    GameObject n_manager;
    network_manager n_manager_script;

    bool started = false;
    bool ready = false;

    bool reliable_message = false;

    int frame_interval = 5;

    public void Prep()
    {
        n_manager = GameObject.Find("Custom Network Manager(Clone)");
        n_manager_script = n_manager.GetComponent<network_manager>();
        current_player = (byte)(n_manager_script.client_players_amount);
        if (current_player != designated_player)
        {
            left_lever.GetComponent<VRTK.VRTK_InteractableObject>().enabled = false;
            right_lever.GetComponent<VRTK.VRTK_InteractableObject>().enabled = false;
        }
    }

    void Start()
    {
    }

    void Update()
    {

        if (n_manager != null)
        {
            started = n_manager_script.started;
            ready = n_manager_script.game_ready;

            server_player = n_manager_script.server_player_control;

            reliable_message = n_manager_script.reliable_message;

            update_world_state();

            if (current_player == designated_player)
            {
                server_get_values_to_send();
            }

            else
            {
                client_update_values();
            }
        }
    }

    void FixedUpdate()
    {
        if (n_manager != null)
        {
            //update_world_state();
        }
    }


    //if not owner and not host, do nothing, else:
    void update_world_state()
    {
        if (current_player == designated_player)
        {
            //
        }
        else
        {
            left_lever.transform.localRotation = Quaternion.Lerp(left_lever.transform.localRotation, Quaternion.Euler(left_x, 90, 0), 0.1f);
            right_lever.transform.localRotation = Quaternion.Lerp(right_lever.transform.localRotation, Quaternion.Euler(right_x, 0, 0), 0.1f);
            //left_lever.transform.localRotation = Quaternion.Euler(left_x, 90, 0);
            //right_lever.transform.localRotation = Quaternion.Euler(right_x, 0, 0);

           // Debug.Log("left x = " + left_x);
        }
    }

    public byte get_client_player_number()
    {
        return current_player;
    }




    // ----------------------------
    // Functions that use Block Copy
    // ----------------------------


    // The client get its values/inputs to send to the server
    void client_send_values()
    {
        float[] left_lever_values = { left_lever.transform.localRotation.eulerAngles.x };
        float[] right_lever_values = { right_lever.transform.localRotation.eulerAngles.x };

        n_manager_script.send_from_client(8, left_lever_values);
        n_manager_script.send_from_client(9, right_lever_values);

    }



    // Server Updates the server larger buffer it is going to send
    public void server_get_values_to_send()
    {

        float[] left_lever_values = { left_lever.transform.localRotation.eulerAngles.x };
        float[] right_lever_values = { right_lever.transform.localRotation.eulerAngles.x };



        n_manager_script.send_from_server(8, left_lever_values);
        n_manager_script.send_from_server(9, right_lever_values);

    }




    // Client get values from the server buffer
    void client_update_values()
    {

        float[] left_lever_values = n_manager_script.client_read_server_buffer(8);
        float[] right_lever_values = n_manager_script.client_read_server_buffer(9);

        left_x = left_lever_values[0];

        right_x = right_lever_values[0];


    }

    // Server Get values from the client buffer, so the client inputs
    public void server_get_client_hands()
    {
        float[] left_lever_values = n_manager_script.server_read_client_buffer(8);
        float[] right_lever_values = n_manager_script.server_read_client_buffer(9);

        left_x = left_lever_values[0];

        right_x = right_lever_values[0];

    }
}