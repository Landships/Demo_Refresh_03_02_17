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

public class Driver_Controller_VR : MonoBehaviour
{
    public int designated_player;
    byte current_player; // owner = player 1


    // Client Queue
    int frame = 0;

    int server_player;

    // general
    float pos_x;
    float pos_y;
    float pos_z;

    float rot_x;
    float rot_y;
    float rot_z;


    //trigger

    GameObject n_manager;
    network_manager n_manager_script;
    Drive_Control_CS drive_control;
    Control_Angles control_angles;

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
            //this.GetComponent<Drive_Control_CS>().enabled = false;
            //BroadcastMessage("DisableDriveWheel");
        }
    }

    void Start()
    {
        drive_control = GetComponent<Drive_Control_CS>();
        control_angles = GetComponent<Control_Angles>();
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

            Drive();
        }
    }

    void FixedUpdate()
    {
        /*if (n_manager != null)
        {
            update_world_state();
        }*/
    }

    void Drive()
    {
        float left_angle = control_angles.GetLeftLeverAngle();
        float right_angle = control_angles.GetRightLeverAngle();

        if (left_angle > 180)
        {
            left_angle = left_angle - 360;
        }

        if (right_angle > 180)
        {
            right_angle = right_angle - 360;
        }

        /*
        left_angle /= 30f;
        right_angle /= 30f;

        if (left_angle >= 0)
        {
            left_angle = 4.0f * left_angle;
        }
        else
        {
            left_angle = 2.0f * left_angle;
        }

        if (right_angle >= 0)
        {
            right_angle = 4.0f * right_angle;
        }
        else
        {
            right_angle = 2.0f * right_angle;
        }

        drive_control.Left_Speed_Step = left_angle;
        drive_control.Right_Speed_Step = right_angle;*/

        drive_control.Left_Speed_Step = left_angle;
        drive_control.Right_Speed_Step = right_angle;

        //Debug.Log("left: " + left_angle + " right : " + right_angle);
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
            this.transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(pos_x, pos_y, pos_z), 0.1f);
            this.transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rot_x, rot_y, rot_z), 0.1f);

            //this.transform.localPosition = new Vector3(pos_x, pos_y, pos_z);
            //this.transform.localRotation = Quaternion.Euler(rot_x, rot_y, rot_z);

  
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
        float[] hull_position_values = { transform.localPosition.x,
                                         transform.localPosition.y,
                                         transform.localPosition.z };

        float[] hull_rotation_values = { transform.localRotation.eulerAngles.x,
                                         transform.localRotation.eulerAngles.y,
                                         transform.localRotation.eulerAngles.z };

        n_manager_script.send_from_client(3, hull_position_values);
        n_manager_script.send_from_client(4, hull_rotation_values);

    }



    // Server Updates the server larger buffer it is going to send
    public void server_get_values_to_send()
    {

        float[] hull_position_values = { transform.localPosition.x,
                                         transform.localPosition.y,
                                         transform.localPosition.z };

        float[] hull_rotation_values = { transform.localRotation.eulerAngles.x,
                                         transform.localRotation.eulerAngles.y,
                                         transform.localRotation.eulerAngles.z };


        n_manager_script.send_from_server(3, hull_position_values);
        n_manager_script.send_from_server(4, hull_rotation_values);

    }




    // Client get values from the server buffer
    void client_update_values()
    {

        float[] hull_position_values = n_manager_script.client_read_server_buffer(3);
        float[] hull_rotation_values = n_manager_script.client_read_server_buffer(4);

        pos_x = hull_position_values[0];
        pos_y = hull_position_values[1];
        pos_z = hull_position_values[2];

        rot_x = hull_rotation_values[0];
        rot_y = hull_rotation_values[1];
        rot_z = hull_rotation_values[2];


    }

    // Server Get values from the client buffer, so the client inputs
    public void server_get_client_hands()
    {
        float[] hull_position_values = n_manager_script.server_read_client_buffer(3);
        float[] hull_rotation_values = n_manager_script.server_read_client_buffer(4);

        pos_x = hull_position_values[0];
        pos_y = hull_position_values[1];
        pos_z = hull_position_values[2];

        rot_x = hull_rotation_values[0];
        rot_y = hull_rotation_values[1];
        rot_z = hull_rotation_values[2];

    }
}