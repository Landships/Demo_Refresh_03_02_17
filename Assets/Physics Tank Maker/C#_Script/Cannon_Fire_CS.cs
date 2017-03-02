using UnityEngine;
using System.Collections;

public class Cannon_Fire_CS : MonoBehaviour
{

    public float Reload_Time = 2.0f;
    public float Recoil_Force = 5000.0f;
    public bool Karl_Flag = false;

    public bool Reload_Flag = true; // Referred to from "Cannon_Vertical".
    public bool Open_Fire_Flag = true;

    Rigidbody MainBody_Rigidbody;
    Transform This_Transform;
    int Switch_LR = 1;
    bool Trouble_Flag = false;

    bool Flag = true;
    int Tank_ID;
    int Input_Type = 4;

    Turret_Horizontal_CS Turret_Horizontal_Script;

    void Complete_Turret()
    { // Called from 'Turret_Finishing" when the sorting is finished.
        This_Transform = transform;
        Turret_Horizontal_Script = GetComponentInParent<Turret_Horizontal_CS>();
        MainBody_Setting_CS Temp_Script = GetComponentInParent<MainBody_Setting_CS>();
        if (Temp_Script)
        {
            MainBody_Rigidbody = Temp_Script.GetComponent<Rigidbody>();
        }
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
                    Stick_Input();
                    break;
                case 4:
                    Mouse_Input();
                    break;
                case 5:
                    Mouse_Input();
                    break;
            }
        }
    }

    void Stick_Input()
    {
        if (Input.GetButton("R_Button"))
        {
            //Fire();
        }
    }

    void Trigger_Input()
    {
        if (Input.GetButton("Fire3"))
        {
            //Fire();
        }
    }

    void KeyBoard_Input()
    {
        if (Input.GetKey("x"))
        {
            //Fire();
        }
    }

    void Mouse_Input()
    {
        if (Input.GetMouseButton(0))
        {
            //Fire();
        }
    }

    public void Fire()
    {
        Debug.Log("FIRE");
        if (Reload_Flag && Trouble_Flag == false && Turret_Horizontal_Script.OpenFire_Flag)
        {
            if (Karl_Flag)
            { // Send message to Karl's Turret_Base with "Recoil_Brake". 
                transform.parent.BroadcastMessage("Fire_Linkage", Switch_LR, SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                BroadcastMessage("Fire_Linkage", Switch_LR, SendMessageOptions.DontRequireReceiver);
            }
            MainBody_Rigidbody.AddForceAtPosition(-This_Transform.forward * Recoil_Force, This_Transform.position, ForceMode.Impulse);
            Reload_Flag = false;
            StartCoroutine("Reload");
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(Reload_Time);
        Reload_Flag = true;
        if (Switch_LR == 1)
        {
            Switch_LR = 2;
        }
        else
        {
            Switch_LR = 1;
        }
    }

    void Turret_Linkage()
    {
        Destroy(this);
    }

    public bool Trouble(float Temp_Time)
    {
        if (!Trouble_Flag)
        {
            Trouble_Flag = true;
            StartCoroutine("Trouble_Count", Temp_Time);
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator Trouble_Count(float Temp_Time)
    {
        yield return new WaitForSeconds(Temp_Time);
        Trouble_Flag = false;
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
            Flag = false;
        }
    }
}