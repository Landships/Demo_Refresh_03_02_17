using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SetupScript : NetworkBehaviour {

	public Mesh Bullet_Mesh;
	//[SyncVar]
	public Material Bullet_Material;
	//[SyncVar]
	public float Bullet_Mass = 5.0f;
	//[SyncVar]
	public float Bullet_Drag = 0.05f;
	//[SyncVar]
	public PhysicMaterial Bullet_PhysicMat;
	//[SyncVar]
	public Vector3 Bullet_Scale = new Vector3(0.762f, 0.762f, 0.762f);
	//[SyncVar]
	public float Bullet_Force = 250.0f;
	//[SyncVar]
	public Vector3 BoxCollider_Scale = new Vector3(1.0f, 1.0f, 1.0f);
	private Rigidbody Temp_Rigidbody;
	public float Delete_Time = 5.0f;
	//[SyncVar]

	//[SyncVar]
	public GameObject Impact_Object;
	//[SyncVar]
	public GameObject Ricochet_Object;

	private bool setup = false;



	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (!setup) 
		{
			BulletSetup ();
			setup = true;
		}
	}


	public void BulletSetup() {
        /*
		Bullet_Generator_CS bullet_template = GameObject.FindGameObjectWithTag("playerbulletgenerator").GetComponent<Bullet_Generator_CS>();

		Bullet_Mesh = bullet_template.Bullet_Mesh;
		Bullet_Material = bullet_template.Bullet_Material;
		Bullet_Mass = bullet_template.Bullet_Mass;
		Bullet_Drag = bullet_template.Bullet_Drag;
		Bullet_PhysicMat = bullet_template.Bullet_PhysicMat;
		Bullet_Scale = bullet_template.Bullet_Scale;
		Bullet_Force = bullet_template.Bullet_Force;
		BoxCollider_Scale = bullet_template.BoxCollider_Scale;
		Delete_Time = bullet_template.Delete_Time;
		Impact_Object = bullet_template.Impact_Object;
		Ricochet_Object = bullet_template.Ricochet_Object;
        */
        /*
		MeshRenderer Temp_MeshRenderer = gameObject.AddComponent<MeshRenderer>();
		Temp_MeshRenderer.material = Bullet_Material;
		MeshFilter Temp_MeshFilter;
		Temp_MeshFilter = gameObject.AddComponent<MeshFilter>();
		Temp_MeshFilter.mesh = Bullet_Mesh;
		Temp_Rigidbody = gameObject.AddComponent<Rigidbody>();
		Temp_Rigidbody.mass = Bullet_Mass;
		Temp_Rigidbody.drag = Bullet_Drag;
		BoxCollider Temp_BoxCollider;
		Temp_BoxCollider = gameObject.AddComponent<BoxCollider>();
		Temp_BoxCollider.size = Vector3.Scale(Temp_MeshFilter.mesh.bounds.size, BoxCollider_Scale);
		Temp_BoxCollider.center = new Vector3(0.0f, 0.0f, (Temp_BoxCollider.size.z - Temp_MeshFilter.mesh.bounds.size.z) / 2.0f);
		Temp_BoxCollider.material = Bullet_PhysicMat;
        

		Bullet_Control_CS Temp_Script;
		Temp_Script = gameObject.AddComponent<Bullet_Control_CS>();
		Temp_Script.Set_Type(0);
		Temp_Script.Set_AP_Value(Delete_Time, Impact_Object, Ricochet_Object);
		Temp_Script.Attack_Multiplier = 1.0f;
        */

        //if (isServer)
		//Temp_Rigidbody.velocity = gameObject.transform.forward * Bullet_Force;
			
		//Temp_Script.Set_Debug_Mode(Debug_Flag);
//		if (Trail_Flag)
//		{
//			TrailRenderer Temp_TrailRenderer;
//			Temp_TrailRenderer = gameObject.AddComponent<TrailRenderer>();
//			Temp_TrailRenderer.startWidth = Trail_Start_Width;
//			Temp_TrailRenderer.endWidth = Trail_End_Width;
//			Temp_TrailRenderer.time = Trail_Time;
//			Temp_TrailRenderer.material = Trail_Material;
//		}
	}
}
