using UnityEngine;
using System.Collections;

[ ExecuteInEditMode ]

public class Cannon_Base_CS : MonoBehaviour {
	
	public Mesh Part_Mesh ;
	public Mesh Collider_Mesh ;
	public Mesh Sub_Collider_Mesh ;

	public int Materials_Num = 1 ;
	public Material[] Materials ;
	public Material Part_Material ;

	public float Offset_X = 0.0f ;
	public float Offset_Y = 0.0f ;
	public float Offset_Z = 0.0f ;

	public float Durability = 150000.0f ;
	public float Sub_Durability = 100000.0f ;
	public float Trouble_Time = 20.0f ; 
	public GameObject Trouble_Effect_Object ;

	public Transform Parent_Transform ;
	
	void Start () {
		Parent_Transform = this.transform ;
		if ( Application.isPlaying ) {
			Destroy ( this ) ;
		}
	}
	
	void Reset () {
		Start () ;
	}
}