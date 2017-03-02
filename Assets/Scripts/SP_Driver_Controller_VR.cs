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

public class SP_Driver_Controller_VR : MonoBehaviour
{
    byte current_player = 1; 


    Drive_Control_CS drive_control;
    Control_Angles control_angles;

    void Start()
    {
        drive_control = GetComponent<Drive_Control_CS>();
        control_angles = GetComponent<Control_Angles>();
    }

    void Update()
    {
        Drive();   
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

        drive_control.Left_Speed_Step = left_angle;
        drive_control.Right_Speed_Step = right_angle;

        //Debug.Log("left: " + left_angle + " right : " + right_angle);
    }

   

    public byte get_client_player_number()
    {
        return current_player;
    }


  
}