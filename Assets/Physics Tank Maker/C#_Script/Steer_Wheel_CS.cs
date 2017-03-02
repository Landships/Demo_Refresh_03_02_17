using UnityEngine;
using System.Collections;

public class Steer_Wheel_CS : MonoBehaviour
{

    public float Reverse = 1.0f;
    public float Max_Angle = 35.0f;
    public float Rotation_Speed = 45.0f;

    float Horizontal;
    float Current;

    HingeJoint This_HingeJoint;
    JointSpring This_JointSpring;

    bool Flag = true;
    int Tank_ID;
    int Input_Type = 4;

    AI_CS AI_Script;
    Drive_Control_CS Control_Script;

    void Start()
    {
        This_HingeJoint = GetComponent<HingeJoint>();
        This_JointSpring = This_HingeJoint.spring;
    }

    void Update()
    {
        if (Flag)
        {
            switch (Input_Type)
            {
                case 0:
                    KeyBoard_Input();
                    break;
                case 1:
                    Stick_Input();
                    break;
                case 2:
                    Trigger_Input();
                    break;
                case 3:
                    Stick_Trigger_Input();
                    break;
                case 4:
                    Mouse_Input();
                    break;
                case 5:
                    Mouse_Input();
                    break;
                case 10:
                    AI_Input();
                    break;
            }
        }
    }

    void KeyBoard_Input()
    {
        if (Input.GetKey("z") == false && Input.GetKey("c") == false)
        {
            Base_Input();
        }
    }

    void Stick_Input()
    {
        if (Input.GetButton("Jump") == false)
        {
            Base_Input();
        }
    }

    void Trigger_Input()
    {
        float L_Temp = Input.GetAxis("L_Trigger") - Input.GetAxis("L_Button");
        float R_Temp = Input.GetAxis("R_Trigger") - Input.GetAxis("R_Button");
        if (L_Temp > 0.0f || R_Temp > 0.0f)
        {
            Horizontal = L_Temp - R_Temp;
            if (Horizontal > 1.0f)
            {
                Horizontal = 1.0f;
            }
            else if (Horizontal < -1.0f)
            {
                Horizontal = -1.0f;
            }
            Steer();
        }
        else if (L_Temp < 0.0f || R_Temp < 0.0f)
        {
            Horizontal = R_Temp - L_Temp;
            if (Horizontal > 1.0f)
            {
                Horizontal = 1.0f;
            }
            else if (Horizontal < -1.0f)
            {
                Horizontal = -1.0f;
            }
            Steer();
        }
    }

    void Stick_Trigger_Input()
    {
        if (Input.GetButton("Jump") == false)
        {
            Horizontal = Input.GetAxis("Horizontal");
            if (Input.GetAxis("R_Trigger") != 0.0f || Input.GetAxis("L_Trigger") != 0.0f || Horizontal != 0.0f)
            {
                Steer();
            }
        }
    }

    void Mouse_Input()
    {
        if (Input.GetKey("e"))
        {
            Horizontal = 0.3f;
        }
        else if (Input.GetKey("d"))
        {
            Horizontal = 1.0f;
        }
        else if (Input.GetKey("c"))
        {
            Horizontal = 1.0f;
        }
        else if (Input.GetKey("q"))
        {
            Horizontal = -0.3f;
        }
        else if (Input.GetKey("a"))
        {
            Horizontal = -1.0f;
        }
        else if (Input.GetKey("z"))
        {
            Horizontal = -1.0f;
        }
        else
        {
            Horizontal = 0.0f;
        }
        Steer();
    }

    void Base_Input()
    {
        Horizontal = Input.GetAxis("Horizontal");
        if (Horizontal != 0.0f)
        {
            Steer();
        }
    }

    void AI_Input()
    {
        Horizontal = AI_Script.Turn_Order;
        if (AI_Script.Slow_Turn_Flag)
        {
            Horizontal *= (1.0f / AI_Script.Slow_Turn_Rate);
        }
        if (Horizontal != 0.0f)
        {
            Steer();
        }
    }

    void Steer()
    {
        if (Control_Script.Stop_Flag)
        {
            return; // No steer
        }
        // Steer
        float Target = Max_Angle * Horizontal;
        if (Mathf.Abs(Target - Current) < 0.5f)
        {
            Current = Target;
        }
        else
        {
            if (Current > Target)
            {
                Current -= Rotation_Speed * Time.deltaTime;
            }
            else if (Current < Target)
            {
                Current += Rotation_Speed * Time.deltaTime;
            }
        }
        This_JointSpring.targetPosition = -Current * Reverse;
        This_HingeJoint.spring = This_JointSpring;
    }

    void Set_Input_Type(int Temp_Input_Type)
    {
        Input_Type = Temp_Input_Type;
    }

    void Set_Tank_ID(int Temp_Tank_ID)
    {
        Tank_ID = Temp_Tank_ID;
    }

    void Receive_Current_ID(int Temp_Current_ID)
    {
        if (Temp_Current_ID == Tank_ID)
        {
            Flag = true;
        }
        else
        {
            if (Input_Type == 10)
            {
                Flag = true;
            }
            else
            {
                Flag = false;
            }
        }
    }

    void Get_AI(AI_CS Temp_Script)
    {
        AI_Script = Temp_Script;
    }

    void MainBody_Linkage()
    { // Called from MainBody's "Damage_Control".
        Destroy(this);
    }

    void Get_Drive_Control(Drive_Control_CS Temp_Script)
    { // Called from "Drive_Control" in the MainBody.
        Control_Script = Temp_Script;
    }

}