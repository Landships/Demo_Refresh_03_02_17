using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
using System;


public class PlayerController_VR : MonoBehaviour
{
    public byte owner;
    byte current_player;

    public GameObject camera_rig;
    public GameObject left_controller;
    public GameObject right_controller;

    public GameObject left_hand;
    public GameObject right_hand;

    // Client Queue
    int frame = 0;

    int server_player;

    // general
    float left_x;
    float left_y;
    float left_z;

    float right_x;
    float right_y;
    float right_z;


    //trigger

    GameObject n_manager;
    network_manager n_manager_script;

    bool started = false;
    bool ready = false;

    bool reliable_message = false;

    int frame_interval = 5;


    void Start()
    {
        n_manager = GameObject.Find("Custom Network Manager(Clone)");
        n_manager_script = n_manager.GetComponent<network_manager>();
        current_player = (byte)(n_manager_script.client_players_amount);
        if (owner == current_player)
        {
            left_hand.GetComponent<MeshRenderer>().enabled = false;
            right_hand.GetComponent<MeshRenderer>().enabled = false;
        }
        //client_update_values(n_manager_script.server_to_client_data_large);
        //server_get_values_to_send();
    }

    void Update()
    {
        started = n_manager_script.started;
        ready = n_manager_script.game_ready;

        server_player = n_manager_script.server_player_control;

        reliable_message = n_manager_script.reliable_message;

        update_world_state();

        if (owner == current_player)
        {
            if (current_player == 1)
            {
                server_get_values_to_send();
            }
            else
            {
                client_send_values();
            }
        }

        else
        {
            if (current_player == 1)
            {
                server_get_client_hands();
            
            }
            else
            {
                client_update_values();
            }
        }
    }

    void FixedUpdate()
    {
        //update_world_state();
    }


//if not owner and not host, do nothing, else:
void update_world_state()
    {
        if (current_player == owner)
        {
            Read_Camera_Rig();
        }
        else
        {
            left_hand.transform.position = Vector3.Lerp(left_hand.transform.position, new Vector3(left_x, left_y, left_z), 0.1f);
            right_hand.transform.position = Vector3.Lerp(right_hand.transform.position, new Vector3(right_x, right_y, right_z), 0.1f);

            //left_hand.transform.position = new Vector3(left_x, left_y, left_z);
            //right_hand.transform.position = new Vector3(right_x, right_y, right_z);
        }
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


    // ----------------------------
    // Functions that use Block Copy
    // ----------------------------

    void client_send_reliable_message(object sender, VRTK.ControllerInteractionEventArgs e) {
        Debug.Log("CLICKED");
        if (current_player == 1) {
            n_manager_script.server_send_reliable();
        } else {
            n_manager_script.client_send_reliable();
        }
    }


    // The client get its values/inputs to send to the server
    void client_send_values() {
        float[] left_controller_values = { left_controller.transform.position.x,
                                           left_controller.transform.position.y,
                                           left_controller.transform.position.z };
        float[] right_controller_values = { right_controller.transform.position.x,
                                            right_controller.transform.position.y,
                                            right_controller.transform.position.z };

        n_manager_script.send_from_client(1, left_controller_values);
        n_manager_script.send_from_client(2, right_controller_values);

    }



    // Server Updates the server larger buffer it is going to send
    public void server_get_values_to_send() {

        float[] left_controller_values = { left_controller.transform.position.x,
                                           left_controller.transform.position.y,
                                           left_controller.transform.position.z };
        float[] right_controller_values = { right_controller.transform.position.x,
                                            right_controller.transform.position.y,
                                            right_controller.transform.position.z };



        n_manager_script.send_from_server(1, left_controller_values);
        n_manager_script.send_from_server(2, right_controller_values);

    }




    // Client get values from the server buffer
    void client_update_values() {

        float[] left_controller_values = n_manager_script.client_read_server_buffer(1);
        float[] right_controller_values = n_manager_script.client_read_server_buffer(2);

        left_x = left_controller_values[0];
        left_y = left_controller_values[1];
        left_z = left_controller_values[2];

        right_x = right_controller_values[0];
        right_y = right_controller_values[1];
        right_z = right_controller_values[2];

    }

        // Server Get values from the client buffer, so the client inputs
    public void server_get_client_hands()
    {
        float[] left_controller_values = n_manager_script.server_read_client_buffer(1);
        float[] right_controller_values = n_manager_script.server_read_client_buffer(2);

        left_x = left_controller_values[0];
        left_y = left_controller_values[1];
        left_z = left_controller_values[2];

        //Debug.Log("left controller vector3: " + right_x + " " + right_y + " " + right_z);

        right_x = right_controller_values[0];
        right_y = right_controller_values[1];
        right_z = right_controller_values[2];
    }





}