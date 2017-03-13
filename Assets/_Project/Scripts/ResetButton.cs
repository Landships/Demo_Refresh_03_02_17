namespace VRTK
{
    using UnityEngine;

    public class ResetButton : VRTK_InteractableObject
    {
        public float downForce;
        public float limit;
        Vector3 upperLim;
        Vector3 lowerLim;
        public GameObject leftLever;
        public GameObject rightLever;

        public override void StartUsing(GameObject usingObject)
        {
            base.StartUsing(usingObject);
            this.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0f, -downForce, 0f), ForceMode.VelocityChange);
            Debug.Log("firing?");
            leftLever.transform.localEulerAngles = new Vector3(0, leftLever.transform.localEulerAngles.y, leftLever.transform.localEulerAngles.z);
            rightLever.transform.localEulerAngles = new Vector3(0, rightLever.transform.localEulerAngles.y, rightLever.transform.localEulerAngles.z);
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
