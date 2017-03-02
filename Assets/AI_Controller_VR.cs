using UnityEngine;
using System.Collections;

public class AI_Controller_VR : MonoBehaviour {

    public int ai_id;
    byte current_player; // owner = 2

    GameObject n_manager;
    network_manager n_manager_script;
    Cannon_Fire_CS cannon_fire;
    AI_CS ai_script;
    Drive_Control_CS drive_control;

    bool started = false;
    bool ready = false;

    bool reliable_message = false;
    int client_player;

    int frame_interval = 5;




    GameObject turret_object;
    public GameObject cannon_base;
    GameObject turret_base;

    // Network Values
    float pos_x;
    float pos_y;
    float pos_z;
    float rot_x;
    float rot_y;
    float rot_z;
    float turret_base_rotation_y;
    float cannon_base_rotation_x;
    float vertical_input;
    float horizontal_input;


    // Use this for initialization
    void Start()
    {
        
    }



    public void Prep()
    {
        turret_object = transform.FindChild("Turret_Objects").gameObject;
        turret_base = GetComponentInChildren<Turret_Horizontal_CS>().gameObject;
        n_manager = GameObject.Find("Custom Network Manager(Clone)");
        n_manager_script = n_manager.GetComponent<network_manager>();
        current_player = (byte)(n_manager_script.client_players_amount);
        drive_control = GetComponent<Drive_Control_CS>();
        cannon_fire = cannon_base.GetComponent<Cannon_Fire_CS>();
        if (current_player == 1)
        {
            transform.FindChild("AI_Core").GetComponent<AI_CS>().enabled = true;
            ai_script = transform.Find("AI_Core").GetComponent<AI_CS>();
            
        }
        BroadcastMessage("Set_Ai_Id", ai_id);
    }



    void Update()
    {

        if (n_manager != null)
        {
            started = n_manager_script.started;
            ready = n_manager_script.game_ready;

            client_player = n_manager_script.server_player_control;

            reliable_message = n_manager_script.reliable_message;

                if (reliable_message)
                {
                    if (n_manager_script.client_read_server_reliable_buffer(2) != 0 && ai_id == 1)
                    {
                        BroadcastMessage("Alert", n_manager_script.client_read_server_reliable_buffer(2));
                    }
                    if (n_manager_script.client_read_server_reliable_buffer(3) != 0 && ai_id == 2)
                    {
                        BroadcastMessage("Alert", n_manager_script.client_read_server_reliable_buffer(3));
                    }
                    if (n_manager_script.client_read_server_reliable_buffer(4) != 0 && ai_id == 3)
                    {
                        BroadcastMessage("Alert", n_manager_script.client_read_server_reliable_buffer(4));
                    }
                    if (n_manager_script.client_read_server_reliable_buffer(5) != 0 && ai_id == 4)
                    {
                        BroadcastMessage("Alert", n_manager_script.client_read_server_reliable_buffer(5));
                    }
                    if (n_manager_script.client_read_server_reliable_buffer(7) == 1 && ai_id == 1)
                    {
                        cannon_fire.Fire();
                    }
                    if (n_manager_script.client_read_server_reliable_buffer(8) == 1 && ai_id == 2)
                    {
                        cannon_fire.Fire();
                    }
                    if (n_manager_script.client_read_server_reliable_buffer(9) == 1 && ai_id == 3)
                    {
                        cannon_fire.Fire();
                    }
                    if (n_manager_script.client_read_server_reliable_buffer(10) == 1 && ai_id == 4)
                    {
                        cannon_fire.Fire();
                    }
            }



            update_world_state();


            if (current_player == 1)
            {
                server_get_values_to_send();
            }

            else
            {
                client_update_values();
            }


        }
    }


    public void Alert_Penetration(int id, int Type)
    {
        if (current_player == 2)
            return;
        //transform.FindChild("Turret").GetComponent<Damage_Control_CS>().Penetration();
        BroadcastMessage("Alert", Type);
        n_manager_script.send_reliable_from_server(ai_id + 1, Type);

    }

    public void OwnerFire()
    {
        cannon_fire.Fire();
        n_manager_script.send_reliable_from_server(6 + ai_id, 1);
        Debug.Log("Emit Fire");
    }
    


    void update_world_state()
    {
        // Server
        if (current_player == 1)
        {
            drive_control.Vertical = ai_script.Speed_Order;
            drive_control.Horizontal = ai_script.Turn_Order;
        }
        // Client
        else
        {
            this.transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(pos_x, pos_y, pos_z), 0.1f);
            this.transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(rot_x, rot_y, rot_z), 0.1f);

            if (Quaternion.Angle(turret_object.transform.localRotation, Quaternion.Euler(0, turret_base_rotation_y, 0)) > 0.1f)
            {
                turret_base.transform.localRotation = Quaternion.Lerp(turret_base.transform.localRotation, Quaternion.Euler(0, turret_base_rotation_y, 0), 0.1f);
                //turret_base.transform.localRotation = Quaternion.Euler(0, turret_base_rotation_y, 0);

            }
            cannon_base.transform.localRotation = Quaternion.Lerp(cannon_base.transform.localRotation, Quaternion.Euler(cannon_base_rotation_x, 0, 0), 0.1f);
            //cannon_base.transform.localRotation = Quaternion.Euler(cannon_base_rotation_x, 0, 0);
            drive_control.Vertical = vertical_input;
            drive_control.Horizontal = horizontal_input;



        }



    }




    void server_get_values_to_send()
    {



        float[] all_values = { transform.localPosition.x,
                               transform.localPosition.y,
                               transform.localPosition.z,
                               transform.localRotation.eulerAngles.x,
                               transform.localRotation.eulerAngles.y,
                               transform.localRotation.eulerAngles.z,
                               turret_base.transform.localEulerAngles.y,
                               cannon_base.transform.localEulerAngles.x,
                               ai_script.Speed_Order,
                               ai_script.Turn_Order
                             };

       

        n_manager_script.send_from_server(11 + ai_id, all_values);


    }



    void client_update_values()
    {
        float[] all_values = n_manager_script.client_read_server_buffer(11 + ai_id);
        pos_x = all_values[0];
        pos_y = all_values[1];
        pos_z = all_values[2];
        rot_x = all_values[3];
        rot_y = all_values[4];
        rot_z = all_values[5];
        turret_base_rotation_y = all_values[6];
        cannon_base_rotation_x = all_values[7];
        vertical_input = all_values[8];
        horizontal_input = all_values[9];

    }












}
