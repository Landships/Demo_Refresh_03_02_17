using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{

    // Use this for initialization
    //public Cannon_Vertical_CS cannonScript;
    public Bullet_Generator_CS bulletScript;
    //public Barrel_Base_CS barrelScript;

    private LineRenderer trajectoryLine;
    private Vector3 position;
    private Vector3 velocity;

    byte current_player = 2; // Default to 2 so that laser is off by default
    GameObject n_manager;
    network_manager n_manager_script;



    void Start()
    {
        trajectoryLine = GetComponent<LineRenderer>();
        trajectoryLine.startColor = Color.red;
        trajectoryLine.endColor = Color.red;
        position = bulletScript.transform.position + -3 * bulletScript.transform.forward;
        velocity = bulletScript.transform.forward * bulletScript.Bullet_Force; //Offset added in original script-What does it do?
        //trajectoryLine.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        position = bulletScript.transform.position + -3 * bulletScript.transform.forward;
        velocity = bulletScript.transform.forward * bulletScript.Bullet_Force;
        updateTrajectory(position, velocity);
        // Use hotkey "t" to toggle whether the laser sight is displayed or not
        if (Input.GetKeyDown("t") == true)
        {
            trajectoryLine.enabled = !trajectoryLine.enabled;
        }
    }

    private void updateTrajectory(Vector3 position, Vector3 velocity)
    {
        Vector3 gravity = new Vector3(0, -9.81f, 0);
        int numSteps = 200;
        //float timeDelta = 2.0f / velocity.magnitude;
        float timeDelta = .1f;
        trajectoryLine.numPositions = numSteps;
        for (int i = 0; i < numSteps; ++i)
        {
            trajectoryLine.SetPosition(i, position);
            position += velocity * timeDelta + 0.5f * gravity * timeDelta * timeDelta;
            velocity += gravity * timeDelta;

        }

    }

    public void Prep()
    {
        n_manager = GameObject.Find("Custom Network Manager(Clone)");
        n_manager_script = n_manager.GetComponent<network_manager>();
        current_player = (byte)(n_manager_script.client_players_amount);

        trajectoryLine.enabled = current_player == 1;
    }
}