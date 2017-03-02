using UnityEngine;
using System.Collections;

public class Cannon_Vertical_CS : MonoBehaviour
{

    public float Max_Elevation = 13.0f;
    public float Max_Depression = 7.0f;
    public float Speed_Mag = 5.0f;
    public float Buffer_Angle = 5.0f;
    public bool Auto_Angle_Flag = true;
    public bool Upper_Course = false;
    public bool AI_Reference = true;

    public float Current_Angle;
    float Target_Angle;


    public float Temp_Vertical; // Referred to from "Sound_Control_CS".



    bool Tracking_Flag = false;
    bool Moving_Flag = false;
    bool Fire_Flag = false;
    float Offset_Angle;
    Vector3 Last_Mouse_Pos;
    float Grabity;
    float RayCast_Count;
    float Cannon_Count;
    float Wait_Count;
    Transform This_Transform;
    Transform Bullet_Generator_Transform;

    int Ray_LayerMask = ~((1 << 10) + (1 << 2)); // Layer 2 = Ignore Ray, Layer 10 = Ignore All.

    bool Flag = true;
    int Tank_ID;
    int Input_Type = 4;

    Turret_Horizontal_CS Turret_Horizontal_Script;
    Bullet_Generator_CS Bullet_Generator_Script;
    Cannon_Fire_CS Cannon_Fire_Script;
    AI_CS AI_Script;
    public AI_Controller_VR ai_controller;

    void Start()
    { // Turret's objects are sorted at the opening.
    }

    void Complete_Turret()
    { // Called from 'Turret_Finishing".
        This_Transform = transform;
        Current_Angle = This_Transform.localEulerAngles.x;
        Max_Elevation = Current_Angle - Max_Elevation;
        Max_Depression = Current_Angle + Max_Depression;
        Grabity = Physics.gravity.y;
        Turret_Horizontal_Script = transform.parent.GetComponent<Turret_Horizontal_CS>();
        Cannon_Fire_Script = GetComponent<Cannon_Fire_CS>();
        Debug.Log("Current = " + Current_Angle + " Max Elevation = " + Max_Elevation + " Max_Depress = " + Max_Depression);
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
                case 8:
                    Crank_Input();
                    break;
            }
        }
    }

    void FixedUpdate()
    {
        if (Moving_Flag)
        {
            switch (Input_Type)
            {
                case 4:
                    Auto_Turn();
                    break;
                case 5:
                    Auto_Turn();
                    break;
                case 10:
                    AI_Input();
                    break;
                case 8:
                    Auto_Turn();
                    break;
            }
        }
    }

    void Crank_Input()
    {
        //Debug.Log(Temp_Vertical.ToString());
        Rotate();
    }

    void Stick_Input()
    {
        if (Input.GetButton("L_Button"))
        {
            Temp_Vertical = -Input.GetAxis("Vertical2");
            Rotate();
        }
    }

    void Trigger_Input()
    {
        if (Input.GetButton("Fire1") == false && Input.GetButton("Jump") == false && Input.GetAxis("Vertical") != 0)
        {
            Temp_Vertical = -Input.GetAxis("Vertical");
            Rotate();
        }
    }

    void KeyBoard_Input()
    {
        if (Input.GetKey("z"))
        {
            Temp_Vertical = -Input.GetAxis("Vertical");
            Rotate();
        }
    }

    void Mouse_Input()
    {
        // Adjust aiming.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Last_Mouse_Pos = Input.mousePosition;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            Offset_Angle += (Input.mousePosition.y - Last_Mouse_Pos.y) * 0.02f;
            Last_Mouse_Pos = Input.mousePosition;
        }
        else {
            Offset_Angle = 0.0f;
        }
    }

    void Start_Tracking()
    { // This function is called from "Turret_Horizontal".
        Tracking_Flag = true;
        Moving_Flag = true;
        if (Input_Type == 10)
        { // AI
            Random_Offset();
            Cannon_Count = 0.0f;
            Wait_Count = 0.0f;
        }
    }

    void Stop_Tracking()
    { // This function is called from "Turret_Horizontal".
        Tracking_Flag = false;
    }

    void Auto_Turn()
    { // Calculate the angle and rate, and rotate the cannon.
      // Calculate the angle.
        if (Tracking_Flag)
        { // Tracking now.
            if (Auto_Angle_Flag && Turret_Horizontal_Script.Free_Aim_Flag == false)
            {
                Target_Angle = Auto_Angle(); // Calculate the proper angle.
            }
            else {
                Target_Angle = Manual_Angle(); // Simply turn toward the target.
            }
            Target_Angle += Mathf.DeltaAngle(0.0f, This_Transform.localEulerAngles.x);
            Target_Angle += Offset_Angle;
        }
        else { // Not tracking. Return to the initial position.
            Target_Angle = -Mathf.DeltaAngle(This_Transform.localEulerAngles.x, 0.0f);  // Calculate the angle to the initial position.
            if (Mathf.Abs(Target_Angle) < 0.01f)
            { // Cannon returned to the initial position.
                Moving_Flag = false; // Stop turning.
            }
        }
        // Calculate Turn Rate.
        if (Target_Angle > 0.01f)
        {
            Temp_Vertical = -Mathf.Lerp(0.0f, 1.0f, Target_Angle / (Speed_Mag * Time.fixedDeltaTime + Buffer_Angle));
            Rotate_In_FixedUpdate();
        }
        else if (Target_Angle < -0.01f)
        {
            Temp_Vertical = Mathf.Lerp(0.0f, 1.0f, -Target_Angle / (Speed_Mag * Time.fixedDeltaTime + Buffer_Angle));
            Rotate_In_FixedUpdate();
        }
        else {
            Temp_Vertical = 0.0f;
        }
    }

    float Auto_Angle()
    { // Calculate the proper angle.
        float Temp_Angle;
        Vector3 Target_Pos = Turret_Horizontal_Script.Target_Pos;
        float Distance_X = Vector2.Distance(new Vector2(Target_Pos.x, Target_Pos.z), new Vector2(This_Transform.position.x, This_Transform.position.z));
        float Distance_Y = Target_Pos.y - This_Transform.position.y;
        float Initial_Velocity = 0.0f;
        switch (Bullet_Generator_Script.Bullet_Type)
        {
            case 0:
                Initial_Velocity = Bullet_Generator_Script.Bullet_Force;
                break;
            case 1:
                Initial_Velocity = Bullet_Generator_Script.Bullet_Force_HE;
                break;
        }
        float Pos_Base = (Grabity * Mathf.Pow(Distance_X, 2.0f)) / (2.0f * Mathf.Pow(Initial_Velocity, 2.0f));
        float Pos_X_Base = Distance_X / Pos_Base;
        float Pos_Y_Base = (Mathf.Pow(Pos_X_Base, 2.0f) / 4.0f) - ((Pos_Base - Distance_Y) / Pos_Base);
        if (Pos_Y_Base >= 0.0f)
        {
            if (Upper_Course)
            {
                Temp_Angle = Mathf.Rad2Deg * Mathf.Atan(-Pos_X_Base / 2.0f + Mathf.Pow(Pos_Y_Base, 0.5f));
            }
            else {
                Temp_Angle = Mathf.Rad2Deg * Mathf.Atan(-Pos_X_Base / 2.0f - Mathf.Pow(Pos_Y_Base, 0.5f));
            }
        }
        else {
            Temp_Angle = 45.0f;
        }
        Vector3 Temp_Pos = This_Transform.parent.forward;
        return Temp_Angle - Mathf.Rad2Deg * Mathf.Atan(Temp_Pos.y / Vector2.Distance(Vector2.zero, new Vector2(Temp_Pos.x, Temp_Pos.z)));
    }

    float Manual_Angle()
    { // Simply turn toward the target.
        float Temp_Angle;
        Vector3 Target_Pos = Turret_Horizontal_Script.Target_Pos;
        Vector3 Temp_Pos = This_Transform.parent.InverseTransformPoint(Target_Pos);
        Temp_Angle = Mathf.Rad2Deg * (Mathf.Asin((Temp_Pos.y - This_Transform.localPosition.y) / Vector3.Distance(This_Transform.localPosition, Temp_Pos)));
        return Temp_Angle;
    }

    void Rotate()
    {
        Current_Angle += Speed_Mag * Temp_Vertical * Time.deltaTime;
        Current_Angle = Mathf.Clamp(Current_Angle, Max_Elevation, Max_Depression);
        This_Transform.localRotation = Quaternion.Euler(new Vector3(Current_Angle, 0.0f, 0.0f));
    }

    void Rotate_In_FixedUpdate()
    {
        Current_Angle += Speed_Mag * Temp_Vertical * Time.fixedDeltaTime;
        Current_Angle = Mathf.Clamp(Current_Angle, Max_Elevation, Max_Depression);
        This_Transform.localRotation = Quaternion.Euler(new Vector3(Current_Angle, 0.0f, 0.0f));
    }

    void AI_Input()
    {
        // Make sure that the cannon can shoot the target.
        if (AI_Script.Direct_Fire)
        { // Direct Fire.
            if (AI_Script.Detect_Flag)
            { // Target is detected.
                RayCast_Count += Time.fixedDeltaTime;
                if (RayCast_Count > 1.0f)
                {
                    Fire_Flag = Cast_Ray(); // Cast Ray and set the Fire_Flag.
                    RayCast_Count = 0.0f;
                }
            }
            else { // Target is not detected.
                Fire_Flag = false;
                RayCast_Count = 0.0f;
            }
        }
        else { // InDirect Fire.
            if (AI_Script.Detect_Flag)
            { // Target is detected.
                Fire_Flag = true;
            }
            else {
                Fire_Flag = false; // Target is not detected.
            }
        }
        // When 'AI_Reference' is true, "AI" refers to this Cannon as Main-Cannon to decide its action.
        if (AI_Reference)
        {
            AI_Script.Fire_Flag = Fire_Flag;
        }
        // Calculate the angle and rate, and rotate the cannon.
        Auto_Turn();
        // Fire process.
        Fire_Process();
    }

    bool Cast_Ray()
    {
        // Cast Ray from "Bullet_Generator" to the target.
        Ray Temp_Ray = new Ray(Bullet_Generator_Transform.position, Turret_Horizontal_Script.Target_Pos - Bullet_Generator_Transform.position);
        RaycastHit Temp_RaycastHit;
        if (Physics.Raycast(Temp_Ray, out Temp_RaycastHit, AI_Script.Visibility_Radius, Ray_LayerMask))
        {
            if (Temp_RaycastHit.transform.root == AI_Script.Target_Root_Transform)
            { // Cannon can directly aim the target.
                return true;
            }
            else { // Ray hits something else.
                Random_Offset();
                return false;
            }
        }
        else { // Ray does not hit anyhing.
            Random_Offset();
            return false;
        }
    }

    void Fire_Process()
    {
        if (AI_Script.Target_Distance < AI_Script.OpenFire_Distance && Turret_Horizontal_Script.Fire_Flag && Fire_Flag)
        { // Turret is ready, and Ray hits the target.
            if (Mathf.Abs(Target_Angle) < AI_Script.Fire_Angle)
            { // Cannon is within the angle range.
                Cannon_Count += Time.fixedDeltaTime;
                if (Cannon_Count > AI_Script.Fire_Count && Cannon_Fire_Script.Reload_Flag)
                { // Cannon_Count is over, and Reload is finished.
                    //This_Transform.SendMessage("Fire", SendMessageOptions.DontRequireReceiver); // Send message to "Cannon_Fire".
                    //call reliable fire
                    ai_controller.OwnerFire();
                    Cannon_Count = 0.0f;
                    Wait_Count = 0.0f;
                    Random_Offset();
                }
            }
            else { // Cannon is not within the angle range.
                if (AI_Script.Direct_Fire)
                {
                    if (AI_Script.Near_Flag)
                    { // The target is within Approach_Distance.
                        Wait_Count += Time.fixedDeltaTime;
                        if (Wait_Count > 5.0f)
                        { // Is the target out of the angle range?
                            Cannon_Count = 0.0f;
                            Wait_Count = 0.0f;
                            Random_Offset();
                        }
                    }
                    else { // The target is out of Approach_Distance.
                        Cannon_Count = 0.0f;
                        Wait_Count = 0.0f;
                    }
                }
                else {
                    Cannon_Count = 0.0f;
                }
            }
        }
        else { // Turret is not ready, or Ray does not hit the target.
            Cannon_Count = 0.0f;
            Wait_Count = 0.0f;
        }
    }

    void Random_Offset()
    { // Give the random value to the "Target_Offset". ( Do not confuse "Target_Offset" and "Offset_Angle".)
        Vector3 Temp_Offset;
        Temp_Offset.x = Random.Range(-1.0f, 1.0f);
        Temp_Offset.y = Random.Range(-AI_Script.AI_Lower_Offset, AI_Script.AI_Upper_Offset);
        Temp_Offset.z = Random.Range(-1.0f, 1.0f);
        Turret_Horizontal_Script.Target_Offset = Temp_Offset;
    }

    void Turret_Linkage()
    {
        This_Transform.localEulerAngles = new Vector3(Max_Depression, 0.0f, 0.0f);
        if (Input_Type == 10 && AI_Reference)
        {
            AI_Script.Fire_Flag = false; // Lost Control. AI can not stop near the target. (in the case of Multiple Turrets)
        }
        Destroy(this);
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
        else {
            Flag = false;
        }
    }

    void Get_Bullet_Generator(GameObject Temp_Object)
    { // Called from "Bullet_Generator".
        Bullet_Generator_Script = Temp_Object.GetComponent<Bullet_Generator_CS>();
        Bullet_Generator_Transform = Temp_Object.transform;
    }

    void Get_AI(AI_CS Temp_Script)
    {
        AI_Script = Temp_Script;
    }

}