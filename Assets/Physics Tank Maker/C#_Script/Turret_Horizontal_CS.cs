using UnityEngine;
using System.Collections;

public class Turret_Horizontal_CS : MonoBehaviour
{

    public bool Limit_Flag;
    public float Max_Right = 170.0f;
    public float Max_Left = 170.0f;
    public float Speed_Mag = 10.0f;
    public float Buffer_Angle = 5.0f;
    public float Acceleration_Time = 0.5f;
    public float Deceleration_Time = 0.1f;
    public float OpenFire_Angle = 180.0f;
    public GameObject Marker_Prefab;

    public bool Fire_Flag = false; // Referred to from "Cannon_Vertical".
    public Vector3 Target_Pos; // Referred to from "Cannon_Vertical".
    public bool Free_Aim_Flag = false; // Referred to from "Cannon_Vertical" and "Marker_Control".
    public Vector3 Target_Offset; // Changed by also "Cannon_Vertical".
    public bool Marker_Flag; // Referred to from "Marker_Control".
    public bool OpenFire_Flag = true; // Referred to from "Cannon_Fire". ( Only in case of Mouse operation. )

    float Current_Angle;
    float Target_Angle;
    public float Temp_Horizontal;
    public float Current_Horizontal; // Referred to from "Sound_Control_CS".
    bool Tracking_Flag = false;
    Transform Target_Transform;
    bool Moving_Flag = false;

    GameObject Marker_Object;
    GameObject Sub_Marker_Object;

    float Offset_Angle;
    Vector3 Last_Mouse_Pos;
    Camera Gun_Camera;
    bool Trouble_Flag = false;

    Transform This_Transform;
    Transform Parent_Transform;
    Transform Root_Transform;

    int Ray_LayerMask = ~((1 << 10) + (1 << 2)); // Layer 2 = Ignore Ray, Layer 10 = Ignore All.

    bool Flag = true;
    int Tank_ID;
    int Input_Type = 4;

    AI_CS AI_Script;

    void Start()
    { // Turret's objects are sorted at the opening.
    }

    void Complete_Turret()
    { // Called from 'Turret_Finishing" when the sorting is finished.
        This_Transform = transform;
        Parent_Transform = This_Transform.parent;
        Root_Transform = This_Transform.root;
        Current_Angle = This_Transform.localEulerAngles.y;
        Max_Right = Current_Angle + Max_Right;
        Max_Left = Current_Angle - Max_Left;
        // Instantiate Marker objects.
        if (Input_Type == 4 || Input_Type == 5 || Input_Type == 10)
        {
            if (Marker_Prefab)
            {
                Marker_Object = Create_Marker(true);
                Sub_Marker_Object = Create_Marker(false);
            }
        }
    }

    GameObject Create_Marker(bool Temp_Flag)
    {
        GameObject Temp_Object = Instantiate(Marker_Prefab, Vector3.zero, Quaternion.identity) as GameObject;
        Temp_Object.transform.parent = GetComponentInParent<Tank_ID_Control_CS>().transform;
        Marker_Flag = false;
        Marker_Control_CS Temp_Script = Temp_Object.GetComponent<Marker_Control_CS>();
        if (Temp_Script)
        {
            Temp_Script.Turret_Horizontal_Script = this;
            Temp_Script.Main_Flag = Temp_Flag;
        }
        return Temp_Object;
    }

    void Update()
    {
        if (Flag)
        {
            switch (Input_Type)
            {
                case 0:
                    //KeyBoard_Input();
                    break;
                case 1:
                    //Stick_Input();
                    break;
                case 2:
                    //Trigger_Input();
                    break;
                case 3:
                    //Stick_Input();
                    break;
                case 8:
                    Crank_Input();
                    break;
            }
        }
    }

    void LateUpdate()
    {
        if ((Input_Type == 4 || Input_Type == 5) && Flag)
        {
            //Mouse_Input();
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
            }
        }
    }

    void Crank_Input()
    {
        Rotate();
    }

    void Stick_Input()
    {
        if (Input.GetButton("L_Button"))
        {
            Temp_Horizontal = Input.GetAxis("Horizontal2");
        }
        else
        {
            Temp_Horizontal = 0.0f;
        }
        Rotate();
    }

    void Trigger_Input()
    {
        if (Input.GetButton("Fire1") == false && Input.GetButton("Jump") == false && Input.GetAxis("Horizontal") != 0)
        {
            Temp_Horizontal = Input.GetAxis("Horizontal");
        }
        else
        {
            Temp_Horizontal = 0.0f;
        }
        Rotate();
    }

    void KeyBoard_Input()
    {
        if ((Input.GetButton("Fire1") || Input.GetKey("z")))
        {
            Temp_Horizontal = Input.GetAxis("Horizontal");
        }
        else
        {
            Temp_Horizontal = 0.0f;
        }
        Rotate();
    }

    void Mouse_Input()
    {
        if (Input.GetMouseButtonDown(2))
        {
            Cast_Ray_Lock();
        }
        else if (Free_Aim_Flag && !Input.GetKey(KeyCode.Space) && !Input.GetKey("f"))
        {
            StartCoroutine("Cast_Ray_Free");
        }
        // Adjust aiming.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Last_Mouse_Pos = Input.mousePosition;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            Offset_Angle += (Input.mousePosition.x - Last_Mouse_Pos.x) * 0.02f;
            Last_Mouse_Pos = Input.mousePosition;
        }
        else
        {
            Offset_Angle = 0.0f;
        }
        // Switch the aiming mode.
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (Free_Aim_Flag)
            {
                Free_Aim_Flag = false; // Return to initial position. 
                Reset_Lock_On();
            }
            else
            {
                Free_Aim_Flag = true; // Free Aiming.
                Tracking_Flag = true;
                Moving_Flag = true;
                Target_Transform = null;
                Marker_Flag = true;
                // Send message to "Cannon_Vertical".
                BroadcastMessage("Start_Tracking", SendMessageOptions.DontRequireReceiver);
            }
        }
    }

    void Cast_Ray_Lock()
    {
        // Detect the camera clicked.
        Vector3 Mouse_Pos = Input.mousePosition;
        Camera Current_Camera;
        if (Gun_Camera && Gun_Camera.enabled && new Rect(Screen.width * Gun_Camera.rect.x, Screen.height * Gun_Camera.rect.y, Screen.width * Gun_Camera.rect.width, Screen.height * Gun_Camera.rect.height).Contains(Mouse_Pos))
        {
            Current_Camera = Gun_Camera;
        }
        else
        {
            Current_Camera = Camera.main;
        }
        // Cast Ray, and Set Target position.
        Ray Temp_Ray = Current_Camera.ScreenPointToRay(Mouse_Pos);
        RaycastHit Temp_RaycastHit;
        if (Physics.Raycast(Temp_Ray, out Temp_RaycastHit, 1000.0f, Ray_LayerMask))
        { // Ray hits something.
            if (Temp_RaycastHit.transform.root != Root_Transform)
            {
                Target_Pos = Temp_RaycastHit.point;
                if (Free_Aim_Flag == true)
                {
                    Free_Aim_Flag = false;
                }
                Set_Lock_On(Temp_RaycastHit.collider.transform);
                Target_Offset = Target_Transform.InverseTransformPoint(Temp_RaycastHit.point);
                if (Target_Transform.localScale != Vector3.one)
                { // for Armor_Collider.
                    Target_Offset.x *= Target_Transform.localScale.x;
                    Target_Offset.y *= Target_Transform.localScale.y;
                    Target_Offset.z *= Target_Transform.localScale.z;
                }
            }
        }
    }

    IEnumerator Cast_Ray_Free()
    {
        yield return null;
        // Detect the camera clicked.
        Vector3 Mouse_Pos = Input.mousePosition;
        Camera Current_Camera;
        if (Gun_Camera && Gun_Camera.enabled && new Rect(Screen.width * Gun_Camera.rect.x, Screen.height * Gun_Camera.rect.y, Screen.width * Gun_Camera.rect.width, Screen.height * Gun_Camera.rect.height).Contains(Mouse_Pos))
        {
            Current_Camera = Gun_Camera;
        }
        else
        {
            Current_Camera = Camera.main;
        }
        // Cast Ray, and Set Target position.
        Ray Temp_Ray = Current_Camera.ScreenPointToRay(Mouse_Pos);
        RaycastHit Temp_RaycastHit;
        if (Physics.Raycast(Temp_Ray, out Temp_RaycastHit, 1000.0f, Ray_LayerMask))
        { // Ray hits something.
            if (Temp_RaycastHit.transform.root != Root_Transform)
            {
                Target_Pos = Temp_RaycastHit.point;
            }
            else
            {
                Mouse_Pos.z = 1000.0f;
                Target_Pos = Current_Camera.ScreenToWorldPoint(Mouse_Pos);
            }
        }
        else
        { // Ray does not hit anythig.
            Mouse_Pos.z = 1000.0f;
            Target_Pos = Current_Camera.ScreenToWorldPoint(Mouse_Pos);
        }
    }

    void Set_Lock_On(Transform Temp_Transform)
    { // This function is also called from AI.
        Target_Transform = Temp_Transform;
        Marker_Flag = Flag;
        // Send message to "Cannon_Vertical".
        BroadcastMessage("Start_Tracking", SendMessageOptions.DontRequireReceiver);
        Tracking_Flag = true;
        Moving_Flag = true;
    }

    void Reset_Lock_On()
    { // This function is also called from AI.
        Target_Transform = null;
        Marker_Flag = false;
        Offset_Angle = 0.0f;
        Tracking_Flag = false;
        // Send message to "Cannon_Vertical".
        BroadcastMessage("Stop_Tracking", SendMessageOptions.DontRequireReceiver);
    }

    void Auto_Turn()
    {
        // Update Target position.
        if (Target_Transform)
        {
            Target_Pos = Target_Transform.position + (Target_Transform.forward * Target_Offset.z) + (Target_Transform.right * Target_Offset.x) + (Target_Transform.up * Target_Offset.y);
        }
        // Calculate Angle.
        if (Tracking_Flag)
        {
            Vector3 Temp_Pos;
            if (Limit_Flag)
            { // Limited rotation.
                Temp_Pos = Parent_Transform.InverseTransformPoint(Target_Pos);
                Target_Angle = Vector2.Angle(Vector2.up, new Vector2(Temp_Pos.x, Temp_Pos.z));
                if (Temp_Pos.x < 0.0f)
                {
                    Target_Angle = -Target_Angle;
                }
                Target_Angle -= Current_Angle;
            }
            else
            { // No limited.
                Temp_Pos = This_Transform.InverseTransformPoint(Target_Pos);
                Target_Angle = Vector2.Angle(Vector2.up, new Vector2(Temp_Pos.x, Temp_Pos.z));
                if (Temp_Pos.x < 0.0f)
                {
                    Target_Angle = -Target_Angle;
                }
            }
            Target_Angle += Offset_Angle;
        }
        else
        { // Return to initial position.
            Target_Angle = Mathf.DeltaAngle(This_Transform.localEulerAngles.y, 0.0f);
            if (Mathf.Abs(Target_Angle) < 0.01f)
            {
                Moving_Flag = false;
            }
        }
        // Calculate Turn Rate.
        if (Target_Angle > 0.01f)
        {
            Temp_Horizontal = Mathf.Lerp(0.0f, 1.0f, Target_Angle / (Speed_Mag * Time.fixedDeltaTime + Buffer_Angle));
            if (Target_Angle > Buffer_Angle)
            {
                Current_Horizontal = Mathf.MoveTowards(Current_Horizontal, Temp_Horizontal, Time.fixedDeltaTime / Acceleration_Time);
            }
            else
            {
                Current_Horizontal = Mathf.MoveTowards(Current_Horizontal, Temp_Horizontal, Time.fixedDeltaTime / Deceleration_Time);
            }
            //if ( !float.IsNaN ( Temp_Horizontal ) ) {
            Rotate_In_FixedUpdate();
            //}
        }
        else if (Target_Angle < -0.01f)
        {
            Temp_Horizontal = -Mathf.Lerp(0.0f, 1.0f, -Target_Angle / (Speed_Mag * Time.fixedDeltaTime + Buffer_Angle));
            if (Target_Angle < -Buffer_Angle)
            {
                Current_Horizontal = Mathf.MoveTowards(Current_Horizontal, Temp_Horizontal, Time.fixedDeltaTime / Acceleration_Time);
            }
            else
            {
                Current_Horizontal = Mathf.MoveTowards(Current_Horizontal, Temp_Horizontal, Time.fixedDeltaTime / Deceleration_Time);
            }
            //if ( !float.IsNaN ( Temp_Horizontal ) ) {
            Rotate_In_FixedUpdate();
            //}
        }
        else
        {
            Temp_Horizontal = 0.0f;
        }
        // Set OpenFire_Flag.
        if (Mathf.Abs(Target_Angle) <= OpenFire_Angle)
        {
            OpenFire_Flag = true;
        }
        else
        {
            OpenFire_Flag = false;
        }
    }

    void Rotate()
    {
        if (!Trouble_Flag)
        {
            if (Temp_Horizontal != 0.0f)
            {
                Current_Horizontal = Mathf.MoveTowards(Current_Horizontal, Temp_Horizontal, Time.deltaTime / Acceleration_Time);
            }
            else
            {
                Current_Horizontal = Mathf.MoveTowards(Current_Horizontal, Temp_Horizontal, Time.deltaTime / Deceleration_Time);
            }
            Current_Angle += Speed_Mag * Current_Horizontal * Time.deltaTime;
            if (Limit_Flag)
            {
                Current_Angle = Mathf.Clamp(Current_Angle, Max_Left, Max_Right);
            }
            This_Transform.localRotation = Quaternion.Euler(new Vector3(0.0f, Current_Angle, 0.0f));
        }
    }

    void Rotate_In_FixedUpdate()
    {
        if (!Trouble_Flag)
        {
            Current_Angle += Speed_Mag * Current_Horizontal * Time.fixedDeltaTime;
            if (Limit_Flag)
            {
                Current_Angle = Mathf.Clamp(Current_Angle, Max_Left, Max_Right);
                if (Current_Angle <= Max_Left || Current_Angle >= Max_Right)
                {
                    Current_Horizontal = 0.0f;
                }
            }
            This_Transform.localRotation = Quaternion.Euler(new Vector3(0.0f, Current_Angle, 0.0f));
        }
    }

    void AI_Input()
    {
        Auto_Turn();
        if (Mathf.Abs(Target_Angle) < AI_Script.Fire_Angle)
        {
            Fire_Flag = true; // Referred to from "Cannon_Vertical".
        }
        else
        {
            Fire_Flag = false; // Referred to from "Cannon_Vertical".
        }
    }

    void Get_Gun_Camera(Camera Temp_Camera)
    {
        Gun_Camera = Temp_Camera;
    }

    void Turret_Linkage()
    {
        if (Marker_Object)
        {
            Destroy(Marker_Object);
        }
        if (Sub_Marker_Object)
        {
            Destroy(Sub_Marker_Object);
        }
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
            if (Tracking_Flag)
            {
                Marker_Flag = true;
            }
        }
        else
        {
            Flag = false;
            Marker_Flag = false;
        }
    }

    void Get_AI(AI_CS Temp_Script)
    {
        AI_Script = Temp_Script;
    }

}