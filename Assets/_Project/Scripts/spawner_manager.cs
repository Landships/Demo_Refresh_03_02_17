using UnityEngine;
using System.Collections;

public class spawner_manager : MonoBehaviour
{
    public GameObject prefab_to_spawn;
    public GameObject prefab_to_spawn_vr;
    static GameObject camera_rig;
    static GameObject left_controller;
    static GameObject right_controller;
    static NetworkPrep prep_script;
    static Ai_Prep ai_1;
    static Ai_Prep ai_2;
    static Ai_Prep ai_3;
    static Ai_Prep ai_4;
    static GameObject gunner_spawn;
    static GameObject driver_spawn;
    static TextHide driver_text;
    static TextHide turret_text;

    void Awake()
    {
       
        camera_rig = GameObject.Find("[CameraRig]");
        left_controller = camera_rig.transform.FindChild("Controller (left)").gameObject;
        right_controller = camera_rig.transform.FindChild("Controller (right)").gameObject;
        prep_script = GameObject.Find("PlayerTank").transform.GetChild(0).GetComponent<NetworkPrep>();
        ai_1 = GameObject.Find("AI_1").transform.GetChild(0).GetComponent<Ai_Prep>();
        ai_2 = GameObject.Find("AI_2").transform.GetChild(0).GetComponent<Ai_Prep>();
        ai_3 = GameObject.Find("AI_3").transform.GetChild(0).GetComponent<Ai_Prep>();
        ai_4 = GameObject.Find("AI_4").transform.GetChild(0).GetComponent<Ai_Prep>();
        driver_text = GameObject.Find("DriverText").GetComponent<TextHide>();
        turret_text = GameObject.Find("TurretText").GetComponent<TextHide>();
        driver_spawn = GameObject.Find("CorrectDriverPos");
        gunner_spawn = GameObject.Find("GunnerPos");
    }


    public void spawn_four_players(byte num_players)
    {
        GameObject n_manager = GameObject.Find("Custom Network Manager(Clone)");
        network_manager n_manager_script = n_manager.GetComponent<network_manager>();

        //Debug.Log("I will spawn " + " players");
        byte tally = 1;
        while (tally <= 2)
        {
            spawn_player(num_players, tally);
            tally++;
        }

        //spawn_player(1, 1);
        //spawn_player(2, 2);
        //spawn_player(3, 3);
        //spawn_player(4, 4);

        n_manager_script.game_ready = true;


    }



    public void spawn_player(byte number, byte owner)
    {
        float x = 0;
        float y = 0;
        float z = 0;


        switch (number)
        {
            case 1:
                x = -0.55f;
                y = -0.626f;
                z = 1.285f;

                break;

            case 2:
                x = 0f;
                y = 0.7f;
                z = -0.5f;

                break;

            case 3:
                x = -15;
                y = 1;
                z = -15;

                break;

            case 4:
                x = 15;
                y = 1;
                z = -15;

                break;
        }


        // Instiantiate VR Players



        GameObject vr_player = Instantiate(prefab_to_spawn_vr, new Vector3(x, y, z), Quaternion.identity) as GameObject;

        vr_player.gameObject.GetComponent<PlayerController_VR>().owner = owner;

        // Debug.Log("DONE");

        GameObject n_manager = GameObject.Find("Custom Network Manager(Clone)");
        network_manager n_manager_script = n_manager.GetComponent<network_manager>();
        //byte current_player = (byte)(n_manager_script.client_players_amount);


        // ADD OWNER TODO!!!!!!!!!!!!!!!!!!
        if (owner == 1)
        {
            vr_player.transform.parent = driver_spawn.transform;
            vr_player.transform.position = driver_spawn.transform.position;
            vr_player.transform.rotation = driver_spawn.transform.rotation;
        }
        else
        {
            vr_player.transform.parent = gunner_spawn.transform;
            vr_player.transform.position = gunner_spawn.transform.position;
            vr_player.transform.rotation = gunner_spawn.transform.rotation;
        }


        vr_player.gameObject.GetComponent<PlayerController_VR>().camera_rig = camera_rig;


        vr_player.gameObject.GetComponent<PlayerController_VR>().left_controller = left_controller.gameObject;
        vr_player.gameObject.GetComponent<PlayerController_VR>().right_controller = right_controller.gameObject;
        vr_player.gameObject.GetComponent<PlayerController_VR>().Setup();

        if (number == 2)
        {
            camera_rig.transform.parent = gunner_spawn.transform;
            camera_rig.transform.position = gunner_spawn.transform.position;
        }

        prep_script.BroadCast();
        ai_1.BroadCast();
        ai_2.BroadCast();
        ai_3.BroadCast();
        ai_4.BroadCast();

        driver_text.DelayHide();
        turret_text.DelayHide();
    }


}
