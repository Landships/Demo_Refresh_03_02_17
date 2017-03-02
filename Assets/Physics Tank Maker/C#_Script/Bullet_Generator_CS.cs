using UnityEngine;
using System.Collections;

public class Bullet_Generator_CS : MonoBehaviour
{

    public Mesh Bullet_Mesh;
    public Material Bullet_Material;
    public float Bullet_Mass = 5.0f;
    public float Bullet_Drag = 0.05f;
    public PhysicMaterial Bullet_PhysicMat;
    public Vector3 Bullet_Scale = new Vector3(0.762f, 0.762f, 0.762f);
    public float Bullet_Force = 250.0f;
    public Vector3 BoxCollider_Scale = new Vector3(1.0f, 1.0f, 1.0f);
    public float Delete_Time = 5.0f;
    public GameObject MuzzleFire_Object;
    public GameObject Impact_Object;
    public GameObject Ricochet_Object;
    public bool Trail_Flag = false;
    public Material Trail_Material;
    public float Trail_Start_Width = 0.01f;
    public float Trail_End_Width = 0.2f;
    public float Trail_Time = 0.1f;

    public Mesh Bullet_Mesh_HE;
    public Material Bullet_Material_HE;
    public float Bullet_Mass_HE = 5.0f;
    public float Bullet_Drag_HE = 0.05f;
    public Vector3 Bullet_Scale_HE = new Vector3(0.762f, 0.762f, 0.762f);
    public float Bullet_Force_HE = 250.0f;
    public Vector3 BoxCollider_Scale_HE = new Vector3(1.0f, 1.0f, 1.0f);
    public float Delete_Time_HE = 5.0f;
    public GameObject MuzzleFire_Object_HE;
    public float Explosion_Force = 60000.0f;
    public float Explosion_Radius = 20.0f;
    public GameObject Explosion_Object;
    public bool Trail_Flag_HE = false;
    public Material Trail_Material_HE;
    public float Trail_Start_Width_HE = 0.01f;
    public float Trail_End_Width_HE = 0.2f;
    public float Trail_Time_HE = 0.1f;

    public float Offset = 0.5f;
    public bool Debug_Flag = false;

    public int Bullet_Type = 0; // Referred to from "Cannon_Vertical".

    int Barrel_Type = 0;
    float Attack_Multiplier = 1.0f;

    Transform This_Transform;

    bool Flag = true;
    int Tank_ID;
    int Input_Type = 4;

    void Start()
    {
        Tank_ID_Control_CS Top_Script = GetComponentInParent<Tank_ID_Control_CS>();
        Attack_Multiplier = Top_Script.Attack_Multiplier;
    }

    void Complete_Turret()
    { // Called from 'Turret_Finishing".
        This_Transform = transform;
        // Send message (this GameObject) to "Cannon_Vertical".
        transform.parent.parent.SendMessage("Get_Bullet_Generator", this.gameObject, SendMessageOptions.DontRequireReceiver);
    }

    void Update()
    {
        if (Flag)
        {
           /* if (Input.GetKeyDown("v"))
            {
                if (Bullet_Type == 1)
                {
                    Bullet_Type = 0;
                }
                else
                {
                    Bullet_Type = 1;
                }
            }*/
        }
    }

    void Fire_Linkage(int Select_LR)
    {
        if (Barrel_Type == 0 || Barrel_Type == Select_LR)
        {
            switch (Bullet_Type)
            {
                case 0:
                    Set_AP();
                    break;
                case 1:
                    Set_HE();
                    break;
            }
        }
    }

    void Set_AP()
    {
        // Create Particle ( Prefab )
        if (MuzzleFire_Object)
        {
            GameObject Fire_Object;
            Fire_Object = Instantiate(MuzzleFire_Object, This_Transform.position, This_Transform.rotation) as GameObject;
            Fire_Object.transform.parent = This_Transform;
        }
        // Create GameObject & Set Transform
        GameObject Bullet_Object = new GameObject("Bullet_AP");
        Bullet_Object.transform.position = This_Transform.position + (This_Transform.forward * Offset);
        Bullet_Object.transform.rotation = This_Transform.rotation;
        Bullet_Object.transform.localScale = Bullet_Scale;
        // Add Components
        MeshRenderer Temp_MeshRenderer = Bullet_Object.AddComponent<MeshRenderer>();
        Temp_MeshRenderer.material = Bullet_Material;
        MeshFilter Temp_MeshFilter;
        Temp_MeshFilter = Bullet_Object.AddComponent<MeshFilter>();
        Temp_MeshFilter.mesh = Bullet_Mesh;
        Rigidbody Temp_Rigidbody = Bullet_Object.AddComponent<Rigidbody>();
        Temp_Rigidbody.mass = Bullet_Mass;
        Temp_Rigidbody.drag = Bullet_Drag;
        BoxCollider Temp_BoxCollider;
        Temp_BoxCollider = Bullet_Object.AddComponent<BoxCollider>();
        Temp_BoxCollider.size = Vector3.Scale(Temp_MeshFilter.mesh.bounds.size, BoxCollider_Scale);
        Temp_BoxCollider.center = new Vector3(0.0f, 0.0f, (Temp_BoxCollider.size.z - Temp_MeshFilter.mesh.bounds.size.z) / 2.0f);
        Temp_BoxCollider.material = Bullet_PhysicMat;
        if (Trail_Flag)
        {
            TrailRenderer Temp_TrailRenderer;
            Temp_TrailRenderer = Bullet_Object.AddComponent<TrailRenderer>();
            Temp_TrailRenderer.startWidth = Trail_Start_Width;
            Temp_TrailRenderer.endWidth = Trail_End_Width;
            Temp_TrailRenderer.time = Trail_Time;
            Temp_TrailRenderer.material = Trail_Material;
        }
        // Add Scripts
        Bullet_Control_CS Temp_Script;
        Temp_Script = Bullet_Object.AddComponent<Bullet_Control_CS>();
        Temp_Script.Set_Type(Bullet_Type);
        Temp_Script.Set_AP_Value(Delete_Time, Impact_Object, Ricochet_Object);
        Temp_Script.Attack_Multiplier = Attack_Multiplier;
        Temp_Script.Set_Debug_Mode(Debug_Flag);
        // Shoot
        Temp_Rigidbody.velocity = Bullet_Object.transform.forward * Bullet_Force;
    }

    void Set_HE()
    {
        // Create Particle ( Prefab )
        if (MuzzleFire_Object_HE)
        {
            GameObject Fire_Object;
            Fire_Object = Instantiate(MuzzleFire_Object_HE, This_Transform.position, This_Transform.rotation) as GameObject;
            Fire_Object.transform.parent = This_Transform;
        }
        // Create GameObject & Set Transform
        GameObject Bullet_Object = new GameObject("Bullet_HE");
        Bullet_Object.transform.position = This_Transform.position + (This_Transform.forward * Offset);
        Bullet_Object.transform.rotation = This_Transform.rotation;
        Bullet_Object.transform.localScale = Bullet_Scale_HE;
        // Add Components
        MeshRenderer Temp_MeshRenderer = Bullet_Object.AddComponent<MeshRenderer>();
        Temp_MeshRenderer.material = Bullet_Material_HE;
        MeshFilter Temp_MeshFilter;
        Temp_MeshFilter = Bullet_Object.AddComponent<MeshFilter>();
        Temp_MeshFilter.mesh = Bullet_Mesh_HE;
        Rigidbody Temp_Rigidbody = Bullet_Object.AddComponent<Rigidbody>();
        Temp_Rigidbody.mass = Bullet_Mass_HE;
        Temp_Rigidbody.drag = Bullet_Drag_HE;
        BoxCollider Temp_BoxCollider;
        Temp_BoxCollider = Bullet_Object.AddComponent<BoxCollider>();
        Temp_BoxCollider.size = Vector3.Scale(Temp_MeshFilter.mesh.bounds.size, BoxCollider_Scale_HE);
        Temp_BoxCollider.center = new Vector3(0.0f, 0.0f, (Temp_BoxCollider.size.z - Temp_MeshFilter.mesh.bounds.size.z) / 2.0f);
        if (Trail_Flag_HE)
        {
            TrailRenderer Temp_TrailRenderer;
            Temp_TrailRenderer = Bullet_Object.AddComponent<TrailRenderer>();
            Temp_TrailRenderer.startWidth = Trail_Start_Width_HE;
            Temp_TrailRenderer.endWidth = Trail_End_Width_HE;
            Temp_TrailRenderer.time = Trail_Time_HE;
            Temp_TrailRenderer.material = Trail_Material_HE;
        }
        // Add Scripts
        Bullet_Control_CS Temp_Script;
        Temp_Script = Bullet_Object.AddComponent<Bullet_Control_CS>();
        Temp_Script.Set_Type(Bullet_Type);
        Temp_Script.Set_HE_Value(Delete_Time_HE, Explosion_Force, Explosion_Radius, Explosion_Object);
        Temp_Script.Attack_Multiplier = Attack_Multiplier;
        Temp_Script.Set_Debug_Mode(Debug_Flag);
        // Shoot
        Temp_Rigidbody.velocity = Bullet_Object.transform.forward * Bullet_Force_HE;
    }

    void Set_Barrel_Type(int Temp_Type)
    { // Called from "Barrel_Base".
        Barrel_Type = Temp_Type;
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
            if (Input_Type != 10)
            {
                Flag = true;
            }
        }
        else
        {
            Flag = false;
        }
    }

    void Get_AI(AI_CS Temp_Script)
    {
        Bullet_Type = Temp_Script.Bullet_Type;
    }

}