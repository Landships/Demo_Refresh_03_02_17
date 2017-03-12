using UnityEngine;

public class CameraControl : MonoBehaviour
{

    UnityStandardAssets.ImageEffects.Grayscale grayscaleScript;
    public GameObject Mainbody;
    public GameObject Turret;
    public GameObject Hatch;
    private float defaultFarPlane;
    public float grayscaleFarPlane = 50;
    public float openHatchDegree = 90;


    // Use this for initialization
    void Start()
    {



        grayscaleScript = GetComponent<UnityStandardAssets.ImageEffects.Grayscale>();
        defaultFarPlane = GetComponent<Camera>().farClipPlane;


    }

    // Update is called once per frame
    void Update()
    {



        grayscaleScript.enabled = true;
        GetComponent<Camera>().farClipPlane = grayscaleFarPlane;

        if (Mainbody == null || Turret == null || Hatch == null)
        {
            Mainbody = GameObject.Find("MainBody");
            Turret = GameObject.FindGameObjectWithTag("PlayerTurret");
            //Turret = GameObject.Find("Turret");
            Hatch = GameObject.Find("shermanhatchcharles");

        }

        if (Mainbody == null || Turret == null || Hatch == null)
        {
            return;
        }

        MeshCollider[] Temp_MeshColliders = Mainbody.GetComponents<MeshCollider>();

        bool intersecting = false;

        foreach (MeshCollider Temp_MeshCollider in Temp_MeshColliders)
        {

            if (GetComponent<Collider>().bounds.Intersects(Temp_MeshCollider.bounds))
            {
                intersecting = true;
            }

        }

        if (GetComponent<Collider>().bounds.Intersects(Turret.GetComponent<MeshCollider>().bounds))
        {
            intersecting = true;
        }




        if (Hatch.transform.rotation.eulerAngles.z >= openHatchDegree
            && GetComponent<Collider>().bounds.Intersects(Mainbody.GetComponent<BoxCollider>().bounds))
        {

            intersecting = true;
        }

        if (intersecting)
        {
            grayscaleScript.enabled = false;
            GetComponent<Camera>().farClipPlane = defaultFarPlane;

        }
        else {
            grayscaleScript.enabled = true;
            GetComponent<Camera>().farClipPlane = grayscaleFarPlane;
        }

    }


}

