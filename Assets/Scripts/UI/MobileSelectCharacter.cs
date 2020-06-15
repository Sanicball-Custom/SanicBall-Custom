using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileSelectCharacter : MonoBehaviour {

    public bool up, down, right, left;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LateUpdate()
    {
        up = down = left = right = false;
    }
    
    public void Up()
    {
        up = true;
    }

    public void Down()
    {
        down = true;

    }

    public void Left()
    {
        left = true;

    }

    public void Right()
    {
        right = true;

    }
}
