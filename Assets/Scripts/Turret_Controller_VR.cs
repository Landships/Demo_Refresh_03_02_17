using UnityEngine;
using System.Collections;

/// <summary>
/// THIS IS OWNED BY PLAYER 2/Client
/// </summary>

public class Turret_Controller_VR : Fire_Controller
{
    public int designated_player;
    byte current_player; // owner = player 2

    // Client Queue
    int frame = 0;

    int server_player;

    // general
    float turret_position_x;
    float turret_position_y;
    float turret_position_z;

    float turret_rotation_x;
    float turret_rotation_y;
    float turret_rotation_z;

    float turret_base_rotation_y;
    float cannon_base_rotation_x;


    //trigger

    GameObject n_manager;
    network_manager n_manager_script;
    public GameObject turret_objects;
    public GameObject cannon_base;
    public GameObject button;
    public GameObject turret;
    Control_Angles control_angles;
    Cannon_Vertical_CS cannon_vertical;
    Turret_Horizontal_CS turret_horizontal;
    Cannon_Fire_CS cannon_fire;
    GameObject turret_base;

    bool started = false;
    bool ready = false;

    bool reliable_message = false;

    int frame_interval = 5;

    public void Prep() {
        n_manager = GameObject.Find("Custom Network Manager(Clone)");
        n_manager_script = n_manager.GetComponent<network_manager>();
        current_player = (byte)(n_manager_script.client_players_amount);
        if (current_player != designated_player) {
            cannon_vertical.enabled = false;
            button.GetComponent<VRTK.Button>().enabled = false;
            turret_horizontal.enabled = false;
        }
    }

    void Start() {
        control_angles = GetComponent<Control_Angles>();
        cannon_vertical = cannon_base.GetComponent<Cannon_Vertical_CS>();
        cannon_fire = cannon_base.GetComponent<Cannon_Fire_CS>();
        turret_horizontal = turret_objects.GetComponentInChildren<Turret_Horizontal_CS>();
        turret_base = turret_horizontal.gameObject;

    }

    void Update() {

        if (n_manager != null) {
            started = n_manager_script.started;
            ready = n_manager_script.game_ready;

            server_player = n_manager_script.server_player_control;

            reliable_message = n_manager_script.reliable_message;

            if (current_player == designated_player) {
                if (reliable_message)
                {
                    if (n_manager_script.client_read_server_reliable_buffer(6) != 0)
                    {
                        Debug.Log("player tank receive penetration");
                        BroadcastMessage("Alert", n_manager_script.client_read_server_reliable_buffer(6));
                    }
                }
                Move_Turret();
                client_send_values();
                
            }
            else
            {
                if (reliable_message)
                {
                    Debug.Log("reliable message in turret_controller");
                    if (n_manager_script.server_read_client_reliable_buffer(1) == 1)
                    {
                        Debug.Log("player tank receive fire");
                        cannon_fire.Fire();
                    }
                }
                server_get_client_hands();
            }
        }
    }

    override public void OwnerFire()
    {
        if (current_player != designated_player)
            return;
        cannon_fire.Fire();
        n_manager_script.send_reliable_from_client(1, 1);
        Debug.Log("Emit Fire");
    }


    public void Alert_Penetration(int Type)
    {
        if (current_player == 2)
            return;
        //BroadcastMessage("Alert", Type);
        //n_manager_script.send_reliable_from_server(6, Type);
        Debug.Log("Alert Penetration, type = " + Type);
        //reliable
    }

    void FixedUpdate() {
        if (n_manager != null) {
            update_world_state();
        }
    }

    void Move_Turret() {
        cannon_vertical.Temp_Vertical = control_angles.GetVertCrankDelta() / 20f;
        turret_horizontal.Temp_Horizontal = control_angles.GetHoriCrankDelta() / 15f;
        
    }



    void update_world_state() {
        if (current_player == designated_player) { 
            //
        } else {
            if (Quaternion.Angle(turret_base.transform.localRotation, Quaternion.Euler(0, turret_base_rotation_y, 0)) > 0.1f)
            {
                turret_base.transform.localRotation = Quaternion.Lerp(turret_base.transform.localRotation, Quaternion.Euler(0, turret_base_rotation_y, 0), 0.1f);
                
            }
            cannon_base.transform.localRotation = Quaternion.Lerp(cannon_base.transform.localRotation, Quaternion.Euler(cannon_base_rotation_x, 0, 0), 0.1f);
            //Debug.Log(cannon_vertical.Current_Angle);
        }
    }


    public byte get_client_player_number() {
        return current_player;
    }


    // ----------------------------
    // Functions that use Block Copy
    // ----------------------------


    // The client get its values/inputs to send to the server
    void client_send_values() {
        float[] cannon_base_rotation_values = { cannon_base.transform.localEulerAngles.x,
                                                turret_base.transform.localEulerAngles.y,
                                                0};
        /*
        float[] hull_rotation_values = { transform.localRotation.eulerAngles.x,
                                         transform.localRotation.eulerAngles.y,
                                         transform.localRotation.eulerAngles.z };
        */
        //Debug.Log("Client Buffer Values: Putting In");
        //Debug.Log(cannon_base_rotation_values[0].ToString());
        //Debug.Log(cannon_base_rotation_values[1].ToString());
        //Debug.Log(cannon_base_rotation_values[2].ToString());
        n_manager_script.send_from_client(6, cannon_base_rotation_values);
        //n_manager_script.send_from_client(4, hull_rotation_values);

    }



    // Server Updates the server larger buffer it is going to send
    public void server_get_values_to_send() {

        float[] cannon_base_rotation_values = { cannon_base.transform.localEulerAngles.x,
                                                turret_base.transform.localEulerAngles.y,
                                                0};
        /*
        float[] hull_rotation_values = { transform.localRotation.eulerAngles.x,
                                         transform.localRotation.eulerAngles.y,
                                         transform.localRotation.eulerAngles.z };
        */

        n_manager_script.send_from_server(6, cannon_base_rotation_values);
        //n_manager_script.send_from_server(4, hull_rotation_values);

    }




    // Client get values from the server buffer
    void client_update_values() {

        float[] cannon_base_rotation_values = n_manager_script.client_read_server_buffer(6);
        //float[] hull_rotation_values = n_manager_script.client_read_server_buffer(4);


        cannon_base_rotation_x = cannon_base_rotation_values[0];
        turret_base_rotation_y = cannon_base_rotation_values[1];
         /*
        pos_x = hull_position_values[0];
        pos_y = hull_position_values[1];
        pos_z = hull_position_values[2];

        rot_x = hull_rotation_values[0];
        rot_y = hull_rotation_values[1];
        rot_z = hull_rotation_values[2];
        */


    }

    // Server Get values from the client buffer, so the client inputs
    public void server_get_client_hands() {
        float[] cannon_base_rotation_values = n_manager_script.server_read_client_buffer(6);
        //Debug.Log("Server Buffer Values: Taking Out");
        //Debug.Log(cannon_base_rotation_values[0].ToString());
        //Debug.Log(cannon_base_rotation_values[1].ToString());
        //Debug.Log(cannon_base_rotation_values[2].ToString());
        cannon_base_rotation_x = cannon_base_rotation_values[0];
        turret_base_rotation_y = cannon_base_rotation_values[1];
        //float[] hull_rotation_values = n_manager_script.server_read_client_buffer(4);

        //pos_x = hull_position_values[0];
        //pos_y = hull_position_values[1];
        //pos_z = hull_position_values[2];

        //rot_x = hull_rotation_values[0];
        //rot_y = hull_rotation_values[1];
        //rot_z = hull_rotation_values[2];

    }
}