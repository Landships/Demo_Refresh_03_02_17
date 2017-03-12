namespace VRTK
{
    using UnityEngine;

    public class Button : VRTK_InteractableObject
    {
        public float downForce;
        public float limit;
        Vector3 upperLim;
        Vector3 lowerLim;
        public Fire_Controller turret_controller;

        public override void StartUsing(GameObject usingObject)
        {
            base.StartUsing(usingObject);
            this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0f, -downForce, 0f), ForceMode.VelocityChange);
            Debug.Log("firing?");
            turret_controller.OwnerFire();
        }

        protected override void Start()
        {
            base.Start();
            upperLim = new Vector3(0, limit, 0);
            lowerLim = new Vector3(0, -limit, 0);
        }

        /*protected override void FixedUpdate() {
            base.FixedUpdate();
            if (transform.localPosition.y > limit) {
                Debug.Log(upperLim);
                transform.localPosition = upperLim;
            }
            if (transform.localPosition.y < -limit) {
                transform.localPosition = lowerLim;
                Debug.Log(lowerLim);
            }
        }*/
    }
}
