using UnityEngine;
using System.Collections;

public class Artillery_Fire_CS : MonoBehaviour
{

    public float Interval_Min;
    public float Interval_Max;
    public float Radius;
    public float Height;
    public float Mass;
    public float Delete_Time;
    public float Explosion_Force;
    public float Explosion_Radius;
    public GameObject Explosion_Object;

    bool Flag = false;
    int Shell_Count;
    float Time_Count;
    Vector3 Target_Pos;
    int Number;

    void Update()
    {
        if (Flag)
        {
            Time_Count += Time.deltaTime;
            float Interval = Random.Range(Interval_Min, Interval_Max);
            if (Time_Count > Interval)
            {
                for (int i = 0; i < Mathf.FloorToInt(Time_Count / Interval); i++)
                {
                    GameObject Shell_Object = new GameObject("Artillery_Shell");
                    // Set position.
                    float Temp_X = Random.Range(0.0f, Radius) * Mathf.Cos(Random.Range(0.0f, 2.0f * Mathf.PI));
                    float Temp_Z = Random.Range(0.0f, Radius) * Mathf.Sin(Random.Range(0.0f, 2.0f * Mathf.PI));
                    Shell_Object.transform.position = new Vector3(Target_Pos.x + Temp_X, Target_Pos.y + Height, Target_Pos.z + Temp_Z);
                    // Add component.
                    Rigidbody Temp_Rigidbody = Shell_Object.AddComponent<Rigidbody>();
                    Temp_Rigidbody.mass = Mass;
                    Shell_Object.AddComponent<SphereCollider>();
                    // Add Scripts
                    Bullet_Control_CS Temp_Bullet_Script;
                    Temp_Bullet_Script = Shell_Object.AddComponent<Bullet_Control_CS>();
                    Temp_Bullet_Script.Set_Type(1); // HE
                    Temp_Bullet_Script.Set_HE_Value(Delete_Time, Explosion_Force, Explosion_Radius, Explosion_Object);
                    // Count the shells.
                    Shell_Count += 1;
                    if (Shell_Count >= Number)
                    {
                        Flag = false;
                        Shell_Count = 0;
                        Time_Count = 0.0f;
                        break;
                    }
                }
                Time_Count = 0.0f;
            }
        }
    }

    public void Fire(Transform Temp_Target, int Temp_Num)
    {
        if (Flag == false)
        {
            Flag = true;
            Number = Temp_Num;
            // Set target's position.
            MainBody_Setting_CS Temp_Script = Temp_Target.GetComponentInChildren<MainBody_Setting_CS>();
            if (Temp_Script)
            {
                Target_Pos = Temp_Script.transform.position;
            }
            else
            {
                Target_Pos = Temp_Target.position;
            }
        }
    }
}
