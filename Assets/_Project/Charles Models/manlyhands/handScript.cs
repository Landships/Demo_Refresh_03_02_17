using UnityEngine;
using System.Collections;

public class handScript : MonoBehaviour {
    private Animator handAnimator;
    public float currentBlend = 0;
    VRTK.VRTK_ControllerEvents controller_events;
    VRTK.VRTK_InteractTouch touch_events;

    // Use this for initialization
    void Start () {
        handAnimator = GetComponent<Animator>();
        controller_events = transform.parent.GetComponent<VRTK.VRTK_ControllerEvents>();
        touch_events = transform.parent.GetComponent<VRTK.VRTK_InteractTouch>();

    }

    // Update is called once per frame
    void Update () {
        if (controller_events.triggerPressed)
        {
            handAnimator.SetFloat("handBlend", 1.0f, 0.1f, Time.deltaTime);
            currentBlend = 1;
        }
        else
        {
            handAnimator.SetFloat("handBlend", 0.0f, 0.1f, Time.deltaTime);
            currentBlend = 0;
        }

    }

    public void hapticFeedBack()
    {
        //SteamVR_Controller.Input(deviceIndex).TriggerHapticPulse(250);
    }
}
