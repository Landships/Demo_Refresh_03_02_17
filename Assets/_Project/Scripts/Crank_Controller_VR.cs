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

public class Crank_Controller_VR : MonoBehaviour
{
    public int designated_player;
    byte current_player; // owner = player 2

    public GameObject vertical_crank;
    public GameObject horizontal_crank;

    // Client Queue
    int frame = 0;

    int server_player;

    // general
    float vertical_x;

    float horizontal_x;


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
            vertical_crank.GetComponent<VRTK.VRTK_InteractableObject>().enabled = false;
            horizontal_crank.GetComponent<VRTK.VRTK_InteractableObject>().enabled = false;
        }
    }

    void Start()
    {
        //n_manager = null;
    }

    void Update()
    {
        
        if (n_manager != null)
        {
            //n_manager_script = n_manager.GetComponent<network_manager>();
            //current_player = (byte)(n_manager_script.client_players_amount);
            started = n_manager_script.started;
            ready = n_manager_script.game_ready;

            server_player = n_manager_script.server_player_control;

            reliable_message = n_manager_script.reliable_message;

            if (current_player == designated_player)
            {
                client_send_values();
            }

            else
            {
                server_get_client_crank_position();
            }
        }       
    }

    void FixedUpdate()
    {
        if (n_manager != null)
        {
            update_world_state();
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
            vertical_crank.transform.localRotation = Quaternion.Lerp(vertical_crank.transform.localRotation, Quaternion.Euler(vertical_x, -90, -90), 0.1f);
            horizontal_crank.transform.localRotation = Quaternion.Lerp(horizontal_crank.transform.localRotation, Quaternion.Euler(horizontal_x, -90, -90), 0.1f);
            //vertical_crank.transform.localRotation = Quaternion.Euler(vertical_x, -90, -90);
            //horizontal_crank.transform.localRotation = Quaternion.Euler(horizontal_x, -90, -90);

            //Debug.Log("vertical x = " + vertical_x);
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
        float[] vertical_crank_values = { vertical_crank.transform.localRotation.eulerAngles.x };
        float[] horizontal_crank_values = { horizontal_crank.transform.localRotation.eulerAngles.x };
        //Debug.Log("vertical crank values" + vertical_crank_values.Length.ToString());
        n_manager_script.send_from_client(10, vertical_crank_values);
        n_manager_script.send_from_client(11, horizontal_crank_values);

    }



    // Server Updates the server larger buffer it is going to send
    public void server_get_values_to_send()
    {

        float[] vertical_crank_values = { vertical_crank.transform.localRotation.eulerAngles.x };
        float[] horizontal_crank_values = { horizontal_crank.transform.localRotation.eulerAngles.x };



        n_manager_script.send_from_server(10, vertical_crank_values);
        n_manager_script.send_from_server(11, horizontal_crank_values);

    }




    // Client get values from the server buffer
    void client_update_values()
    {

        float[] vertical_crank_values = n_manager_script.client_read_server_buffer(10);
        float[] horizontal_crank_values = n_manager_script.client_read_server_buffer(11);

        vertical_x = vertical_crank_values[0];

        horizontal_x = horizontal_crank_values[0];


    }

    // Server Get values from the client buffer, so the client inputs
    public void server_get_client_crank_position()
    {
        float[] vertical_crank_values = n_manager_script.server_read_client_buffer(10);
        float[] horizontal_crank_values = n_manager_script.server_read_client_buffer(11);

        vertical_x = vertical_crank_values[0];

        horizontal_x = horizontal_crank_values[0];

    }
}