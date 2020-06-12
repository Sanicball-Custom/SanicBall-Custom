using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sanicball.Gameplay;

public class mobileMovementManager : MonoBehaviour{


    public Ball ball;
    public bool brake;
    public bool hold;
    public bool jumping;
    public bool jumped;
    public bool respawn;
    public bool leftpu;
    public bool rightpu;
    public bool pause;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void LateUpdate()
    {
        jumped = false;
        respawn = false;
        leftpu = false;
        rightpu = false;
        pause = false;
    }

    public void Brake(bool b)
    {
        brake = b;
    }

    public void JumpHold(bool j)
    {
        jumping = j;
    }

    public void JumpClick()
    {
        jumped = true;
    }

    public void Respawn()
    {
        respawn = true;
    }

    public void leftPU()
    {
        leftpu = true;
        Debug.Log("left");
    }

    public void rightPU()
    {
        rightpu = true;
        Debug.Log("right");
    }

    public void Pause()
    {
        pause = true;
    }
}
