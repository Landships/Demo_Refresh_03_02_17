using UnityEngine;
using System.Collections;

public class turretTrigger : MonoBehaviour {

		private GameObject HandObject;
		private Position Hand;

	public bool no_show = true;
	public  Camera scope_camera;
	public	Camera main_cam; 

		public enum detectionplane { xy, xz, yz };
		public enum rotationaxis { x, y, z };
		public enum contact { required, not_required };

		public float uppercap_degrees;
		public float lowercap_degrees;
		public detectionplane d_plane;
		public rotationaxis r_axis;
		public contact contact_required;

		private Renderer rend;

		public GameObject center;
		public Vector3 prevpos;
		public Vector3 currentpos;
		private bool firsttime;

		private float d_angle;
		public float angle;

		public bool leavingcollider = false;
		public bool intersecting = false;
		public bool interacting = false;
		public bool holdintersecting = false;



		public void Start()
		{
			d_angle = 0;
			firsttime = true;

			rend = GetComponent<Renderer>();
			rend.material.color = Color.red;

		}



		void OnTriggerEnter(Collider other)
		{

			if (other.gameObject.tag == "MainCamera")
			{
				HandObject = other.gameObject;

				intersecting = true;
				if (no_show) {
					scope_camera.transform.Translate(0, 175, 0);
					main_cam.transform.Translate (0, 0, 20);
					no_show = false;
				}

			}
		}

		void OnTriggerExit(Collider other)
		{
			rend.material.color = Color.red;
			scope_camera.transform.Translate (0, -175, 0);
			main_cam.transform.Translate (0, 0, -20);
			no_show = true;
			intersecting = false;
			if (leavingcollider)
			{
				leavingcollider = false;
			}

		}


		void Update()
		{


			if (contact_required == contact.not_required)
			{

				if ((intersecting || interacting) && !leavingcollider)
				{

					if (HandObject.GetComponent<PseudoHand>().trigger_on)
					{

						interacting = true;
						rend.material.color = Color.magenta;
					}

					else
					{
						interacting = false;
						if (intersecting)
							rend.material.color = Color.green;
						else
						{
							rend.material.color = Color.red;
						}
					}
				}
			}
			else
			{
				if (intersecting && !leavingcollider)
				{

					if (HandObject.GetComponent<PseudoHand>().trigger_on)
					{

						rend.material.color = Color.magenta;
					}

					else
					{
						rend.material.color = Color.green;
					}
				}
			}

		}
		
//		void Calculate()
//		{
//			if (firsttime)
//			{
//				prevpos = HandObject.transform.position-center.transform.position;
//				currentpos = prevpos;
//				firsttime = false;
//			}
//
//			else
//			{
//				currentpos = HandObject.transform.position-center.transform.position;
//
//
//				switch (d_plane)
//				{
//				case detectionplane.xy:
//					d_angle = Mathf.Atan2(prevpos.x * currentpos.y - prevpos.y * currentpos.x, prevpos.x * currentpos.x + prevpos.y * currentpos.y);
//					break;
//				case detectionplane.xz:
//					d_angle = Mathf.Atan2(prevpos.z * currentpos.x - prevpos.x * currentpos.z, prevpos.z * currentpos.z + prevpos.x * currentpos.x);
//					break;
//				case detectionplane.yz:
//					d_angle = Mathf.Atan2(prevpos.y * currentpos.z - prevpos.z * currentpos.y, prevpos.y * currentpos.y + prevpos.z * currentpos.z);
//					break;
//				}
//
//				prevpos = currentpos;
//
//				if (angle+d_angle < uppercap_degrees*Mathf.PI/180 && angle+d_angle > -lowercap_degrees*Mathf.PI / 180)
//				{
//					Rotate();
//				}
//				else
//				{
//					firsttime = true;
//					interacting = false;
//					leavingcollider = true;
//					rend.material.color = Color.red;
//				}
//			}
//
//		}
//
//		[Server]
//		void Rotate()
//		{
//			switch (r_axis)
//			{
//			case rotationaxis.x:
//				transform.Rotate((d_angle * 180 / Mathf.PI), 0, 0);
//				break;
//			case rotationaxis.y:
//				transform.Rotate(0, (d_angle * 180 / Mathf.PI), 0);
//				break;
//			case rotationaxis.z:
//				transform.Rotate(0, 0, (d_angle * 180 / Mathf.PI));
//				break;
//			}
//			angle += d_angle;
//		}

	}
