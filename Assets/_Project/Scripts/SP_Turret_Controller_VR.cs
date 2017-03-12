using UnityEngine;
using System.Collections;

/// <summary>
/// THIS IS OWNED BY PLAYER 2/Client
/// </summary>

public class SP_Turret_Controller_VR : Fire_Controller
{
    //trigger
    public GameObject turret_objects;
    public GameObject cannon_base;
    public GameObject button;
    public GameObject turret;
    Control_Angles control_angles;
    Cannon_Vertical_CS cannon_vertical;
    Turret_Horizontal_CS turret_horizontal;
    Cannon_Fire_CS cannon_fire;
    GameObject turret_base;

    byte current_player = 2;


    public void Prep() {
        BroadcastMessage("Set_Current_Player", current_player);
    }

    void Start() {
        control_angles = GetComponent<Control_Angles>();
        cannon_vertical = cannon_base.GetComponent<Cannon_Vertical_CS>();
        cannon_fire = cannon_base.GetComponent<Cannon_Fire_CS>();
        turret_horizontal = turret_objects.GetComponentInChildren<Turret_Horizontal_CS>();
        turret_base = turret_horizontal.gameObject;

    }

    void Update() {
       Move_Turret();
   }

    override public void OwnerFire()
    {
        cannon_fire.Fire();
    }


    public void Alert_Turret_Penetration()
    {
        turret.GetComponent<Damage_Control_CS>().Penetration();
        //reliable
    }

    void Move_Turret() {
        cannon_vertical.Temp_Vertical = control_angles.GetVertCrankDelta() / 20f;
        turret_horizontal.Temp_Horizontal = control_angles.GetHoriCrankDelta() / 20f;
        
    }

    public byte get_client_player_number() {
        return current_player;
    }

}