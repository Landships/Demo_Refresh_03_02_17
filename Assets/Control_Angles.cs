using UnityEngine;
using System.Collections;


public class Control_Angles : MonoBehaviour {

    public GameObject left_lever;
    public GameObject right_lever;
    public GameObject hori_crank;
    public GameObject vert_crank;

    HingeJoint left_lever_joint;
    HingeJoint right_lever_joint;
    HingeJoint hori_crank_joint;
    HingeJoint vert_crank_joint;

    Transform left_lever_transform;
    Transform right_lever_transform;
    Transform hori_crank_transform;
    Transform vert_crank_transform;

    float vert_prev;

    float vert_change;

    float hori_prev;

    float hori_change;


    void Start() {
        left_lever_joint = left_lever.GetComponent<HingeJoint>();
        right_lever_joint = right_lever.GetComponent<HingeJoint>();
        hori_crank_joint = hori_crank.GetComponent<HingeJoint>();
        vert_crank_joint = vert_crank.GetComponent<HingeJoint>();

        left_lever_transform = left_lever.GetComponent<Transform>();
        right_lever_transform = right_lever.GetComponent<Transform>();
        hori_crank_transform = hori_crank.GetComponent<Transform>();
        vert_crank_transform = vert_crank.GetComponent<Transform>();

        vert_prev = vert_crank_joint.angle;
        hori_prev = hori_crank_joint.angle;
    }

    void Update() {

        float curr = vert_crank_joint.angle;
        if (curr < 0) {
            curr = 360 + curr;
        }
        vert_change = Mathf.DeltaAngle(vert_prev, curr); 
        vert_prev = curr;

        curr = hori_crank_joint.angle;
        if (curr < 0)
        {
            curr = 360 + curr;
        }
        hori_change = Mathf.DeltaAngle(hori_prev, curr);
        hori_prev = curr;
    }

    public float GetLeftLeverAngle() {
        //Debug.Log("Left Lever Angle is " + left_lever_joint.angle);
        return left_lever.transform.localRotation.eulerAngles.x;
    }

    public float GetRightLeverAngle() {
        return right_lever.transform.localRotation.eulerAngles.x;
    }

    public float GetVertCrankDelta() {
        return vert_change;
    }

    public float GetHoriCrankDelta()
    {
        return hori_change;
    }

}