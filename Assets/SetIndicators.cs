using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetIndicators : MonoBehaviour {

    public Control_Angles angleScript;
    public GameObject upIndicator;
    public GameObject downIndicator;
    public GameObject leftIndicator;
    public GameObject rightIndicator;
    private Renderer upRenderer;
    private Renderer downRenderer;
    private Renderer leftRenderer;
    private Renderer rightRenderer;

    void Start () {
        upRenderer = upIndicator.GetComponent<Renderer>();
        downRenderer = downIndicator.GetComponent<Renderer>();
        leftRenderer = leftIndicator.GetComponent<Renderer>();
        rightRenderer = rightIndicator.GetComponent<Renderer>();
    }
	
	// Update is called once per frame
	void Update () {
        if (angleScript.GetHoriCrankDelta() > 0.1) {
            rightRenderer.material.SetColor("Red", Color.red);
        }

        else {
            rightRenderer.material.SetColor("Red", Color.black);
        }

        if (angleScript.GetHoriCrankDelta() < -0.1) {
            leftRenderer.material.SetColor("Red", Color.red);
        }

        else {
            leftRenderer.material.SetColor("Red", Color.black);
        }

        if (angleScript.GetVertCrankDelta() > 0.1) {
            upRenderer.material.SetColor("Red", Color.red);
        }

        else {
            upRenderer.material.SetColor("Red", Color.black);
        }

        if (angleScript.GetVertCrankDelta() < -0.1) {
            downRenderer.material.SetColor("Red", Color.red);
        }

        else {
            downRenderer.material.SetColor("Red", Color.black);
        }



    }
}
