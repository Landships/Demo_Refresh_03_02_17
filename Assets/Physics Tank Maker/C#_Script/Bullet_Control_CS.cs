using UnityEngine;
using System.Collections;

public class Bullet_Control_CS : MonoBehaviour
{

    public int Type; // 0=AP , 1=HE
    public float Delete_Time;
    public float Explosion_Force;
    public float Explosion_Radius;
    public GameObject Impact_Object;
    public GameObject Ricochet_Object;
    public GameObject Explosion_Object;
    public float Attack_Multiplier = 1.0f;
    public bool Debug_Flag;

    Transform This_Transform;
    Rigidbody This_Rigidbody;

    bool Live_Flag = true;
    int RayCast_Switch = 0;
    Vector3 Next_Position;
    GameObject Hit_Object;
    Vector3 Hit_Normal;

    void Awake()
    {  // (Note.) Sometimes OnCollisionEnter() is called earlier than Start().
        This_Transform = transform;
        This_Rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Destroy(this.gameObject, Delete_Time);
    }

    void FixedUpdate()
    {
        if (Live_Flag)
        {
            This_Transform.LookAt(This_Transform.position + This_Rigidbody.velocity);
            switch (RayCast_Switch)
            {
                case 0:
                    Ray Temp_Ray = new Ray(This_Transform.position, This_Rigidbody.velocity);
                    int layerMask = ~((1 << 10) + (1 << 2));
                    RaycastHit Temp_RaycastHit;
                    Physics.Raycast(Temp_Ray, out Temp_RaycastHit, This_Rigidbody.velocity.magnitude * Time.fixedDeltaTime, layerMask);
                    if (Temp_RaycastHit.collider)
                    {
                        Next_Position = Temp_RaycastHit.point;
                        Hit_Object = Temp_RaycastHit.collider.gameObject;
                        Hit_Normal = Temp_RaycastHit.normal;
                        RayCast_Switch = 1;
                    }
                    break;
                case 1:
                    This_Transform.position = Next_Position;
                    This_Rigidbody.position = Next_Position;
                    Hit(Hit_Object, Hit_Normal);
                    break;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (Live_Flag)
        {
            Debug.Log("on collision enter");
            Hit(collision.gameObject, collision.contacts[0].normal);
        }
    }

    void Hit(GameObject Temp_Object, Vector3 Temp_Normal)
    {
        Live_Flag = false;
        if (Type == 1)
        { // HE
            Destroy(GetComponent<Renderer>());
            Destroy(GetComponent<Rigidbody>());
            Destroy(GetComponent<Collider>());
            // Create HE Explosion Particle.
            if (Explosion_Object)
            {
                Instantiate(Explosion_Object, This_Transform.position, Quaternion.identity);
            }
        }


        // Send Message
        if (Temp_Object)
        {
            Damage_Control_CS Temp_Script = Temp_Object.GetComponent<Damage_Control_CS>();
            if (Temp_Script)
            { // Hit object has "Damage_Control" script.
                if (Type == 0)
                { // AP
                  // Create AP Ricochet Particle.
                    if (Ricochet_Object)
                    {
                        Instantiate(Ricochet_Object, This_Transform.position, Quaternion.identity);
                    }
                    // Calculate Hit_Energy.
                    float Hit_Angle = Mathf.Abs(90.0f - Vector3.Angle(This_Rigidbody.velocity, Temp_Normal));
                    float Hit_Energy = 500f * This_Rigidbody.mass * Mathf.Pow(This_Rigidbody.velocity.magnitude, 2);
                    Hit_Energy *= Mathf.Lerp(0.0f, 1.0f, Mathf.Sqrt(Hit_Angle / 90.0f));
                    Hit_Energy *= Attack_Multiplier;
                    // Output for debug.
                    if (Debug_Flag)
                    {
                        Debug.Log("AP Damage " + Hit_Energy + " on " + Temp_Object.name);
                    }
                    if (Temp_Script.Breaker(Hit_Energy))
                    {
                        Destroy(this.gameObject);
                    }
                }
                else
                { // HE
                    Explosion_Force *= Attack_Multiplier;
                    // Output for debug.
                    if (Debug_Flag)
                    {
                        Debug.Log("HE Damage " + Explosion_Force + " on " + Temp_Object.name);
                    }
                    // Send 'Explosion_Force' to "Damage_Control" script.
                    Temp_Script.Breaker(Explosion_Force);
                }
            }
            else
            { // Hit object does not have "Damage_Control" script.
                if (Type == 0)
                { // AP
                  // Create AP Impact Particle.
                    if (Impact_Object)
                    {
                        Instantiate(Impact_Object, This_Transform.position, Quaternion.identity);
                    }
                }
            }
        }
        // Explosion process.
        if (Type == 1)
        { // HE
          // Search objects in the Explosion_Radius.
            Collider[] Temp_Colliders = Physics.OverlapSphere(This_Transform.position, Explosion_Radius);
            foreach (Collider Target_Collider in Temp_Colliders)
            {
                //StartCoroutine ( "Add_Explosion_Force" , Target_Collider ) ;
                Add_Explosion_Force(Target_Collider);
            }
            Destroy(this.gameObject, 0.01f * Explosion_Radius);
        }
    }

    void Add_Explosion_Force(Collider Temp_Target_Collider)
    {
        //yield return new WaitForSeconds ( 0.01f * Vector3.Distance ( This_Transform.position , Temp_Target_Collider.transform.position ) ) ;
        if (Temp_Target_Collider)
        {
            Vector3 Ray_Direction = (Temp_Target_Collider.transform.position - This_Transform.position).normalized;
            Ray Temp_Ray = new Ray(This_Transform.position, Ray_Direction);
            int layerMask = ~((1 << 10) + (1 << 2));
            RaycastHit Temp_RaycastHit;
            if (Physics.Raycast(Temp_Ray, out Temp_RaycastHit, Explosion_Radius, layerMask))
            {
                if (Temp_RaycastHit.collider == Temp_Target_Collider)
                {
                    float Distance_Loss = Mathf.Pow((Explosion_Radius - Temp_RaycastHit.distance) / Explosion_Radius, 2);
                    // Add force.
                    Rigidbody Temp_Rigidbody = Temp_Target_Collider.GetComponent<Rigidbody>();
                    if (Temp_Rigidbody)
                    {
                        Temp_Rigidbody.AddForce(Ray_Direction * Explosion_Force * Distance_Loss);
                    }
                    // Add damage.
                    Damage_Control_CS Temp_Script = Temp_Target_Collider.GetComponent<Damage_Control_CS>();
                    if (Temp_Script)
                    {
                        Temp_Script.Breaker(Explosion_Force * Distance_Loss);
                    }
                }
            }
        }
    }

    public void Set_Type(int Temp_Type)
    {
        Type = Temp_Type;
    }

    public void Set_AP_Value(float Temp_Time, GameObject Temp_Impact_Object, GameObject Temp_Ricochet_Object)
    {
        Delete_Time = Temp_Time;
        Impact_Object = Temp_Impact_Object;
        Ricochet_Object = Temp_Ricochet_Object;
    }

    public void Set_HE_Value(float Temp_Time, float Temp_Explosion_Force, float Temp_Explosion_Radius, GameObject Temp_Explosion_Object)
    {
        Delete_Time = Temp_Time;
        Explosion_Force = Temp_Explosion_Force;
        Explosion_Radius = Temp_Explosion_Radius;
        Explosion_Object = Temp_Explosion_Object;
    }

    public void Set_Debug_Mode(bool Temp_Flag)
    {
        Debug_Flag = Temp_Flag;
    }
}